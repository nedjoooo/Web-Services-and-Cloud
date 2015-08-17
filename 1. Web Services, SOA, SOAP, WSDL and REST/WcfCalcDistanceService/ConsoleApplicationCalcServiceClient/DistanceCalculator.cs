using ConsoleApplicationCalcServiceClient.ServiceReferenceCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationCalcServiceClient
{
    class DistanceCalculator
    {
        static void Main(string[] args)
        {
            CalcDistanceServiceClient calc = new CalcDistanceServiceClient();
            var result = calc.CalcDistance(new Point() { X = 10, Y = 10 }, new Point() { X = 15, Y = 15 });
            Console.WriteLine(result);
        }
    }
}
