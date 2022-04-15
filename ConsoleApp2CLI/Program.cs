using System;
using System.Linq;
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
        /// parameters are case insensitive
        /// return type:
        ///   1  arguments are valid
        ///  -1  arguments are invalid
        ///   1  ask for help, print help message,if input valid , return 1, if not , return -1
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

            Validate(args);
        }

     public static int Validate(string[] args)
     {
           args = args.Select(s => s.ToLowerInvariant()).ToArray();
            // only one parameter
            if (args.Length ==1 && args[0].ToLower() == "help")
            {
                Console.WriteLine("call for help");
                return 1;
            }

            if (args.Length > 1 && args.Contains("help"))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "help":
                            continue;
                        case "name":
                            try
                            {
                                var argument = args[i + 1];

                                if (argument.Length < 3 || argument.Length >10 )
                                {
                                    return -1;
                                }

                                return 1;
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                                return -1;
                            }

                        case "count":
                            try
                            {
                                var argument = args[i + 1];
                                if(string.IsNullOrWhiteSpace(argument))
                                {
                                    Console.WriteLine("invalid argument for count ");
                                    return -1;
                                }

                                if (!int.TryParse(argument,out var number))
                                {
                                    Console.WriteLine("argument for count is not a number ");
                                    return -1;
                                }

                                if (number<10 || number > 100)
                                {
                                    Console.WriteLine("argument for count is out of range");
                                    return -1;
                                }

                                return 1;

                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                                return -1;
                            }

                        default:
                            Console.WriteLine("only accept two kinds of  parameters which are count and name");
                            return -1;

                    } 
                }
            }

            var isValid = true;;
            if (args.Length > 1 && !args.Contains("help"))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        
                        case "name":
                            try
                            {
                                var argument = args[i + 1];

                                if (argument.Length < 3 || argument.Length > 10)
                                {
                                    isValid = false;
                                }

                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                                isValid = false;
                            }
                            break;

                        case "count":
                            try
                            {
                                var argument = args[i + 1];
                                if (string.IsNullOrWhiteSpace(argument))
                                {
                                    isValid = false;
                                }

                                if (!int.TryParse(argument, out var number))
                                {
                                    isValid = false;
                                }

                                if (number < 10 || number > 100)
                                {
                                    isValid = false;
                                }

                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                                return -1;
                            }
                            break;

                        default:
                            isValid= false;
                            break;

                    }
                }
            }

            return isValid == true? 1 : -1;


     }
    }
}

