using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Yugege;

namespace Tank_BigWar_Server
{
    class DengluChuli:SingleTon<DengluChuli>
    {

        public void Start()
        {
            MessageManager.Ins.Add(ServerID.C2SDenglu, Denglu_);
        }

        private void Denglu_(object obj)
        {
            object[] arr = obj as object[];
            byte[] data = arr[0] as byte[];
            Client client = arr[1] as Client;
            DengluBack back = new DengluBack();
            Denglu denglu = Denglu.Parser.ParseFrom(data);
            if(
                denglu.Username=="User1"&&denglu.Password=="123"||
                denglu.Username=="User2"&&denglu.Password=="123"
                )
            {
                back.Jie = JieGuo.Cheng;
                client.name = denglu.Username;

                IPEndPoint iPEndPoint = client.socket.RemoteEndPoint as IPEndPoint;
                if(NetManager.Ins.Youke_dic.ContainsKey(iPEndPoint.Port))
                {
                    NetManager.Ins.Youke_dic.Remove(iPEndPoint.Port);
                }

                NetManager.Ins.Online_dic.Add(client.name, client);

                Console.WriteLine("————"+ client.name + "登录成功！");

            }
            else
            {
                back.Jie = JieGuo.Bai;
            }
            NetManager.Ins.SendToClient(ServerID.S2CDengluBack, back.ToByteArray(), client);

        }
    }
}
