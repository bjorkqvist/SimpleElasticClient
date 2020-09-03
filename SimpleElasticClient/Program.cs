using Nest;
using System;
using System.Threading.Tasks;

namespace SimpleElasticClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var settings = new ConnectionConfiguration()
            //   .PrettyJson();

            var connectionSettings = new ConnectionSettings()
                .DefaultIndex("cars*")
                .DefaultFieldNameInferrer(p => p)
                .OnRequestCompleted(info =>
                {
                    // info.Uri is /_search/ without the default index
                    // my ES instance throws an error on the .kibana index (@timestamp field not mapped because I sort on @timestamp) 
                });

            //var client = new ElasticLowLevelClient(settings);
            ElasticClient client = new ElasticClient(connectionSettings);
            

            
            await ConnectAndInsertDataAsync(client, new Car("Volvo", 20001));
            await ConnectAndInsertDataAsync(client, new Car("Saab", 19599994));
            await ConnectAndInsertDataAsync(client, new Car("Tesla", 20001));
            await ConnectAndInsertDataAsync(client, new Car("Vw", 20001));

            Console.WriteLine("Got response, now search");


            var searchResult = await SearchForCarAsync(client, "Saab");
            Console.WriteLine(searchResult);


        }

        private static async Task<IndexResponse> ConnectAndInsertDataAsync(ElasticClient client, Car car) // Here RawData is a object of a Class. you can create a class with few sample properties and can pass in the form of JSON from Postman  
        {
            IndexResponse response = new IndexResponse();

            try
            {
                response = await client.IndexAsync(car, i => i.Index("cars"));

                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oooops!" + ex);
            }
            return response;
        }

        private static async Task<ISearchResponse<Car>> SearchForCarAsync(ElasticClient client, string str)
        {
            var searchResponse = await client.SearchAsync<Car>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Brand)
                        .Query(str)
                    )
                )
            );

            return searchResponse;
        }
    }
}
