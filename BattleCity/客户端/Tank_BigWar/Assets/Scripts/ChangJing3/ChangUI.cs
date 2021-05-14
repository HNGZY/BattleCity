using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using Yugege;

public class ChangUI : MonoBehaviour
{
    public Text WeiKashi;
    public Text KaishiYouxi;
    public Text MyFenshu;
    public Text OtherFenshu;
    public Text Shengchang1;
    public Text ShengChang2;
    public Text ShengTime;
    public Text Jieguo;

    // Start is called before the first frame update
    void Start()
    {
        MyFenshu.gameObject.SetActive(false);
        OtherFenshu.gameObject.SetActive(false);
        Shengchang1.gameObject.SetActive(false);
        ShengChang2.gameObject.SetActive(false);
        KaishiYouxi.gameObject.SetActive(false);
        ShengTime.gameObject.SetActive(false);
        Jieguo.gameObject.SetActive(false);

        MessageManager.Ins.Add(ClientID.WeiKaishi, WeiKai_);
        MessageManager.Ins.Add(ClientID.Kaishi, Kaishi_);
        MessageManager.Ins.Add(ClientID.MyFen, MyFenshu_);
        MessageManager.Ins.Add(ClientID.OtherFen, OtherFen_);
        MessageManager.Ins.Add(ClientID.ShengYuTime, ShengTime_);
        MessageManager.Ins.Add(ClientID.Shengli, Sheng_);
        MessageManager.Ins.Add(ClientID.Shibai, Bai_);

    }

    private void Bai_(object obj)
    {
        Jieguo.text = "你失败了！";
        Jieguo.gameObject.SetActive(true);
    }

    private void Sheng_(object obj)
    {
        Jieguo.text = "你赢了！";
        Jieguo.gameObject.SetActive(true);
    }
    private void ShengTime_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];
        TimeNow timeNow = TimeNow.Parser.ParseFrom(data);

        ShengTime.text ="剩余时间："+ timeNow.Now;



    }

    private void OtherFen_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];
        OtherScore otherScore = OtherScore.Parser.ParseFrom(data);
        OtherFenshu.text ="对方分数："+ otherScore.Score;

    }

    private void MyFenshu_(object obj)
    {
        object[] arr = obj as object[];
        byte[] data = arr[0] as byte[];
        MyScore myScore = MyScore.Parser.ParseFrom(data);
        MyFenshu.text ="我的分数："+ myScore.Score;
    }

    private void Kaishi_(object obj)
    {
        OtherFenshu.text = "对方分数：";
        MyFenshu.text = "我的分数：";
        StartCoroutine(Kaishi());
        MyFenshu.gameObject.SetActive(true);
        OtherFenshu.gameObject.SetActive(true);
        Shengchang1.gameObject.SetActive(true);
        ShengChang2.gameObject.SetActive(true);
        KaishiYouxi.gameObject.SetActive(true);
        ShengTime.gameObject.SetActive(true);
        Jieguo.gameObject.SetActive(false);
        WeiKashi.gameObject.SetActive(false);
        
    }

    IEnumerator Kaishi()
    {
        yield return new WaitForSeconds(0.5f);
        KaishiYouxi.gameObject.SetActive(false);
    }

    private void WeiKai_(object obj)
    {
        MyFenshu.gameObject.SetActive(false);
        OtherFenshu.gameObject.SetActive(false);
      
        KaishiYouxi.gameObject.SetActive(false);
        ShengTime.gameObject.SetActive(false);
        Jieguo.gameObject.SetActive(false);

        WeiKashi.gameObject.SetActive(true);

    }
}
