using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    Dictionary<int, Action<object>> dic = new Dictionary<int, Action<object>>();
    public void Add(int id,Action<object> action)
    {
        if(dic.ContainsKey(id))
        {
            dic[id] += action;
        }
        else
        {
            dic.Add(id, action);
        }
    }
    public void Send(int id,params object[] mess)
    {
        if(dic.ContainsKey(id))
        {
            dic[id](mess);
        }
    }
}
