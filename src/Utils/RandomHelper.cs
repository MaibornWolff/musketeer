using System;
using System.Collections.Generic;

namespace Musketeer.Utils
{
    public class RandomHelper
    {
        public List<int> GetRandomNumbers(int minValue, int maxValue, int numberOfRandomNumbers)
        {
            var resultsList = new List<int>();
            var random = new Random();

            while(resultsList.Count < numberOfRandomNumbers)
            {
                var number = random.Next(minValue, maxValue);
                if(!resultsList.Contains(number))
                    resultsList.Add(number);
            }
            return resultsList;
        }
    }
}