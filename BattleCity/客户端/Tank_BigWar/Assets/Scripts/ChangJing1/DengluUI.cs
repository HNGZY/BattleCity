using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using Yugege;
using System;
using UnityEngine.SceneManagement;

public class DengluUI : MonoBehaviour
{
    public InputField Username;
    public InputField Password;
    public Button Deng;
    public InputField Ip;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        MessageManager.Ins.Add(ClientID.S2CDengluBack, DengluBack_);
        Deng.onClick.AddListener(() =>
        {
            if(Username.text!=""&&Password.text!=""&&Ip.text!="")
            {
                Denglu denglu = new Denglu();
                denglu.Username = Username.text;
                denglu.Password = Password.text;
                NetManager.Ins.SendToServer(ClientID.C2SDenglu, denglu.ToByteArray());

            }

        });
        button.onClick.AddListener(() =>
        {
            if(Ip.text!="")
            {
                NetManager.Ins.str = Ip.text;
                NetManager.Ins.Start();

            }
        });
    }

    private void DengluBack_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];

        DengluBack back = DengluBack.Parser.ParseFrom(data);
        if(back.Jie==JieGuo.Cheng)
        {
            Debug.Log("登录成功！");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("登录失败！账号或密码错误！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
