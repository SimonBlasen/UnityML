using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPowertrainAxe : MonoBehaviour
{
    public GameObject prefabPartVisible;

    private Vector3 rotateAroundAxe;
    public float speedRelativeToWheel = 0f;

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

    }

    private void FixedUpdate()
    {
        if (instPart == null)
            Debug.Log("instPart");
        if (rotateAroundAxe == null)
            Debug.Log("rotateAroundAxe");
        if (car == null)
            Debug.Log("car");
        if (speedRelativeToWheel != 0f)
        {
            instPart.transform.Rotate(rotateAroundAxe, speedRelativeToWheel * car.CurrentAvgRPM * (1f / 60f));
            //Debug.Log(rotateAroundAxe.ToString() + "," + car.CurrentAvgRPM);
        }
        else
        {
            Debug.Log("Rel speed is 0");
        }
    }

    public void SetProperties(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position, Vector3 axeRotateAround, float speedRelativeToWheel)
    {
        instPart = (GameObject)Instantiate(prefabPartVisible);
        instPart.transform.parent = transform;
        instPart.GetComponent<PartVisible>().SetRotation(rotation);
        instPart.GetComponent<PartVisible>().SetDirection(direction);
        instPart.GetComponent<PartVisible>().SetPosition(position);
        instPart.GetComponent<PartVisible>().SetPart(type);
        this.speedRelativeToWheel = speedRelativeToWheel;
        rotateAroundAxe = axeRotateAround;
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
