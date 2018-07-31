using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class UdpClient
{
    public delegate void RecieveUdpMessage(byte[] datas);
    public event RecieveUdpMessage RecieveUdpData;

    private string address;
    private int port;

    IPEndPoint ep;
    Socket socket;
    private Thread thread;

    public UdpClient(string address, int port)
    {
        this.address = address;
        this.port = port;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPHostEntry nameToIpAddress = Dns.GetHostEntry(address);
        if (nameToIpAddress.AddressList.Length > 0)
        {
            IPAddress broadcast = IPAddress.Parse(nameToIpAddress.AddressList[0].ToString());

            byte[] sendbuf = Encoding.ASCII.GetBytes("Hallo test");
            ep = new IPEndPoint(broadcast, port);

            //socket.SendTo(sendbuf, ep);

            thread = new Thread(new ThreadStart(Process));
            thread.Start();
        }
        else
        {
            //TODO 
            // Could not solve hostname
        }

    }

    public void Send(byte[] data)
    {
        //IPAddress broadcast = IPAddress.Parse(address);


        //ep = new IPEndPoint(broadcast, port);
        socket.SendTo(data, ep);
    }
    

    private void Process()
    {
        int cnt = 0;
        MemoryStream mem = new MemoryStream();// Empfangspuffer
        byte[] buffer = new byte[2048*32];
        while (socket == null)
        {

        }
        while (true)
        {
            //Debug.Log("While true");
            while (socket.Available > 0)
            {
                //Debug.Log("Went in");
                int bytesRead = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (bytesRead <= 0) continue;
                mem.Write(buffer, 0, bytesRead);
            }
            if (mem.Length > 0 && socket.Available == 0)
            {
                RecieveUdpData(mem.ToArray());
                //form.Invoke(new MethodInvoker(delegate () { RecieveData(this, System.Text.Encoding.ASCII.GetString(mem.ToArray(), 0, mem.ToArray().Length), mem.ToArray()); }));
                mem.Seek(0, SeekOrigin.Begin);
                mem.SetLength(0);
            }
            else
            {
                cnt++;
            }
            Thread.Sleep(1);
        }
    }
}
