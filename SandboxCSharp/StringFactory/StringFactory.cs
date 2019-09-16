using System;
using Unity;

namespace SandboxCSharp.StringFactory
{
    class StringFactory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
        }

        public void Run(IUnityContainer Container)
        {
            string aString = "C'est une chaîne de caractère";
            int aInt = 254;

            Console.WriteLine($"{aString} with v:{ aInt }, name of the class {nameof(StringFactory)}");
            Console.WriteLine($"### Avec le dolar... pas besoin de string::Format {aString} with v:{ aInt }, name of the class {nameof(StringFactory)}");

            new TestMessage() { WillThroughException = false }.Run();
            try
            {
                new TestMessage() { WillThroughException = true }.Run();
            }
            catch (Exception)
            {

           }
        }
    }



    public class TestMessage
    {
        public bool WillThroughException { get; set; } = false;
        public string Run()
        {
            String Output = "";
            try
            {
                Console.WriteLine(string.Format($"### {this.GetType().Name}.Run"));

                if (WillThroughException)
                {
                    throw new Exception("This is crasy");
                }

                Output = "DONE";
                Console.WriteLine($"### Successfully completed {this.GetType().Name}.Run, Output->{Output}");
                return Output;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"### EXEPTION:\"{ex.Message}\" Error in {this.GetType().Name}.Run");
                throw ex;
            }
        }
    }
}
