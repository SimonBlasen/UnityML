using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedCar
{
    private CalculatedPart seat;
    private List<CalculatedPart> staticParts;
    private List<CalculatedWheel> wheels;
    private List<CalculatedWheelAxe> wheelAxes;
    private List<CalculatedPowertrainAxe> powertrainAxes;
    private List<CalculatedSteerPart> steerParts;
    private List<CalculatedSteerBar> steerBars;
    private List<CalculatedSteerAxe> steerAxes;
    private List<CalculatedMinigun> miniguns;

    private int idCounter = 0;
    private Vector3 seatOffset = Vector3.zero;

    private float topSpeed = 50;    // in km/h
    private float fullTorque = 60;  // in KW
    private float tireGrip = 0f;    // Between 0 and 1
    private float weight = 1000f;   // in kilo

    private BoundingBox completeBoundingBox;

    public CalculatedCar()
    {
        completeBoundingBox = new BoundingBox();

        staticParts = new List<CalculatedPart>();
        wheels = new List<CalculatedWheel>();
        wheelAxes = new List<CalculatedWheelAxe>();
        powertrainAxes = new List<CalculatedPowertrainAxe>();
        steerParts = new List<CalculatedSteerPart>();
        steerBars = new List<CalculatedSteerBar>();
        steerAxes = new List<CalculatedSteerAxe>();
        miniguns = new List<CalculatedMinigun>();

        seatOffset = Vector3.zero;
        idCounter = 0;
    }

    public float Weight
    {
        get
        {
            return weight;
        }
        set
        {
            weight = value;
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].SpringStrength = 35000 * (weight * 0.001f) * 0.6f;
                wheels[i].DamperStrength = 4500 * (weight * 0.001f) * 0.6f;
            }
        }
    }

    public Vector3 SeatOffset
    {
        get
        {
            return seatOffset;
        }
    }

    public float Grip
    {
        get
        {
            return tireGrip;
        }
        set
        {
            tireGrip = value;
        }
    }

    public float TopSpeed
    {
        get
        {
            return topSpeed;
        }
        set
        {
            topSpeed = value;
        }
    }

    public float FullTorque
    {
        get
        {
            return fullTorque;
        }
        set
        {
            fullTorque = value;
        }
    }

    public float Health
    {
        get
        {
            float tempHealth = 0f;
            for (int i = 0; i < staticParts.Count; i++)
            {
                tempHealth += staticParts[i].Health;
            }

            return tempHealth;
        }
    }

    public int AddSeat(Vector3 position, PartDirection direction, PartRotation rotation)
    {
        return AddSeat(position, direction, rotation, -1f);
    }

    public int AddSeat(Vector3 position, PartDirection direction, PartRotation rotation, float health)
    {
        seatOffset = -1 * position;

        seat = new CalculatedPart();
        seat.Position = position + seatOffset;
        seat.Rotation = rotation;
        seat.Direction = direction;
        seat.Type = PartType.PartSeat;
        if (health != -1)
        {
            seat.Health = health;
        }

        seat.ID = idCounter;
        idCounter++;

        return seat.ID;
    }

    public int AddPart(PartType type, Vector3 position, PartDirection direction, PartRotation rotation)
    {
        return AddPart(type, position, direction, rotation, -1f);
    }

    public int AddPart(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, float health)
    {
        CalculatedPart newPart = new CalculatedPart();
        newPart.Position = position + seatOffset;
        newPart.Direction = direction;
        newPart.Rotation = rotation;
        newPart.Type = type;
        if (health != -1)
        {
            newPart.Health = health;
        }

        newPart.ID = idCounter;
        idCounter++;

        staticParts.Add(newPart);

        return newPart.ID;
    }

    public int AddPart(PartConfiguration configuration)
    {
        return AddPart(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation);
    }

    public int AddPart(PartConfiguration configuration, float health)
    {
        return AddPart(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, health);
    }

    public int AddWheel(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, bool powered, bool steered, float radius, float grip, float maxSteerAngle)
    {
        return AddWheel(type, position, direction, rotation, powered, steered, radius, grip, maxSteerAngle, -1f);
    }

    public int AddWheel(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, bool powered, bool steered, float radius, float grip, float maxSteerAngle, float health)
    {
        CalculatedWheel newWheel = new CalculatedWheel();
        newWheel.Position = position + seatOffset;
        newWheel.Direction = direction;
        newWheel.Rotation = rotation;
        newWheel.Powering = powered;
        newWheel.Steering = steered;
        newWheel.Type = type;
        newWheel.Radius = radius;
        newWheel.Grip = grip;
        newWheel.SteerAngle = maxSteerAngle >= 0 ? maxSteerAngle : -1 * maxSteerAngle;
        newWheel.SteerCorrectDirection = maxSteerAngle >= 0;

        newWheel.DamperStrength = 4500 * (Weight * 0.001f) * 0.6f;
        newWheel.SpringStrength = 35000 * (Weight * 0.001f) * 0.6f;
        if (health != -1)
        {
            newWheel.Health = health;
        }

        newWheel.ID = idCounter;
        idCounter++;

        wheels.Add(newWheel);

        return newWheel.ID;
    }

    public int AddWheel(PartConfiguration configuration, bool powered, bool steered, float radius, float grip, float maxSteerAngle)
    {
        return AddWheel(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, powered, steered, radius, grip, maxSteerAngle);
    }

    public int AddWheel(PartConfiguration configuration, bool powered, bool steered, float radius, float grip, float maxSteerAngle, float health)
    {
        return AddWheel(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, powered, steered, radius, grip, maxSteerAngle, health);
    }

    public int AddPowertrainAxe(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, int accordingWheelID, float factorToWheel, Vector3 rotateAroundAxe)
    {
        return AddPowertrainAxe(type, position, direction, rotation, accordingWheelID, factorToWheel, rotateAroundAxe, -1f);
    }

    public int AddPowertrainAxe(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, int accordingWheelID, float factorToWheel, Vector3 rotateAroundAxe, float health)
    {
        CalculatedPowertrainAxe newPtAxe = new CalculatedPowertrainAxe();
        newPtAxe.Position = position + seatOffset;
        newPtAxe.Direction = direction;
        newPtAxe.Rotation = rotation;
        newPtAxe.AccordingWheelID = accordingWheelID;
        newPtAxe.FactorToWheel = factorToWheel;
        newPtAxe.RotateAroundAxe = rotateAroundAxe;
        newPtAxe.Type = type;
        if (health != -1)
        {
            newPtAxe.Health = health;
        }

        newPtAxe.ID = idCounter;
        idCounter++;

        powertrainAxes.Add(newPtAxe);

        return newPtAxe.ID;
    }

    public int AddPowertrainAxe(PartConfiguration configuration, int accordingWheelID, float factorToWheel, Vector3 rotateAroundAxe)
    {
        return AddPowertrainAxe(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, accordingWheelID, factorToWheel, rotateAroundAxe);
    }

    public int AddPowertrainAxe(PartConfiguration configuration, int accordingWheelID, float factorToWheel, Vector3 rotateAroundAxe, float health)
    {
        return AddPowertrainAxe(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, accordingWheelID, factorToWheel, rotateAroundAxe, health);
    }

    public int AddSteerPart(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, bool invertedDirection)
    {
        return AddSteerPart(type, position, direction, rotation, invertedDirection, -1f);
    }

    public int AddSteerPart(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, bool invertedDirection, float health)
    {
        CalculatedSteerPart newSteerPart = new CalculatedSteerPart();

        newSteerPart.Position = position + seatOffset;
        newSteerPart.Direction = direction;
        newSteerPart.Rotation = rotation;
        newSteerPart.Type = type;

        newSteerPart.InvertedDirection = invertedDirection;
        if (health != -1)
        {
            newSteerPart.Health = health;
        }

        newSteerPart.ID = idCounter;
        idCounter++;

        steerParts.Add(newSteerPart);

        return newSteerPart.ID;
    }

    public int AddSteerPart(PartConfiguration configuration, bool invertedDirection)
    {
        return AddSteerPart(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, invertedDirection);
    }

    public int AddSteerPart(PartConfiguration configuration, bool invertedDirection, float health)
    {
        return AddSteerPart(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, invertedDirection, health);
    }

    public int AddSteerBar(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, Vector3 moveAlongAxe, float halfwayDistance)
    {
        return AddSteerBar(type, position, direction, rotation, moveAlongAxe, halfwayDistance, -1f);
    }

    public int AddSteerBar(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, Vector3 moveAlongAxe, float halfwayDistance, float health)
    {
        CalculatedSteerBar newSteerBar = new CalculatedSteerBar();

        newSteerBar.Position = position + seatOffset;
        newSteerBar.Direction = direction;
        newSteerBar.Rotation = rotation;
        newSteerBar.Type = type;

        newSteerBar.AxeMoveAlong = moveAlongAxe;
        newSteerBar.HalfwayDistance = halfwayDistance;
        if (health != -1)
        {
            newSteerBar.Health = health;
        }

        newSteerBar.ID = idCounter;
        idCounter++;

        steerBars.Add(newSteerBar);

        return newSteerBar.ID;
    }

    public int AddSteerBar(PartConfiguration configuration, Vector3 moveAlongAxe, float halfwayDistance)
    {
        return AddSteerBar(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, moveAlongAxe, halfwayDistance);
    }

    public int AddSteerBar(PartConfiguration configuration, Vector3 moveAlongAxe, float halfwayDistance, float health)
    {
        return AddSteerBar(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, moveAlongAxe, halfwayDistance, health);
    }

    public int AddMinigun(PartType type, Vector3 position, PartDirection direction, PartRotation rotation, float health)
    {
        CalculatedMinigun newMinigun = new CalculatedMinigun();

        newMinigun.Position = position + seatOffset;
        newMinigun.Direction = direction;
        newMinigun.Rotation = rotation;
        newMinigun.Type = type;

        if (health != -1)
        {
            newMinigun.Health = health;
        }

        newMinigun.ID = idCounter;
        idCounter++;

        miniguns.Add(newMinigun);

        return newMinigun.ID;
    }

    public int AddMinigun(PartConfiguration configuration)
    {
        return AddMinigun(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, -1f);
    }

    public int AddMinigun(PartConfiguration configuration, float health)
    {
        return AddMinigun(configuration.partType, configuration.partPosition.ToVector3(), configuration.partDirection, configuration.partRotation, health);
    }

    public CalculatedPart PartSeat
    {
        get
        {
            return seat;
        }
    }

    public CalculatedSteerBar[] PartSteerBars
    {
        get
        {
            return steerBars.ToArray();
        }
    }

    public CalculatedSteerPart[] PartSteerParts
    {
        get
        {
            return steerParts.ToArray();
        }
    }

    public CalculatedPart[] PartStatics
    {
        get
        {
            return staticParts.ToArray();
        }
    }

    public CalculatedWheel[] PartWheels
    {
        get
        {
            return wheels.ToArray();
        }
    }

    public CalculatedPowertrainAxe[] PartPowertrainAxes
    {
        get
        {
            return powertrainAxes.ToArray();
        }
    }

    public CalculatedMinigun[] PartMiniguns
    {
        get
        {
            return miniguns.ToArray();
        }
    }

    public override string ToString()
    {
        string minigunsString = "";
        if (miniguns.Count > 0)
        {
            minigunsString = miniguns.Count + "x Minigun\n";
        }

        return "Calculated Car" + "\n"
            + "Amount of parts: " + (staticParts.Count + wheelAxes.Count + wheelAxes.Count + powertrainAxes.Count + steerAxes.Count + steerBars.Count + steerParts.Count) + "\n"
            + "Top Speed: " + topSpeed + "\n"
            + "Power: " + fullTorque + "\n"
            + "Grip: " + tireGrip + "\n"
            + "Weight: " + weight + "\n"
            + "Weapons: " + "\n"
            + minigunsString;
    }
}
