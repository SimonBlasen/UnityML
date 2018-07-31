using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGenTrack : InteractiveHandler
{
    [Header("References")]
    [SerializeField]
    private GameObject castPlane;
    [SerializeField]
    private bool rerender = false;
    [SerializeField]
    private bool updateTrack = false;
    [Space]
    [SerializeField]
    private GameObject prefabControlpoint;
    [SerializeField]
    private GenerationRenderer generationRenderer;
    [SerializeField]
    private GenRendererGUI genRendererGUI;
    [SerializeField]
    private LinesRenderer linesRenderer;

    private GeneratedTrack copy;

    private List<GameObject> instObjects = new List<GameObject>();
    private List<GameObject> lineLeft = new List<GameObject>();
    private List<GameObject> lineRight = new List<GameObject>();
    private List<int> leftIndices = new List<int>();
    private List<int> rightIndices = new List<int>();

    private Camera cameraToUse;

    private InteractiveCheckpointGeneral lastHovered = null;
    private InteractiveCheckpointGeneral recentlyHovered = null;

    // Use this for initialization
    void Start ()
    {
        cameraToUse = GameObject.Find("Camera").GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {
		if (rerender)
        {
            rerender = false;
            ReRender();
        }

        if (updateTrack)
        {
            updateTrack = false;
            SetTrack(Track);
        }


        RaycastHit[] hits = Physics.RaycastAll(cameraToUse.ScreenPointToRay(Input.mousePosition), 3000f);

        RaycastHit hit;
        bool didHit = Physics.Raycast(cameraToUse.ScreenPointToRay(Input.mousePosition), out hit, 3000f, LayerMask.GetMask("Interaction"));

        bool foundOne = false;

        if (didHit && Input.GetMouseButton(0) == false)
        {
            hits = new RaycastHit[] { hit };
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.GetComponent<InteractiveCheckpointGeneral>() != null)
                {
                    if (lastHovered != null && lastHovered.gameObject.GetInstanceID() != hits[i].collider.gameObject.GetInstanceID())
                    {
                        lastHovered.Hovered = false;
                        lastHovered = null;
                    }

                    if (recentlyHovered != null && recentlyHovered.gameObject.GetInstanceID() != hits[i].collider.gameObject.GetInstanceID() && hits[i].collider.gameObject.GetComponent<InteractiveCheckpointGeneral>().GetType() != typeof(InteractiveCheckpointDir) && hits[i].collider.gameObject.GetComponent<InteractiveCheckpointGeneral>().GetType() != typeof(InteractiveCheckpointBez))
                    {
                        recentlyHovered.RecentlyHovered = false;
                        recentlyHovered = null;
                    }

                    hits[i].collider.gameObject.GetComponent<InteractiveCheckpointGeneral>().Hovered = true;
                    lastHovered = hits[i].collider.gameObject.GetComponent<InteractiveCheckpointGeneral>();
                    if (lastHovered.GetType() != typeof(InteractiveCheckpointBez) && lastHovered.GetType() != typeof(InteractiveCheckpointDir))
                    {
                        recentlyHovered = lastHovered;
                    }
                    foundOne = true;
                    break;
                }
            }
        }

        if ((!foundOne) && Input.GetMouseButton(0) == false)
        {
            if (lastHovered != null)
            {
                lastHovered.Hovered = false;
                lastHovered = null;
            }
        }

        if (Input.GetMouseButton(0) && lastHovered != null)
        {
            RaycastHit hit2;
            castPlane.GetComponent<MeshCollider>().Raycast(cameraToUse.ScreenPointToRay(Input.mousePosition), out hit2, 3000f);

            lastHovered.gameObject.transform.position = hit2.point;
        }

        if (recentlyHovered != null)
        {
            recentlyHovered.RecentlyHovered = true;
        }
    }

    public void DeleteElement()
    {
        int insertIndex = recentlyHovered.GetComponent<InteractiveCheckpoint>().ElementsIndex;
        
        List<GeneratedElement> newElements = new List<GeneratedElement>();
        for (int i = 0; i < insertIndex; i++)
        {
            bool jumpStraight = false;
            if (insertIndex != 0 && i == insertIndex - 1)
            {
                if (copy.Elements[insertIndex - 1].GetType() == typeof(GeneratedStraight) && copy.Elements[insertIndex + 1].GetType() == typeof(GeneratedStraight))
                {
                    jumpStraight = true;
                }
            }

            if (!jumpStraight)
            {
                newElements.Add(copy.Elements[i]);
            }
        }

        for (int i = insertIndex + 1; i < copy.Elements.Length; i++)
        {
            newElements.Add(copy.Elements[i]);
        }

        GeneratedTrack newTrack = new GeneratedTrack();
        for (int i = 0; i < newElements.Count; i++)
        {
            newTrack.AddElement(newElements[i]);
        }

        newTrack.SetTerrainModifier(copy.TerrainModifier);
        newTrack.Analyze();

        SetTrack(newTrack);
        
        MakeEditable();

        genRendererGUI.EnableEditButtons();
    }

    public void InsertInbetween(bool straight)
    {
        int insertIndex = recentlyHovered.GetComponent<InteractiveCheckpoint>().ElementsIndex;
        int indexBefore = (insertIndex - 1) < 0 ? copy.Elements.Length - 1 : insertIndex - 1;
        int indexAfter = (insertIndex + 1) % copy.Elements.Length;
        if (straight == false || (straight && copy.Elements[indexBefore].GetType() == typeof(GeneratedBezSpline) && copy.Elements[insertIndex].GetType() == typeof(GeneratedBezSpline)))
        {
            List<GeneratedElement> newElements = new List<GeneratedElement>();
            for (int i = 0; i < insertIndex; i++)
            {
                newElements.Add(copy.Elements[i]);
            }

            GeneratedElement newElement = null;
            if (straight)
            {
                GeneratedStraight newStraight = new GeneratedStraight(copy.Elements[indexBefore].EndPosition, copy.Elements[indexBefore].EndDirection, 0f, copy.Elements[indexBefore].WidthEnd);
                newElement = newStraight;
            }
            else
            {
                GeneratedBezSpline newSpline = new GeneratedBezSpline(copy.Elements[indexBefore].EndPosition, copy.Elements[indexBefore].EndDirection, copy.Elements[indexBefore].EndPosition, copy.Elements[indexBefore].EndDirection, copy.Elements[indexBefore].WidthEnd, copy.Elements[indexBefore].WidthEnd);
                newElement = newSpline;
            }

            newElements.Add(newElement);

            for (int i = insertIndex; i < copy.Elements.Length; i++)
            {
                newElements.Add(copy.Elements[i]);
            }

            GeneratedTrack newTrack = new GeneratedTrack();
            for (int i = 0; i < newElements.Count; i++)
            {
                newTrack.AddElement(newElements[i]);
            }

            newTrack.SetTerrainModifier(copy.TerrainModifier);
            newTrack.Analyze();

            SetTrack(newTrack);

            MakeEditable();

            genRendererGUI.EnableEditButtons();
        }
    }

    public void SetTrack(GeneratedTrack track)
    {
        copy = track;

        for (int i = 0; i < instObjects.Count; i++)
        {
            Destroy(instObjects[i]);
        }
        instObjects.Clear();


        linesRenderer.RefreshTrack(copy);

        genRendererGUI.DistableEditButtons();
    }

    public void MakeEditable()
    {
        copy = copy.Copy();

        for (int i = 0; i < copy.Elements.Length; i++)
        {
            int i2 = (i + 1) % copy.Elements.Length;
            int i0 = i - 1 < 0 ? copy.Elements.Length - 1 : i - 1;
            GameObject instCP = Instantiate(prefabControlpoint);
            instCP.layer = 9;
            instCP.GetComponent<InteractiveCheckpoint>().Moveable = true;
            instCP.GetComponent<InteractiveCheckpoint>().ElementsIndex = i;
            instCP.GetComponent<InteractiveCheckpoint>().CpRole = IntCpRole.MIDPOINT;
            instCP.GetComponent<InteractiveCheckpoint>().Position = copy.Elements[i].Position;
            instCP.GetComponent<InteractiveCheckpoint>().GenTrack = this;
            instCP.GetComponent<InteractiveCheckpoint>().ParamDirection = copy.Elements[i].Direction;


            instCP.GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 4f;
            instCP.GetComponent<InteractiveCheckpoint>().ParamDistanceNext = 4f;

            if (copy.Elements[i0].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)copy.Elements[i0];
                instCP.GetComponent<InteractiveCheckpoint>().ParamDistancePrev = spline.LastTwoCpsDistance;
            }
            if (copy.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)copy.Elements[i];
                instCP.GetComponent<InteractiveCheckpoint>().ParamDistanceNext = spline.FirstTwoCpsDistance;
            }

            if (copy.Elements[i0].GetType() == typeof(GeneratedBezSpline) && copy.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                instCP.GetComponent<InteractiveCheckpoint>().Turnable = true;
            }
            else
            {
                instCP.GetComponent<InteractiveCheckpoint>().Turnable = false;
            }

            instObjects.Add(instCP);

        }


        linesRenderer.RefreshTrack(copy);
    }


    public override void CPChanged(int index, IntCpRole role, Vector3 newPosition, float newNextDistance, float newPrevDistance, float newDirection)
    {
        newPosition = new Vector3(newPosition.x, 0f, newPosition.z);

        int i = index - 1 < 0 ? copy.Elements.Length - 1 : index - 1;
        int i2 = index;
        
        if (role == IntCpRole.MIDPOINT)
        {
            if (copy.Elements[i].GetType() == typeof(GeneratedBezSpline) && copy.Elements[i2].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline old1 = (GeneratedBezSpline)copy.Elements[i];
                GeneratedBezSpline old2 = (GeneratedBezSpline)copy.Elements[i2];

                GeneratedBezSpline new1 = new GeneratedBezSpline(old1.Position, old1.Direction, newPosition, old1.EndDirection, old1.WidthStart, old1.WidthEnd, old1.FirstTwoCpsDistance, old1.LastTwoCpsDistance);
                GeneratedBezSpline new2 = new GeneratedBezSpline(newPosition, old2.Direction, old2.EndPosition, old2.EndDirection, old2.WidthStart, old2.WidthEnd, old2.FirstTwoCpsDistance, old2.LastTwoCpsDistance);

                copy.ReplaceElement(i, new1);
                copy.ReplaceElement(i2, new2);

                //Debug.Log("Updated 2 BezSplines: " + newPosition.ToString());
            }
            else if (copy.Elements[i].GetType() == typeof(GeneratedBezSpline) && copy.Elements[i2].GetType() == typeof(GeneratedStraight))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)copy.Elements[i];
                GeneratedStraight straight = (GeneratedStraight)copy.Elements[i2];
                GeneratedBezSpline afterSpline = (GeneratedBezSpline)copy.Elements[(i2 + 1) % copy.Elements.Length];

                float newStraightDirectinon = Vector2.Angle(new Vector2(0f, 1f), new Vector2(straight.EndPosition.x, straight.EndPosition.z) - new Vector2(newPosition.x, newPosition.z));
                if (Vector2.Angle(new Vector2(1f, 0f), new Vector2(straight.EndPosition.x, straight.EndPosition.z) - new Vector2(newPosition.x, newPosition.z)) > 90f)
                {
                    newStraightDirectinon = 360 - newStraightDirectinon;
                }
                newStraightDirectinon = newStraightDirectinon * Mathf.PI / 180f;

                instObjects[i2].GetComponent<InteractiveCheckpoint>().ParamDirection = newStraightDirectinon;
                instObjects[i2].GetComponent<InteractiveCheckpoint>().DirectionChanged();
                instObjects[(i2 + 1) % copy.Elements.Length].GetComponent<InteractiveCheckpoint>().ParamDirection = newStraightDirectinon;
                instObjects[(i2 + 1) % copy.Elements.Length].GetComponent<InteractiveCheckpoint>().DirectionChanged();

                GeneratedBezSpline new1 = new GeneratedBezSpline(spline.Position, spline.Direction, newPosition, newStraightDirectinon, spline.WidthStart, spline.WidthEnd, spline.FirstTwoCpsDistance, spline.LastTwoCpsDistance);
                GeneratedBezSpline new2 = new GeneratedBezSpline(afterSpline.Position, newStraightDirectinon, afterSpline.EndPosition, afterSpline.EndDirection, afterSpline.WidthStart, afterSpline.WidthEnd, afterSpline.FirstTwoCpsDistance, afterSpline.LastTwoCpsDistance);
                GeneratedStraight newStraight = new GeneratedStraight(newPosition, newStraightDirectinon, Vector2.Distance(new Vector2(newPosition.x, newPosition.z), new Vector2(afterSpline.Position.x, afterSpline.Position.z)), straight.WidthStart, straight.WidthEnd);

                copy.ReplaceElement(i, new1);
                copy.ReplaceElement(i2, newStraight);
                copy.ReplaceElement((i2 + 1) % copy.Elements.Length, new2);
                //Debug.Log("Updated Straight - Spline: " + newPosition.ToString());
            }
            else if (copy.Elements[i].GetType() == typeof(GeneratedStraight) && copy.Elements[i2].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)copy.Elements[i2];
                GeneratedStraight straight = (GeneratedStraight)copy.Elements[i];
                GeneratedBezSpline afterSpline = (GeneratedBezSpline)copy.Elements[i - 1 >= 0 ? i - 1 : copy.Elements.Length - 1];

                float newStraightDirectinon = Vector2.Angle(new Vector2(0f, 1f), new Vector2(newPosition.x, newPosition.z) - new Vector2(straight.Position.x, straight.Position.z));
                if (Vector2.Angle(new Vector2(1f, 0f), new Vector2(newPosition.x, newPosition.z) - new Vector2(straight.Position.x, straight.Position.z)) > 90f)
                {
                    newStraightDirectinon = 360 - newStraightDirectinon;
                }
                newStraightDirectinon = newStraightDirectinon * Mathf.PI / 180f;

                instObjects[i].GetComponent<InteractiveCheckpoint>().ParamDirection = newStraightDirectinon;
                instObjects[i].GetComponent<InteractiveCheckpoint>().DirectionChanged();
                instObjects[i2].GetComponent<InteractiveCheckpoint>().ParamDirection = newStraightDirectinon;
                instObjects[i2].GetComponent<InteractiveCheckpoint>().DirectionChanged();

                GeneratedBezSpline new1 = new GeneratedBezSpline(newPosition, newStraightDirectinon, spline.EndPosition, spline.EndDirection, spline.WidthStart, spline.WidthEnd, spline.FirstTwoCpsDistance, spline.LastTwoCpsDistance);
                GeneratedBezSpline new2 = new GeneratedBezSpline(afterSpline.Position, afterSpline.Direction, afterSpline.EndPosition, newStraightDirectinon, afterSpline.WidthStart, afterSpline.WidthEnd, afterSpline.FirstTwoCpsDistance, afterSpline.LastTwoCpsDistance);
                GeneratedStraight newStraight = new GeneratedStraight(straight.Position, newStraightDirectinon, Vector2.Distance(new Vector2(newPosition.x, newPosition.z), new Vector2(straight.Position.x, straight.Position.z)), straight.WidthStart, straight.WidthEnd);

                copy.ReplaceElement(i, newStraight);
                copy.ReplaceElement(i2, new1);
                copy.ReplaceElement(i - 1 >= 0 ? i - 1 : copy.Elements.Length - 1, new2);
                //Debug.Log("Updated Spline - Straight: " + newPosition.ToString());
            }
        }
        else if (role == IntCpRole.NEXT_B1)
        {
            if (copy.Elements[i2].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline old = (GeneratedBezSpline)copy.Elements[i2];
                GeneratedBezSpline new1 = new GeneratedBezSpline(old.Position, old.Direction, old.EndPosition, old.EndDirection, old.WidthStart, old.WidthEnd, newNextDistance, old.LastTwoCpsDistance);
                copy.ReplaceElement(i2, new1);
            }
        }
        else if (role == IntCpRole.PREV_B2)
        {
            if (copy.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline old = (GeneratedBezSpline)copy.Elements[i];
                GeneratedBezSpline new1 = new GeneratedBezSpline(old.Position, old.Direction, old.EndPosition, old.EndDirection, old.WidthStart, old.WidthEnd, old.FirstTwoCpsDistance, newPrevDistance);
                copy.ReplaceElement(i, new1);
            }
        }
        else if (role == IntCpRole.DIRECTION)
        {
            if (copy.Elements[i].GetType() == typeof(GeneratedBezSpline) && copy.Elements[index].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline old1 = (GeneratedBezSpline)copy.Elements[i];
                GeneratedBezSpline old2 = (GeneratedBezSpline)copy.Elements[index];

                GeneratedBezSpline new1 = new GeneratedBezSpline(old1.Position, old1.Direction, old1.EndPosition, newDirection, old1.WidthStart, old1.WidthEnd, old1.FirstTwoCpsDistance, old1.LastTwoCpsDistance);
                GeneratedBezSpline new2 = new GeneratedBezSpline(old2.Position, newDirection, old2.EndPosition, old2.EndDirection, old2.WidthStart, old2.WidthEnd, old2.FirstTwoCpsDistance, old2.LastTwoCpsDistance);

                copy.ReplaceElement(i, new1);
                copy.ReplaceElement(index, new2);
            }
        }
        
        linesRenderer.RefreshTrack(copy);
    }

    public void ReRender()
    {
        copy.Analyze();

        copy.SetTerrainModifier(generationRenderer.TerrainModifier);
        copy.GenerateBorder();

        
        generationRenderer.Render(copy);

        SetTrack(copy);
    }


    public override void TrackUpdated()
    {
        updateTrack = true;
        //SetTrack(Track);
    }

    public GeneratedTrack Copy
    {
        get
        {
            return copy;
        }
    }
}
