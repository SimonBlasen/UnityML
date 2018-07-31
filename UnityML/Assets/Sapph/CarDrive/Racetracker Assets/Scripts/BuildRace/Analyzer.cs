using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Analyzer
{
    public static CalculatedCar AnalyzeCar(byte[] bytes)
    {
        PartConfiguration[] c = FileLoader.LoadFromBytes(bytes);

        CalculatedCar car = new CalculatedCar();

        //TODO: Health für jedes Part ausrechnen. Es ex. Part.Health als float Wert

        if (c != null)
        {
            UndirGraph<PartConfiguration> graph = CalculateGraph(c);
            Debug.Log("Graph #nodes: " + graph.Nodes.Count);
            Debug.Log("Graph #edges: " + graph.Edges.Count);

            //graph.WriteToFile(".\\tempGraph.graph");
            File.WriteAllLines(".\\tempGraph.graph", graph.ToDot());






            
            List<FuturePowertrainAxe> ptAxes = new List<FuturePowertrainAxe>();

            #region Antriebsstrang
            // Den ersten Motor findens

            int indexM = -1;

            for (int i = 0; i < c.Length; i++)
            {
                if (Part.MakePart(c[i].partType).IsMotor)
                {
                    indexM = i;
                    break;
                }
            }

            int amountOfMotors = 0;

            if (indexM != -1)
            {
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i].partType == c[indexM].partType)
                    {
                        amountOfMotors++;
                    }
                }

                for (int i = 0; i < c.Length; i++)
                {
                    FuturePowertrainAxe axe = makePowertrainAxe(c, i);
                    if (axe != null)
                    {
                        axe.factorToMotor = 0f;
                        ptAxes.Add(axe);
                    }
                }

                UList<int> usedAxes = new UList<int>();

                // Suche den Motor

                for (int i = 0; i < ptAxes.Count; i++)
                {
                    for (int j = 0; j < ptAxes[i].references.Count; j++)
                    {
                        if (Part.MakePart(c[ptAxes[i].indexToConfig].partType).IsAxe && ptAxes[i].references[j] == indexM)
                        {
                            fillFuturePowertrainAxes(ptAxes, c, ptAxes[i].indexToConfig, 1f, usedAxes);
                        }
                    }
                }
            }

            int motorAbsIndex = -1;
            float motorFactor = 1;
            float wheelFactor = 0;
            for (int i = 0; i < ptAxes.Count; i++)
            {
                if (ptAxes[i].factorToMotor != 0f)
                {
                    Debug.Log("is not 0f");
                }

                if (Part.MakePart(c[ptAxes[i].indexToConfig].partType).IsMotor && ptAxes[i].factorToMotor != 0f)
                {
                    motorFactor = ptAxes[i].factorToMotor;
                    motorAbsIndex = ptAxes[i].indexToConfig;
                }
                else if (Part.MakePart(c[ptAxes[i].indexToConfig].partType).IsWheel && ptAxes[i].factorToMotor != 0f)
                {
                    wheelFactor = ptAxes[i].factorToMotor;
                }

                for (int j = 0; j < ptAxes[i].references.Count; j++)
                {
                    if (Part.MakePart(c[ptAxes[i].references[j]].partType).IsMotor && ptAxes[i].factorToMotor != 0f)
                    {
                        motorFactor = ptAxes[i].factorToMotor;
                        motorAbsIndex = ptAxes[i].references[j];
                    }
                    else if (Part.MakePart(c[ptAxes[i].references[j]].partType).IsWheel && ptAxes[i].factorToMotor != 0f)
                    {
                        wheelFactor = ptAxes[i].factorToMotor;
                    }
                }


            }

            if (motorFactor < 0f)
                motorFactor *= -1;
            if (wheelFactor < 0f)
                wheelFactor *= -1;

            Debug.Log("Motor factor: " + motorFactor);
            Debug.Log("Wheel factor: " + wheelFactor);

            if (motorAbsIndex != -1)
            {
                car.TopSpeed = (wheelFactor / motorFactor) * Part.MakePart(c[motorAbsIndex].partType).MotorTopspeed;
                car.FullTorque = (motorFactor / wheelFactor) * Part.MakePart(c[motorAbsIndex].partType).MotorTorque * amountOfMotors;
            }
            else
            {
                return null;
            }

            for (int i = 0; i < ptAxes.Count; i++)
            {
                if (ptAxes[i].factorToMotor == 0f)
                {
                    ptAxes.RemoveAt(i);
                    i--;
                }
            }

            #endregion

            #region Mass

            float mass = 0f;

            for (int i = 0; i < c.Length; i++)
            {
                mass += Part.MakePart(c[i].partType).Mass;
            }

            car.Weight = mass * 3.1f;

            #endregion



            #region Steering

            int indexSteerwheel = -1;

            for (int i = 0; i < c.Length; i++)
            {
                if (c[i].partType == PartType.PartSteerWheel)
                {
                    indexSteerwheel = i;
                    break;
                }
            }

            List<FuturePowertrainAxe> ptSteerAxes = new List<FuturePowertrainAxe>();

            if (indexSteerwheel != -1)
            {
                for (int i = 0; i < c.Length; i++)
                {
                    FuturePowertrainAxe axe = makePowertrainAxe(c, i);
                    if (axe != null)
                    {
                        axe.factorToMotor = 0f;
                        ptSteerAxes.Add(axe);
                    }
                }

                UList<int> usedAxes = new UList<int>();

                for (int i = 0; i < ptSteerAxes.Count; i++)
                {
                    for (int j = 0; j < ptSteerAxes[i].references.Count; j++)
                    {
                        if (Part.MakePart(c[ptSteerAxes[i].indexToConfig].partType).IsAxe && ptSteerAxes[i].references[j] == indexSteerwheel)
                        {
                            fillFuturePowertrainAxes(ptSteerAxes, c, ptSteerAxes[i].indexToConfig, 1f, usedAxes);
                        }
                    }
                }
            }

            float toothBarFactor = 0f;
            int toothBarIndex = -1;
            Vector3 toothBarDirection = Vector3.zero;
            float maxSteerAngle = 0f;

            for (int i = 0; i < ptSteerAxes.Count; i++)
            {
                FuturePowertrainAxe axeF = ptSteerAxes[i];
                for (int j = 0; j < axeF.references.Count; j++)
                {
                    if (c[axeF.references[j]].partType == PartType.PartToothBar5)
                    {
                        toothBarIndex = axeF.references[j];
                        toothBarFactor = axeF.factorToMotor;
                        toothBarDirection = c[axeF.references[j]].partDirection.ToVector3();
                    }
                }
                
            }

            bool isSuspInverted = false;
            int steerSuspLength = 0;
            List<FutureSteadyPart> allSteerSteadys = new List<FutureSteadyPart>();

            List<FutureSteerPart> futureSteerParts = new List<FutureSteerPart>();

            List<FutureSteerWheel> steeredWheels = new List<FutureSteerWheel>();

            if (toothBarFactor != 0f && toothBarIndex != -1)
            {
                Debug.Log("Found a steer bar connected with a steering wheel!!");

                UList<int> usedSteerSteadys = new UList<int>();
                findSteadyParts(allSteerSteadys, c, toothBarIndex, usedSteerSteadys, true);
                for (int i = 0; i < allSteerSteadys.Count; i++)
                {
                    allSteerSteadys[i].factorToMotor = toothBarFactor;
                    //allSteerSteadys[i].indexToConfig = toothBarIndex;
                }

                Debug.Log("Amount of steer steadys: " + allSteerSteadys.Count);

                UList<int> steerSuspIndexes = allSteerSusps(c);
                Vector3Int suspConPos = new Vector3Int();
                Vector3Int axeConPos = new Vector3Int();
                for (int i = 0; i < allSteerSteadys.Count; i++)
                {
                    if (Part.MakePart(c[allSteerSteadys[i].indexToConfig].partType).IsAxe)
                    {
                        for (int j = 0; j < steerSuspIndexes.Count; j++)
                        {
                            suspConPos = c[steerSuspIndexes[j]].partPosition.Add(c[steerSuspIndexes[j]].partDirection.ToVector3Int().Multiply(-1 * (Part.MakePart(c[steerSuspIndexes[j]].partType).AxeLength - 1)));
                            
                            for (int k = 0; k < Part.MakePart(c[allSteerSteadys[i].indexToConfig].partType).AxeLength; k++)
                            {
                                axeConPos = c[allSteerSteadys[i].indexToConfig].partPosition.Add(c[allSteerSteadys[i].indexToConfig].partDirection.ToVector3Int().Multiply(k));

                                if (axeConPos.Equals(suspConPos))
                                {
                                    FutureSteerPart fsp = new FutureSteerPart();
                                    fsp.indexToConfig = steerSuspIndexes[j];
                                    if (c[steerSuspIndexes[j]].partDirection == PartDirection.North)
                                    {
                                        fsp.factorToMotor = toothBarFactor;
                                        isSuspInverted = false;
                                        if (toothBarDirection.x + toothBarDirection.y + toothBarDirection.z < 0)
                                        {
                                            toothBarDirection *= -1;
                                        }
                                    }
                                    else if (c[steerSuspIndexes[j]].partDirection == PartDirection.South)
                                    {
                                        fsp.factorToMotor = -toothBarFactor;
                                        isSuspInverted = true;
                                        if (toothBarDirection.x + toothBarDirection.y + toothBarDirection.z > 0)
                                        {
                                            toothBarDirection *= -1;
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogWarning("The Steer Susp has a unexpected direction. Factor is still 0. Direction is: " + c[steerSuspIndexes[j]].partDirection.ToString());
                                    }

                                    steerSuspLength = Part.MakePart(c[steerSuspIndexes[j]].partType).AxeLength;

                                    futureSteerParts.Add(fsp);
                                }
                            }
                        }
                    }
                }


                UList<int> tempWheels = allWheels(c, new UList<int>());

                for (int i = 0; i < futureSteerParts.Count; i++)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        if (Part.MakePart(c[j].partType).IsAxe)
                        {
                            for (int k = 0; k < Part.MakePart(c[j].partType).AxeLength; k++)
                            {
                                axeConPos = c[j].partPosition.Add(c[j].partDirection.ToVector3Int().Multiply(k));
                                
                                if (axeConPos.Equals(c[futureSteerParts[i].indexToConfig].partPosition))
                                {
                                    // Die Achse ist mit dem steerpart verbunden

                                    for (int m = 0; m < tempWheels.Count; m++)
                                    {
                                        for (int l = 0; l < Part.MakePart(c[j].partType).AxeLength; l++)
                                        {
                                            Vector3Int axeConNewPos = c[j].partPosition.Add(c[j].partDirection.ToVector3Int().Multiply(l));

                                            if (axeConNewPos.Equals(c[tempWheels[m]].partPosition))
                                            {
                                                FutureSteerWheel fsw = new FutureSteerWheel();
                                                fsw.indexToConfig = tempWheels[m];
                                                fsw.factorToMotor = toothBarFactor;
                                                steeredWheels.Add(fsw);
                                                Debug.Log("Found a steered wheel");

                                                maxSteerAngle = toothBarFactor * (1f / steerSuspLength) * 90f;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion













            //int index = -1;
            //
            //for (int i = 0; i < c.Length; i++)
            //{
            //    if (c[i].partType == PartType.PartMotorElectro1)
            //    {
            //        Debug.Log("Found a electro motor");
            //    }
            //    if (Part.MakePart(c[i].partType).IsMotor)
            //    {
            //        Debug.Log("Detected a motor");
            //        index = i;
            //        break;
            //    }
            //}
            //
            //if (index == -1)
            //{
            //    Debug.Log("No motor found");
            //    return null;
            //}
            //else // Es gibt (mindestens) einen Motor
            //{
            /*Debug.Log("Motor found");

            Vector3Int motorPos = c[index].partPosition;
            Vector3Int motorDirection = c[index].partDirection.ToVector3Int();

            int matchingAxeIndex = whichAxeIsHere(motorPos, c);*/

            //for (int i = 0; i < c.Length; i++)
            //{
            //    //Durchsuche alle Achsen, ob diese mit dem Motor verbunden ist
            //    if (isAxe(c[i].partType)
            //        && (c[i].partDirection.ToVector3Int() == motorDirection || c[i].partDirection.Opposite().ToVector3Int() == motorDirection))
            //    {
            //        int axeLength = calcAxeLength(c[i].partType);
            //        Vector3Int currPos = c[i].partPosition;
            //        for (int j = 0; j < axeLength; j++)
            //        {
            //            if (currPos.Equals(motorPos))
            //            {
            //                matchingAxeIndex = i;
            //                break;
            //            }
            //            currPos = currPos.Add(c[i].partDirection.ToVector3Int());
            //        }
            //    }
            //
            //    if (matchingAxeIndex != -1)
            //    {
            //        break;
            //    }
            //}

            /*List<FuturePowertrainAxe> axes = new List<FuturePowertrainAxe>();

            if (matchingAxeIndex == -1)
            {
                Debug.Log("No Axe at motors position found");
                return null;
            }
            else // Es gibt eine Achse, die mit dem Motor verbunden ist. Der Index der Achse ist in matchingAxeIndex
            {
                // Die Achse abklappern und alle direkt verbundenen Objekte anfügen. Dann über Zahnräder und Verlängerungen rekursiv suchen, ohne schon besuchte Sachen nochmal zu checken

                Debug.Log("Found a Axe attached to the motor");
                afp.Add(matchingAxeIndex);

                FuturePowertrainAxe thisAxe = new FuturePowertrainAxe();
                thisAxe.indexToConfig = matchingAxeIndex;
                thisAxe.rotateAroundVector = c[matchingAxeIndex].partDirection.ToVector3();
                thisAxe.factorToMotor = 1f;
                axes.Add(thisAxe);

                UList<int> directlyAttachedParts = toAxeAttachedParts(matchingAxeIndex, c, afp);

                afp.AddRange(directlyAttachedParts);
                afp.Add(matchingAxeIndex);

                for (int i = 0; i < directlyAttachedParts.Count; i++)
                {
                    if (Part.MakePart(c[directlyAttachedParts[i]].partType).IsWheel == false)
                    {
                        FuturePowertrainAxe newAxe = new FuturePowertrainAxe();
                        newAxe.indexToConfig = directlyAttachedParts[i];
                        newAxe.rotateAroundVector = c[matchingAxeIndex].partDirection.ToVector3();
                        newAxe.factorToMotor = 1f;
                        axes.Add(newAxe);
                    }

                    //if (c[directlyAttachedParts[i]].partType == PartType.PartWheelStreet1)
                    //{
                    //    for (int j = 0; j < axes.Count; j++)
                    //    {
                    //        axes[j].wheelID = directlyAttachedParts[i];
                    //    }
                    //}
                }
            }*/
            UList<int> afp = new UList<int>();    //Already Found Parts

            UList<int> wheels = allWheels(c, afp);

            afp.AddRange(wheels);







            car.Grip = Part.MakePart(c[wheels[0]].partType).Grip;




            // TODO usw usf



            int seatIndex = getSeat(c, afp);
            if (seatIndex == -1)
            {
                Debug.Log("No seat found");
                return null;
            }
            else
            {
                Debug.Log("Found a seat");
                car.AddSeat(c[seatIndex].partPosition.ToVector3(), c[seatIndex].partDirection, c[seatIndex].partRotation, calculateHealth(graph, c, seatIndex, seatIndex));

                if (wheels.Count >= 4)
                {
                    for (int i = 0; i < wheels.Count; i++)
                    {
                        bool steered = false;
                        float factorToothBarWheel = 0f;
                        for (int j = 0; j < steeredWheels.Count; j++)
                        {
                            if (steeredWheels[j].indexToConfig == wheels[i])
                            {
                                steered = true;
                                factorToothBarWheel = steeredWheels[j].factorToMotor;
                                break;
                            }
                        }
                        car.AddWheel(c[wheels[i]], true, steered, Part.MakePart(c[wheels[i]].partType).WheelRadius, Part.MakePart(c[wheels[i]].partType).Grip, factorToothBarWheel * 25f, calculateHealth(graph, c, wheels[i], seatIndex));
                    }
                    //car.AddWheel(c[wheels[0]], true, true, Part.MakePart(c[wheels[0]].partType).WheelRadius, Part.MakePart(c[wheels[0]].partType).Grip);
                    //car.AddWheel(c[wheels[1]], true, true, Part.MakePart(c[wheels[1]].partType).WheelRadius, Part.MakePart(c[wheels[1]].partType).Grip);
                    //car.AddWheel(c[wheels[2]], true, false, Part.MakePart(c[wheels[2]].partType).WheelRadius, Part.MakePart(c[wheels[2]].partType).Grip);
                    //car.AddWheel(c[wheels[3]], true, false, Part.MakePart(c[wheels[3]].partType).WheelRadius, Part.MakePart(c[wheels[3]].partType).Grip);

                }
                else
                {
                    Debug.Log("Less than 4 wheels found");
                    return null;
                }

                //for (int i = 0; i < axes.Count; i++)
                //{
                //    car.AddPowertrainAxe(c[axes[i].indexToConfig], 3, axes[i].factorToMotor, axes[i].rotateAroundVector);
                //}

                for (int i = 0; i < ptAxes.Count; i++)
                {
                    car.AddPowertrainAxe(c[ptAxes[i].indexToConfig], 3, ptAxes[i].factorToMotor, ptAxes[i].rotateAroundVector, calculateHealth(graph, c, ptAxes[i].indexToConfig, seatIndex));
                    afp.Add(ptAxes[i].indexToConfig);
                }

                for (int i = 0; i < allSteerSteadys.Count; i++)
                {
                    car.AddSteerBar(c[allSteerSteadys[i].indexToConfig], toothBarDirection, (maxSteerAngle / 90f) * steerSuspLength, calculateHealth(graph, c, allSteerSteadys[i].indexToConfig, seatIndex));
                    afp.Add(allSteerSteadys[i].indexToConfig);
                }

                for (int i = 0; i < futureSteerParts.Count; i++)
                {
                    car.AddSteerPart(c[futureSteerParts[i].indexToConfig], isSuspInverted, calculateHealth(graph, c, futureSteerParts[i].indexToConfig, seatIndex));
                    afp.Add(futureSteerParts[i].indexToConfig);
                }

                CalculatedMinigun[] calcMiniguns = findMiniguns(c, graph, seatIndex);
                for (int i = 0; i < calcMiniguns.Length; i++)
                {
                    int indexToConfig = -1;
                    for (int j = 0; j < c.Length; j++)
                    {
                        if (calcMiniguns[i].Type == c[j].partType && calcMiniguns[i].Position == c[j].partPosition.ToVector3() && calcMiniguns[i].Direction == c[j].partDirection && calcMiniguns[i].Rotation == c[j].partRotation)
                        {
                            indexToConfig = j;
                            break;
                        }
                    }
                    if (indexToConfig == -1)
                    {
                        Debug.LogError("indexToConfig is -1 in Analyzer in search for Miniguns");
                    }
                    car.AddMinigun(calcMiniguns[i].Type, calcMiniguns[i].Position, calcMiniguns[i].Direction, calcMiniguns[i].Rotation, calculateHealth(graph, c, indexToConfig, seatIndex));
                    afp.Add(indexToConfig);
                }

                for (int i = 0; i < c.Length; i++)
                {
                    if (afp.Contains(i) == false)
                    {
                        car.AddPart(c[i], calculateHealth(graph, c, i, seatIndex));
                    }
                }

                Debug.Log("Calculation successfull");

                return car;
            }
            //}
        }

        Debug.Log("Reached end of the method");
        return null;
    }

    public static CalculatedCar AnalyzeCar(string carFile)
    {
        if (File.Exists(carFile))
        {
            byte[] bytes = File.ReadAllBytes(carFile);
            return AnalyzeCar(bytes);
        }

        return null;
    }

    public static UndirGraph<PartConfiguration> CalculateGraph(PartConfiguration[] c)
    {
        UndirGraph<PartConfiguration> graph = new UndirGraph<PartConfiguration>();
        for (int i = 0; i < c.Length; i++)
        {
            graph.AddNode(c[i]);
        }

        for (int i = 0; i < c.Length; i++)
        {
            PartConfiguration part = c[i];
            
            for (int j = 0; j < c.Length; j++)
            {
                if (i != j)
                {
                    PartConfiguration other = c[j];
                    if (arePartConfigsConected(part, other))
                    {
                        graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                    }

                    // Gears
                    else if (Part.MakePart(part.partType).IsGear && Part.MakePart(other.partType).IsGear)
                    {
                        if (part.partType == PartType.PartGear8 && other.partType == PartType.PartGear8
                            && (part.partDirection == other.partDirection || part.partDirection == other.partDirection.Opposite())
                            && (
                                    (part.partPosition.x == other.partPosition.x && part.partPosition.y == other.partPosition.y && Mathf.Abs(part.partPosition.z - other.partPosition.z) == 1)
                                || (part.partPosition.x == other.partPosition.x && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.y - other.partPosition.y) == 1)
                                || (part.partPosition.y == other.partPosition.y && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.x - other.partPosition.x) == 1)))
                        {
                            graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                        }

                        if (part.partType == PartType.PartGear8 && other.partType == PartType.PartGear24
                            && (part.partDirection == other.partDirection || part.partDirection == other.partDirection.Opposite())
                            && (
                                    (part.partPosition.x == other.partPosition.x && part.partPosition.y == other.partPosition.y && Mathf.Abs(part.partPosition.z - other.partPosition.z) == 2)
                                || (part.partPosition.x == other.partPosition.x && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.y - other.partPosition.y) == 2)
                                || (part.partPosition.y == other.partPosition.y && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.x - other.partPosition.x) == 2)))
                        {
                            graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                        }

                        if (part.partType == PartType.PartGear24 && other.partType == PartType.PartGear8
                            && (part.partDirection == other.partDirection || part.partDirection == other.partDirection.Opposite())
                            && (
                                    (part.partPosition.x == other.partPosition.x && part.partPosition.y == other.partPosition.y && Mathf.Abs(part.partPosition.z - other.partPosition.z) == 2)
                                || (part.partPosition.x == other.partPosition.x && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.y - other.partPosition.y) == 2)
                                || (part.partPosition.y == other.partPosition.y && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.x - other.partPosition.x) == 2)))
                        {
                            graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                        }

                        if (part.partType == PartType.PartGear24 && other.partType == PartType.PartGear24
                            && (part.partDirection == other.partDirection || part.partDirection == other.partDirection.Opposite())
                            && (
                                    (part.partPosition.x == other.partPosition.x && part.partPosition.y == other.partPosition.y && Mathf.Abs(part.partPosition.z - other.partPosition.z) == 3)
                                || (part.partPosition.x == other.partPosition.x && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.y - other.partPosition.y) == 3)
                                || (part.partPosition.y == other.partPosition.y && part.partPosition.z == other.partPosition.z && Mathf.Abs(part.partPosition.x - other.partPosition.x) == 3)))
                        {
                            graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                        }
                    }

                    // Toothbar
                    else if (part.partType == PartType.PartToothBar5)
                    {
                        Connectionpoint[] connectors = Part.MakePart(PartType.PartToothBar5).GetConnectionpoints(part.partDirection, part.partRotation);

                        if (other.partType == PartType.PartGear8
                            && (connectors[0].FacedDirection.Cross(part.partDirection) == other.partDirection || connectors[0].FacedDirection.Cross(part.partDirection) == other.partDirection.Opposite())
                            && (part.partPosition.x == other.partPosition.x && part.partPosition.z == other.partPosition.z && other.partPosition.y - part.partPosition.y == 1))
                        {
                            graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                        }

                        if (other.partType == PartType.PartGear24
                            && (connectors[0].FacedDirection.Cross(part.partDirection) == other.partDirection || connectors[0].FacedDirection.Cross(part.partDirection) == other.partDirection.Opposite())
                            && (part.partPosition.x == other.partPosition.x && part.partPosition.z == other.partPosition.z && other.partPosition.y - part.partPosition.y == 2))
                        {
                            graph.AddEdge(graph.GetNode(part), graph.GetNode(other));
                        }
                    }
                }
            }
        }

        return graph;
    }

    private static bool arePartConfigsConected(PartConfiguration a, PartConfiguration b)
    {
        Part part = Part.MakePart(a.partType);
        Part other = Part.MakePart(b.partType);
        Connectionpoint[] connectionPoints = part.GetConnectionpoints(a.partDirection, a.partRotation);
        Connectionpoint[] otherConnectors = other.GetConnectionpoints(b.partDirection, b.partRotation);

        for (int i = 0; i < connectionPoints.Length; i++)
        {
            for (int k = 0; k < otherConnectors.Length; k++)
            {
                if (otherConnectors[k].ConnectorPosition.Add(b.partPosition).Equals(connectionPoints[i].ConnectorPosition.Add(a.partPosition)))
                {
                    // Zwei Connectoren befinden sich auf der gleichen Position

                    return true;
                }
            }
        }

        return false;
    }


    private static float calculateHealth(Graph<PartConfiguration> graph, PartConfiguration[] c, int index, int seatIndex)
    {
        if (index == seatIndex)
        {
            return float.MaxValue;
        }

        float health = 0f;
        Part part = Part.MakePart(c[index].partType);
        health += part.Health;

        Graph<PartConfiguration> without = graph.Clone();

        List<Node<PartConfiguration>> attNodes = new List<Node<PartConfiguration>>();
        for (int i = 0; i < without.GetOutcommingEdges(without.GetNode(c[index])).Count; i++)
        {
            attNodes.Add(without.GetOutcommingEdges(without.GetNode(c[index]))[i].To);
        }

        without.DeleteNode(without.GetNode(c[index]));


        for (int i = 0; i < attNodes.Count; i++)
        {
            if (without.ExistsPathFromTo(attNodes[i], without.GetNode(c[seatIndex])))
            {
                health += Part.MakePart(attNodes[i].ID.partType).Health;
            }
        }

        return health;
    }

    private static CalculatedMinigun[] findMiniguns(PartConfiguration[] c, Graph<PartConfiguration> graph, int seatIndex)
    {
        List<CalculatedMinigun> miniGuns = new List<CalculatedMinigun>();
        for (int i = 0; i < graph.Nodes.Count; i++)
        {
            if (graph.Nodes[i].ID.partType == PartType.PartWeaponMinigun && graph.ExistsPathFromTo(graph.Nodes[i], graph.GetNode(c[seatIndex])))
            {
                CalculatedMinigun mini = new CalculatedMinigun();
                mini.Position = graph.Nodes[i].ID.partPosition.ToVector3();
                mini.Direction = graph.Nodes[i].ID.partDirection;
                mini.Rotation = graph.Nodes[i].ID.partRotation;
                mini.Type = graph.Nodes[i].ID.partType;
                miniGuns.Add(mini);
            }
        }

        return miniGuns.ToArray();
    }


    private static void findSteadyParts(List<FutureSteadyPart> parts, PartConfiguration[] c, int startPartIndex, UList<int> usedParts, bool useRecursive)
    {
        FutureSteadyPart steadyPart = new FutureSteadyPart();
        steadyPart.indexToConfig = startPartIndex;
        parts.Add(steadyPart);
        usedParts.Add(startPartIndex);

        if (useRecursive)
        {
            //Vector3Int[] conPoss = null;
            Connectionpoint[] conPoints = Part.MakePart(c[startPartIndex].partType).GetConnectionpoints(c[startPartIndex].partDirection, c[startPartIndex].partRotation);

            //if (c[startPartIndex].partType == PartType.PartPinRound2x1)
            //{
            //    conPoss = new Vector3Int[2];
            //    conPoss[0] = Part.MakePart(c[startPartIndex].partType).GetConnectionpoints(c[startPartIndex].partDirection, c[startPartIndex].partRotation)[0].ConnectorPosition.Add(c[startPartIndex].partPosition);
            //    conPoss[1] = Part.MakePart(c[startPartIndex].partType).GetConnectionpoints(c[startPartIndex].partDirection, c[startPartIndex].partRotation)[1].ConnectorPosition.Add(c[startPartIndex].partPosition);
            //}
            //else if (c[startPartIndex].partType == PartType.PartPinRound3x1)
            //{
            //    conPoss = new Vector3Int[3];
            //    conPoss[0] = Part.MakePart(c[startPartIndex].partType).GetConnectionpoints(c[startPartIndex].partDirection, c[startPartIndex].partRotation)[0].ConnectorPosition.Add(c[startPartIndex].partPosition);
            //    conPoss[1] = Part.MakePart(c[startPartIndex].partType).GetConnectionpoints(c[startPartIndex].partDirection, c[startPartIndex].partRotation)[1].ConnectorPosition.Add(c[startPartIndex].partPosition);
            //    conPoss[2] = Part.MakePart(c[startPartIndex].partType).GetConnectionpoints(c[startPartIndex].partDirection, c[startPartIndex].partRotation)[2].ConnectorPosition.Add(c[startPartIndex].partPosition);
            //}

            for (int i = 0; i < c.Length; i++)
            {
                //Connectionpoint[] conPoints = Part.MakePart(c[i].partType).GetConnectionpoints(c[i].partDirection, c[i].partRotation);

                Connectionpoint[] conPointsNew = Part.MakePart(c[i].partType).GetConnectionpoints(c[i].partDirection, c[i].partRotation);

                for (int j = 0; j < conPointsNew.Length; j++)
                {
                    for (int k = 0; k < conPoints.Length; k++)
                    {
                        if (conPoints[k].ConnectorPosition.Add(c[startPartIndex].partPosition).Equals(conPointsNew[j].ConnectorPosition.Add(c[i].partPosition))
                            && (conPoints[k].Connectortype == ConnectorType.ROUND_PIN && conPointsNew[j].Connectortype == ConnectorType.ROUND_HOLE
                                    || conPoints[k].Connectortype == ConnectorType.ROUND_HOLE && conPointsNew[j].Connectortype == ConnectorType.ROUND_PIN))
                        {
                            if (usedParts.Contains(i) == false)
                            {
                                findSteadyParts(parts, c, i, usedParts, true);
                            }
                        }
                        else if (conPoints[k].ConnectorPosition.Add(c[startPartIndex].partPosition).Equals(conPointsNew[j].ConnectorPosition.Add(c[i].partPosition))
                            && (conPoints[k].Connectortype == ConnectorType.ROUND_HOLE && conPointsNew[j].Connectortype == ConnectorType.CROSS_PIN))
                        {
                            if (usedParts.Contains(i) == false)
                            {
                                findSteadyParts(parts, c, i, usedParts, false);
                            }
                        }
                    }
                }
            }
        }
    }

    private static void fillFuturePowertrainAxes(List<FuturePowertrainAxe> axes, PartConfiguration[] c, int startAxeIndex, float startFactor, UList<int> usedAxes)
    {
        int startIndexInAxes = -1;
        for (int i = 0; i < axes.Count; i++)
        {
            if (axes[i].indexToConfig == startAxeIndex)
            {
                startIndexInAxes = i;
            }
        }

        if (startIndexInAxes != -1)
        {
            if (Part.MakePart(c[axes[startIndexInAxes].indexToConfig].partType).IsGear)
            {
                float thisGearTooths = Part.MakePart(c[axes[startIndexInAxes].indexToConfig].partType).GearTooths;

                // DEBUG
                List<int> references = new List<int>();
                for (int a = 0; a < axes[startIndexInAxes].references.Count; a++)
                {
                    references.Add(axes[startIndexInAxes].references[a]);
                }
                //

                for (int i = 0; i < axes[startIndexInAxes].references.Count; i++)
                {
                    for (int j = 0; j < axes.Count; j++)
                    {
                        if (axes[startIndexInAxes].references[i] == axes[j].indexToConfig && Part.MakePart(c[axes[j].indexToConfig].partType).IsGear)
                        {
                            if (usedAxes.Contains(axes[j].indexToConfig) == false)
                            {
                                axes[j].factorToMotor = -startFactor * (thisGearTooths / Part.MakePart(c[axes[j].indexToConfig].partType).GearTooths);
                                usedAxes.Add(axes[j].indexToConfig);
                                fillFuturePowertrainAxes(axes, c, axes[j].indexToConfig, axes[j].factorToMotor, usedAxes);
                            }
                        }
                        else if (axes[startIndexInAxes].references[i] == axes[j].indexToConfig)
                        {
                            if (usedAxes.Contains(axes[j].indexToConfig) == false)
                            {
                                axes[j].factorToMotor = startFactor;
                                usedAxes.Add(axes[j].indexToConfig);
                                fillFuturePowertrainAxes(axes, c, axes[j].indexToConfig, startFactor, usedAxes);
                            }
                        }
                    }
                }
            }

            else
            {
                for (int i = 0; i < axes[startIndexInAxes].references.Count; i++)
                {
                    for (int j = 0; j < axes.Count; j++)
                    {
                        if (axes[startIndexInAxes].references[i] == axes[j].indexToConfig)
                        {
                            if (usedAxes.Contains(axes[j].indexToConfig) == false)
                            {
                                axes[j].factorToMotor = startFactor;
                                usedAxes.Add(axes[j].indexToConfig);
                                fillFuturePowertrainAxes(axes, c, axes[j].indexToConfig, startFactor, usedAxes);
                            }
                        }
                    }
                }
            }
        }
    }

    private static FuturePowertrainAxe makePowertrainAxe(PartConfiguration[] c, int partIndex)
    {
        FuturePowertrainAxe axe = null;

        if (c[partIndex].partType == PartType.PartCrossExtend)
        {
            axe = new FuturePowertrainAxe();
            axe.indexToConfig = partIndex;
            axe.rotateAroundVector = c[partIndex].partDirection.ToVector3();
            if (axe.rotateAroundVector.x == -1)
                axe.rotateAroundVector.x = 1;
            if (axe.rotateAroundVector.y == -1)
                axe.rotateAroundVector.y = 1;
            if (axe.rotateAroundVector.z == -1)
                axe.rotateAroundVector.z = 1;
            for (int i = 0; i < c.Length; i++)
            {
                if (Part.MakePart(c[i].partType).IsAxe && c[i].partPosition.Equals(c[partIndex].partPosition))
                {
                    axe.references.Add(i);
                }
                else if (Part.MakePart(c[i].partType).IsAxe && c[i].partPosition.Equals(c[partIndex].partPosition.Add(c[partIndex].partDirection.ToVector3Int()))
                    && (c[i].partDirection == c[partIndex].partDirection || c[i].partDirection.Opposite() == c[partIndex].partDirection))
                {
                    axe.references.Add(i);
                }
                else if (Part.MakePart(c[i].partType).IsAxe
                    && c[i].partPosition.Equals(c[partIndex].partPosition.Add(c[partIndex].partDirection.Opposite().ToVector3Int().Multiply(Part.MakePart(c[i].partType).AxeLength - 1)))
                    && (c[i].partDirection == c[partIndex].partDirection || c[i].partDirection.Opposite() == c[partIndex].partDirection))
                {
                    axe.references.Add(i);
                }
                else if (Part.MakePart(c[i].partType).IsAxe
                    && c[i].partPosition.Equals(c[partIndex].partPosition.Add(c[partIndex].partDirection.ToVector3Int().Multiply(Part.MakePart(c[i].partType).AxeLength)))
                    && (c[i].partDirection == c[partIndex].partDirection || c[i].partDirection.Opposite() == c[partIndex].partDirection))
                {
                    axe.references.Add(i);
                }
            }
        }

        else if (Part.MakePart(c[partIndex].partType).IsAxe)
        {
            UList<int> directlyAttached = directlyToAxeAttachedParts(c, partIndex);
            axe = new FuturePowertrainAxe();
            axe.indexToConfig = partIndex;
            axe.rotateAroundVector = c[partIndex].partDirection.ToVector3();

            for (int i = 0; i < directlyAttached.Count; i++)
            {
                axe.references.Add(directlyAttached[i]);
            }
        }

        else if (Part.MakePart(c[partIndex].partType).IsGear)
        {
            UList<int> directlyAttached = directlyToGearAttachedGears(c, partIndex);
            UList<int> directlyAttachedSteerBars = directlyToGearAttachedSteerBars(c, partIndex);
            axe = new FuturePowertrainAxe();
            axe.indexToConfig = partIndex;
            axe.rotateAroundVector = c[partIndex].partDirection.ToVector3();

            for (int i = 0; i < directlyAttached.Count; i++)
            {
                //if (Part.MakePart(c[directlyAttached[i]].partType).IsAxe)
                //{
                    axe.references.Add(directlyAttached[i]);
                //}
            }

            for (int i = 0; i < directlyAttachedSteerBars.Count; i++)
            {
                axe.references.Add(directlyAttachedSteerBars[i]);
                Debug.Log("Found a steer bar attached to a gear");
            }
        }


        


        return axe;
    }

    private static int getSeat(PartConfiguration[] c, List<int> afp)
    {
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i].partType == PartType.PartSeat)
            {
                return i;
            }
        }

        return -1;
    }

    private static UList<int> allSteerSusps(PartConfiguration[] c)
    {
        UList<int> futures = new UList<int>();

        for (int i = 0; i < c.Length; i++)
        {
            if (Part.MakePart(c[i].partType).IsSteerSusp)
            {
                futures.Add(i);
            }
        }

        return futures;
    }

    private static UList<int> allWheels(PartConfiguration[] c, UList<int> afp)
    {
        UList<int> futures = new UList<int>();

        for (int i = 0; i < c.Length; i++)
        {
            if (Part.MakePart(c[i].partType).IsWheel)
            {
                futures.Add(i);
            }
        }

        return futures;
    }

    private static UList<int> eliminateDoubles(UList<int> list)
    {
        UList<int> retList = new UList<int>();

        List<int> theList = list.Distinct().ToList();
        for (int i = 0; i < theList.Count; i++)
        {
            retList.Add(theList[i]);
        }

        return retList;
    }

    private static UList<int> toAxeAttachedParts(int originAxe, PartConfiguration[] configs, UList<int> afp)
    {
        UList<int> futures = new UList<int>();

        afp.Add(originAxe);

        Vector3Int[] possiblePositions = new Vector3Int[Part.MakePart(configs[originAxe].partType).AxeLength + 2];
        Vector3Int startPos = configs[originAxe].partPosition.Add(configs[originAxe].partDirection.Opposite().ToVector3Int());
        for (int i = 0; i < possiblePositions.Length; i++)
        {
            possiblePositions[i] = startPos;
            startPos = startPos.Add(configs[originAxe].partDirection.ToVector3Int());
        }

        for (int i = 0; i < configs.Length; i++)
        {
            for (int j = 0; j < possiblePositions.Length; j++)
            {
                if (possiblePositions[j].Equals(configs[i].partPosition)
                    && (Part.MakePart(configs[i].partType).IsGear) 
                    && (configs[i].partDirection == configs[originAxe].partDirection || configs[i].partDirection.Opposite() == configs[originAxe].partDirection))
                {
                    futures.Add(i);
                }
                else if (possiblePositions[j].Equals(configs[i].partPosition) && (configs[i].partType == PartType.PartCrossExtend))
                {
                    futures.Add(i);

                    int attachedAxe = -1;
                    if (j == 0)
                    {
                        attachedAxe = whichAxeIsHere(possiblePositions[0], configs);
                    }
                    else if (j == 1)
                    {
                        attachedAxe = whichAxeIsHere(possiblePositions[0], configs);
                    }
                    else if (j == possiblePositions.Length - 1)
                    {
                        attachedAxe = whichAxeIsHere(possiblePositions[possiblePositions.Length - 1], configs);
                    }
                    else if (j == possiblePositions.Length - 2)
                    {
                        attachedAxe = whichAxeIsHere(possiblePositions[possiblePositions.Length - 1], configs);
                    }

                    if (attachedAxe != -1)
                    {
                        futures.Add(attachedAxe);
                        // Achtung Rekursion!!
                        if (afp.Contains(attachedAxe) == false)
                        {
                            futures.AddRange(toAxeAttachedParts(attachedAxe, configs, afp));
                        }
                    }
                }
            }
        }

        return futures;
    }

    private static UList<int> directlyToAxeAttachedParts(PartConfiguration[] c, int originAxe)
    {
        UList<int> futures = new UList<int>();

        Vector3Int[] possiblePositions = new Vector3Int[Part.MakePart(c[originAxe].partType).AxeLength + 2];

        Vector3Int startPos = c[originAxe].partPosition.Add(c[originAxe].partDirection.Opposite().ToVector3Int());
        for (int i = 0; i < possiblePositions.Length; i++)
        {
            possiblePositions[i] = startPos;
            startPos = startPos.Add(c[originAxe].partDirection.ToVector3Int());
        }

        for (int i = 0; i < c.Length; i++)
        {
            for (int j = 0; j < possiblePositions.Length; j++)
            {
                if (possiblePositions[j].Equals(c[i].partPosition)
                    && (Part.MakePart(c[i].partType).IsGear)
                    && (c[i].partDirection == c[originAxe].partDirection || c[i].partDirection.Opposite() == c[originAxe].partDirection)
                    && j != 0 && j != possiblePositions.Length - 1)
                {
                    futures.Add(i);
                }
                else if (possiblePositions[j].Equals(c[i].partPosition) && (c[i].partType == PartType.PartCrossExtend)
                    && (c[i].partDirection == c[originAxe].partDirection || c[i].partDirection.Opposite() == c[originAxe].partDirection))
                {
                    futures.Add(i);
                }
                else if (possiblePositions[j].Equals(c[i].partPosition)
                    && Part.MakePart(c[i].partType).IsMotor
                    && (c[i].partDirection == c[originAxe].partDirection || c[i].partDirection.Opposite() == c[originAxe].partDirection)
                    && j != 0 && j != possiblePositions.Length - 1)
                {
                    futures.Add(i);
                }
                else if (possiblePositions[j].Equals(c[i].partPosition)
                    && Part.MakePart(c[i].partType).IsWheel
                    && (c[i].partDirection == c[originAxe].partDirection || c[i].partDirection.Opposite() == c[originAxe].partDirection)
                    && j != 0 && j != possiblePositions.Length - 1)
                {
                    futures.Add(i);
                }
                else if (possiblePositions[j].Equals(c[i].partPosition)
                    && c[i].partType == PartType.PartSteerWheel
                    && (c[i].partDirection == c[originAxe].partDirection || c[i].partDirection.Opposite() == c[originAxe].partDirection)
                    && j != 0 && j != possiblePositions.Length - 1)
                {
                    futures.Add(i);
                }
            }
        }

        return futures;
    }

    private static UList<int> directlyToGearAttachedSteerBars(PartConfiguration[] c, int originGear)
    {
        UList<int> futures = new UList<int>();

        for (int i = 0; i < c.Length; i++)
        {
            if (Part.MakePart(c[i].partType).IsAxe)
            {
                UList<int> attParts = directlyToAxeAttachedParts(c, i);
                if (attParts.Contains(originGear))
                {
                    futures.Add(i);
                    break;
                }
            }
        }

        Vector3Int downPos = new Vector3Int(0, -1, 0);
        Vector3Int upPos = new Vector3Int(0, 1, 0);
        if (c[originGear].partType == PartType.PartGear24)
        {
            downPos = new Vector3Int(0, -2, 0);
            upPos = new Vector3Int(0, 2, 0);
        }

        if (c[originGear].partDirection != PartDirection.Down && c[originGear].partDirection != PartDirection.Up)
        {
            for (int j = 0; j < c.Length; j++)
            {
                if (c[originGear].partPosition.Add(downPos).Equals(c[j].partPosition) && c[j].partType == PartType.PartToothBar5 && (c[j].partDirection != c[originGear].partDirection && c[j].partDirection != c[originGear].partDirection.Opposite() && c[j].partDirection != PartDirection.Up && c[j].partDirection != PartDirection.Down))
                {
                    futures.Add(j);
                }
                else if (c[originGear].partPosition.Add(upPos).Equals(c[j].partPosition) && c[j].partType == PartType.PartToothBar5 && (c[j].partDirection != c[originGear].partDirection && c[j].partDirection != c[originGear].partDirection.Opposite() && c[j].partDirection != PartDirection.Up && c[j].partDirection != PartDirection.Down))
                {
                    futures.Add(j);
                }
            }
        }


        return futures;
    }

    private static UList<int> directlyToGearAttachedGears(PartConfiguration[] c, int originGear)
    {
        UList<int> futures = new UList<int>();

        for (int i = 0; i < c.Length; i++)
        {
            if (Part.MakePart(c[i].partType).IsAxe)
            {
                UList<int> attParts = directlyToAxeAttachedParts(c, i);
                if (attParts.Contains(originGear))
                {
                    futures.Add(i);
                    break;
                }
            }
        }
        


        Vector3Int[] possiblePositions;// = new Vector3Int[12];
        
        if (c[originGear].partType == PartType.PartGear8)
        {
            possiblePositions = new Vector3Int[8];
        }
        else if (c[originGear].partType == PartType.PartGear24)
        {
            possiblePositions = new Vector3Int[12];
        }
        else if (c[originGear].partType == PartType.PartGearCorner4)
        {
            possiblePositions = new Vector3Int[12];
        }

        Vector3Int walkDir0 = new Vector3Int();
        Vector3Int walkDir1 = new Vector3Int();

        Vector3Int lookDirection = c[originGear].partDirection.ToVector3Int();

        if (lookDirection.x != 0)
        {
            walkDir0 = new Vector3Int(0, 1, 0);
            walkDir1 = new Vector3Int(0, 0, 1);
        }
        else if (lookDirection.y != 0)
        {
            walkDir0 = new Vector3Int(1, 0, 0);
            walkDir1 = new Vector3Int(0, 0, 1);
        }
        else if (lookDirection.z != 0)
        {
            walkDir0 = new Vector3Int(0, 1, 0);
            walkDir1 = new Vector3Int(1, 0, 0);
        }

        

        if (c[originGear].partType == PartType.PartGear8)
        {
            for (int i = -2; i <= 2; i++)
            {
                if (i != 0)
                {
                    Vector3Int pos = c[originGear].partPosition.Add(walkDir0.Multiply(i));
                    for (int j = 0; j < c.Length; j++)
                    {
                        if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear24 && (i == -2 || i == 2) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                        else if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear8 && (i == -1 || i == 1) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                    }
                }
            }
            for (int i = -2; i <= 2; i++)
            {
                if (i != 0)
                {
                    Vector3Int pos = c[originGear].partPosition.Add(walkDir1.Multiply(i));
                    for (int j = 0; j < c.Length; j++)
                    {
                        if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear24 && (i == -2 || i == 2) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                        else if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear8 && (i == -1 || i == 1) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                    }
                }
            }
        }






        else if (c[originGear].partType == PartType.PartGear24)
        {
            for (int i = -3; i <= 3; i++)
            {
                if (i != 0 && i != 1 && i != -1)
                {
                    Vector3Int pos = c[originGear].partPosition.Add(walkDir0.Multiply(i));
                    for (int j = 0; j < c.Length; j++)
                    {
                        if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear24 && (i == -3 || i == 3) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                        else if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear8 && (i == -2 || i == 2) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                    }
                }
            }
            for (int i = -3; i <= 3; i++)
            {
                if (i != 0 && i != 1 && i != -1)
                {
                    Vector3Int pos = c[originGear].partPosition.Add(walkDir1.Multiply(i));
                    for (int j = 0; j < c.Length; j++)
                    {
                        if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear24 && (i == -3 || i == 3) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                        else if (pos.Equals(c[j].partPosition) && c[j].partType == PartType.PartGear8 && (i == -2 || i == 2) && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                        {
                            futures.Add(j);
                        }
                    }
                }
            }
        }






        else if (c[originGear].partType == PartType.PartGearCorner4)
        {
            Vector3Int[] posSame = new Vector3Int[4];//c[originGear].partPosition.Add(walkDir0.Multiply(-2));
            posSame[0] = c[originGear].partPosition.Add(walkDir0.Multiply(-2));
            posSame[1] = c[originGear].partPosition.Add(walkDir0.Multiply(2));
            posSame[2] = c[originGear].partPosition.Add(walkDir1.Multiply(-2));
            posSame[3] = c[originGear].partPosition.Add(walkDir1.Multiply(2));

            Vector3Int[] posSide = new Vector3Int[4];//c[originGear].partPosition.Add(walkDir0.Multiply(-2));
            posSide[0] = c[originGear].partPosition.Add(walkDir0.Multiply(-1)).Add(c[originGear].partDirection.ToVector3Int());
            posSide[1] = c[originGear].partPosition.Add(walkDir0.Multiply(-1)).Add(c[originGear].partDirection.Opposite().ToVector3Int());
            posSide[2] = c[originGear].partPosition.Add(walkDir0.Multiply(1)).Add(c[originGear].partDirection.ToVector3Int());
            posSide[3] = c[originGear].partPosition.Add(walkDir0.Multiply(1)).Add(c[originGear].partDirection.Opposite().ToVector3Int());

            Vector3Int[] posTop = new Vector3Int[4];//c[originGear].partPosition.Add(walkDir0.Multiply(-2));
            posTop[0] = c[originGear].partPosition.Add(walkDir1.Multiply(-1)).Add(c[originGear].partDirection.ToVector3Int());
            posTop[1] = c[originGear].partPosition.Add(walkDir1.Multiply(-1)).Add(c[originGear].partDirection.Opposite().ToVector3Int());
            posTop[2] = c[originGear].partPosition.Add(walkDir1.Multiply(1)).Add(c[originGear].partDirection.ToVector3Int());
            posTop[3] = c[originGear].partPosition.Add(walkDir1.Multiply(1)).Add(c[originGear].partDirection.Opposite().ToVector3Int());

            for (int j = 0; j < c.Length; j++)
            {
                for (int k = 0; k < posSame.Length; k++)
                {
                    //if (c[j].partType == PartType.PartGearCorner4)
                   // {
                    //    Debug.Log("Same\nc[j].partDirection=" + c[j].partDirection.ToString() + "\nc[originGear].partDirection=" + c[originGear].partDirection.ToString() + "\nc[j].partPosition=" + c[j].partPosition.ToString() + "\nposSame[k]=" + posSame[k].ToString());
                    //}
                    if (posSame[k].Equals(c[j].partPosition) && c[j].partType == PartType.PartGearCorner4 && (c[j].partDirection == c[originGear].partDirection || c[j].partDirection == c[originGear].partDirection.Opposite()))
                    {
                        futures.Add(j);
                    }
                }
                for (int k = 0; k < posSide.Length; k++)
                {
                    //if (c[j].partType == PartType.PartGearCorner4)
                    //{
                    //    Debug.Log("Side\nc[j].partDirection=" + c[j].partDirection.ToVector3Int().ToString() + "\nwalkDir0=" + walkDir0.ToString() + "\nc[j].partPosition=" + c[j].partDirection.Opposite().ToVector3Int().ToString());
                    //}
                    if (posSide[k].Equals(c[j].partPosition) && c[j].partType == PartType.PartGearCorner4 && (c[j].partDirection.ToVector3Int().Equals(walkDir0) || c[j].partDirection.Opposite().ToVector3Int().Equals(walkDir0)))
                    {
                        futures.Add(j);
                    }
                }
                for (int k = 0; k < posTop.Length; k++)
                {
                    //if (c[j].partType == PartType.PartGearCorner4)
                    //{
                    //    Debug.Log("Top \nc[j].partDirection=" + c[j].partDirection.ToVector3Int().ToString() + "\nwalkDir1=" + walkDir1.ToString() + "\nc[j].partPosition=" + c[j].partDirection.Opposite().ToVector3Int().ToString());
                    //}
                    if (posTop[k].Equals(c[j].partPosition) && c[j].partType == PartType.PartGearCorner4 && (c[j].partDirection.ToVector3Int().Equals(walkDir1) || c[j].partDirection.Opposite().ToVector3Int().Equals(walkDir1)))
                    {
                        futures.Add(j);
                    }
                }

            }
        }


        return futures;
    }

    private static int whichAxeIsHere(Vector3Int position, PartConfiguration[] c)
    {
        for (int i = 0; i < c.Length; i++)
        {
            //Durchsuche alle Achsen, ob diese mit dem Motor verbunden ist
            if (Part.MakePart(c[i].partType).IsAxe)
            {
                int axeLength = Part.MakePart(c[i].partType).AxeLength;
                Vector3Int currPos = c[i].partPosition;
                for (int j = 0; j < axeLength; j++)
                {
                    if (currPos.Equals(position))
                    {
                        return i;
                    }
                    currPos = currPos.Add(c[i].partDirection.ToVector3Int());
                }
            }
        }

        return -1;
    }



    public class FuturePowertrainAxe
    {
        public FuturePowertrainAxe()
        {
            references = new UList<int>();
        }

        public int indexToConfig;
        public float factorToMotor;
        public Vector3 rotateAroundVector;
        public int wheelID;

        public UList<int> references;
    }
    
    public class FutureSteadyPart
    {
        public FutureSteadyPart()
        {
            references = new UList<int>();
        }

        public int indexToConfig;
        public float factorToMotor;
        public Vector3 rotateAroundVector;
        public int wheelID;

        public UList<int> references;
    }

    public class FutureSteerPart
    {
        public FutureSteerPart()
        {

        }

        public int indexToConfig;
        public float factorToMotor;
    }

    public class FutureSteerWheel
    {
        public FutureSteerWheel()
        {

        }

        public int indexToConfig;
        public float factorToMotor;
    }












    public static CalculatedCarD AnalyzeCarD(string carFile)
    {
        PartConfiguration[] configs = FileLoader.LoadFromFile(carFile);

        CalculatedCarD car = new CalculatedCarD();

        if (configs != null)
        {
            Vector3 midOfWheels = Vector3.zero;

            car.parts = new CalculatedPartD[configs.Length];

            for (int i = 0; i < configs.Length; i++)
            {
                car.parts[i] = new CalculatedPartD();
                car.parts[i].subParts = null;
                car.parts[i].partType = configs[i].partType;
                car.parts[i].moving = false;
                car.parts[i].position = configs[i].partPosition.ToVector3();
                car.parts[i].partDirection = configs[i].partDirection;
                car.parts[i].partRotation = configs[i].partRotation;
                car.parts[i].steering = false;

                if (configs[i].partType == PartType.PartWheelStreet1)
                {
                    midOfWheels.x += configs[i].partPosition.x;
                    midOfWheels.y += configs[i].partPosition.y;
                    midOfWheels.z += configs[i].partPosition.z;
                }
            }

            midOfWheels.x *= 0.25f;
            midOfWheels.y *= 0.25f;
            midOfWheels.z *= 0.25f;

            car.cameraPosition = (new Vector3(0, 5f, -20f)) + midOfWheels;
            car.wheelMiddle = midOfWheels;

            for (int i = 0; i < configs.Length; i++)
            {
                if (configs[i].partType == PartType.PartWheelStreet1)
                {
                    if (configs[i].partPosition.x < midOfWheels.x)
                    {
                        if (configs[i].partPosition.z > midOfWheels.z)
                        {
                            car.wheelFL = new CalculatedWheelD();
                            car.wheelFL.position = configs[i].partPosition.ToVector3();
                            car.wheelFL.radius = 1.5f;
                            car.wheelFL.width = 1f;
                        }
                        else
                        {
                            car.wheelRL = new CalculatedWheelD();
                            car.wheelRL.position = configs[i].partPosition.ToVector3();
                            car.wheelRL.radius = 1.5f;
                            car.wheelRL.width = 1f;
                        }
                    }
                    else
                    {
                        if (configs[i].partPosition.z > midOfWheels.z)
                        {
                            car.wheelFR = new CalculatedWheelD();
                            car.wheelFR.position = configs[i].partPosition.ToVector3();
                            car.wheelFR.radius = 1.5f;
                            car.wheelFR.width = 1f;
                        }
                        else
                        {
                            car.wheelRR = new CalculatedWheelD();
                            car.wheelRR.position = configs[i].partPosition.ToVector3();
                            car.wheelRR.radius = 1.5f;
                            car.wheelRR.width = 1f;
                        }
                    }
                }
            }


            car.weight = 10f;
            car.maxSpeed = 30f;
            car.power = 2500f;

            return car;
        }


        return null;
    }


}


public class CalculatedCarD
{
    public CalculatedPartD[] parts;

    public CalculatedWheelD wheelFL;
    public CalculatedWheelD wheelFR;
    public CalculatedWheelD wheelRL;
    public CalculatedWheelD wheelRR;

    public float weight = 1f;
    public float power = 1f;
    public float maxSpeed = 10f;

    public Vector3 wheelMiddle;

    public Vector3 cameraPosition = Vector3.zero;

    public override string ToString()
    {
        return "Amount of parts: " + parts.Length + "\n"
            + "Weight: " + weight + "\n"
            + "Power: " + power + "\n"
            + "Max Speed: " + maxSpeed + "\n"
            + "Camera Position: " + cameraPosition.ToString() + "\n"
            + "Wheel Front Left:  " + wheelFL.ToString() + "\n"
            + "Wheel Front Right: " + wheelFR.ToString() + "\n"
            + "Wheel Rear Left:   " + wheelRL.ToString() + "\n"
            + "Wheel Rear Right:  " + wheelRR.ToString() + "\n";
    }
}

public class CalculatedPartD
{
    public CalculatedPartD[] subParts;

    public PartType partType;

    public bool moving = false;
    public float speedToMotor = 0f;
    public Vector3 axeToMoveAround = Vector3.zero;
    public Vector3 axeMovePosition = Vector3.zero;
    public Vector3 position;
    public PartDirection partDirection;
    public PartRotation partRotation;

    
    public bool steering = false;
    public Vector3 axeToMoveAroundSteering = Vector3.zero;
    public Vector3 axeMovePositionSteering = Vector3.zero;
}

public class CalculatedWheelD
{
    public float radius;
    public Vector3 position;
    public float width;

    public override string ToString()
    {
        return "Position: " + position.ToString() + ", Radius: " + radius + ", Width: " + width;
    }
}