using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenParts : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TestCar car = null;
    [SerializeField]
    private OtherCar otherCar = null;

    private List<PartConfiguration> brokenParts = new List<PartConfiguration>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddBrokenPart(PartConfiguration part)
    {
        brokenParts.Add(part);
    }
}
