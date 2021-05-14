using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Yugege;
public class Move : MonoBehaviour
{
    Rigidbody rigidbody;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        NetManager.Ins.SendToServer(ClientID.C2SOnline, new byte[0]);



        MessageManager.Ins.Add(ClientID.S2CPoxNow, NowPos_);
        MessageManager.Ins.Add(ClientID.Kaishi, Kai_);
    }

    private void Kai_(object obj)
    {
        transform.position = dir;
    }

    Vector3 dir;
    private void NowPos_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data =  arr[0] as byte[];
        PosNow now = PosNow.Parser.ParseFrom(data);
        dir = new Vector3(now.X, now.Y, now.Z);
        transform.position = dir;
    }

    private void MovePos()
    {
        Move_pos pos = new Move_pos();
        pos.X = transform.position.x;
        pos.Y = transform.position.y;
        pos.Z = transform.position.z;

        NetManager.Ins.SendToServer(ClientID.C2SMovePos, pos.ToByteArray());
    }
    private void MoveRot()
    {
        Vector3 dir = transform.rotation.eulerAngles;
        Move_rot rot = new Move_rot();
        rot.X = dir.x;
        rot.Y = dir.y;
        rot.Z = dir.z;

        NetManager.Ins.SendToServer(ClientID.C2SMoveRot, rot.ToByteArray());

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 5);
            MovePos();
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * 5);
            MovePos();
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -1, 0);
            MoveRot();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 1, 0);
            MoveRot();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "子弹")
        {
            source.Play();
            //向服务器发送我掉血  对方同步
            NetManager.Ins.SendToServer(ClientID.Hurt, new byte[0]);
            rigidbody.AddExplosionForce(150, other.transform.position, 20);
            StartCoroutine(Tong());
        }
    }

    IEnumerator Tong()
    {

        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.03f);
            MovePos();
            MoveRot();
            MessageManager.Ins.Send(1, new byte[0]);
        }
    }

    private void OnDestroy()
    {
        GameObject xiaoguo = Instantiate(Resources.Load<GameObject>("效果"));
        xiaoguo.transform.position = transform.position;
        xiaoguo.transform.localScale = new Vector3(3, 3, 3);
    }

}
