using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject carObject;

    [Header("Settings")]
    [SerializeField]
    private float carObjectsHeight = 8000f;

    [Header("References")]
    [SerializeField]
    private MultiplayerManager manager;
    [SerializeField]
    private GameObject topDownCamera;

    private Vector3 mapMiddle = Vector3.zero;
    private float xWidth = 0f;
    private float zWidth = 0f;
    private List<GameObject> gos = new List<GameObject>();
    private List<string> names = new List<string>();

    private List<GameObject> instCars = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mapMiddle != Vector3.zero || xWidth != 0f || zWidth != 0f)
        {
            for (int i = 0; i < gos.Count; i++)
            {
                instCars[i].transform.position = new Vector3(gos[i].transform.position.x, carObjectsHeight, gos[i].transform.position.z);
            }
        }
        else
        {
            if (manager.map != null)
            {
                mapMiddle = manager.map.trackMiddle;
                xWidth = manager.map.trackWidthX;
                zWidth = manager.map.trackWidthZ;
                topDownCamera.transform.position = new Vector3(mapMiddle.x, carObjectsHeight + 1000f, mapMiddle.z);
                topDownCamera.GetComponent<Camera>().orthographicSize = Mathf.Max(xWidth, zWidth) * 0.5f;
            }
        }
	}

    public void AddObject(GameObject obj, string name)
    {
        gos.Add(obj);
        names.Add(name);
        GameObject instCar = (GameObject)Instantiate(carObject, new Vector3(0f, carObjectsHeight, 0f), Quaternion.identity, transform);
        instCars.Add(instCar);
    }

    public void RemoveObject(string name)
    {
        int index = -1;
        for (int i = 0; i < names.Count; i++)
        {
            if (name == names[i])
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            gos.RemoveAt(index);
            names.RemoveAt(index);
            Destroy(instCars[index]);
            instCars.RemoveAt(index);
        }
    }

    public Vector3 MapMiddle
    {
        get
        {
            return mapMiddle;
        }
        set
        {
            mapMiddle = value;
        }
    }

    public float MapXWidth
    {
        get
        {
            return xWidth;
        }
        set
        {
            xWidth = value;
        }
    }

    public float MapZWidth
    {
        get
        {
            return zWidth;
        }
        set
        {
            zWidth = value;
        }
    }
}
