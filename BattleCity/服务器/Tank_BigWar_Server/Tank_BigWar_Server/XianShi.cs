using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Yugege;
namespace Tank_BigWar_Server
{
    class XianShi:SingleTon<XianShi>
    {
        public void Start()
        {
            MessageManager.Ins.Add(ServerID.Hurt, Hurt_);
        }

        private void Hurt_(object obj)
        {
            object[] arr = obj as object[];
            Client client = arr[1] as Client;
            

            foreach (var item in NetManager.Ins.Online_dic.Values)
            {
                if(item.name!=client.name)
                {
                    item.Score += 10;
                    MyScore myScore = new MyScore();
                    myScore.Score = item.Score.ToString();
                    NetManager.Ins.SendToClient(ServerID.MyFen, myScore.ToByteArray(), item);


                    OtherScore otherScore = new OtherScore();
                    otherScore.Score = item.Score.ToString();
                    NetManager.Ins.SendToClient(ServerID.OtherFen, otherScore.ToByteArray(),client);

                }
            }


        }
    }
}
