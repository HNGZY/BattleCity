using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Yugege;

namespace Tank_BigWar_Server
{
    class TimeKongzhi:SingleTon<TimeKongzhi>
    {
        Timer timer;
        Timer timer2;
        public void Start()
        {
            MessageManager.Ins.Add(1, KaiShi_);
            timer = new Timer(1000);
            timer.Elapsed += Jishi_;
            timer2 = new Timer(1000);
            timer2.Elapsed += Dengdai_;


        }
        public void Chongxin()
        {
            b = 0;
            timer2.Start();
        }

        private void Bian(Client client)
        {
            if (client.name == "User1")
            {
                client.Pox_X = -45;
                client.Pox_Y = 0;
                client.Pox_Z = -45;
                a++;
            }
            else
            {
                client.Pox_X = 45;
                client.Pox_Y = 0;
                client.Pox_Z = -45;
                a++;
            }
            PosNow pos = new PosNow();
            pos.X = client.Pox_X;
            pos.Y = client.Pox_Y;
            pos.Z = client.Pox_Z;
            NetManager.Ins.SendToClient(ServerID.S2CPoxNow, pos.ToByteArray(), client);

        }

        private void Dengdai_(object sender, ElapsedEventArgs e)
        {
            b++;
            if(b==1)
            {
                Bian(NetManager.Ins.Online_dic["User1"]);
                Bian(NetManager.Ins.Online_dic["User2"]);
            }
            if(b==3)
            {
                foreach (var item in NetManager.Ins.Online_dic.Values)
                {
                    NetManager.Ins.SendToClient(ServerID.Kaishi, new byte[0], item);
                }
                KaiShi_(null);
                timer2.Stop();
            }
        }

        int a ;
        int b;
        private void KaiShi_(object obj)
        {
            a = 60;
            timer.Start();
        }

        private void Jishi_(object sender, ElapsedEventArgs e)
        {
            a--;
            if(a>=0)
            {
                TimeNow timeNow = new TimeNow();
                timeNow.Now = a.ToString();
                foreach (var item in NetManager.Ins.Online_dic.Values)
                {
                    NetManager.Ins.SendToClient(ServerID.ShengYuTime, timeNow.ToByteArray(), item);
                }
            }
            else
            {
                MyShengChang mySheng = new MyShengChang();

                if (NetManager.Ins.Online_dic["User1"].Score > NetManager.Ins.Online_dic["User2"].Score)
                {
                    NetManager.Ins.Online_dic["User1"].ShengChang++;
                    mySheng.Shengchang = NetManager.Ins.Online_dic["User1"].ShengChang.ToString();
                    NetManager.Ins.SendToClient(ServerID.Shengli, new byte[0], NetManager.Ins.Online_dic["User1"]);
                    NetManager.Ins.SendToClient(ServerID.Shibai, new byte[0], NetManager.Ins.Online_dic["User2"]);
                }
                else
                {
                    NetManager.Ins.Online_dic["User2"].ShengChang++;
                    mySheng.Shengchang = NetManager.Ins.Online_dic["User2"].ShengChang.ToString();
                    NetManager.Ins.SendToClient(ServerID.Shengli, new byte[0], NetManager.Ins.Online_dic["User2"]);
                    NetManager.Ins.SendToClient(ServerID.Shibai, new byte[0], NetManager.Ins.Online_dic["User1"]);

                }

                NetManager.Ins.SendToClient(ServerID.Shengchang1, mySheng.ToByteArray(), NetManager.Ins.Online_dic["User1"]);
                NetManager.Ins.SendToClient(ServerID.Shengchang1, mySheng.ToByteArray(), NetManager.Ins.Online_dic["User2"]);
                NetManager.Ins.Online_dic["User1"].Score = 0;
                NetManager.Ins.Online_dic["User2"].Score = 0;

                timer.Stop();
                Chongxin();
            }


        }
    }
}
