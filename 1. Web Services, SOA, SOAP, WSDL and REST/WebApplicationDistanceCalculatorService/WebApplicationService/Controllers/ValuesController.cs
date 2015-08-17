using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplicationService.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/points/distance")]
        public double CalcDistance(int startX, int startY, int endX, int endY)
        {
            int deltaX = startX - endX;
            int deltaY = startY - endY;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
