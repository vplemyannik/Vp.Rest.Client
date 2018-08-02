using System;
using System.Threading.Tasks;
using Vp.Rest.Client;
using Vp.Rest.Client.Configuration;

namespace Sample.Google
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var restImple = new RestImplementationBuilder()
                .WithBaseUrl("https://jsonplaceholder.typicode.com/")
                .WithTimeout(TimeSpan.FromSeconds(60))
                .Build();

            try
            {
                var  apiClient = restImple.Create<GoogleApi>();
                var result = await apiClient.GetTodos(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }


    public interface GoogleApi
    {
        [Rest(RestMethod.GET, "todos/{todosId}")]
        Task<Todos> GetTodos(int todosId);
    }

    public class Todos
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}