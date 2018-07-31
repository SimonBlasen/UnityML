using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBuildingContainer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject partBuilding;

    List<PartBuilding> parts = new List<PartBuilding>();

    // Use this for initialization
    void Start()
    {
        //AddPart(PartType.Part1x1x5, PartDirection.North, PartRotation.Up, new Vector3Int(0, 0, 0));
        //AddPart(PartType.Part1x1x5, PartDirection.Up, PartRotation.Up, new Vector3Int(1, 0, 1));
        //AddPart(PartType.PartPinRound2x1, PartDirection.East, PartRotation.Up, new Vector3Int(0, 0, 1));
        //AddPart(PartType.PartPinRound2x1, PartDirection.East, PartRotation.Up, new Vector3Int(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Trys to place a part with the given parameters
    /// </summary>
    /// <param name="type">The type of the part to place</param>
    /// <param name="direction">The direction of the part to place</param>
    /// <param name="rotation">The rotation of the part to place</param>
    /// <param name="position">The position, where the part should be placed</param>
    /// <returns>Whether the part has been successfully placed</returns>
    public bool AddPart(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position)
    {
        bool canBePlaced = CanBePlaced(type, direction, rotation, position);

        if (!canBePlaced)
            return false;
        else
        {
            GameObject newPartGameobject = (GameObject)Instantiate(partBuilding);

            parts.Add(newPartGameobject.GetComponent<PartBuilding>());

            parts[parts.Count - 1].SetPosition(position);
            parts[parts.Count - 1].SetDirection(direction);
            parts[parts.Count - 1].SetRotation(rotation);
            parts[parts.Count - 1].SetPart(type);

            connectOverlapingConnectors(parts[parts.Count - 1]);

            Debug.Log("Added part: " + type.ToString() + " at: " + position.ToString());

            return true;
        }

    }

    public List<PartBuilding> Parts
    {
        get
        {
            return parts;
        }
    }

    public bool RemovePart(PartBuilding part)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (part == parts[i])
            {
                disconnectOverlappingConnectors(part);
                Destroy(part.gameObject);
                parts.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public bool HitCollider(Collider hitCollider, ConnectorType type, PartDirection direction, out Vector3Int connectorPosition)
    {
        bool hitting = false;
        connectorPosition = new Vector3Int(0, 0, 0);
        for (int i = 0; i < parts.Count; i++)
        {
            Vector3Int posit;
            hitting = parts[i].MatchConnection(hitCollider, type, direction, out posit);
            if (hitting)
            {
                connectorPosition = posit.Add(parts[i].Position);
                break;
            }
        }

        return hitting;
    }

    public PartBuilding HitPart(Collider hitCollider)
    {
        if (hitCollider != null)
        {
            PartBuilding thePart = null;
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].IsOneOfYourColliders(hitCollider))
                {
                    thePart = parts[i];
                    break;
                }
            }

            return thePart;
        }
        else
        {
            return null;
        }
    }

    public bool CanBePlaced(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position)
    {
        Part part = Part.MakePart(type);
        Connectionpoint[] connectionPoints = part.GetConnectionpoints(direction, rotation);

        bool oneComplication = false;

        for (int i = 0; i < connectionPoints.Length; i++)
        {
            for (int j = 0; j < parts.Count; j++)
            {
                // First check the bounding box of the part

                if (parts[j].BoundingBoxContains(connectionPoints[i].ConnectorPosition.Add(position)))
                {
                    for (int k = 0; k < parts[j].ConnectionPoints.Length; k++)
                    {
                        if (parts[j].ConnectionPoints[k].ConnectorPosition.Add(parts[j].Position).Equals(connectionPoints[i].ConnectorPosition.Add(position)))
                        {
                            if (parts[j].ConnectionPoints[k].ConnectorPosition.Add(parts[j].Position).y == 0 && parts[j].ConnectionPoints[k].ConnectorPosition.Add(parts[j].Position).z == 0)
                            {
                                int debugTest = 0;
                                debugTest++;
                            }

                            bool canOneDirectionBe = false;
                            for (int l = 0; l < parts[j].ConnectionPoints[k].BoxCollidersAmount; l++)
                            {
                                //bool canBe = parts[j].ConnectionPoints[k].CanBeConnected(connectionPoints[i].Connectortype, connectionPoints[i].FacedDirection, l);
                                //if (canBe)
                                //{
                                //    canOneDirectionBe = true;
                                //    break;
                                //}

                                for (int m = 0; m < connectionPoints[i].BoxCollidersAmount; m++)
                                {
                                    bool canBe = parts[j].ConnectionPoints[k].CanBeConnected(connectionPoints[i].Connectortype, connectionPoints[i].GetBoxColliderDirection(m), l);
                                    if (canBe)
                                    {
                                        canOneDirectionBe = true;
                                        break;
                                    }
                                }
                            }

                            if (!canOneDirectionBe)
                            {
                                oneComplication = true;
                                break;
                            }
                        }
                    }
                }

                // End of checking bounding box


                if (oneComplication)
                {
                    break;
                }
            }

            if (oneComplication)
            {
                break;
            }
        }

        return !oneComplication;
    }

    private void disconnectOverlappingConnectors(PartBuilding removedPart)
    {
        for (int i = 0; i < removedPart.ConnectionPoints.Length; i++)
        {
            for (int j = 0; j < parts.Count; j++)
            {
                // First check the bounding box of the part

                if (parts[j].BoundingBoxContains(removedPart.ConnectionPoints[i].ConnectorPosition.Add(removedPart.Position)))
                {
                    for (int k = 0; k < parts[j].ConnectionPoints.Length; k++)
                    {
                        if (parts[j].ConnectionPoints[k].ConnectorPosition.Add(parts[j].Position).Equals(removedPart.ConnectionPoints[i].ConnectorPosition.Add(removedPart.Position)))
                        {
                            parts[j].ConnectionPoints[k].IsConnected = false;
                            removedPart.ConnectionPoints[i].IsConnected = false;
                        }
                    }
                }
            }
        }
    }

    private void connectOverlapingConnectors(PartBuilding placedPart)
    {
        bool oneComplication = false;

        for (int i = 0; i < placedPart.ConnectionPoints.Length; i++)
        {
            for (int j = 0; j < parts.Count; j++)
            {
                // First check the bounding box of the part

                if (parts[j] != placedPart && parts[j].BoundingBoxContains(placedPart.ConnectionPoints[i].ConnectorPosition.Add(placedPart.Position)))
                {
                    for (int k = 0; k < parts[j].ConnectionPoints.Length; k++)
                    {
                        if (parts[j].ConnectionPoints[k].ConnectorPosition.Add(parts[j].Position).Equals(placedPart.ConnectionPoints[i].ConnectorPosition.Add(placedPart.Position)))
                        {
                            bool canOneDirectionBe = false;
                            for (int l = 0; l < parts[j].ConnectionPoints[k].BoxCollidersAmount; l++)
                            {
                                bool canBe = parts[j].ConnectionPoints[k].CanBeConnected(placedPart.ConnectionPoints[i].Connectortype, placedPart.ConnectionPoints[i].FacedDirection, l);
                                if (canBe)
                                {
                                    canOneDirectionBe = true;
                                    break;
                                }
                            }

                            if (canOneDirectionBe)
                            {
                                parts[j].ConnectionPoints[k].IsConnected = true;
                                placedPart.ConnectionPoints[i].IsConnected = true;
                            }

                        }
                    }

                    if (oneComplication)
                    {
                        break;
                    }
                }
                
            }

            if (oneComplication)
            {
                break;
            }
        }
    }
}
