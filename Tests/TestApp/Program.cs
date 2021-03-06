﻿using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static int Main(String[] args)
        {
            if (args.FirstOrDefault() == "ARGS")
                foreach (String arg in args.Skip(1))
                    Console.WriteLine("ARG: '{0}'", arg);

            if (args.FirstOrDefault() == "ENVS")
                foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
                    Console.WriteLine("ENV: '{0}' = '{1}'", (String)env.Key, (String)env.Value);

            if (args.FirstOrDefault() == "EXITCODE")
                return int.Parse(args[1]);

            if (args.FirstOrDefault() == "FILE")
                Console.WriteLine("FILE CONTENT: '{0}'", File.ReadAllText(args.Skip(1).FirstOrDefault()));

            if (args.FirstOrDefault() == "OUTPUT")
                foreach (String output in args.Skip(1))
                {
                    if (output.StartsWith("ERR:")) Console.Error.WriteLine(output);
                    if (output.StartsWith("OUT:")) Console.Out.WriteLine(output);
                }

            if (args.FirstOrDefault() == "WORKDIR")
                Console.WriteLine("WORKING DIRECTORY: '{0}'", Environment.CurrentDirectory);

            return 0;
        }
    }
}
