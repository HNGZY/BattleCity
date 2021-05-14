using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_BigWar_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeKongzhi.Ins.Start();
            XianShi.Ins.Start();
            FireChuli.Ins.Start();
            MoveChuli.Ins.Start();
            DengluChuli.Ins.Start();
            NetManager.Ins.Start();
            Console.ReadKey();
        }
    }
}
