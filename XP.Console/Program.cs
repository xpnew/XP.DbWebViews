using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            //System.Console.Write("{0}*{1}", (1 == 2 ? 3 : 4) / 2, "10");
            System.Console.Write("dxper.net".Substring(3).Replace(".", "").ToUpper());

            System.Console.Read();


        }
    }
}
