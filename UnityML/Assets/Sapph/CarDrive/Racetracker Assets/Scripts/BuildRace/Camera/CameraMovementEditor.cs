using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementEditor : MonoBehaviour
{
    [SerializeField]
    private float flySpeed = 10f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private GameObject innerComponent;
    [SerializeField]
    private GameObject halfInnerComponent;



    private bool shift;
    private bool ctrl;
    private float accelerationAmount = 30f;
    private float accelerationRatio = 3f;
    private float slowDownRatio = 0.2f;

	// Use this for initialization
	void Start ()
    {
        
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(Vector3.forward * flySpeed * Input.GetAxis("Vertical"));
        }


        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Vector3.right * flySpeed * Input.GetAxis("Horizontal"));
        }


        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * flySpeed);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.down * flySpeed);
        }





        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
        halfInnerComponent.transform.Rotate(0, rotationX, 0);


        float rotationY = Input.GetAxis("Mouse Y") * -1 * rotationSpeed;
        if (halfInnerComponent.transform.rotation.eulerAngles.x + rotationY >= 180 && halfInnerComponent.transform.rotation.eulerAngles.x + rotationY <= 270)
        {
            halfInnerComponent.transform.rotation.Set(0.5f, halfInnerComponent.transform.rotation.y, halfInnerComponent.transform.rotation.z, halfInnerComponent.transform.rotation.w);
        }
        else if (halfInnerComponent.transform.rotation.eulerAngles.x + rotationY < 180 && halfInnerComponent.transform.rotation.eulerAngles.x + rotationY >= 90)
        {
            halfInnerComponent.transform.rotation.Set(-0.5f, halfInnerComponent.transform.rotation.y, halfInnerComponent.transform.rotation.z, halfInnerComponent.transform.rotation.w);
        }
        else
        {
            innerComponent.transform.Rotate(rotationY, 0, 0);
        }


        //if (Input.GetKeyDown(KeyCode.M))
        //    playerObject.transform.position = transform.position; //Moves the player to the flycam's position. Make sure not to just move the player's camera.
    }
}
