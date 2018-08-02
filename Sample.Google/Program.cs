using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
                .AddLogging(async request =>
                {
                     var requestString = request.Content == null
                    ? null
                    : await request.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);
                    
                    var stringBuilder = new StringBuilder(512);
                    stringBuilder.AppendLine($"METHOD: {request.Method.Method}");
                    stringBuilder.AppendLine($"URI: {request.RequestUri}");
                    stringBuilder.AppendLine($"REQUEST HEADERS: ");
                    foreach (var header in request.Headers)
                    {
                        stringBuilder.AppendLine("\t" + header.Key + " : ");
                        stringBuilder.Append(string.Join(", ", header.Value));
                        
                    }
                    stringBuilder.AppendLine($"REQUEST: {requestString}");
                    Console.WriteLine(stringBuilder.ToString());

                }, async response =>
                    {
                        var responseString = response.Content == null
                            ? null
                            : await response.Content
                                .ReadAsStringAsync()
                                .ConfigureAwait(false);
                    
                        var stringBuilder = new StringBuilder(512);
                        stringBuilder.AppendLine($"REQUEST HEADERS: ");
                        foreach (var header in response.Headers)
                        {
                            stringBuilder.Append("\t" + header.Key + " : ");
                            stringBuilder.Append(string.Join(", ", header.Value));
                        
                        }
                        stringBuilder.AppendLine($"RESPONSE: {responseString}");
                        Console.WriteLine(stringBuilder.ToString());
                    })
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