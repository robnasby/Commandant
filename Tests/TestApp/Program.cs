using System;
using System.Collections;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static int Main(String[] args)
        {
            if (args.FirstOrDefault() == "SUCCEED") return 0;
            if (args.FirstOrDefault() == "FAIL") return -1;

            if (args.FirstOrDefault() == "ARGS")
                foreach (String arg in args.Skip(1))
                    Console.WriteLine("ARG: '{0}'", arg);

            if (args.FirstOrDefault() == "ENVS")
                foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
                    Console.WriteLine("ENV: '{0}' = '{1}'", (String)env.Key, (String)env.Value);

            return 0;
        }
    }
}
