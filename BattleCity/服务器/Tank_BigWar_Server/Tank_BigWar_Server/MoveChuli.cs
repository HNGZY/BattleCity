using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Yugege;

namespace Tank_BigWar_Server
{
    class MoveChuli:SingleTon<MoveChuli>
    {
        public void Start()
        {
            MessageManager.Ins.Add(ServerID.C2SOnline, OnLine_);

            MessageManager.Ins.Add(ServerID.C2SMovePos, MovePos_);

            MessageManager.Ins.Add(ServerID.C2SMoveRot, MoveRot_);

            MessageManager.Ins.Add(ServerID.C2SMovePaoT, Move_PaoT_);
        }

        private void Move_PaoT_(object obj)
        {
            object[] arr = obj as object[];
            byte[] data = arr[0] as byte[];
            Client client = arr[1] as Client;

            Move_PaoT rot = Move_PaoT.Parser.ParseFrom(data);

            client.Rot_Pao_X = rot.X;
            client.Rot_Pao_Y = rot.Y;
            client.Rot_Pao_Z = rot.Z;

            //炮管
            Move_PaoT_Back move_PaoT_Back = new Move_PaoT_Back();
            move_PaoT_Back.Name = client.name;
            move_PaoT_Back.X = client.Rot_Pao_X;
            move_PaoT_Back.Y = client.Rot_Pao_Y;
            move_PaoT_Back.Z = client.Rot_Pao_Z;

            foreach (var item in NetManager.Ins.Online_dic.Values)
            {
                if (item.IsFighting == true && item.name != client.name)
                {
                    NetManager.Ins.SendToClient(ServerID.S2CMovePaoTBack, move_PaoT_Back.ToByteArray(), item);
                }
            }

        }

        private void MoveRot_(object obj)
        {
            object[] arr = obj as object[];
            byte[] data = arr[0] as byte[];
            Client client = arr[1] as Client;


            Move_rot rot = Move_rot.Parser.ParseFrom(data);

            client.Rot_X = rot.X;
            client.Rot_Y= rot.Y;
            client.Rot_Z = rot.Z;


            //朝向
            Move_rot_Back move_Rot_Back = new Move_rot_Back();
            move_Rot_Back.Name = client.name;
            move_Rot_Back.X = client.Rot_X;
            move_Rot_Back.Y = client.Rot_Y;
            move_Rot_Back.Z = client.Rot_Z;


            foreach (var item in NetManager.Ins.Online_dic.Values)
            {
                if (item.IsFighting == true && item.name != client.name)
                {
                    NetManager.Ins.SendToClient(ServerID.S2CMoveRotBack, move_Rot_Back.ToByteArray(), item);
                }
            }
        }

        private void MovePos_(object obj)
        {
            object[] arr = obj as object[];
            byte[] data = arr[0] as byte[];
            Client client = arr[1] as Client;
            Move_pos pos = Move_pos.Parser.ParseFrom(data);

            client.Pox_X = pos.X;
            client.Pox_Y = pos.Y;
            client.Pox_Z = pos.Z;

            Move_pos_Bakc move_Pos_Bakc = new Move_pos_Bakc();
            move_Pos_Bakc.Name = client.name;
            move_Pos_Bakc.X = client.Pox_X;
            move_Pos_Bakc.Y = client.Pox_Y;
            move_Pos_Bakc.Z = client.Pox_Z;


            foreach (var item in NetManager.Ins.Online_dic.Values)
            {
                if (item.IsFighting == true && item.name != client.name)
                {
                    NetManager.Ins.SendToClient(ServerID.S2CMovePosBack, move_Pos_Bakc.ToByteArray(), item);
                }
            }


        }

        int a=0;
        private void OnLine_(object obj)
        {
            object[] arr = obj as object[];
            Client client = arr[1] as Client;
            client.IsFighting = true;

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

            if (a == 2)
            {
                foreach (var item in NetManager.Ins.Online_dic.Values)
                {
                    NetManager.Ins.SendToClient(ServerID.Kaishi, new byte[0], item);
                }
                a = 0;



                //开始计时
                MessageManager.Ins.Send(1);
            }
            else
            {
                NetManager.Ins.SendToClient(ServerID.WeiKaishi, new byte[0], client);
            }
        }
            //foreach (var item in NetManager.Ins.Online_dic.Values)
            //{
                //if(item.IsFighting==true&&item.name!=client.name)
                //{

                //    //////////////////////////////////////////////////////////////////
                //    //共享自己的信息

                //    //位置
                //    Move_pos_Bakc move_Pos_Bakc = new Move_pos_Bakc();
                //    move_Pos_Bakc.Name = client.name;
                //    move_Pos_Bakc.X = client.Pox_X;
                //    move_Pos_Bakc.Y = client.Pox_Y;
                //    move_Pos_Bakc.Z = client.Pox_Z;
                //    NetManager.Ins.SendToClient(ServerID.S2CMovePosBack, move_Pos_Bakc.ToByteArray(), item);

                //    //朝向
                //    Move_rot_Back move_Rot_Back = new Move_rot_Back();
                //    move_Rot_Back.Name = client.name;
                //    move_Rot_Back.X = client.Rot_X;
                //    move_Rot_Back.Y = client.Rot_Y;
                //    move_Rot_Back.Z = client.Rot_Z;
                //    NetManager.Ins.SendToClient(ServerID.S2CMoveRotBack, move_Rot_Back.ToByteArray(), item);

                //    //炮管
                //    Move_PaoT_Back move_PaoT_Back = new Move_PaoT_Back();
                //    move_PaoT_Back.Name = client.name;
                //    move_PaoT_Back.X = client.Rot_Pao_X;
                //    move_PaoT_Back.Y = client.Rot_Pao_Y;
                //    move_PaoT_Back.Z = client.Rot_Pao_Z;
                //    NetManager.Ins.SendToClient(ServerID.S2CMovePaoTBack, move_PaoT_Back.ToByteArray(), item);



                //    //////////////////////////////////////////////////////////////////

                //    //获取其他人的信息

                //    //位置
                //    Move_pos_Bakc move_Pos_Bakc1 = new Move_pos_Bakc();
                //    move_Pos_Bakc1.Name = item.name;
                //    move_Pos_Bakc1.X = item.Pox_X;
                //    move_Pos_Bakc1.Y = item.Pox_Y;
                //    move_Pos_Bakc1.Z = item.Pox_Z;
                //    NetManager.Ins.SendToClient(ServerID.S2CMovePosBack, move_Pos_Bakc.ToByteArray(), client);

                //    //朝向
                //    Move_rot_Back move_Rot_Back1 = new Move_rot_Back();
                //    move_Rot_Back1.Name = item.name;
                //    move_Rot_Back1.X = item.Rot_X;
                //    move_Rot_Back1.Y = item.Rot_Y;
                //    move_Rot_Back1.Z = item.Rot_Z;
                //    NetManager.Ins.SendToClient(ServerID.S2CMoveRotBack, move_Rot_Back.ToByteArray(), client);

                //    //炮管
                //    Move_PaoT_Back move_PaoT_Back1 = new Move_PaoT_Back();
                //    move_PaoT_Back1.Name = item.name;
                //    move_PaoT_Back1.X = item.Rot_Pao_X;
                //    move_PaoT_Back1.Y = item.Rot_Pao_Y;
                //    move_PaoT_Back1.Z = item.Rot_Pao_Z;
                //    NetManager.Ins.SendToClient(ServerID.S2CMovePaoTBack, move_PaoT_Back.ToByteArray(), client);








                //}





















    }
}
