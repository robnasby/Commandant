using System;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static int Main(String[] args)
        {
            if (args.FirstOrDefault() == "SUCCEED") return 0;
            if (args.FirstOrDefault() == "FAIL") return -1;

            return 0;
        }
    }
}
