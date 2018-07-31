using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart : MonoBehaviour
{
    public GameObject prefabPartVisible;
    public GameObject prefabPartVisibleMulti;

    [SerializeField]
    private float health;

    private float maxHealth;

    private List<PartConfiguration> configs = new List<PartConfiguration>();

    private List<PartConfiguration> invisParts = new List<PartConfiguration>();

    private GameObject instPart;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetProperties(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position)
    {
        instPart = (GameObject)Instantiate(prefabPartVisible);
        instPart.transform.parent = transform;
        instPart.GetComponent<PartVisible>().SetRotation(rotation);
        instPart.GetComponent<PartVisible>().SetDirection(direction);
        instPart.GetComponent<PartVisible>().SetPosition(position);
        instPart.GetComponent<PartVisible>().SetPart(type);
    }

    public int AddProperties(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position)
    {
        if (instPart == null)
        {
            instPart = (GameObject)Instantiate(prefabPartVisibleMulti);
            instPart.transform.parent = transform;
            instPart.transform.position = Vector3.zero;
            instPart.tag = gameObject.tag;
        }
        instPart.GetComponent<PartVisibleMulti>().AddPart(type, position, direction, rotation);

        configs.Add(new PartConfiguration(direction, rotation, position, type));

        return instPart.GetComponent<PartVisibleMulti>().VerticesCount;
    }

    private void refreshMeshs()
    {
        instPart.GetComponent<PartVisibleMulti>().ClearComplete();
        for (int i = 0; i < configs.Count; i++)
        {
            if (!invisParts.Contains(configs[i]))
            {
                instPart.GetComponent<PartVisibleMulti>().AddPart(configs[i].partType, configs[i].partPosition, configs[i].partDirection, configs[i].partRotation, false);
            }
        }
        instPart.GetComponent<PartVisibleMulti>().RecalculateNormals();
    }

    /**
     * Returns, if the part is being found in this MovingPart
     * */
    public bool AddInvisPart(PartConfiguration part)
    {
        bool found = false;
        for (int i = 0; i < configs.Count; i++)
        {
            if (configs[i] == part)
            {
                invisParts.Add(part);
                found = true;
                break;
            }
        }

        refreshMeshs();

        return found;
    }

    public void ClearInvisParts()
    {
        invisParts.Clear();

        refreshMeshs();
    }

    private DrivingCar car;

    public DrivingCar CarReference
    {
        get
        {
            return car;
        }
        set
        {
            car = value;
            gameObject.transform.parent = car.gameObject.transform;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
}
