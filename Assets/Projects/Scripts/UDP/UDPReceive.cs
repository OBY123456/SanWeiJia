using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using Newtonsoft.Json;
using MTFrame.MTEvent;
using UnityEngine.UI;

public class UDPReceive : MonoBehaviour
{
    private IPEndPoint ipEndPoint;
    private Socket socket;
    private Thread thread;
    private byte[] bytes;           //接收到的字节
    private int bytesLength;        //长度
    private string receiveMsg = "";   //接收到的信息

    private Queue<string> GetVs = new Queue<string>();

    //public Text LogText;

    void Start()
    {
        Init();
    }
    //初始化
    private void Init()
    {
        if(Config.Instance.configData.IP!=null)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(Config.Instance.configData.IP), Config.Instance.configData.Port);    //端口号要与发送端一致
        }
        else
        {
            ipEndPoint = new IPEndPoint(IPAddress.Any, Config.Instance.configData.Port);    //端口号要与发送端一致
        }
        LogMsg.Instance.Log(ipEndPoint.ToString());
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        socket.Bind(ipEndPoint);
        thread = new Thread(new ThreadStart(Receive));      //开启一个线程，接收发送端的消息
        thread.IsBackground = true;
        thread.Start();
    }
    //接收消息函数
    private void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)sender;
        while (true)
        {
            bytes = new byte[1024];
            try
            {
                //获取服务端端数据
                bytesLength = socket.ReceiveFrom(bytes, ref remote);
            }
            catch
            {

            }

            receiveMsg = Encoding.UTF8.GetString(bytes, 0, bytesLength);
            if (!string.IsNullOrEmpty(receiveMsg) && !string.IsNullOrWhiteSpace(receiveMsg))
            {
                GetVs.Enqueue(receiveMsg);
            }
        }
    }
    //关闭socket，关闭thread
    private void OnDisable()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
        if (thread != null)
        {
            thread.Interrupt();
            thread.Abort();
        }
    }

    private void Update()
    {
        //数据在这里转换
        lock (GetVs)
        {
            if (GetVs.Count > 0)
            {
                string st = GetVs.Dequeue();
               // LogText.text = st.ToString();
                EventParamete eventParamete = new EventParamete();
                eventParamete.AddParameter(st);
                EventManager.TriggerEvent(GenericEventEnumType.Message, MTFrame.EventType.DataToPanel.ToString(), eventParamete);
            }
        }
    }
}
