using Elasticsearch.Net;
using System;
using System.Threading.Tasks;

namespace SimpleElasticClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var settings = new ConnectionConfiguration()
                .PrettyJson();

            var client = new ElasticLowLevelClient(settings);

            var car = new Car("Volvo", 20001);

            var response = await client.IndexAsync<StringResponse>("car", PostData.Serializable(car));
            var responseString = response.Body;

            

            Console.WriteLine(responseString);

        }
    }
}
