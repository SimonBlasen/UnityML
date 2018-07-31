using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSteerBar : MonoBehaviour
{
    public GameObject prefabPartVisible;

    private Vector3 movingTowardsAxe;
    private float halfwayDistance;

    [SerializeField]
    private float health;

    private float maxHealth;

    private GameObject instPart;

    private Vector3 originLocalPos = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        instPart.transform.localPosition = movingTowardsAxe * (car.CurrentSteerAngle / car.MaxSteerAngle) * halfwayDistance + originLocalPos;
    }

    public void SetProperties(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position, Vector3 movingTowardsAxe, float halfwayDistance)
    {
        instPart = (GameObject)Instantiate(prefabPartVisible);
        instPart.transform.parent = transform;
        instPart.GetComponent<PartVisible>().SetRotation(rotation);
        instPart.GetComponent<PartVisible>().SetDirection(direction);
        instPart.GetComponent<PartVisible>().SetPosition(position);
        instPart.GetComponent<PartVisible>().SetPart(type);
        this.halfwayDistance = halfwayDistance;
        this.movingTowardsAxe = movingTowardsAxe;

        originLocalPos = instPart.transform.localPosition;
    }

    private DrivingCar car;

    public DrivingCar CarReference
    {
        get
        {
            return car;
        }
        set
        {
            car = value;
            gameObject.transform.parent = car.gameObject.transform;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
}
