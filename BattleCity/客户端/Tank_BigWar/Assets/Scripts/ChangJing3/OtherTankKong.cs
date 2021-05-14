using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Yugege;
public class OtherTankKong : Singleton<OtherTankKong>
{
    Dictionary<string, OtherTank> dic = new Dictionary<string, OtherTank>();


    public void Start()
    {
        MessageManager.Ins.Add(ClientID.S2CMovePosBack, MovePos_);

        MessageManager.Ins.Add(ClientID.S2CMoveRotBack, MoveRot_);

        MessageManager.Ins.Add(ClientID.S2CMovePaoTBack, MovePaoT_);

    }

    private void MovePaoT_(object obj)
    {

        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];
        Move_PaoT_Back back = Move_PaoT_Back.Parser.ParseFrom(data);
        Vector3 dir = new Vector3(back.X, back.Y, back.Z);
        if (!dic.ContainsKey(back.Name))
        {
            GameObject game = GameObject.Instantiate(Resources.Load<GameObject>("OtherTank"));
            OtherTank other = game.GetComponent<OtherTank>();
            dic.Add(back.Name, other);
            Debug.Log(back.Name);
        }
        dic[back.Name].Move_PaoT(dir);
    }

    private void MoveRot_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];
        Move_rot_Back back = Move_rot_Back.Parser.ParseFrom(data);
        Vector3 dir = new Vector3(back.X, back.Y, back.Z);
        if (!dic.ContainsKey(back.Name))
        {
            GameObject game = GameObject.Instantiate(Resources.Load<GameObject>("OtherTank"));
            OtherTank other = game.GetComponent<OtherTank>();
            dic.Add(back.Name, other);
            Debug.Log(back.Name);
        }
        dic[back.Name].Move_Rot(dir);


    }

    private void MovePos_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];
        Move_pos_Bakc back = Move_pos_Bakc.Parser.ParseFrom(data);
        Vector3 dir = new Vector3(back.X,back.Y,back.Z);
        if(!dic.ContainsKey(back.Name))
        {
            GameObject game = GameObject.Instantiate(Resources.Load<GameObject>("OtherTank"));
            OtherTank other = game.GetComponent<OtherTank>();
            dic.Add(back.Name, other);
            Debug.Log(back.Name);
        }
        dic[back.Name].Move_Pos(dir);
        
    }

    
}
