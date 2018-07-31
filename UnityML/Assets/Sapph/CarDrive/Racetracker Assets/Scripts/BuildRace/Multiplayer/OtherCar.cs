using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCar : DrivingCar
{
    [Header("References")]
    [SerializeField]
    private TextMesh textPlayername;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject partVisiblePrefafb;
    [SerializeField]
    private GameObject partMovingPart;
    [SerializeField]
    private GameObject partMovingPowertrainAxe;
    [SerializeField]
    private GameObject partMovingSteerAxe;
    [SerializeField]
    private GameObject partMovingSteerBar;
    [SerializeField]
    private GameObject partMovingSteerPart;
    [SerializeField]
    private GameObject partMovingWheel;
    [SerializeField]
    private GameObject partMovingWheelAxe;
    [SerializeField]
    private GameObject partMovingMinigun;
    [SerializeField]
    private GameObject wheelColliderPrefab;


    private bool model3DLoaded = false;

    private GameObject testCar = null;

    //[SerializeField]
    //private Rigidbody m_Rigidbody;

    private MovingPart[] mParts;
    private MovingPowertrainAxe[] mPowertrainAxes;
    private MovingSteerAxe[] mSteerAxes;
    private MovingSteerBar[] mSteerBars;
    private MovingSteerPart[] mSteerParts;
    private MovingWheel[] mWheels;
    private MovingWheelAxe[] mWheelAxes;
    private MovingMinigun[] mMiniguns;

    private List<int> steerWheelIndexes;
    private List<int> powerWheelIndexes;

    public Vector3 velocity = Vector3.zero;
    public float backTireRadius = 1f;

    private float m_Topspeed = 1f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (testCar != null)
        {
            textPlayername.transform.LookAt(testCar.transform);
        }

        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().pitch = velocity.magnitude / m_Topspeed;
        }
    }

    public GameObject TestCar
    {
        get
        {
            return testCar;
        }
        set
        {
            testCar = value;
        }
    }

    public bool Model3DLoaded
    {
        get
        {
            return model3DLoaded;
        }
    }

    public string PlayerDisplayname
    {
        get
        {
            return textPlayername.text;
        }
        set
        {
            textPlayername.text = value;
        }
    }


    public void ApplyCar(CalculatedCar car)
    {
        steerWheelIndexes = new List<int>();
        powerWheelIndexes = new List<int>();

        if (car != null)
        {
            //mParts = new MovingPart[car.PartStatics.Length];
            List<MovingPart> tempParts = new List<MovingPart>();
            tempParts.Add(((GameObject)Instantiate(partMovingPart)).GetComponent<MovingPart>());
            tempParts[tempParts.Count - 1].tag = gameObject.tag;
            tempParts[tempParts.Count - 1].CarReference = this;
            int vertsCount = 0;
            for (int i = 0; i < car.PartStatics.Length; i++)
            {
                //mParts[i] = ((GameObject)Instantiate(partMovingPart)).GetComponent<MovingPart>();
                //mParts[i].tag = gameObject.tag;
                //mParts[i].SetProperties(car.PartStatics[i].Type, car.PartStatics[i].Direction, car.PartStatics[i].Rotation, new Vector3Int(car.PartStatics[i].Position));
                //mParts[i].CarReference = this;

                vertsCount = tempParts[tempParts.Count - 1].AddProperties(car.PartStatics[i].Type, car.PartStatics[i].Direction, car.PartStatics[i].Rotation, new Vector3Int(car.PartStatics[i].Position));
                if (vertsCount > 50000)
                {
                    tempParts.Add(((GameObject)Instantiate(partMovingPart)).GetComponent<MovingPart>());
                    tempParts[tempParts.Count - 1].tag = gameObject.tag;
                    tempParts[tempParts.Count - 1].CarReference = this;
                }
            }
            mParts = tempParts.ToArray();
            Debug.Log("MovingPartsAmount: " + mParts.Length);


            mMiniguns = new MovingMinigun[car.PartMiniguns.Length];
            for (int i = 0; i < car.PartMiniguns.Length; i++)
            {
                mMiniguns[i] = ((GameObject)Instantiate(partMovingMinigun)).GetComponent<MovingMinigun>();
                mMiniguns[i].tag = gameObject.tag;
                mMiniguns[i].SetProperties(car.PartMiniguns[i].Type, car.PartMiniguns[i].Direction, car.PartMiniguns[i].Rotation, new Vector3Int(car.PartMiniguns[i].Position));
                mMiniguns[i].CarReference = this;
            }

            mPowertrainAxes = new MovingPowertrainAxe[car.PartPowertrainAxes.Length];
            for (int i = 0; i < car.PartPowertrainAxes.Length; i++)
            {
                mPowertrainAxes[i] = ((GameObject)Instantiate(partMovingPowertrainAxe)).GetComponent<MovingPowertrainAxe>();
                mPowertrainAxes[i].SetProperties(car.PartPowertrainAxes[i].Type, car.PartPowertrainAxes[i].Direction, car.PartPowertrainAxes[i].Rotation, new Vector3Int(car.PartPowertrainAxes[i].Position), car.PartPowertrainAxes[i].RotateAroundAxe, car.PartPowertrainAxes[i].FactorToWheel);
                mPowertrainAxes[i].CarReference = this;
            }

            mWheels = new MovingWheel[car.PartWheels.Length];

            //m_WheelMeshLocalRotations = new Quaternion[mWheels.Length];
            //m_WheelColliders = new WheelCollider[mWheels.Length];

            for (int i = 0; i < car.PartWheels.Length; i++)
            {
                mWheels[i] = ((GameObject)Instantiate(partMovingWheel)).GetComponent<MovingWheel>();
                mWheels[i].SetProperties(car.PartWheels[i].Type, car.PartWheels[i].Direction, car.PartWheels[i].Rotation, new Vector3Int(car.PartWheels[i].Position));
                mWheels[i].CarReference = this;

                if (car.PartWheels[i].Powering)
                {
                    powerWheelIndexes.Add(i);
                }
                if (car.PartWheels[i].Steering)
                {
                    steerWheelIndexes.Add(i);
                }

                //m_WheelColliders[i] = ((GameObject)Instantiate(wheelColliderPrefab)).GetComponent<WheelCollider>();
                //
                //m_WheelColliders[i].gameObject.transform.parent = transform;
                //
                //m_WheelColliders[i].gameObject.transform.position = car.PartWheels[i].Position;
                //m_WheelColliders[i].radius = car.PartWheels[i].Radius;
                //m_WheelMeshes[i] = mWheels[i].instPart;
                //m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }

            backTireRadius = car.PartWheels[2].Radius;

            float kwToTorqueFactor = 50f;
            //m_SpeedType = SpeedType.KPH;
            //m_FullTorqueOverAllWheels = car.FullTorque * kwToTorqueFactor;
            m_Topspeed = car.TopSpeed;

            //for (int i = 0; i < m_WheelColliders.Length; i++)
            //{
            //    float ffFact = 0.07f;
            //    float sfFact = 0.2f;
            //    WheelFrictionCurve ff = new WheelFrictionCurve();
            //    ff.extremumSlip = 0.4f + car.Grip * 4f * ffFact;
            //    ff.extremumValue = 1f + car.Grip * 10f * ffFact;
            //    ff.asymptoteSlip = 0.8f + car.Grip * 8f * ffFact;
            //    ff.asymptoteValue = 0.5f + car.Grip * 5f * ffFact;
            //    ff.stiffness = 1f;
            //    WheelFrictionCurve sf = new WheelFrictionCurve();
            //    sf.extremumSlip = 0.2f + car.Grip * 2f * sfFact;
            //    sf.extremumValue = 1f + car.Grip * 10f * sfFact;
            //    sf.asymptoteSlip = 0.5f + car.Grip * 5f * sfFact;
            //    sf.asymptoteValue = 0.75f + car.Grip * 7.5f * sfFact;
            //    sf.stiffness = 1f;
            //    m_WheelColliders[i].forwardFriction = ff;
            //    m_WheelColliders[i].sidewaysFriction = sf;
            //}
            //
            //m_WheelColliders[0].attachedRigidbody.centerOfMass = Vector3.zero;
            //
            //m_MaxHandbrakeTorque = float.MaxValue;

            //m_Rigidbody = GetComponent<Rigidbody>();
            //m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);

            //m_Rigidbody.mass = car.Weight;


            //transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);


            model3DLoaded = true;
        }
    }

    public override float CurrentAvgRPM
    {
        get
        {
            return velocity.magnitude / (2f * Mathf.PI * backTireRadius);
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    public override float CurrentSteerAngle
    {
        get
        {
            return 0f;
        }
    }

    public override float MaxSteerAngle
    {
        get
        {
            return 30f;
        }
    }
}
