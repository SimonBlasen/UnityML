using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SappNetwork {

    private Client server;
    private UdpClient udpServer;

    private List<Message> messages;
    private List<Message> udpMessages;

    public delegate void RecievedMessage(byte[] datas);
    public event RecievedMessage ReceiveMessage;
    

    public SappNetwork()
    {
        messages = new List<Message>();
        udpMessages = new List<Message>();
    }

    public bool Connect(string ip, int port)
    {
        server = new Client(ip, port);
        //udpServer = new UdpClient(ip, port);

        server.RecieveData += Server_RecieveData;
        //udpServer.RecieveUdpData += UdpServer_RecieveUdpData;

        return server.Connect();
    }

    public bool Connect(string ip, int port, int udpPort)
    {
        server = new Client(ip, port);
        udpServer = new UdpClient(ip, udpPort);

        server.Connect();

        server.RecieveData += Server_RecieveData;
        udpServer.RecieveUdpData += UdpServer_RecieveUdpData;
        return true;
        //return server.Connect();
    }

    public bool ConnectOnlyUdp(string ip, int udpPort)
    {
        udpServer = new UdpClient(ip, udpPort);
        
        udpServer.RecieveUdpData += UdpServer_RecieveUdpData;
        return true;
    }

    private void UdpServer_RecieveUdpData(byte[] datas)
    {
        bool okay = true;

        okay = okay & (datas.Length > 2);

        if (okay)
        {
            int messageLength = 0;

            byte[] splitData;
            int offset = 0;
            do
            {
                messageLength = (datas[offset] << 8) | datas[offset + 1];
                splitData = new byte[messageLength];
                for (int i = 0; i < messageLength; i++)
                {
                    splitData[i] = datas[offset + i + 2];
                }

                //ReceiveMessage(splitData);
                udpMessages.Add(new Message(splitData));

                offset += messageLength + 2;

            } while (offset < datas.Length);
        }
    }

    public void Disconnect()
    {
        server.Disconnect();
    }

    public bool HasMessage
    {
        get
        {
            return messages.Count > 0;
        }
    }

    public bool HasUdpMessage
    {
        get
        {
            return udpMessages.Count > 0;
        }
    }

    public byte[] GetMessage()
    {
        if (HasMessage)
        {
            byte[] data;
            if (messages[0] != null && messages[0].data != null)
                data = messages[0].data;
            else
                data = null;

            messages.RemoveAt(0);

            return data;
        }
        else
        {
            return null;
        }
    }

    public byte[] GetUdpMessage()
    {
        if (HasUdpMessage)
        {
            byte[] data;
            if (udpMessages[0] != null && udpMessages[0].data != null)
                data = udpMessages[0].data;
            else
                data = null;

            udpMessages.RemoveAt(0);

            return data;
        }
        else
        {
            return null;
        }
    }

    public void Send(byte[] data)
    {
        byte[] dataWithLength = new byte[data.Length + 2];
        dataWithLength[0] = (byte)(data.Length >> 8);
        dataWithLength[1] = (byte)data.Length;
        for (int i = 0; i < data.Length; i++)
        {
            dataWithLength[i + 2] = data[i];
        }

        //TODO included if for singleplayer
        if (server != null)
            server.send(dataWithLength);
    }

    public void SendUdp(byte[] data)
    {
        /*byte[] dataWithLength = new byte[data.Length + 2];
        dataWithLength[0] = (byte)(data.Length >> 8);
        dataWithLength[1] = (byte)data.Length;
        for (int i = 0; i < data.Length; i++)
        {
            dataWithLength[i + 2] = data[i];
        }*/

        //TODO included if for singleplayer
        if (udpServer != null)
            udpServer.Send(data);
    }

    private void Server_RecieveData(Client sender, string data, byte[] datas)
    {
        if (lengthing)
        {
            bool okay = true;

            okay = okay & (datas.Length > 2);

            if (okay)
            {
                int messageLength = 0;

                byte[] splitData;
                int offset = 0;
                do
                {
                    messageLength = (datas[offset] << 8) | datas[offset + 1];
                    splitData = new byte[messageLength];
                    for (int i = 0; i < messageLength; i++)
                    {
                        splitData[i] = datas[offset + i + 2];
                    }

                    //ReceiveMessage(splitData);
                    messages.Add(new Message(splitData));

                    offset += messageLength + 2;

                } while (offset < datas.Length);
            }
        }
        else
        {
            messages.Add(new Message(datas));
        }
    }

    private bool lengthing = true;

    public bool EnableLengthen
    {
        get
        {
            return lengthing;
        }
        set
        {
            lengthing = value;
        }
    }
}
