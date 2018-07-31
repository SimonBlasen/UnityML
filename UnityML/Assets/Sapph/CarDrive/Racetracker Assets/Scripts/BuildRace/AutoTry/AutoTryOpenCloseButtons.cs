using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoTryOpenCloseButtons : MonoBehaviour
{
    [SerializeField]
    private Button buttonClose;
    [SerializeField]
    private Button buttonOpen;
    [SerializeField]
    private GUIPropertiesManager gui;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ButtonCloseClick()
    {
        gui.GUIShown = false;
        buttonOpen.enabled = true;
        buttonOpen.GetComponentInChildren<Text>().enabled = true;
        buttonOpen.GetComponent<Image>().enabled = true;
        buttonClose.enabled = false;
        buttonClose.GetComponentInChildren<Text>().enabled = false;
        buttonClose.GetComponent<Image>().enabled = false;
    }

    public void ButtonOpenClick()
    {
        gui.GUIShown = true;
        buttonClose.enabled = true;
        buttonClose.GetComponentInChildren<Text>().enabled = true;
        buttonClose.GetComponent<Image>().enabled = true;
        buttonOpen.enabled = false;
        buttonOpen.GetComponentInChildren<Text>().enabled = false;
        buttonOpen.GetComponent<Image>().enabled = false;
    }
}
