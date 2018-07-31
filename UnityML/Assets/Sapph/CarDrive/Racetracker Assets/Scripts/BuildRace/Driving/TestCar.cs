using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CarDriveType
{
    FrontWheelDrive,
    RearWheelDrive,
    FourWheelDrive
}

public enum SpeedType
{
    MPH,
    KPH
}

public class TestCar : DrivingCar
{
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

    [Header("References")]
    [SerializeField]
    private GameObject boxCollidersContainer;
    [SerializeField]
    private Text speedText;

    private MovingPart[] mParts;
    private MovingPowertrainAxe[] mPowertrainAxes;
    private MovingSteerAxe[] mSteerAxes;
    private MovingSteerBar[] mSteerBars;
    private MovingSteerPart[] mSteerParts;
    private MovingWheel[] mWheels;
    private MovingWheelAxe[] mWheelAxes;
    private MovingMinigun[] mMiniguns;

    private float maximumHealth = 0f;
    private float currentHealth = 0f;

    private List<int> steerWheelIndexes;
    private List<int> powerWheelIndexes;

    private bool[] shootingWeapons = new bool[1];

    private CalculatedCar calculatedCar;
    private CarPropertiesSetting carProperties;
    
    [SerializeField]
    private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
    [SerializeField]
    public WheelCollider[] m_WheelColliders = new WheelCollider[4];
    [SerializeField]
    private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField]
    private WheelEffects[] m_WheelEffects = new WheelEffects[4];
    [SerializeField]
    private Vector3 m_CentreOfMassOffset;
    [SerializeField]
    private float m_MaximumSteerAngle;
    [Range(0, 1)]
    [SerializeField]
    private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
    [Range(0, 1)]
    [SerializeField]
    private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField]
    private float m_FullTorqueOverAllWheels;
    [SerializeField]
    private float m_ReverseTorque;
    [SerializeField]
    private float m_MaxHandbrakeTorque;
    [SerializeField]
    private float m_Downforce = 100f;
    [SerializeField]
    private SpeedType m_SpeedType;
    [SerializeField]
    private float m_Topspeed = 200;
    [SerializeField]
    private static int NoOfGears = 5;
    [SerializeField]
    private float m_RevRangeBoundary = 1f;
    [SerializeField]
    private float m_SlipLimit;
    [SerializeField]
    private float m_BrakeTorque;
    [SerializeField]
    private Rigidbody m_Rigidbody;

    private Quaternion[] m_WheelMeshLocalRotations;
    private Vector3 m_Prevpos, m_Pos;
    private float m_SteerAngle;
    private int m_GearNum;
    private float m_GearFactor;
    private float m_OldRotation;
    private float m_CurrentTorque;
    //private Rigidbody m_Rigidbody;
    private const float k_ReversingThreshold = 0.01f;

    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }
    public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 2.23693629f; } }
    public float MaxSpeed { get { return m_Topspeed; } }
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    //public bool destruct = false;
    //public bool clear = false;


    // Use this for initialization
    private void Start()
    {
        currHealth = maxHealth;
        for (int i = 0; i < shootingWeapons.Length; i++)
        {
            shootingWeapons[i] = false;
        }
    }

    private PartConfiguration partDestruct = null;

    private void Update()
    {
        /*if (destruct)
        {
            mParts[0].AddInvisPart(partDestruct);
            destruct = false;
        }

        if (clear)
        {
            mParts[0].ClearInvisParts();
            clear = false;
        }*/
    }

    public void Flip()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        transform.position = transform.position + new Vector3(0f, 10f, 0f);
        transform.rotation = Quaternion.identity;
    }

    private bool freezed = false;

    public bool Freeze
    {
        get
        {
            return freezed;
        }
        set
        {
            freezed = value;
            if (freezed)
            {
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                m_Rigidbody.constraints = RigidbodyConstraints.None;
            }
        }
    }

    public override float CurrentAvgRPM
    {
        get
        {
            return m_WheelColliders[2].rpm;
        }
    }

    public override float CurrentSteerAngle
    {
        get
        {
            return m_SteerAngle;
        }
    }

    public override float MaxSteerAngle
    {
        get
        {
            return m_MaximumSteerAngle;
        }
    }

    public void ApplyCar(CalculatedCar car, CarPropertiesSetting carProps)
    {
        steerWheelIndexes = new List<int>();
        powerWheelIndexes = new List<int>();

        calculatedCar = car;
        carProperties = carProps;

        if (car != null)
        {
            maximumHealth = car.Health;
            currentHealth = maximumHealth;

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

                //mParts[i] = ((GameObject)Instantiate(partMovingPart)).GetComponent<MovingPart>();
                if (vertsCount == 0)
                {
                    partDestruct = new PartConfiguration(car.PartStatics[i].Direction, car.PartStatics[i].Rotation, new Vector3Int(car.PartStatics[i].Position), car.PartStatics[i].Type);
                }
                vertsCount = tempParts[tempParts.Count - 1].AddProperties(car.PartStatics[i].Type, car.PartStatics[i].Direction, car.PartStatics[i].Rotation, new Vector3Int(car.PartStatics[i].Position));
                
                if (vertsCount > 50000)
                {
                    tempParts.Add(((GameObject)Instantiate(partMovingPart)).GetComponent<MovingPart>());
                    tempParts[tempParts.Count - 1].tag = gameObject.tag;
                    tempParts[tempParts.Count - 1].CarReference = this;
                }
                Debug.Log("VertsCurrentCount: " + vertsCount);
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
                mPowertrainAxes[i].tag = gameObject.tag;
                mPowertrainAxes[i].SetProperties(car.PartPowertrainAxes[i].Type, car.PartPowertrainAxes[i].Direction, car.PartPowertrainAxes[i].Rotation, new Vector3Int(car.PartPowertrainAxes[i].Position), car.PartPowertrainAxes[i].RotateAroundAxe, car.PartPowertrainAxes[i].FactorToWheel);
                mPowertrainAxes[i].CarReference = this;
            }

            mSteerParts = new MovingSteerPart[car.PartSteerParts.Length];
            for (int i = 0; i < car.PartSteerParts.Length; i++)
            {
                mSteerParts[i] = ((GameObject)Instantiate(partMovingSteerPart)).GetComponent<MovingSteerPart>();
                mSteerParts[i].tag = gameObject.tag;
                mSteerParts[i].SetProperties(car.PartSteerParts[i].Type, car.PartSteerParts[i].Direction, car.PartSteerParts[i].Rotation, new Vector3Int(car.PartSteerParts[i].Position), car.PartSteerParts[i].InvertedDirection);
                mSteerParts[i].CarReference = this;
            }

            mSteerBars = new MovingSteerBar[car.PartSteerBars.Length];
            for (int i = 0; i < car.PartSteerBars.Length; i++)
            {
                mSteerBars[i] = ((GameObject)Instantiate(partMovingSteerBar)).GetComponent<MovingSteerBar>();
                mSteerBars[i].tag = gameObject.tag;
                mSteerBars[i].SetProperties(car.PartSteerBars[i].Type, car.PartSteerBars[i].Direction, car.PartSteerBars[i].Rotation, new Vector3Int(car.PartSteerBars[i].Position), car.PartSteerBars[i].AxeMoveAlong, car.PartSteerBars[i].HalfwayDistance);
                mSteerBars[i].CarReference = this;
            }

            mWheels = new MovingWheel[car.PartWheels.Length];

            m_WheelMeshLocalRotations = new Quaternion[mWheels.Length];
            m_WheelColliders = new WheelCollider[mWheels.Length];

            for (int i = 0; i < car.PartWheels.Length; i++)
            {
                mWheels[i] = ((GameObject)Instantiate(partMovingWheel)).GetComponent<MovingWheel>();
                mWheels[i].tag = gameObject.tag;
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

                m_WheelColliders[i] = ((GameObject)Instantiate(wheelColliderPrefab)).GetComponent<WheelCollider>();

                m_WheelColliders[i].gameObject.transform.parent = transform;

                m_WheelColliders[i].gameObject.transform.position = car.PartWheels[i].Position;
                m_WheelColliders[i].radius = car.PartWheels[i].Radius;
                m_WheelMeshes[i] = mWheels[i].instPart;
                m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }

            float kwToTorqueFactor = 50f;
            m_SpeedType = SpeedType.KPH;
            m_FullTorqueOverAllWheels = car.FullTorque * kwToTorqueFactor;
            m_Topspeed = car.TopSpeed;

            for (int i = 0; i < m_WheelColliders.Length; i++)
            {
                float ffFact = 0.07f;
                float sfFact = 0.2f;
                WheelFrictionCurve ff = new WheelFrictionCurve();
                ff.extremumSlip = 0.4f + car.Grip * 4f * ffFact;
                ff.extremumValue = 1f + car.Grip * 10f * ffFact;
                ff.asymptoteSlip = 0.8f + car.Grip * 8f * ffFact;
                ff.asymptoteValue = 0.5f + car.Grip * 5f * ffFact;
                ff.stiffness = 1f;
                WheelFrictionCurve sf = new WheelFrictionCurve();
                sf.extremumSlip = 0.2f + car.Grip * 2f * sfFact;
                sf.extremumValue = 1f + car.Grip * 10f * sfFact;
                sf.asymptoteSlip = 0.5f + car.Grip * 5f * sfFact;
                sf.asymptoteValue = 0.75f + car.Grip * 7.5f * sfFact;
                sf.stiffness = 1f;
                m_WheelColliders[i].forwardFriction = ff;
                m_WheelColliders[i].sidewaysFriction = sf;

                JointSpring js = new JointSpring();
                //js.spring = 35000 * (car.Weight * 0.001f) * 0.6f;
                //js.damper = 4500 * (car.Weight * 0.001f) * 0.6f;
                if (carProps == null)
                {
                    js.spring = car.PartWheels[i].SpringStrength;
                    js.damper = car.PartWheels[i].DamperStrength;
                }
                else
                {
                    switch(i)
                    {
                        case 0:
                            js.spring = carProps.SpringStrengthLF;
                            js.damper = carProps.DamperStrengthLF;
                            break;
                        case 1:
                            js.spring = carProps.SpringStrengthRF;
                            js.damper = carProps.DamperStrengthRF;
                            break;
                        case 2:
                            js.spring = carProps.SpringStrengthLR;
                            js.damper = carProps.DamperStrengthLR;
                            break;
                        case 3:
                            js.spring = carProps.SpringStrengthRR;
                            js.damper = carProps.DamperStrengthRR;
                            break;
                    }
                }
                js.targetPosition = 0.5f;
                m_WheelColliders[i].suspensionSpring = js;
            }

            m_WheelColliders[0].attachedRigidbody.centerOfMass = Vector3.zero;

            m_MaxHandbrakeTorque = float.MaxValue;

            m_Rigidbody = GetComponent<Rigidbody>();
            m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);

            m_Rigidbody.mass = car.Weight;


            //transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);



        }
    }

    public void ApplyCar(CalculatedCar car)
    {
        ApplyCar(car, null);
    }


    public void RefreshCar()
    {

    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = currentHealth < 0f ? 0f : currentHealth;
    }

    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float)NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum / (float)NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }


    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        if (currHealth <= 0f)
        {
            accel = 0f;
        }

        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat;
        }

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheels.
        //Assuming that wheels 0 and 1 are the front wheels.
        m_SteerAngle = steering * m_MaximumSteerAngle;

        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            if (steerWheelIndexes.Contains(i))
            {
                m_WheelColliders[i].steerAngle = m_SteerAngle;
            }
        }

        //m_WheelColliders[0].steerAngle = m_SteerAngle;
        //m_WheelColliders[1].steerAngle = m_SteerAngle;

        SteerHelper();
        ApplyDrive(accel, footbrake);
        CapSpeed();

        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        if (handbrake > 0f)
        {
            var hbTorque = handbrake * m_MaxHandbrakeTorque;
            m_WheelColliders[2].brakeTorque = hbTorque;
            m_WheelColliders[3].brakeTorque = hbTorque;
        }


        CalculateRevs();
        GearChanging();

        AddDownForce();
        CheckForWheelSpin();
        TractionControl();
    }


    private void CapSpeed()
    {
        float speed = m_Rigidbody.velocity.magnitude;
        switch (m_SpeedType)
        {
            case SpeedType.MPH:

                speed *= 2.23693629f;
                if (speed > m_Topspeed)
                    m_Rigidbody.velocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.velocity.normalized;
                break;

            case SpeedType.KPH:
                speed *= 3.6f;
                if (speed > m_Topspeed)
                    m_Rigidbody.velocity = (m_Topspeed / 3.6f) * m_Rigidbody.velocity.normalized;
                break;
        }

        if (speedText != null)
        {
            speedText.text = Mathf.Round(speed) + " " + m_SpeedType.ToString();
        }
    }


    private void ApplyDrive(float accel, float footbrake)
    {

        float thrustTorque;
        //switch (m_CarDriveType)
        //{
        //    case CarDriveType.FourWheelDrive:
        //        thrustTorque = accel * (m_CurrentTorque / 4f);
        //        for (int i = 0; i < 4; i++)
        //        {
        //            m_WheelColliders[i].motorTorque = thrustTorque;
        //        }
        //        break;
        //
        //    case CarDriveType.FrontWheelDrive:
        //        thrustTorque = accel * (m_CurrentTorque / 2f);
        //        m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
        //        break;
        //
        //    case CarDriveType.RearWheelDrive:
        //        thrustTorque = accel * (m_CurrentTorque / 2f);
        //        m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
        //        break;
        //
        //}


        int amountOfPoweredWheels = 0;
        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            if (powerWheelIndexes.Contains(i))
            {
                amountOfPoweredWheels++;
            }
        }

        thrustTorque = accel * (m_CurrentTorque / ((float)amountOfPoweredWheels));
        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            if (powerWheelIndexes.Contains(i))
            {
                m_WheelColliders[i].motorTorque = thrustTorque;
            }
        }

        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
            {
                m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
            }
        }
    }


    private void SteerHelper()
    {
        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            WheelHit wheelhit;
            m_WheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_Downforce *
                                                     m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
    }


    // checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin()
    {
        // loop through all wheels
        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            WheelHit wheelHit;
            m_WheelColliders[i].GetGroundHit(out wheelHit);

            if (i == 2 && Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit && GetComponent<AudioSource>() != null && false)
            {
                GetComponent<AudioSource>().pitch = ((wheelHit.forwardSlip) + (CurrentSpeed / m_Topspeed)) > 1f ? 1f : ((wheelHit.forwardSlip) + (CurrentSpeed / m_Topspeed));
                //Debug.Log("Slip: " + wheelHit.forwardSlip + ", Limit: " + m_SlipLimit);
            }
            else if (i == 2 && GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().pitch = CurrentSpeed / m_Topspeed;
            }

            if (m_WheelEffects[0] != null)
            {
                // is the tire slipping above the given threshhold
                if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
                {
                    m_WheelEffects[i].EmitTyreSmoke();

                    // avoiding all four tires screeching at the same time
                    // if they do it can lead to some strange audio artefacts
                    if (!AnySkidSoundPlaying())
                    {
                        m_WheelEffects[i].PlayAudio();
                    }
                    continue;
                }

                

                // if it wasnt slipping stop all the audio
                if (m_WheelEffects[i].PlayingAudio)
                {
                    m_WheelEffects[i].StopAudio();
                }
                // end the trail generation
                m_WheelEffects[i].EndSkidTrail();
            }
        }
    }

    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        WheelHit wheelHit;
        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            if (powerWheelIndexes.Contains(i))
            {
                m_WheelColliders[i].GetGroundHit(out wheelHit);

                AdjustTorque(wheelHit.forwardSlip);
            }
        }
        /*
        switch (m_CarDriveType)
        {
            case CarDriveType.FourWheelDrive:
                // loop through all wheels
                for (int i = 0; i < m_WheelColliders.Length; i++)
                {
                    if (powerWheelIndexes.Contains(i))
                    {
                        m_WheelColliders[i].GetGroundHit(out wheelHit);

                        AdjustTorque(wheelHit.forwardSlip);
                    }
                }
                break;

            case CarDriveType.RearWheelDrive:
                m_WheelColliders[2].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                m_WheelColliders[3].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;

            case CarDriveType.FrontWheelDrive:
                m_WheelColliders[0].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                m_WheelColliders[1].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;
        }*/
    }


    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
        {
            m_CurrentTorque -= 10 * m_TractionControl;
        }
        else
        {
            m_CurrentTorque += 10 * m_TractionControl;
            if (m_CurrentTorque > m_FullTorqueOverAllWheels)
            {
                m_CurrentTorque = m_FullTorqueOverAllWheels;
            }
        }
    }


    private bool AnySkidSoundPlaying()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_WheelEffects[i].PlayingAudio)
            {
                return true;
            }
        }
        return false;
    }
}