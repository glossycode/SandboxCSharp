using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Unity;
using SandboxCSharp.Extention;

namespace SandboxCSharp.XML
{
    class XMLCompare : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
        }

        public void Run(IUnityContainer Container)
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(@"XML\file1.xml");

            XmlDocument doc2 = new XmlDocument();
            doc2.Load(@"XML\file2.xml");

            Compare(doc1, doc2);
        }

        private void Compare(XmlDocument doc1, XmlDocument doc2)
        {
            XmlNode root1 = doc1.DocumentElement;
            XmlNode root2 = doc2.DocumentElement;

            Console.WriteLine($"Compare doc1:\t{root1.FullString()}\r\nwith doc2:\t{root2.FullString()}");

            NewMethod("new node in root1", root1, root2);
            NewMethod("new node in root2", root2, root1);
        }

        private static void NewMethod(string comment, XmlNode reference, XmlNode nodeToCompare)
        {
            foreach (XmlNode child in reference.ChildNodes)
            {
                string xpath = child.GetXPath();
                if (nodeToCompare.SelectSingleNode(xpath) == null)
                {
                    Console.WriteLine($"{comment} : {xpath}");
                    Console.WriteLine($"    => content: {child.FullString()}");
                    Console.WriteLine($"");
                }
            }
        }
    }
}
