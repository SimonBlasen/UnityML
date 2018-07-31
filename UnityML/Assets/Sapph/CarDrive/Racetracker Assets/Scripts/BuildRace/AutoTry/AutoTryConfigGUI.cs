using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoTryConfigGUI : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private Canvas canvasRightPanel;
    [SerializeField]
    private AutoDrive autoDrive;
    [SerializeField]
    private Slider sliderSteerAngle;
    [SerializeField]
    private Slider sliderSpeed;
    [SerializeField]
    private InputField inputSteerAngle;
    [SerializeField]
    private InputField inputSpeed;
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Button buttonRightClose;
    [SerializeField]
    private Button buttonRightOpen;



    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        inputSpeed.text = System.Convert.ToString(sliderSpeed.value);
        inputSteerAngle.text = System.Convert.ToString(sliderSteerAngle.value);
    }

    public void CheckboxSelectionChange()
    {
        autoDrive.DriveMode = (AutoDriveMode)dropdown.value;
    }

    public void SliderChange()
    {
        autoDrive.Speed = sliderSpeed.value;
        autoDrive.SteerAngle = sliderSteerAngle.value;
    }

    public void ButtonRightOpenClick()
    {
        canvasRightPanel.enabled = true;
        buttonRightClose.enabled = true;
        buttonRightClose.GetComponentInChildren<Text>().enabled = true;
        buttonRightClose.GetComponent<Image>().enabled = true;
        buttonRightOpen.enabled = false;
        buttonRightOpen.GetComponentInChildren<Text>().enabled = false;
        buttonRightOpen.GetComponent<Image>().enabled = false;
    }

    public void ButtonRightCloseClick()
    {
        canvasRightPanel.enabled = false;
        buttonRightClose.enabled = false;
        buttonRightClose.GetComponentInChildren<Text>().enabled = false;
        buttonRightClose.GetComponent<Image>().enabled = false;
        buttonRightOpen.enabled = true;
        buttonRightOpen.GetComponentInChildren<Text>().enabled = true;
        buttonRightOpen.GetComponent<Image>().enabled = true;
    }
}
