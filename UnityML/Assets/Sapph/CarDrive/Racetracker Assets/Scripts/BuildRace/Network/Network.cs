using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum ReceivingState
{
    IDLE, AIRLIST, FARCHUNKS, WORLD, INTERACTIVES
}

public class Network : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private MultiplayerManager manager;

    private SappNetwork network;

    private bool started = false;

    private bool sendingEnabled = false;

    private List<byte> longBuffer = new List<byte>();
    private ReceivingState recState = ReceivingState.IDLE;
    private int longBufferAmount = 0;

	// Use this for initialization
	void Start ()
    {
        network = new SappNetwork();

        //string ip = "192.168.178.63";
        //int port = 24248;

        string ip = IpAndPort.ip;
        int port = IpAndPort.port;

        //network.Connect("m.m-core.eu", 24242);
        if (ip != "null" && port != -1)
        {
            network.Connect(ip, port, port + 1);
        }

        //network.SendUdp(System.Text.Encoding.ASCII.GetBytes("Hallo ddudududuu"));

        //sendingEnabled = true;
        started = true;
	}

    void SendOwnUDPPortDelayed()
    {
        SendOwnUDPPort(manager.OwnID);
    }

    void Update ()
    {
	    if (network.HasMessage)
        {
            byte[] data = network.GetMessage();
            if (data == null)
            {
                Debug.Log("Received NULL");
            }
            else
            {
                Debug.Log("Received with length: " + data.Length);

                // Receiving own ID
                if (data.Length == 3 && data[0] == 0 && data[1] == 5)
                {
                    byte ownID = data[2];
                    manager.OwnID = ownID;

                    SendOwnUDPPort(ownID);
                    Invoke("SendOwnUDPPortDelayed", 1);
                    Invoke("SendOwnUDPPortDelayed", 2);

                    //Invoke("WaitShort2", 3);

                    SendOwnUsername(ownID, IpAndPort.playername);

                    sendingEnabled = true;


                    manager.Initiated = true;
                }

                // Receiving map number
                if (data.Length == 3 && data[0] == 0 && data[1] == 9)
                {
                    byte mapNumber = data[2];

                    manager.SetMapNumber(mapNumber);
                }

                // Receiving playerlist
                if (data.Length >= 2 && data[0] == 0 && data[1] == 3)
                {
                    int counter = 2;
                    while (counter < data.Length)
                    {
                        byte playerID = data[counter];
                        int nameLength = (data[counter + 1]);
                        string name = System.Text.Encoding.Default.GetString(data, counter + 2, nameLength);

                        manager.SetPlayerName(playerID, name);

                        counter += nameLength + 2;
                    }
                }

                // Receiving other players car
                if (data.Length >= 2 && data[0] == 0 && data[1] == 7)
                {
                    byte playerID = data[2];
                    int byteLength = (data[3] << 24) | (data[4] << 16) | (data[5] << 8) | (data[6]);
                    byte[] carBytes = new byte[byteLength];
                    for (int i = 0; i < carBytes.Length; i++)
                    {
                        carBytes[i] = data[i + 7];
                    }
                    manager.SetPlayerCarBytes(playerID, carBytes);
                }

                // Player disconnected
                if (data.Length >= 2 && data[0] == 0 && data[1] == 22)
                {
                    byte playerID = data[2];

                    manager.RemovePlayer(playerID);
                }

                // Machinegun Button Press
                if (data.Length >= 2 && data[0] == 0 && data[1] == 25)
                {
                    byte playerID = data[2];
                    byte buttonID = data[3];
                    bool buttonDown = data[4] == 1 ? true : false;

                    manager.SetOtherShooting(playerID, buttonID, buttonDown);
                }

                // Damage Player
                if (data.Length >= 2 && data[0] == 0 && data[1] == 28)
                {
                    byte playerDoingDamageID = data[2];
                    byte playerReceivingDamageID = data[3];

                    float damage = System.BitConverter.ToSingle(data, 4);

                    manager.DoDamageToPlayer(playerReceivingDamageID, damage);
                }

                // Bullet Impact Spawn
                if (data.Length >= 2 && data[0] == 0 && data[1] == 27)
                {
                    byte playerDoingDamageID = data[2];
                    byte weaponID = data[3];

                    float xPos = System.BitConverter.ToSingle(data, 4);
                    float yPos = System.BitConverter.ToSingle(data, 8);
                    float zPos = System.BitConverter.ToSingle(data, 12);

                    //manager.SetOtherShooting(playerID, buttonID, buttonDown);
                }
            }
        }

        if (network.HasUdpMessage)
        {
            byte[] data = network.GetUdpMessage();
            if (data == null)
            {
                Debug.Log("Received NULL UDP");
            }
            else
            {
                //Debug.Log("Received UDP with length: " + data.Length);

                // Receiving positions
                if (data.Length >= 3 && data[0] == 0 && data[1] == 14)
                {
                    for (int i = 2; i < data.Length; i += 41)
                    {
                        byte playersID = data[i];

                        float xPos = System.BitConverter.ToSingle(data, i + 1);
                        float yPos = System.BitConverter.ToSingle(data, i + 5);
                        float zPos = System.BitConverter.ToSingle(data, i + 9);
                        float xRot = System.BitConverter.ToSingle(data, i + 13);
                        float yRot = System.BitConverter.ToSingle(data, i + 17);
                        float zRot = System.BitConverter.ToSingle(data, i + 21);
                        float wRot = System.BitConverter.ToSingle(data, i + 25);
                        float xVel = System.BitConverter.ToSingle(data, i + 29);
                        float yVel = System.BitConverter.ToSingle(data, i + 33);
                        float zVel = System.BitConverter.ToSingle(data, i + 37);

                        if (manager.otherPlayers != null && manager.otherPlayers[playersID] != null && manager.otherPlayers[playersID].GetComponent<OtherCar>().Model3DLoaded)
                        {
                            //manager.otherPlayers[playersID].transform.position = Vector3.Lerp(manager.otherPlayers[playersID].transform.position, new Vector3(xPos, yPos, zPos), manager.lerpRate);
                            manager.otherPlayers[playersID].transform.position = new Vector3(xPos, yPos, zPos);
                            //manager.possesToLerpTp[playersID] = new Vector4(xPos, yPos, zPos, Mathf.Sqrt(wRot));
                            //Debug.Log("Velocity: " + wRot);
                            manager.otherPlayers[playersID].transform.rotation = Quaternion.Euler(xRot, yRot, zRot);

                            manager.otherPlayers[playersID].GetComponent<OtherCar>().Velocity = new Vector3(xVel, yVel, zVel);

                            manager.PosInterpol.AddInformation(playersID, new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot), new Vector3(xVel, yVel, zVel));
                        }
                    }
                }
            }
        }
	}

    public void SendChatMessage(byte ownID, string message)
    {

    }
    
    public void SendDisconnect(byte ownID)
    {
        byte[] bytes = new byte[3];
        bytes[0] = 0;
        bytes[1] = 21;
        bytes[2] = ownID;
        network.Send(bytes);
    }

    public void SendRequestMapNumber(byte ownID)
    {
        byte[] bytes = new byte[3];
        bytes[0] = 0;
        bytes[1] = 8;
        bytes[2] = ownID;
        network.Send(bytes);
    }

    public void SendRequestCar(byte ownID, byte otherID)
    {
        byte[] bytes = new byte[4];
        bytes[0] = 0;
        bytes[1] = 4;
        bytes[2] = ownID;
        bytes[3] = otherID;
        network.Send(bytes);
    }

    public void SendOwnCar(byte ownID, byte[] carBytes)
    {
        byte[] bytes = new byte[7 + carBytes.Length];
        bytes[0] = 0;
        bytes[1] = 1;
        bytes[2] = ownID;
        bytes[3] = (byte)(carBytes.Length >> 24);
        bytes[4] = (byte)(carBytes.Length >> 16);
        bytes[5] = (byte)(carBytes.Length >> 8);
        bytes[6] = (byte)(carBytes.Length);
        for (int i = 0; i < carBytes.Length; i++)
        {
            bytes[i + 7] = carBytes[i];
        }
        network.Send(bytes);
    }

    public void SendOwnPosition(byte ownID, Vector3 position, float xRot, float yRot, float zRot, float wRot, Vector3 vel)
    {
        //byte[] xBytes = System.BitConverter.GetBytes(weaponDrops[i].Position.x);
        //float playerX = System.BitConverter.ToSingle(message, 2);

        byte[] xBytes = System.BitConverter.GetBytes(position.x);
        byte[] yBytes = System.BitConverter.GetBytes(position.y);
        byte[] zBytes = System.BitConverter.GetBytes(position.z);
        byte[] xRotBytes = System.BitConverter.GetBytes(xRot);
        byte[] yRotBytes = System.BitConverter.GetBytes(yRot);
        byte[] zRotBytes = System.BitConverter.GetBytes(zRot);
        byte[] wRotBytes = System.BitConverter.GetBytes(wRot);
        byte[] xVel = System.BitConverter.GetBytes(vel.x);
        byte[] yVel = System.BitConverter.GetBytes(vel.y);
        byte[] zVel = System.BitConverter.GetBytes(vel.z);

        byte[] bytes = new byte[43];
        bytes[0] = 0;
        bytes[1] = 13;
        bytes[2] = ownID;
        bytes[3] = xBytes[0];
        bytes[4] = xBytes[1];
        bytes[5] = xBytes[2];
        bytes[6] = xBytes[3];
        bytes[7] = yBytes[0];
        bytes[8] = yBytes[1];
        bytes[9] = yBytes[2];
        bytes[10] = yBytes[3];
        bytes[11] = zBytes[0];
        bytes[12] = zBytes[1];
        bytes[13] = zBytes[2];
        bytes[14] = zBytes[3];
        bytes[15] = xRotBytes[0];
        bytes[16] = xRotBytes[1];
        bytes[17] = xRotBytes[2];
        bytes[18] = xRotBytes[3];
        bytes[19] = yRotBytes[0];
        bytes[20] = yRotBytes[1];
        bytes[21] = yRotBytes[2];
        bytes[22] = yRotBytes[3];
        bytes[23] = zRotBytes[0];
        bytes[24] = zRotBytes[1];
        bytes[25] = zRotBytes[2];
        bytes[26] = zRotBytes[3];
        bytes[27] = wRotBytes[0];
        bytes[28] = wRotBytes[1];
        bytes[29] = wRotBytes[2];
        bytes[30] = wRotBytes[3];
        bytes[31] = xVel[0];
        bytes[32] = xVel[1];
        bytes[33] = xVel[2];
        bytes[34] = xVel[3];
        bytes[35] = yVel[0];
        bytes[36] = yVel[1];
        bytes[37] = yVel[2];
        bytes[38] = yVel[3];
        bytes[39] = zVel[0];
        bytes[40] = zVel[1];
        bytes[41] = zVel[2];
        bytes[42] = zVel[3];

        network.SendUdp(bytes);
    }

    public void SendOwnUsername(byte ownID, string username)
    {
        List<byte> bytes = new List<byte>();
        bytes.Add(0);
        bytes.Add(2);
        bytes.Add(ownID);
        //System.Text.Encoding.Default.GetString(sub);
        byte[] name = System.Text.Encoding.UTF8.GetBytes(username);
        bytes.AddRange(name);

        network.Send(bytes.ToArray());
    }

    public void SendOwnUDPPort(byte playerID)
    {
        byte[] bytes = new byte[3];
        bytes[0] = (byte)(0);
        bytes[1] = (byte)(6);
        bytes[2] = playerID;

        network.SendUdp(bytes);
    }

    public void SendMachinegunButtonPress(byte ownID, byte buttonID, bool isDown)
    {
        byte[] bytes = new byte[5];
        bytes[0] = 0;
        bytes[1] = 23;
        bytes[2] = ownID;
        bytes[3] = buttonID;
        bytes[4] = isDown ? (byte)1 : (byte)0;

        network.Send(bytes);
    }

    public void SendDamagePlayer(byte ownID, byte damagedPlayerID, float damage)
    {
        byte[] damageBytes = System.BitConverter.GetBytes(damage);

        byte[] bytes = new byte[8];
        bytes[0] = 0;
        bytes[1] = 24;
        bytes[2] = ownID;
        bytes[3] = damagedPlayerID;
        bytes[4] = damageBytes[0];
        bytes[5] = damageBytes[1];
        bytes[6] = damageBytes[2];
        bytes[7] = damageBytes[3];

        network.Send(bytes);
    }

    public void SendShootMinigun(byte ownID, Vector3 spawnPos, Vector3 direction)
    {
        byte[] xBytes = System.BitConverter.GetBytes(spawnPos.x);
        byte[] yBytes = System.BitConverter.GetBytes(spawnPos.y);
        byte[] zBytes = System.BitConverter.GetBytes(spawnPos.z);
        byte[] xVel = System.BitConverter.GetBytes(direction.x);
        byte[] yVel = System.BitConverter.GetBytes(direction.y);
        byte[] zVel = System.BitConverter.GetBytes(direction.z);

        byte[] bytes = new byte[27];
        bytes[0] = 0;
        bytes[1] = 1;
        bytes[2] = ownID;
        bytes[3] = xBytes[0];
        bytes[4] = xBytes[1];
        bytes[5] = xBytes[2];
        bytes[6] = xBytes[3];
        bytes[7] = yBytes[0];
        bytes[8] = yBytes[1];
        bytes[9] = yBytes[2];
        bytes[10] = yBytes[3];
        bytes[11] = zBytes[0];
        bytes[12] = zBytes[1];
        bytes[13] = zBytes[2];
        bytes[14] = zBytes[3];
        bytes[15] = xVel[0];
        bytes[16] = xVel[1];
        bytes[17] = xVel[2];
        bytes[18] = xVel[3];
        bytes[19] = yVel[0];
        bytes[20] = yVel[1];
        bytes[21] = yVel[2];
        bytes[22] = yVel[3];
        bytes[23] = zVel[0];
        bytes[24] = zVel[1];
        bytes[25] = zVel[2];
        bytes[26] = zVel[3];

        network.SendUdp(bytes);
    }

    public void SendSpawnBulletImpact(byte ownID, Vector3 position, byte weapon)
    {
        byte[] posXBytes = System.BitConverter.GetBytes(position.x);
        byte[] posYBytes = System.BitConverter.GetBytes(position.y);
        byte[] posZBytes = System.BitConverter.GetBytes(position.z);

        byte[] bytes = new byte[16];
        bytes[0] = 0;
        bytes[1] = 26;
        bytes[2] = ownID;
        bytes[3] = weapon;
        bytes[4] = posXBytes[0];
        bytes[5] = posXBytes[1];
        bytes[6] = posXBytes[2];
        bytes[7] = posXBytes[3];
        bytes[8] = posYBytes[0];
        bytes[9] = posYBytes[1];
        bytes[10] = posYBytes[2];
        bytes[11] = posYBytes[3];
        bytes[12] = posZBytes[0];
        bytes[13] = posZBytes[1];
        bytes[14] = posZBytes[2];
        bytes[15] = posZBytes[3];

        network.Send(bytes);
    }

    




    private byte[] decat(byte[] data)
    {
        if (data.Length > 2)
        {
            byte[] newdata = new byte[data.Length - 2];

            for (int j = 0; j < newdata.Length; j++)
            {
                newdata[j] = data[j + 2];
            }

            return newdata;
        }
        else
        {
            return null;
        }
    }

    private byte[] concat(short pre, byte[] data)
    {
        byte[] newdata = new byte[data.Length + 2];
        newdata[0] = (byte)(pre >> 8);
        newdata[1] = (byte)pre;

        for (int j = 0; j < data.Length; j++)
        {
            newdata[j + 2] = data[j];
        }

        return newdata;
    }

    public bool SendingEnable
    {
        get
        {
            return sendingEnabled;
        }
        set
        {
            sendingEnabled = value;
        }
    }

    private string getNameFromByte(byte[] rawData)
    {
        byte[] sub = new byte[rawData.Length - 2];
        for (int i = 0; i < sub.Length; i++)
        {
            sub[i] = rawData[i + 2];
        }
        return System.Text.Encoding.Default.GetString(sub);
    }
}
