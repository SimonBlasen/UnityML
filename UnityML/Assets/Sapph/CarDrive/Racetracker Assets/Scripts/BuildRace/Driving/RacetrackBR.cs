using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacetrackBR : MonoBehaviour
{
    public Transform[] playerSpawnPositions;
    public Transform[] playerRaceStartPositions;
    public TextMesh[] boxTexts;

    public Vector3 trackMiddle = Vector3.zero;
    public float trackWidthX = 0f;
    public float trackWidthZ = 0f;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject checkpointsPrefab;

    [Header("For Singleplayer Test")]
    [SerializeField]
    private Text currentLapText;
    [SerializeField]
    private Text lastLapText;
    [SerializeField]
    private Text bestLapText;

    private GameObject instTimekeeper = null;


    // Use this for initialization
    void Start()
    {
        if (checkpointsPrefab != null)
        {
            instTimekeeper = (GameObject)Instantiate(checkpointsPrefab, Vector3.zero, Quaternion.identity);
            if (GameObject.Find("Multiplayer Manager") != null)
            {
                TimeKeeper.currentLapText = GameObject.Find("Multiplayer Manager").GetComponent<MultiplayerManager>().currLaptimeText;
                TimeKeeper.lastLapText = GameObject.Find("Multiplayer Manager").GetComponent<MultiplayerManager>().lastLaptimeText;
                TimeKeeper.bestLapText = GameObject.Find("Multiplayer Manager").GetComponent<MultiplayerManager>().bestLaptimeText;
                GameObject.Find("Multiplayer Manager").GetComponent<MultiplayerManager>().timeKeeper = TimeKeeper;
            }
            else
            {
                TimeKeeper.currentLapText = currentLapText;
                TimeKeeper.lastLapText = lastLapText;
                TimeKeeper.bestLapText = bestLapText;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Timekeeper TimeKeeper
    {
        get
        {
            if (instTimekeeper != null)
            {
                return instTimekeeper.GetComponent<Timekeeper>();
            }
            else
            {
                return null;
            }
        }
    }
}

