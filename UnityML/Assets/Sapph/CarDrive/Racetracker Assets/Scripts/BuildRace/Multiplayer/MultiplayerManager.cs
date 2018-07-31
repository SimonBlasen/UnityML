using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject testCar;
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private Network network;
    [SerializeField]
    private Canvas canvasMenu;
    [SerializeField]
    public Timekeeper timeKeeper;
    [SerializeField]
    public Text bestLaptimeText;
    [SerializeField]
    public Text lastLaptimeText;
    [SerializeField]
    public Text currLaptimeText;
    [SerializeField]
    public Minimap miniMap;
    [SerializeField]
    private PosInterpolator posInterpol;

    [Header("Settings")]
    [SerializeField]
    private float startYRotation = 0f;
    [SerializeField]
    private float otherPlayersInfoRefreshRate = 5f;
    [SerializeField]
    private float posSendRate = 0.1f;


    private float infoRefreshCounter = 0f;
    private float posSendCounter = 0f;

    [Header("Prefafbs")]
    [SerializeField]
    private GameObject[] maps;
    [SerializeField]
    private GameObject prefabOtherCar;

    private bool initialized = false;
    private byte ownID;
    private bool menuOpened = false;

    public RacetrackBR map = null;

    private ChatCommands commands;

    public OtherPlayer[] players;
    public GameObject[] otherPlayers;
    public Vector4[] possesToLerpTp;

    private bool[] beforeShootings;

    void Start()
    {
        ConnectorFile connFile = FileLoader.LoadConnFile(".\\model" + Level1.garageIndex + ".connb");
        CarPropertiesSetting carProps = FileLoader.LoadSettingFromFile(connFile.PropertiesFile);

        testCar.GetComponent<TestCar>().Freeze = true;
        testCar.GetComponent<TestCar>().ApplyCar(Racing.calculatedCarNew, carProps);
        testCar.transform.position = startPosition.position;

        beforeShootings = new bool[testCar.GetComponent<TestCar>().ShootingWeapon.Length];

        if (miniMap != null)
        {
            miniMap.AddObject(testCar, IpAndPort.playername);
        }

        players = new OtherPlayer[255];
        otherPlayers = new GameObject[255];
        

        //Instantiate(maps[0]);
    }

    // Update is called once per frame
    void Update()
    {
        infoRefreshCounter += Time.deltaTime;
        posSendCounter += Time.deltaTime;

        if (infoRefreshCounter >= otherPlayersInfoRefreshRate)
        {
            infoRefreshCounter = 0f;

            for (int i = 0; i < 255; i++)
            {
                if (players[i] != null)
                {

                    if (otherPlayers[i] == null && ownID != i)
                    {
                        otherPlayers[i] = (GameObject)Instantiate(prefabOtherCar, Vector3.zero, Quaternion.identity);
                        otherPlayers[i].GetComponent<OtherCar>().ID = (byte)i;
                        if (miniMap != null)
                        {
                            miniMap.AddObject(otherPlayers[i], players[i].PlayerName);
                        }

                    }

                    if (ownID != i)
                    {
                        OtherCar otherCar = otherPlayers[i].GetComponent<OtherCar>();
                        if (otherCar.Model3DLoaded == false && ownID != i)
                        {
                            network.SendRequestCar(ownID, (byte)i);
                        }
                    }
                }
                else
                {

                }
            }
        }

        if (posSendCounter >= posSendRate)
        {
            network.SendOwnPosition(ownID, testCar.transform.position, testCar.transform.eulerAngles.x, testCar.transform.eulerAngles.y, testCar.transform.eulerAngles.z, 0f, testCar.GetComponent<Rigidbody>().velocity);
        }


        for (byte i = 0; i < 255; i++)
        {
            if (otherPlayers[i] != null)
            {
                otherPlayers[i].transform.position = posInterpol.GetInfoPosition(i);
                otherPlayers[i].transform.rotation = Quaternion.Euler(posInterpol.GetInfoRotation(i));
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openMenu(!menuOpened);
        }

        for (int i = 0; i < testCar.GetComponent<TestCar>().ShootingWeapon.Length; i++)
        {
            if (testCar.GetComponent<TestCar>().ShootingWeapon[i] != beforeShootings[i])
            {
                network.SendMachinegunButtonPress(ownID, (byte)i, testCar.GetComponent<TestCar>().ShootingWeapon[i]);
                beforeShootings[i] = testCar.GetComponent<TestCar>().ShootingWeapon[i];
            }
        }

    }

    public byte OwnID
    {
        get
        {
            return ownID;
        }
        set
        {
            ownID = value;
            testCar.GetComponent<TestCar>().ID = ownID;
        }
    }

    public PosInterpolator PosInterpol
    {
        get
        {
            return posInterpol;
        }
    }

    private void ChatSystem_TextEntered(string inputText)
    {
        if (inputText.Length > 0 && inputText.Substring(0, 1) == "/")
        {
            commands.CommandExec(inputText.Substring(1));
        }
        else if (inputText.Length > 0)
        {
            network.SendChatMessage(ownID, inputText);
        }
    }

    public void SetMapNumber(byte mapNumber)
    {
        map = ((GameObject)Instantiate(maps[mapNumber])).GetComponent<RacetrackBR>();
        
        testCar.GetComponent<TestCar>().Freeze = false;

        byte[] carBytes = File.ReadAllBytes(".\\model" + Level1.garageIndex + ".baguette");

        network.SendOwnCar(ownID, carBytes);
    }

    public void SetPlayerName(byte playersID, string name)
    {
        if (players[playersID] == null)
        {
            players[playersID] = new OtherPlayer();
            players[playersID].PlayerName = name;
        }
        else
        {
            if (players[playersID].PlayerName != name)
            {
                players[playersID].PlayerName = name;
            }
        }
    }

    public void SetPlayerCarBytes(byte playerID, byte[] carBytes)
    {
        CalculatedCar calculatedCar = Analyzer.AnalyzeCar(carBytes);
        if (otherPlayers[playerID] != null && calculatedCar != null)
        {
            OtherCar otherCar = otherPlayers[playerID].GetComponent<OtherCar>();
            otherCar.TestCar = testCar;
            otherCar.ApplyCar(calculatedCar);
            otherCar.PlayerDisplayname = players[playerID].PlayerName;
            Debug.Log("The car was applied");
        }
        else
        {
            if (calculatedCar == null)
            {
                Debug.Log("calculatedCar was null");
            }
            else
            {
                Debug.Log("otherPlayers was null");
            }
        }
    }

    public void SetOtherShooting(byte otherPlayersID, int weapon, bool buttonDown)
    {
        if (otherPlayers[otherPlayersID] != null)
        {
            otherPlayers[otherPlayersID].GetComponent<OtherCar>().SetShooting(weapon, buttonDown);
        }
    }

    public void DoDamageToPlayer(byte playerID, float damage)
    {
        if (playerID == ownID)
        {
            testCar.GetComponent<TestCar>().Damage(damage);
        }
        else
        {
            if (otherPlayers[playerID] != null)
            {
                otherPlayers[playerID].GetComponent<OtherCar>().Damage(damage);
            }
        }
    }

    public void RemovePlayer(byte playerID)
    {
        if (otherPlayers[playerID] != null)
        {
            Destroy(otherPlayers[playerID]);
        }

        if (miniMap != null)
        {
            miniMap.RemoveObject(players[playerID].PlayerName);
        }

        otherPlayers[playerID] = null;
        players[playerID] = null;
    }

    public bool Initiated
    {
        get
        {
            return false;
        }
        set
        {
            initialized = value;

            network.SendRequestMapNumber(ownID);
        }
    }

    private void openMenu(bool opened)
    {
        canvasMenu.enabled = opened;
        menuOpened = opened;

        testCar.GetComponent<CarMoveScript>().enabled = !menuOpened;

    }

    public void ButtonClickResume()
    {
        if (menuOpened)
        {
            openMenu(false);
        }
    }

    public void ButtonClickBacktoBox()
    {
        if (menuOpened)
        {
            testCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
            testCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            if (timeKeeper != null)
            {
                timeKeeper.StopCurrentMeasurement();
            }
        }
    }

    public Network Network
    {
        get
        {
            return network;
        }
    }

    public void ButtonClickBacktoMainmenu()
    {
        if (menuOpened)
        {
            network.SendDisconnect(ownID);

            SceneManager.LoadScene("Garages");
        }
    }
}
