using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandboxCSharp.Generic
{
    class LynqTest
    {
        int Type;
        string Name;

        public static void Test()
        {
            var list = MakeList();
            // get type 
            var type = list.Where(x => x.Name == el2).Select((x) => x.Type).FirstOrDefault();

            Console.WriteLine($"The type for {el2} is {type}");


            // get not existing element type 
            var type2 = list.Where(x => x.Name == "La lune est jaune").Select((x) => x.Type).FirstOrDefault();

            Console.WriteLine($"The type for La lune est jaune is {type2}");

        }

        static string el2 = "Element 2";

        public static List<LynqTest> MakeList()
        {
            return new List<LynqTest>() { new LynqTest() { Name="Element 1", Type = 123 },
                new LynqTest() { Name=el2, Type = 55 },
                new LynqTest() {  Name = "Element 3", Type = 18}
            };
        }
    }
}
