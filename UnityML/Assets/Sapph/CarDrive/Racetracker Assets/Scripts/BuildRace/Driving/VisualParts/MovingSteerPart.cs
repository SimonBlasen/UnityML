using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSteerPart : MonoBehaviour
{
    public GameObject prefabPartVisible;

    private bool turnInverted;

    [SerializeField]
    private float health;

    private float maxHealth;

    private GameObject instPart;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        instPart.transform.localRotation = Quaternion.Euler(0f, car.CurrentSteerAngle, 0f);
    }

    public void SetProperties(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position, bool inverted)
    {
        instPart = (GameObject)Instantiate(prefabPartVisible);
        instPart.transform.parent = transform;
        instPart.GetComponent<PartVisible>().SetRotation(rotation);
        instPart.GetComponent<PartVisible>().SetDirection(direction);
        instPart.GetComponent<PartVisible>().SetPosition(position);
        instPart.GetComponent<PartVisible>().SetPart(type);
        turnInverted = inverted;
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
