using SandboxCSharp.Unity;
using System;

namespace SandboxCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var facto in UnityBuilder.Instance().Factories)
            {                
                Console.WriteLine($"\r\nRunning >> {facto.GetType().FullName}");
                facto.Run(UnityBuilder.Instance().Container);
                Console.WriteLine("");
            }
            
            Console.Write("\r\nHit a key to terminate ... ");
            Console.ReadKey();
        }
    }
}
