using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_BigWar_Server
{
    class MessageManager:SingleTon<MessageManager>
    {
        Dictionary<int, Action<object>> dic = new Dictionary<int, Action<object>>();
        public void Add(int id, Action<object> action)
        {
            if (dic.ContainsKey(id))
            {
                dic[id] += action;
            }
            else
            {
                dic.Add(id, action);
            }
        }
        public void Send(int id, params object[] mess)
        {
            if (dic.ContainsKey(id))
            {
                dic[id](mess);
            }
        }
    }
}
