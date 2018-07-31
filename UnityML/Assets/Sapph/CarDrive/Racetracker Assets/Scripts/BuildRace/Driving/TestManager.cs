using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject testCar;
    [SerializeField]
    private Transform startPosition;

    [Header("Settings")]
    [SerializeField]
    private float startYRotation = 0f;


    // Use this for initialization
    void Start ()
    {
        testCar.GetComponent<TestCar>().Freeze = true;


        Racing.calculatedCarNew = Analyzer.AnalyzeCar(".\\model" + 3 + ".baguette");

        testCar.GetComponent<TestCar>().ApplyCar(Racing.calculatedCarNew);
        testCar.transform.position = startPosition.position;
        //testCar.transform.rotation = Quaternion.Euler(0, startYRotation, 0);

        testCar.GetComponent<TestCar>().Freeze = false;
        Debug.Log("Car unfreezed");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Garages");
        }
    }
}
