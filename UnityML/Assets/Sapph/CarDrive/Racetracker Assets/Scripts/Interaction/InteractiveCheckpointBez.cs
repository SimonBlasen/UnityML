using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveCheckpointBez : InteractiveCheckpointGeneral
{
    private Vector3 oldPosition = Vector3.zero;
    GameObject line = null;

    // Use this for initialization
    void Start()
    {
        line = new GameObject();
        line.layer = 9;
        line.transform.SetParent(transform);
        line.transform.localPosition = Vector3.zero;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(Color.green, Color.green);
        lr.SetWidth(0.3f, 0.3f);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
        if (parent != null)
        {
            line.GetComponent<LineRenderer>().SetPosition(1, parent.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldPosition != transform.position)
        {
            if (Moveable)
            {
                Vector2 twodPos;
                Ray2D directionRay = new Ray2D(new Vector2(Parent.Position.x, Parent.Position.z), Parent.VectorDirectionForward);
                Ray2D fromMouseRightRay = new Ray2D(new Vector2(transform.position.x, transform.position.z), Parent.VectorDirectionToRight);

                bool parallel;
                Vector2 intersect = Utils.Intersect2D(directionRay, fromMouseRightRay, out parallel);
                if (parallel)
                {
                    twodPos = new Vector2(transform.position.x, transform.position.z);
                }
                else
                {
                    twodPos = intersect;
                }

                transform.position = new Vector3(twodPos.x, transform.position.y, twodPos.y);

                oldPosition = transform.position;

                line.GetComponent<LineRenderer>().SetPosition(0, oldPosition);

                if (IsPrev)
                {
                    Parent.DistancePrevChanged();
                }
                else
                {
                    Parent.DistanceNextChanged();
                }

            }
            else
            {
                transform.position = oldPosition;
            }
        }

    }

    public void ParentPosChanged()
    {
        line.GetComponent<LineRenderer>().SetPosition(1, parent.transform.position);
    }

    public void ParentDirChanged()
    {
        float curDistance = Distance;

        Vector3 newPos = Parent.Position + (new Vector3(Parent.VectorDirectionForward.x, 0f, Parent.VectorDirectionForward.y)) * curDistance * (IsPrev ? -1f : 1f);

        Position = new Vector3(newPos.x, Parent.GenTrack.Track.GetTensorHeight(newPos), newPos.z);

        line.GetComponent<LineRenderer>().SetPosition(0, oldPosition);
    }


    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
            oldPosition = transform.position;
        }
    }

    private InteractiveCheckpoint parent = null;

    public InteractiveCheckpoint Parent
    {
        get
        {
            return parent;
        }
            set
        {
            parent = value;

            if (line != null)
            {
                line.GetComponent<LineRenderer>().SetPosition(1, parent.transform.position);
            }
        }
    }

    public IntCpRole CpRole { get; set; }

    public bool Moveable { get; set; }

    public bool IsPrev { get; set; }

    public float Distance
    {
        get
        {
            return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(Parent.Position.x, Parent.Position.z));
        }
        set
        {
            Vector3 newPos = Parent.Position + (new Vector3(Parent.VectorDirectionForward.x, 0f, Parent.VectorDirectionForward.y)) * value * (IsPrev ? -1f : 1f);

            Position = new Vector3(newPos.x, transform.position.y, newPos.z);
        }
    }
}
















