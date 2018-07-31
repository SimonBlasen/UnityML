using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AutoDriveMode
{
    LEFT_TURN = 2, RIGHT_TURN = 3, ACC_DEC = 1, ALTERNATE = 4, STOP = 0, STRAIGHT = 5
}

public class AutoDrive : MonoBehaviour
{
    private TestCar m_Car; // the car controller we want to use
    [SerializeField]
    private AutoDriveMode driveMode = AutoDriveMode.STOP;
    private bool driveModeJustChanged = true;

    private int state = 0;
    private float counter = 0f;
    
    void Start ()
    {
        Speed = 40f;
        m_Car = GetComponent<TestCar>();
    }


    private void FixedUpdate()
    {
        // Statt Input automatisch fahren, entsprechend den Einstellungen, die in der GUI ausgewählt werden

        // pass the input to the car!
        float h = 0f;
        float v = 0f;
        float handbrake = 0f;

        switch (driveMode)
        {
            case AutoDriveMode.STOP:
                if (m_Car.CurrentSpeed > 1f)
                {
                    v = -1f;
                    h = 0f;
                }
                else if (m_Car.CurrentSpeed < -1f)
                {
                    v = 1f;
                    h = 0f;
                }
                else
                {
                    v = 0f;
                    h = 0f;
                }
                //handbrake = 1f;
                break;
            case AutoDriveMode.ACC_DEC:
                handbrake = 0f;
                switch (state)
                {
                    case 0:
                        counter += Time.deltaTime;
                        v = 1f;
                        h = 0f;
                        //if (counter > 6f)
                        if (m_Car.CurrentSpeed > Speed)
                        {
                            state = 1;
                            counter = 0f;
                        }
                        break;
                    case 1:
                        counter += Time.deltaTime;
                        v = -1f;
                        h = 0f;
                        if (m_Car.CurrentSpeed < 1f)
                        {
                            state = 0;
                            counter = 0f;
                        }
                        break;
                }
                break;
            case AutoDriveMode.LEFT_TURN:
                handbrake = 0f;
                if (m_Car.CurrentSpeed > Speed)
                {
                    v = 0f;
                }
                else
                {
                    v = 1f;
                }
                h = SteerAngle;
                break;
            case AutoDriveMode.RIGHT_TURN:
                handbrake = 0f;
                if (m_Car.CurrentSpeed > Speed)
                {
                    v = 0f;
                }
                else
                {
                    v = 1f;
                }
                h = -SteerAngle;
                break;
            case AutoDriveMode.ALTERNATE:
                handbrake = 0f;
                if (m_Car.CurrentSpeed > Speed)
                {
                    v = 0f;
                }
                else
                {
                    v = 1f;
                }
                switch (state)
                {
                    case 0:
                        counter += Time.deltaTime;
                        h = SteerAngle;
                        if (counter > 2f)
                        {
                            state = 1;
                            counter = 0f;
                        }
                        break;
                    case 1:
                        counter += Time.deltaTime;
                        h = -SteerAngle;
                        if (counter > 2f)
                        {
                            state = 0;
                            counter = 0f;
                        }
                        break;
                }
                break;
            case AutoDriveMode.STRAIGHT:
                handbrake = 0f;
                if (m_Car.CurrentSpeed > Speed)
                {
                    v = 0f;
                }
                else
                {
                    v = 1f;
                }
                h = 0f;
                break;
        }

        
        if (driveModeJustChanged)
        {
            driveModeJustChanged = false;
            v = -1f;
            handbrake = 0f;
            h = 0f;
        }


        if (m_Car != null)
        {
            m_Car.Move(h, v, v, handbrake);
        }



        if (Input.GetKeyDown(KeyCode.Delete))
        {
            m_Car.Flip();
        }
    }

    public float SteerAngle { get; set; }
    public float Speed { get; set; }

    public AutoDriveMode DriveMode
    {
        get
        {
            return driveMode;
        }
        set
        {
            driveMode = value;
            driveModeJustChanged = true;
            state = 0;
        }
    }
}
