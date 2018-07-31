using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveCheckpointDir : InteractiveCheckpointGeneral
{
    private Vector3 oldPosition = Vector3.zero;
    float factor = 8f;
    GameObject line = null;

    // Use this for initialization
    void Start()
    {
        if (moveable)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (oldPosition != transform.position)
        {
            if (Moveable)
            {
                //transform.position = new Vector3(transform.position.x, Parent.GenTrack.Copy.GetTensorHeight(transform.position), transform.position.z);

                Vector2 toSelf = new Vector2((transform.position - Parent.transform.position).x, (transform.position - Parent.transform.position).z);
                transform.position = Parent.transform.position + new Vector3((toSelf.normalized * factor).x, Parent.GenTrack.Track.GetTensorHeight(new Vector3((toSelf.normalized * factor).x, 0f, (toSelf.normalized * factor).y)), (toSelf.normalized * factor).y);

                //GenTrack.CPChanged(ElementsIndex, CpRole, transform.position);

                Parent.DirectionChanged();

                oldPosition = transform.position;

                line.GetComponent<LineRenderer>().SetPosition(0, oldPosition);
            }
            else
            {
                transform.position = oldPosition;
            }
        }

    }

    public void ParentPosChanged()
    {
        if (line != null)
        {
            line.GetComponent<LineRenderer>().SetPosition(1, parent.transform.position);
        }
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

    private bool moveable = false;

    public bool Moveable
    {
        get
        {
            return moveable;
        }
        set
        {
            moveable = value;

            if ((!moveable) && line != null)
            {
                Destroy(line);
            }
        }
    }

    public float Direction
    {
        get
        {
            Vector2 toRight = (new Vector2(transform.position.x, transform.position.z)) - (new Vector2(Parent.transform.position.x, Parent.transform.position.z));
            float angle = Vector2.Angle(new Vector2(0f, 1f), toRight);
            if (Vector2.Angle(new Vector2(1f, 0f), toRight) > 90f)
            {
                angle = 360f - angle;
            }
            angle -= 90f;
            if (angle < 0f)
            {
                angle += 360f;
            }

            return angle * Mathf.PI / 180f;
        }
        set
        {
            Vector2 toRight = new Vector2(Mathf.Cos(value), -Mathf.Sin(value));
            transform.position = Parent.transform.position + new Vector3(toRight.x * factor, Parent.GenTrack.Track.GetTensorHeight(new Vector3(toRight.x, 0f, toRight.y)), toRight.y * factor);
        }
    }
}