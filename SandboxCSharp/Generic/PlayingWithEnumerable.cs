using SandboxCSharp.Logger;
using System;
using System.Collections.Generic;

namespace SandboxCSharp.Generic
{
    class PlayingWithEnumerable
    {        
        private static readonly ILogger _logger = LogManager.Instance().GetLogger(typeof(PlayingWithEnumerable));

        public static void Test()
        {
            PlayingWithEnumerable toy = new PlayingWithEnumerable();

            _logger.Info($"Test with -1:");
            foreach (var d in toy.GetData(-1))
            {
                _logger.Info($"\t=>\t{d}");
            }

            _logger.Info($"Test with 0:");
            foreach (var d in toy.GetData(0))
            {
                _logger.Info($"\t=>\t{d}");
            }

            _logger.Info($"Test with 1:");
            foreach (var d in toy.GetData(1))
            {
                _logger.Info($"\t=>\t{d}");
            }

            _logger.Info($"Test with 10");
            foreach (var d in toy.GetData(10))
            {
                _logger.Info($"\t=>\t{d}");
            }
        }

        public IEnumerable<MyClassOfProperties> GetData(int count)
        {
            if (count < 0)
                return new List<MyClassOfProperties>(); // return an empty list

            if (count == 0)
                return new MyClassOfProperties[]{ }; // return an empty list

            if (count == 1)
                return new MyClassOfProperties[] { new MyClassOfProperties() { Id = 999, Name = "Test avec 1" } };

            return GetXElements(count);
        }

        public static IEnumerable<MyClassOfProperties> GetXElements(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new MyClassOfProperties() { Id = i, Name = $"Name{i}" };
            }
        }
    }


    public class MyClassOfProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Id}:{Name}";
        }
    }

}
