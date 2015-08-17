using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationCalcDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://localhost:33647");
            var request = new RestRequest("api/points/distance", Method.POST);
            request.AddParameter("startX", 10);
            request.AddParameter("startY", 10);
            request.AddParameter("endX", 15);
            request.AddParameter("endY", 15);

            var response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
