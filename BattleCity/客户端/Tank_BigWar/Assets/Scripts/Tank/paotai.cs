using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Yugege;
using System;

public class paotai : MonoBehaviour
{
    public GameObject game;
    public GameObject zi;
    // Start is called before the first frame update
    void Start()
    {
        zi = Resources.Load<GameObject>("子弹");
        MessageManager.Ins.Add(ClientID.S2CFireBack, Fireback_);
        MessageManager.Ins.Add(1, Tong_);
    }

    private void Tong_(object obj)
    {
        MovePaoT();
    }

    private void MovePaoT()
    {
        Move_PaoT paoT = new Move_PaoT();
        Vector3 dir = transform.rotation.eulerAngles;
        paoT.X = dir.x;
        paoT.Y = dir.y;
        paoT.Z = dir.z;

        NetManager.Ins.SendToServer(ClientID.C2SMovePaoT, paoT.ToByteArray());

    }

    private void Fireback_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];

        FireBack back = FireBack.Parser.ParseFrom(data);

        Vector3 dir1 = new Vector3(back.PosX, back.PosY, back.PosZ);
        Vector3 dir2 = new Vector3(back.RotX, back.RotY, back.RotZ);

        GameObject zidan = Instantiate(zi);

        zidan.transform.position = dir1;
        zidan.transform.rotation = Quaternion.Euler(dir2);
    }

    IEnumerator Yan()
    {
        yield return new WaitForSeconds(1);
        bo = true;
    }
    bool bo=true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && bo)
        {
            StartCoroutine(Yan());
            bo = false;
            GameObject zidan = Instantiate(zi);
            zidan.transform.position = game.transform.position;
            zidan.transform.rotation = game.transform.rotation;
            zidan.name = "子弹";

            GameObject game1 = Instantiate(Resources.Load<GameObject>("火光"));
            game1.transform.position = game.transform.position;
            game1.transform.rotation = game.transform.rotation;

            Vector3 dir = game.transform.rotation.eulerAngles;
            Fire fire = new Fire();
            fire.PosX = game.transform.position.x;
            fire.PosY = game.transform.position.y;
            fire.PosZ = game.transform.position.z;

            fire.RotX = dir.x;
            fire.RotY = dir.y;
            fire.RotZ = dir.z;

            NetManager.Ins.SendToServer(ClientID.C2SFire, fire.ToByteArray());

        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -2, 0);
            MovePaoT();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 1, 0);
            MovePaoT();
        }
    }



}
