using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Net;
using System.IO;

public class Client
{
    const int sleepTime = 1;

    public delegate void RecieveMessage(Client sender, string data, byte[] datas);
    public event RecieveMessage RecieveData;

    private Thread thread;
    public Socket socket = null;
    private string address;
    private int port;

    public Client(string address, int port)
    {
        this.address = address;
        this.port = port;

        thread = new Thread(new ThreadStart(Process));
        thread.Start();
    }

    /*
     * returns the amount of transfered data
     * */
    public int send(Byte[] data)
    {
        try
        {
            return socket.Send(data);
        }
        catch
        {
            return 0;
        }
    }

    /*
     * returns the amount of transfered data
     * */
    public int send(string message)
    {
        Byte[] bytes = new byte[message.Length];
        char[] chars = message.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            bytes[i] = Convert.ToByte(chars[i]);
        }
        return this.send(bytes);
    }

    public bool Connect()
    {
        try
        {
            IPHostEntry hostInfo = Dns.GetHostByName(address);
            System.Net.IPEndPoint ep = new System.Net.IPEndPoint(hostInfo.AddressList[0], port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ep);
            thread.Start();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void Disconnect()
    {
        if (socket != null)
            socket.Close();
        socket = null;
        if (thread != null)
            thread.Abort();
    }

    private void Process()
    {
        int cnt = 0;
        MemoryStream mem = new MemoryStream();// Empfangspuffer
        byte[] buffer = new byte[5];
        while (socket == null)
        {

        }
        while (true)
        {
            while (socket.Available > 0)
            {
                int bytesRead = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (bytesRead <= 0) continue;
                mem.Write(buffer, 0, bytesRead);
            }
            if (mem.Length > 0 && socket.Available == 0)
            {
                RecieveData(this, /*System.Text.Encoding.ASCII.GetString(mem.ToArray(), 0, mem.ToArray().Length)*/"", mem.ToArray());
                //form.Invoke(new MethodInvoker(delegate () { RecieveData(this, System.Text.Encoding.ASCII.GetString(mem.ToArray(), 0, mem.ToArray().Length), mem.ToArray()); }));
                mem.Seek(0, SeekOrigin.Begin);
                mem.SetLength(0);
            }
            else
            {
                cnt++;
            }
            Thread.Sleep(sleepTime);
        }
    }
}
