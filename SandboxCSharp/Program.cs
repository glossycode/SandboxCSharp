using SandboxCSharp.Logger;
using SandboxCSharp.Unity;
using System;

namespace SandboxCSharp
{
    class Program
    {
        static void Main(string[] args)
        {                        
            int index = 1;
            foreach (var facto in UnityBuilder.Instance().Factories)
            {
                Console.WriteLine($"{index++} - {facto.GetType().FullName}");
            }
            Console.WriteLine($"Choose the test to run (all : default or 0)");

            int num = 0;
            string selection = Console.ReadLine();
            if (!string.IsNullOrEmpty(selection))
            {
                int.TryParse(selection, out num);
            }

            index = 1;
            foreach (var facto in UnityBuilder.Instance().Factories)
            {
                if (num == index++ || num == 0)
                {
                    Console.WriteLine($"\r\nRunning >> {facto.GetType().FullName}");
                    facto.Run(UnityBuilder.Instance().Container);
                    Console.WriteLine("");
                }
            }

            Console.Write("\r\nHit a key to terminate ... ");
            Console.ReadKey();
        }
    }
}
