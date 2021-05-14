using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetManager : Singleton<NetManager>
{
    public string str;
    Socket socket;
    byte[] da = new byte[1024];
    Queue<byte[]> dataList = new Queue<byte[]>();
    MyMemoryStream stream = new MyMemoryStream();
    public void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.BeginConnect(str, 18888, SynConnect, null);
    }

    private void SynConnect(IAsyncResult ar)
    {
        try
        {
            socket.EndConnect(ar);
            Debug.Log("服务器连接成功！");
            socket.BeginReceive(da, 0, da.Length, SocketFlags.None, SynReceive, null);
        }
        catch (Exception)
        {
            Debug.LogError("ip错误或服务器未开启！");
        }
    }

    private void SynReceive(IAsyncResult ar)
    {
        int len=0;
        try
        {
            len = socket.EndReceive(ar);
        }
        catch (Exception)
        {
            Debug.LogError("与服务器断开连接！");
        }
        if(len>0)
        {
            byte[] data = new byte[len];
            Buffer.BlockCopy(da, 0, data, 0, data.Length);
            stream.Position = stream.Length;
            stream.Write(data, 0, data.Length);
            //至少有一个不完整的包
            while (stream.Length>2)
            {
                stream.Position = 0;
                ushort bodylen = stream.ReadUshort();
                int alllen = bodylen + 2;
                if(stream.Length>=alllen)
                {
                    stream.Position = 2;
                    byte[] body = new byte[bodylen];
                    stream.Read(body, 0, body.Length);

                    dataList.Enqueue(body);

                    int shenglen = (int)stream.Length - alllen;
                    if(shenglen>0)
                    {
                        byte[] daSheng = new byte[shenglen];
                        stream.Read(daSheng, 0, daSheng.Length);
                        stream.SetLength(0);
                        stream.Position = 0;
                        stream.Write(daSheng, 0, daSheng.Length);

                    }
                    else
                    {
                        stream.SetLength(0);
                        stream.Position = 0;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            try
            {
                socket.BeginReceive(da, 0, da.Length, SocketFlags.None, SynReceive, null);
            }
            catch (Exception)
            {
                Debug.LogError("与服务器断开连接！");
            }

        }
    }

    public void DataUpdata()
    {
        while (dataList.Count>0)
        {
            byte[] data = dataList.Dequeue();
            int id = BitConverter.ToInt32(data, 0);
            Debug.LogError("收到的ID为：" + id + "\t长度为：" + (data.Length + 2));
            byte[] conText = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, conText, 0, conText.Length);
            MessageManager.Ins.Send(id, conText);

        }
    }


    public void Close()
    {
        socket.Close();
    }

    public void SendToServer(int id,byte[] conText)
    {
        byte[] data = MakeData(id, conText);
        Debug.Log("发送的ID为：" + id + "长度为：" + data.Length);
        socket.BeginSend(data, 0, data.Length, SocketFlags.None, SynSend, null);
    }

    private void SynSend(IAsyncResult ar)
    {
        try
        {
            socket.EndSend(ar);
        }
        catch (Exception)
        {
            Debug.LogError("ip错误或服务器未开启！");
        }
    }

    private byte[] MakeData(int id, byte[] conText)
    {
        using (MyMemoryStream myMemory=new MyMemoryStream())
        {
            byte[] iddata = BitConverter.GetBytes(id);
            int len = iddata.Length + conText.Length;
            myMemory.WriteUShort((ushort)len);
            myMemory.Write(iddata, 0, iddata.Length);
            myMemory.Write(conText, 0, conText.Length);

            return myMemory.ToArray();
        }
    }
}
