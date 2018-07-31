using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMoveScript : MonoBehaviour {

    public Camera theCam;

    private Vector3 normalRot;
    private Vector3 normalCamPos = Vector3.zero;
    private Vector3 backCamPos;

    private bool shiftPressed = false;

    private TestCar m_Car; // the car controller we want to use
    private TestCar2 m_Car2; // the car controller we want to use


    private void Awake()
    {
        // get the car controller
        m_Car = GetComponent<TestCar>();
        m_Car2 = GetComponent<TestCar2>();
    }


    private void FixedUpdate()
    {
        if (normalCamPos == Vector3.zero)
        {
            normalCamPos = theCam.transform.localPosition;
            normalRot = theCam.transform.localRotation.eulerAngles;
            backCamPos = normalCamPos;
            backCamPos.z = backCamPos.z * -1;
        }

        // pass the input to the car!
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float handbrake = Input.GetAxis("Jump");
        if (m_Car != null)
        {
            m_Car.Move(h, v, v, handbrake);
        }
        else if (m_Car2 != null)
        {
            m_Car2.Move(h, v, v, handbrake);
        }



        if (Input.GetKeyDown(KeyCode.Delete))
        {
            m_Car.Flip();
        }

        if ((Input.GetKey(KeyCode.RightShift)) && shiftPressed == false)
        {
            theCam.transform.localPosition = backCamPos;
            Vector3 backRot = normalRot;
            backRot.y = backRot.y + 180;
            theCam.transform.localRotation = Quaternion.Euler(backRot);
            shiftPressed = true;
            Debug.Log("Look back");
        }
        else if ((Input.GetKey(KeyCode.RightShift) == false) && shiftPressed)
        {
            shiftPressed = false;
            theCam.transform.localPosition = normalCamPos;
            theCam.transform.localRotation = Quaternion.Euler(normalRot);
            Debug.Log("Look forward");
        }

        if (Input.GetKey(KeyCode.M))
        {
            m_Car.SetShooting(0, true);
        }
        else
        {
            m_Car.SetShooting(0, false);
        }

    }
}
