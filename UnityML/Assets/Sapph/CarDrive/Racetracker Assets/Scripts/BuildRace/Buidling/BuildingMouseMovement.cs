using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartConfiguration
{
    public PartDirection partDirection;
    public PartRotation partRotation;
    public Vector3Int partPosition;
    public PartType partType;

    public bool marked = false;

    public PartConfiguration(PartDirection direction, PartRotation rotation, Vector3Int position)
    {
        partDirection = direction;
        partRotation = rotation;
        partPosition = position;
        partType = PartType.NONE;
    }

    public PartConfiguration(PartDirection direction, PartRotation rotation, Vector3Int position, PartType type)
    {
        partDirection = direction;
        partRotation = rotation;
        partPosition = position;
        partType = type;
    }

    public PartConfiguration()
    {
        partDirection = PartDirection.North;
        partRotation = PartRotation.Up;
        partPosition = new Vector3Int(0, 0, 0);
        partType = PartType.NONE;
    }

    public override bool Equals(object obj)
    {
        if (obj is PartConfiguration)
        {
            PartConfiguration other = (PartConfiguration)obj;
            return other.partDirection == partDirection && other.partRotation == partRotation && other.partPosition == partPosition && other.partType == partType;
        }
        else
        {
            return base.Equals(obj);
        }
    }

    public static bool operator ==(PartConfiguration a, PartConfiguration b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(PartConfiguration a, PartConfiguration b)
    {
        return !a.Equals(b);
    }

    public override string ToString()
    {
        return partType.ToString() + "_" + partPosition.ToString();
    }
}

public enum CursorMode
{
    PLACING, IDLE
}

public class BuildingMouseMovement : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private PartBuildingContainer partBuildingContainer;//This must be a list, contained by one container, who manages all built parts
    [SerializeField]
    private GameObject debugCube;
    [SerializeField]
    private Camera activeCamera;
    [SerializeField]
    private GameObject partView;
    [SerializeField]
    private GameObject lineRendererSelected;
    [SerializeField]
    private CameraMovementEditor cameraMovementEditor;
    [SerializeField]
    private Canvas canvasCrosshair;
    [SerializeField]
    private ManagerBuilding manager;



    private int connectorIndex = 0;
    private bool forceUpdate = false;

    private Collider lastCollider = null;
    private bool canSomethingBeConnected = false;
    private Vector3Int connectorPosition = new Vector3Int();
    private Vector3Int offsetedPartPosition = new Vector3Int();

    private List<PartConfiguration> configurations = new List<PartConfiguration>();
    private int configurationIndex = 0;

    private CursorMode cursorMode = CursorMode.IDLE;

    // IDLE things

    private PartBuilding selectedPart = null;
    private PartBuilding hoveredPart = null;


    public bool enableDebugMessages = false;
    //

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Connector") && guiOpened == false)
        {
            if (cursorMode == CursorMode.PLACING && (lastCollider != hit.collider || forceUpdate) && partView.GetComponent<PartView>().IsNotEmpty)
            {
                //Vector3 hitpoint = hit.point;
                //hitpoint.x = (int)(hitpoint.x - 0.5f);
                //hitpoint.y = (int)(hitpoint.y + 1);
                //hitpoint.z = (int)(hitpoint.z + 0.5f);
                //Vector3 wayToOrigin = new Vector3(1f, 0f, 0f);
                //PartDirection originDirection = PartDirection.West;
                //PartRotation originRotation = PartRotation.Up;

                forceUpdate = false;

                lastCollider = hit.collider;

                fillAllPossibleConfigurations(hit.collider);
                
                applyCurrentConfiguration();

                //Debug.Log("Amount: " + configurations.Count);

                //ConnectorType connectorType = partView.GetComponent<PartView>().ConnectionPoints[connectorIndex].Connectortype;
                //PartDirection connectorDirection = partView.GetComponent<PartView>().ConnectionPoints[connectorIndex].FacedDirection;
                //PartType partType = partView.GetComponent<PartView>().Type;
                //PartDirection partDirection = partView.GetComponent<PartView>().Direction;
                //PartRotation partRotation = partView.GetComponent<PartView>().Rotation;
                //
                ////Vector3Int offsetedPartPosition = partView.GetComponent<PartView>().Position.Sub(partView.GetComponent<PartView>().ConnectionPoints[connectorIndex].ConnectorPosition);
                //
                //
                //bool canMatch = partBuildingContainer.HitCollider(hit.collider, connectorType, connectorDirection, out connectorPosition);
                //
                //if (canMatch)
                //{
                //    offsetedPartPosition = connectorPosition.Sub(partView.GetComponent<PartView>().ConnectionPoints[connectorIndex].ConnectorPosition);
                //
                //    canSomethingBeConnected = partBuildingContainer.CanBePlaced(partType, partDirection, partRotation, offsetedPartPosition);
                //
                //    if (canSomethingBeConnected)
                //    {
                //        partView.GetComponent<PartView>().Visible = true;
                //        //debugCube.transform.position = connectorPosition.ToVector3();
                //        partView.GetComponent<PartView>().SetPosition(offsetedPartPosition);
                //        Debug.Log("Can match");
                //    }
                //    else
                //    {
                //        canSomethingBeConnected = false;
                //        partView.GetComponent<PartView>().Visible = false;
                //        //debugCube.transform.position = new Vector3(0f, -5f, 0f);
                //        //Debug.Log("Can NOT match [1]");
                //    }
                //}
                //else
                //{
                //    canSomethingBeConnected = false;
                //    partView.GetComponent<PartView>().Visible = false;
                //    //debugCube.transform.position = new Vector3(0f, -5f, 0f);
                //    //Debug.Log("Can NOT match [0]");
                //}
            }

            if (cursorMode == CursorMode.IDLE && lastCollider != hit.collider)
            {
                lastCollider = hit.collider;

                hoverPart(false);
            }
        }
        else
        {
            if (cursorMode == CursorMode.IDLE)
            {
                hoverPart(true);
            }
        }

        if (Input.GetMouseButtonDown(0) && guiOpened == false) //&& canSomethingBeConnected
        {
            switch (cursorMode)
            {
                case CursorMode.PLACING:
                    if (configurations.Count > 0)
                    {
                        PartType partType = partView.GetComponent<PartView>().Type;
                        PartDirection partDirection = partView.GetComponent<PartView>().Direction;
                        PartRotation partRotation = partView.GetComponent<PartView>().Rotation;

                        partBuildingContainer.AddPart(partType, partDirection, partRotation, partView.GetComponent<PartView>().Position);

                        manager.ReducePartAmount(partType);

                        Debug.Log("Placed a part at: " + partView.GetComponent<PartView>().Position.ToString());
                    }
                    break;
                case CursorMode.IDLE:
                    selectHoveredPart();
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1) && guiOpened == false)
        {

            switch (cursorMode)
            {
                case CursorMode.PLACING:
                    if (configurations.Count > 0)
                    {
                        configurationIndex++;
                        applyCurrentConfiguration();
                        //PartType partType = partView.GetComponent<PartView>().Type;
                        //PartDirection partDirection = partView.GetComponent<PartView>().Direction;
                        //PartRotation partRotation = partView.GetComponent<PartView>().Rotation;
                        //
                        //partBuildingContainer.AddPart(partType, partDirection, partRotation, offsetedPartPosition);
                        //
                        //Debug.Log("Placed a part at: " + offsetedPartPosition.ToString());
                    }
                    break;
                case CursorMode.IDLE:
                    
                    break;
            }
        }

        if (Input.GetMouseButtonDown(2) && guiOpened == false)
        {
            PartType type = getHoveredPartType();
            if (type != PartType.NONE)
            {
                SelectPart(type);
                SwitchToMode(CursorMode.PLACING);
            }

            switch (cursorMode)
            {
                case CursorMode.PLACING:
                    break;
                case CursorMode.IDLE:
                    break;
            }

        }

        //if (Input.GetButton("Key1") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x3);
        //}
        //else if (Input.GetButton("Key2") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x5);
        //}
        //else if (Input.GetButton("Key3") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x7);
        //}
        //else if (Input.GetButton("Key4") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x9);
        //}
        //else if (Input.GetButton("Key5") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x11);
        //}
        //else if (Input.GetButton("Key6") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x13);
        //}
        //else if (Input.GetButton("Key7") && guiOpened == false)
        //{
        //    SelectPart(PartType.Part1x1x15);
        //}
        //else if (Input.GetButton("Key8") && guiOpened == false)
        //{
        //    SelectPart(PartType.PartPinRound2x1);
        //}
        //else if (Input.GetButton("Key9") && guiOpened == false)
        //{
        //    SelectPart(PartType.PartTurn1x1x2);
        //}
        //else if (Input.GetButton("Key0") && guiOpened == false)
        //{
        //    SelectPart(PartType.PartPinRound3x1);
        //}




        else if (Input.GetKey(KeyCode.B) && guiOpened == false)
        {
            SwitchToMode(CursorMode.PLACING);
        }
        else if (Input.GetKey(KeyCode.V) && guiOpened == false)
        {
            SwitchToMode(CursorMode.IDLE);
        }

        if (Input.GetKey(KeyCode.Delete) && guiOpened == false)
        {
            if (selectedPart != null)
            {
                PartType removedType = selectedPart.Type;
                bool success = partBuildingContainer.RemovePart(selectedPart);

                if (success)
                {
                    manager.IncreasePartAmount(removedType);
                }

                Debug.Log("Tried to remove part: " + success.ToString());
                selectedPart = null;
                hoveredPart = null;
                selectHoveredPart();
            }
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0 && guiOpened == false)
        {
            SelectConnector(connectorIndex + 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && guiOpened == false)
        {
            SelectConnector(connectorIndex - 1);
        }

        //if (Input.GetButton("Part1"))
        //{
        //    SelectPart(PartType.Part1x1x5);
        //}
        //if (Input.GetButton("Part2"))
        //{
        //    SelectPart(PartType.PartPinRound2x1);
        //}

        if (Input.GetKey(KeyCode.Escape))
        {
            //SceneManager.LoadScene("Garages");
            manager.ButtonBackToGarageClick();
        }
    }

    public void SwitchToMode(CursorMode mode)
    {
        switch (mode)
        {
            case CursorMode.IDLE:
                partView.GetComponent<PartView>().Visible = false;
                cursorMode = CursorMode.IDLE;
                break;
            case CursorMode.PLACING:
                hoveredPart = null;
                selectedPart = null;
                hoverPart(true);
                selectHoveredPart();
                cursorMode = CursorMode.PLACING;
                break;
        }
    }

    private PartType getHoveredPartType()
    {
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Connector"))
        {
            PartBuilding hitPart = partBuildingContainer.HitPart(hit.collider);

            if (hitPart != null)
            {
                return hitPart.Type;
            }
        }
        return PartType.NONE;
    }

    private void selectHoveredPart()
    {
        if (hoveredPart != null)
        {
            selectedPart = hoveredPart;
            Vector3 minV = selectedPart.BoundingBox.MinValues.ToVector3() + new Vector3(-0.02f, -0.02f, -0.02f);
            Vector3 maxV = selectedPart.BoundingBox.MaxValues.ToVector3() + new Vector3(1.02f, 1.02f, 1.02f);

            Vector3[] boundingBox = new Vector3[16];
            boundingBox[0] = new Vector3(minV.x, minV.y, minV.z);
            boundingBox[1] = new Vector3(minV.x, minV.y, maxV.z);
            boundingBox[2] = new Vector3(minV.x, maxV.y, maxV.z);
            boundingBox[3] = new Vector3(minV.x, maxV.y, minV.z);
            boundingBox[4] = new Vector3(minV.x, minV.y, minV.z);
            boundingBox[5] = new Vector3(maxV.x, minV.y, minV.z);
            boundingBox[6] = new Vector3(maxV.x, maxV.y, minV.z);
            boundingBox[7] = new Vector3(maxV.x, maxV.y, maxV.z);
            boundingBox[8] = new Vector3(maxV.x, minV.y, maxV.z);
            boundingBox[9] = new Vector3(minV.x, minV.y, maxV.z);
            boundingBox[10] = new Vector3(minV.x, maxV.y, maxV.z);
            boundingBox[11] = new Vector3(maxV.x, maxV.y, maxV.z);
            boundingBox[12] = new Vector3(maxV.x, minV.y, maxV.z);
            boundingBox[13] = new Vector3(maxV.x, minV.y, minV.z);
            boundingBox[14] = new Vector3(maxV.x, maxV.y, minV.z);
            boundingBox[15] = new Vector3(minV.x, maxV.y, minV.z);

            for (int i = 0; i < boundingBox.Length; i++)
            {
                boundingBox[i] = boundingBox[i] + selectedPart.Position.ToVector3() - new Vector3(0.5f, 0.5f, 0.5f);
            }

            LineRenderer lines = lineRendererSelected.GetComponent<LineRenderer>();
            lines.SetPositions(boundingBox);

            // Remove hovered-box
            Vector3[] boundingBox2 = new Vector3[16];
            for (int i = 0; i < boundingBox2.Length; i++)
            {
                boundingBox2[i] = new Vector3(0f, 0f, 0f);
            }
            LineRenderer lines2 = GetComponent<LineRenderer>();
            lines2.SetPositions(boundingBox2);
        }
        else
        {
            selectedPart = null;
            Vector3[] boundingBox = new Vector3[16];
            for (int i = 0; i < boundingBox.Length; i++)
            {
                boundingBox[i] = new Vector3(0f, 0f, 0f);
            }
            LineRenderer lines = lineRendererSelected.GetComponent<LineRenderer>();
            lines.SetPositions(boundingBox);
        }
    }

    private void hoverPart(bool deHover)
    {
        hoveredPart = partBuildingContainer.HitPart(lastCollider);

        if (hoveredPart != null && deHover == false && hoveredPart != selectedPart)
        {
            Vector3 minV = hoveredPart.BoundingBox.MinValues.ToVector3() + new Vector3(-0.02f, -0.02f, -0.02f);
            Vector3 maxV = hoveredPart.BoundingBox.MaxValues.ToVector3() + new Vector3(1.02f, 1.02f, 1.02f);

            Vector3[] boundingBox = new Vector3[16];
            boundingBox[0] = new Vector3(minV.x, minV.y, minV.z);
            boundingBox[1] = new Vector3(minV.x, minV.y, maxV.z);
            boundingBox[2] = new Vector3(minV.x, maxV.y, maxV.z);
            boundingBox[3] = new Vector3(minV.x, maxV.y, minV.z);
            boundingBox[4] = new Vector3(minV.x, minV.y, minV.z);
            boundingBox[5] = new Vector3(maxV.x, minV.y, minV.z);
            boundingBox[6] = new Vector3(maxV.x, maxV.y, minV.z);
            boundingBox[7] = new Vector3(maxV.x, maxV.y, maxV.z);
            boundingBox[8] = new Vector3(maxV.x, minV.y, maxV.z);
            boundingBox[9] = new Vector3(minV.x, minV.y, maxV.z);
            boundingBox[10] = new Vector3(minV.x, maxV.y, maxV.z);
            boundingBox[11] = new Vector3(maxV.x, maxV.y, maxV.z);
            boundingBox[12] = new Vector3(maxV.x, minV.y, maxV.z);
            boundingBox[13] = new Vector3(maxV.x, minV.y, minV.z);
            boundingBox[14] = new Vector3(maxV.x, maxV.y, minV.z);
            boundingBox[15] = new Vector3(minV.x, maxV.y, minV.z);

            for (int i = 0; i < boundingBox.Length; i++)
            {
                boundingBox[i] = boundingBox[i] + hoveredPart.Position.ToVector3() - new Vector3(0.5f, 0.5f, 0.5f);
            }

            LineRenderer lines = GetComponent<LineRenderer>();
            lines.SetPositions(boundingBox);
        }
        else
        {
            hoveredPart = null;
            Vector3[] boundingBox = new Vector3[16];
            for (int i = 0; i < boundingBox.Length; i++)
            {
                boundingBox[i] = new Vector3(0f, 0f, 0f);
            }
            LineRenderer lines = GetComponent<LineRenderer>();
            lines.SetPositions(boundingBox);
        }
    }

    private void applyCurrentConfiguration()
    {
        if (configurations.Count > 0)
        {
            //Debug.Log("Aufruf[0]: " + Time.time);
            configurationIndex = configurationIndex % configurations.Count;

            partView.GetComponent<PartView>().Visible = false;
            partView.GetComponent<PartView>().SetDirection(configurations[configurationIndex].partDirection);
            partView.GetComponent<PartView>().SetRotation(configurations[configurationIndex].partRotation);

            //Debug.Log("Aufruf[1]: " + Time.time);
            partView.GetComponent<PartView>().SetPosition(configurations[configurationIndex].partPosition);

            partView.GetComponent<PartView>().Visible = true;
            //Debug.Log("Aufruf[2]: " + Time.time);
        }
    }

    private void fillAllPossibleConfigurations(Collider coll)
    {
        configurations.Clear();

        partView.GetComponent<PartView>().Visible = false;

        // Alle möglichkeiten durch machen, dann wieder visible, und die Liste vorher mit den möglichkeiten füllen, um mit rechtsklick durch zu iterieren

        for (int i = 0; i < 4; i++)
        {
            PartRotation rot = PartRotation.Up;
            if (i == 1)
                rot = PartRotation.Down;
            else if (i == 2)
                rot = PartRotation.Right;
            else if (i == 3)
                rot = PartRotation.Left;


            partView.GetComponent<PartView>().SetDirection(PartDirection.Down);
            partView.GetComponent<PartView>().SetRotation(rot);
            bool can = couldPartPePlaced(coll);
            if (can)
            {
                configurations.Add(new PartConfiguration(PartDirection.Down, rot, offsetedPartPosition));
            }

            partView.GetComponent<PartView>().SetDirection(PartDirection.Up);
            partView.GetComponent<PartView>().SetRotation(rot);
            can = couldPartPePlaced(coll);
            if (can)
            {
                configurations.Add(new PartConfiguration(PartDirection.Up, rot, offsetedPartPosition));
            }

            partView.GetComponent<PartView>().SetDirection(PartDirection.East);
            partView.GetComponent<PartView>().SetRotation(rot);
            can = couldPartPePlaced(coll);
            if (can)
            {
                configurations.Add(new PartConfiguration(PartDirection.East, rot, offsetedPartPosition));
            }

            partView.GetComponent<PartView>().SetDirection(PartDirection.West);
            partView.GetComponent<PartView>().SetRotation(rot);
            can = couldPartPePlaced(coll);
            if (can)
            {
                configurations.Add(new PartConfiguration(PartDirection.West, rot, offsetedPartPosition));
            }

            partView.GetComponent<PartView>().SetDirection(PartDirection.South);
            partView.GetComponent<PartView>().SetRotation(rot);
            can = couldPartPePlaced(coll);
            if (can)
            {
                configurations.Add(new PartConfiguration(PartDirection.South, rot, offsetedPartPosition));
            }

            partView.GetComponent<PartView>().SetDirection(PartDirection.North);
            partView.GetComponent<PartView>().SetRotation(rot);
            can = couldPartPePlaced(coll);
            if (can)
            {
                configurations.Add(new PartConfiguration(PartDirection.North, rot, offsetedPartPosition));
            }
        }

        //Debug.Log("Liste hat: " + configurations.Count);
    }

    private bool couldPartPePlaced(Collider coll)
    {
        ConnectorType connectorType = partView.GetComponent<PartView>().ConnectionPointsNotSolid[connectorIndex].Connectortype;
        //PartDirection connectorDirection = partView.GetComponent<PartView>().ConnectionPoints[connectorIndex].FacedDirection;
        PartDirection[] connectorDirections = partView.GetComponent<PartView>().ConnectionPointsNotSolid[connectorIndex].AcceptedDirections;
        PartType partType = partView.GetComponent<PartView>().Type;
        PartDirection partDirection = partView.GetComponent<PartView>().Direction;
        PartRotation partRotation = partView.GetComponent<PartView>().Rotation;

        if (enableDebugMessages) Debug.Log("Try to connect: " + connectorType.ToString());

        if (partDirection == PartDirection.South && partRotation == PartRotation.Down && partType == PartType.PartCorner2x4)
        {
            int sdf = 0;
            sdf++;
        }

        if (partType == PartType.NONE)
            return false;

        bool canOneMatch = false;
        for (int i = 0; i < connectorDirections.Length; i++)
        {
            canOneMatch = partBuildingContainer.HitCollider(coll, connectorType, connectorDirections[i], out connectorPosition);
            if (canOneMatch)
            {
                break;
            }
        }

        if (enableDebugMessages) Debug.Log("canOneMatch is: " + canOneMatch.ToString());

        //bool canMatch = partBuildingContainer.HitCollider(coll, connectorType, connectorDirection, out connectorPosition);

        if (canOneMatch)
        {
            offsetedPartPosition = connectorPosition.Sub(partView.GetComponent<PartView>().ConnectionPointsNotSolid[connectorIndex].ConnectorPosition);

            if (partType == PartType.PartGearCorner4)
            {
                int sdf = 0;
                sdf++;
            }

            canSomethingBeConnected = partBuildingContainer.CanBePlaced(partType, partDirection, partRotation, offsetedPartPosition);
            

            if (canSomethingBeConnected)
            {
                return true;
            }
        }

        return false;
    }

    private bool guiOpened = false;

    public bool GUIOpened
    {
        get
        {
            return guiOpened;
        }
        set
        {
            guiOpened = value;
            if (guiOpened)
            {
                cameraMovementEditor.enabled = false;
                canvasCrosshair.enabled = false;
            }
            else
            {
                cameraMovementEditor.enabled = true;
                canvasCrosshair.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }



    public void DeselectPart()
    {
        partView.GetComponent<PartView>().Visible = false;
    }

    public void SelectPart(PartType type)
    {
        Debug.Log("Part selected");
        partView.GetComponent<PartView>().SetPart(type);
        SelectConnector(connectorIndex);
        forceUpdate = true;
    }

    public void SelectDirection(PartDirection direction)
    {
        partView.GetComponent<PartView>().SetDirection(direction);
        forceUpdate = true;
    }

    public void SelectRotation(PartRotation rotation)
    {
        partView.GetComponent<PartView>().SetRotation(rotation);
        forceUpdate = true;
    }

    public PartBuildingContainer PartBuildingContainer
    {
        get
        {
            return partBuildingContainer;
        }
    }

    public void SelectConnector(int index)
    {
        int oldValue = connectorIndex;
        if (partView.GetComponent<PartView>().IsNotEmpty)
        {
            connectorIndex = (index >= 0 ? (index >= partView.GetComponent<PartView>().ConnectionPointsNotSolid.Length ? partView.GetComponent<PartView>().ConnectionPointsNotSolid.Length - 1 : index) : 0);
        }
        else
        {
            connectorIndex = 0;
        }

        if (oldValue != connectorIndex)
        {
            forceUpdate = true;

            //applyCurrentConfiguration();

            //Debug.Log("ConnectorIndex: " + connectorIndex);
        }
    }
}
