using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfCalcDistanceService
{
   public class ServiceCalculator : ICalcDistanceService
    {

        public double CalcDistance(Point startPoint, Point endPoint)
        {
            int deltaX = startPoint.X - endPoint.X;
            int deltaY = startPoint.Y - endPoint.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
