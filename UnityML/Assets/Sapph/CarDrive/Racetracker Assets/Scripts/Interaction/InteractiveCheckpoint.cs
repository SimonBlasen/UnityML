using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntCpRole
{
    MIDPOINT, PREV_B2, NEXT_B1, DIRECTION
}

public class InteractiveCheckpoint : InteractiveCheckpointGeneral
{
    public GameObject prefabInteractiveCPBez;
    public GameObject prefabInteractiveCPDir;
    private Vector3 oldPosition = Vector3.zero;

    private bool instantiated = false;

    // Use this for initialization
    void Start ()
    {
        if (!instantiated)
        {
            instantiated = true;
            GameObject instB1Next = Instantiate(prefabInteractiveCPBez, transform);
            B1Next = instB1Next.GetComponent<InteractiveCheckpointBez>();
            GameObject instB2Prev = Instantiate(prefabInteractiveCPBez, transform);
            B2Prev = instB2Prev.GetComponent<InteractiveCheckpointBez>();

            GameObject instDir = Instantiate(prefabInteractiveCPDir, transform);
            BDirection = instDir.GetComponent<InteractiveCheckpointDir>();

            B1Next.Parent = this;
            B2Prev.Parent = this;
            BDirection.Parent = this;
            B1Next.Moveable = true;
            B2Prev.Moveable = true;
            BDirection.Moveable = turnable;

            B1Next.IsPrev = false;
            B2Prev.IsPrev = true;
        }
	}

	// Update is called once per frame
	void Update ()
    {
		if (oldPosition != transform.position)
        {
            if (Moveable)
            {
                //transform.position = new Vector3(transform.position.x, GenTrack.Copy.GetTensorHeight(transform.position), transform.position.z);

                GenTrack.CPChanged(ElementsIndex, CpRole, transform.position, 0f, 0f, 0f);

                oldPosition = transform.position;

                B1Next.ParentPosChanged();
                B2Prev.ParentPosChanged();
                BDirection.ParentPosChanged();
            }
            else
            {
                transform.position = oldPosition;
            }
        }

    }

    public void DirectionChanged()
    {
        direction = BDirection.Direction;

        GenTrack.CPChanged(ElementsIndex, IntCpRole.DIRECTION, Vector3.zero, 0f, 0f, direction);

        B1Next.ParentDirChanged();
        B2Prev.ParentDirChanged();
    }

    public void DistanceNextChanged()
    {
        GenTrack.CPChanged(ElementsIndex, IntCpRole.NEXT_B1, Vector3.zero, B1Next.Distance, 0f, 0f);
    }
    public void DistancePrevChanged()
    {
        GenTrack.CPChanged(ElementsIndex, IntCpRole.PREV_B2, Vector3.zero, 0f, B2Prev.Distance, 0f);
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

    private bool turnable = false;

    public bool Turnable
    {
        get
        {
            return turnable;
        }
        set
        {
            turnable = value;
            if (!instantiated)
            {
                Start();
            }
            BDirection.Moveable = turnable;
        }
    }

    public int ElementsIndex { get; set; }

    public IntCpRole CpRole { get; set; }

    public InteractiveHandler GenTrack { get; set; }

    public bool Moveable { get; set; }

    public InteractiveCheckpointBez B1Next { get;set; }

    public InteractiveCheckpointBez B2Prev { get; set; }

    public InteractiveCheckpointDir BDirection { get; set; }

    private float direction = 0f;

    public Vector2 VectorDirectionForward
    {
        get
        {
            return (new Vector2(Mathf.Sin(direction), Mathf.Cos(direction))).normalized;
        }
    }

    public Vector2 VectorDirectionToRight
    {
        get
        {
            return (new Vector2(Mathf.Cos(direction), -Mathf.Sin(direction))).normalized;
        }
    }

    public float ParamDirection
    {
        get
        {
            return direction;
        }
        set
        {
            if (!instantiated)
            {
                Start();
            }
            direction = value;
            BDirection.Direction = direction;
        }
    }

    public float ParamDistanceNext
    {
        get
        {
            return B1Next.Distance;
        }
        set
        {
            if (!instantiated)
            {
                Start();
            }
            B1Next.Distance = value;
        }
    }
    public float ParamDistancePrev
    {
        get
        {
            return B2Prev.Distance;
        }
        set
        {
            if (!instantiated)
            {
                Start();
            }

            B2Prev.Distance = value;
        }
    }
}




