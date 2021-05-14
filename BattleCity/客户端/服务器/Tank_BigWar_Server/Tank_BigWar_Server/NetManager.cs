using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tank_BigWar_Server
{
    class NetManager:SingleTon<NetManager>
    {
        Socket socket;
        public Dictionary<int, Client> Youke_dic;
        public Dictionary<string, Client> Online_dic;

        public void Start()
        {
            Youke_dic = new Dictionary<int, Client>();
            Online_dic = new Dictionary<string, Client>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 18888));
            socket.Listen(100);

            Console.WriteLine("服务器已开启！");
            socket.BeginAccept(SynAccept, null);
        }

        private void SynAccept(IAsyncResult ar)
        {
            Client client = new Client();
            Socket clientSocket = socket.EndAccept(ar);
            client.socket = clientSocket;
            IPEndPoint iPEndPoint = client.socket.RemoteEndPoint as IPEndPoint;
            Youke_dic.Add(iPEndPoint.Port, client);
            Console.WriteLine("有游客进入！");
            try
            {
                client.socket.BeginReceive(client.data, 0, client.data.Length, SocketFlags.None, SynReceive, client);

                socket.BeginAccept(SynAccept, null);
            }
            catch (Exception)
            {
                Duankai(client);
            }
        }

        /// <summary>
        /// 断开方法
        /// </summary>
        /// <param name="client"></param>
        private void Duankai(Client client)
        {
            if(client.socket!=null)
            {
                IPEndPoint iPEndPoint = client.socket.RemoteEndPoint as IPEndPoint;
                if (Youke_dic.ContainsKey(iPEndPoint.Port))
                {
                    Console.WriteLine("有游客退出！");
                    Youke_dic[iPEndPoint.Port].socket.Shutdown(SocketShutdown.Both);
                    Youke_dic[iPEndPoint.Port].socket.Close();
                    Youke_dic.Remove(iPEndPoint.Port);
                }
                else
                {
                    Console.WriteLine(client.name + "已退出！");
                    Online_dic[client.name].socket.Shutdown(SocketShutdown.Both);
                    Online_dic[client.name].socket.Close();
                    Online_dic.Remove(client.name);
                }
            }

        }


        /// <summary>
        /// 接收客户端发的消息
        /// </summary>
        /// <param name="ar"></param>
        private void SynReceive(IAsyncResult ar)
        {
            int len = 0;
            Client client = ar.AsyncState as Client;


            try
            {
                len = client.socket.EndReceive(ar);
            }
            catch (Exception)
            {
                //Duankai(client);
            }
            if (len > 0)
            {
                byte[] dataReceive = new byte[len];
                Buffer.BlockCopy(client.data, 0, dataReceive, 0, len);
                //将刚接收的数据存到缓存区
                client.memoryStream.Position = client.memoryStream.Length;
                client.memoryStream.Write(dataReceive, 0, dataReceive.Length);

                //至少有一个不完整的包
                while (client.memoryStream.Length > 2)
                {
                    client.memoryStream.Position = 0;
                    ushort bodylen = client.memoryStream.ReadUshort();
                    int alllen = bodylen + 2;
                    //至少有一个完整的包
                    if (client.memoryStream.Length >= alllen)
                    {
                        client.memoryStream.Position = 2;
                        byte[] dataBody = new byte[bodylen];
                        client.memoryStream.Read(dataBody, 0, dataBody.Length);
                        int id = BitConverter.ToInt32(dataBody, 0);
                        byte[] conText = new byte[dataBody.Length - 4];
                        Console.WriteLine("收到的ID：" + id + "\t长度：" + alllen);
                        Buffer.BlockCopy(dataBody, 4, conText, 0, conText.Length);

                        MessageManager.Ins.Send(id, conText, client);




                        int shenglen = (int)client.memoryStream.Length - alllen;


                        //有剩余
                        if (shenglen > 0)
                        {
                            byte[] sheng = new byte[shenglen];
                            client.memoryStream.Read(sheng, 0, sheng.Length);

                            client.memoryStream.SetLength(0);
                            client.memoryStream.Position = 0;

                            client.memoryStream.Write(sheng, 0, sheng.Length);
                        }
                        else
                        {
                            client.memoryStream.SetLength(0);
                            client.memoryStream.Position = 0;
                            break;
                        }


                    }
                    else
                    {
                        break;
                    }
                }
                client.socket.BeginReceive(client.data, 0, client.data.Length, SocketFlags.None, SynReceive, client);
            }
            else
            {
                Duankai(client);
            }





        }


        /// <summary>
        /// 发送消息给客户端
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <param name="client"></param>
        public void SendToClient(int id, byte[] context, Client client)
        {
            byte[] data = MakeData(id, context);
            Console.WriteLine("要发送的id:" + id + "\t长度为：" + data.Length);
            try
            {
                client.socket.BeginSend(data, 0, data.Length, SocketFlags.None, SynSend, client);
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="ar"></param>
        private void SynSend(IAsyncResult ar)
        {
            Client client = ar.AsyncState as Client;
            try
            {
                client.socket.EndSend(ar);
            }
            catch (Exception)
            {
                Duankai(client);
            }
        }
        /// <summary>
        /// 将id和内容拼接成要用的字节流
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private byte[] MakeData(int id, byte[] context)
        {
            using (MyMemoryStream memoryStream = new MyMemoryStream())
            {
                byte[] dataid = BitConverter.GetBytes(id);
                int len = dataid.Length + context.Length;
                memoryStream.WriteUShort((ushort)len);
                memoryStream.Write(dataid, 0, dataid.Length);
                memoryStream.Write(context, 0, context.Length);
                return memoryStream.ToArray();
            }
        }

    }

    public class Client
    {
        public Socket socket;
        public byte[] data = new byte[1024];
        public string name = "";

        public MyMemoryStream memoryStream = new MyMemoryStream();

        public bool IsFighting = false;

        public float Pox_X;
        public float Pox_Y;
        public float Pox_Z;

        public float Rot_X;
        public float Rot_Y;
        public float Rot_Z;

        public float Rot_Pao_X;
        public float Rot_Pao_Z;
        public float Rot_Pao_Y;

        public int Score = 0;
        public int ShengChang = 0;
    }
}
