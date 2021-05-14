using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Yugege;
namespace Tank_BigWar_Server
{
    class FireChuli:SingleTon<FireChuli>
    {
        public void Start()
        {
            MessageManager.Ins.Add(ServerID.C2SFire, Fire_);
        }

        private void Fire_(object obj)
        {
            object[] arr = obj as object[];
            byte[] data = arr[0] as byte[];
            Client client = arr[1] as Client;
            Fire fire = Fire.Parser.ParseFrom(data);
            FireBack back = new FireBack();
            back.PosX = fire.PosX;
            back.PosY = fire.PosY;
            back.PosZ = fire.PosZ;

            back.RotX = fire.RotX;
            back.RotY = fire.RotY;
            back.RotZ = fire.RotZ;
            
            foreach (var item in NetManager.Ins.Online_dic.Values)
            {
                if (item.IsFighting == true && item.name != client.name)
                {
                    NetManager.Ins.SendToClient(ServerID.S2CFireBack, back.ToByteArray(), item);
                }
            }
        }
    }
}
