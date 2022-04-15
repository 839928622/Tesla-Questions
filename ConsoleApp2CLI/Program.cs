using System;
using System.Reflection;

namespace ConsoleApp2CLI
{
    internal class Program
    {
        /// <summary>
        /// description: need to support 3 kinds of  argument, help, count and name,arguments will be pass like below:
        /// ["--name","--NAME","--count","10"]
        ///
        /// the length of name should between 3 and 10
        /// the range of count should between 10 and 100
        ///
        /// return type:
        ///   1  arguments are valid
        ///  -1  arguments are invalid
        ///   1  ask for help, print help message
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            

            if (args.Length == 0)
            {
                var versionString = Assembly.GetEntryAssembly()?
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion
                    .ToString();

                Console.WriteLine($"ConsoleApp2CLI v{versionString}");
                Console.WriteLine("-------------");
            }
        }

     public int Validate(string[] args)
     {
            throw new NotImplementedException();
     }
    }
}

