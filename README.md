# Vp.Rest.Client
Is open source project that allow you create rest client. How it works?
You should defined the api contract by interface. Thats all!
The library implements your interface at runtime.

# Example

1. Define contract

```csharp
    public interface TodosApiContract
    {
        [Rest(RestMethod.GET, "todos/{todosId}")]
        Task<Todos> GetTodos(int todosId);
        
        [Rest(RestMethod.POST, "todos")]
        Task CreateTodos(Todos todos);
        
        [Rest(RestMethod.PUT, "todos/1")]
        Task UpdateTodos(Todos todos);
        
        [Rest(RestMethod.DELETE, "todos/{todosId}")]
        Task DeleteTodos(int todosId);
    }
```
2. Define Settings for client

```csharp
 var restFactory = new RestImplementationBuilder()
                .WithBaseUrl("https://jsonplaceholder.typicode.com/")
                .WithTimeout(TimeSpan.FromSeconds(60))
                .Build();
```

3. Usage

```csharp

var todos = new Todos
{
    UserId = 1,
    Title = "title"
    Completed = false
};

 var  apiClient = restFactory.Create<TodosApiContract>();
 await apiClient.CreateTodos(todos);
 Todos result = await apiClient.GetTodos(1);
 await apiClient.DeleteTodos(todos);
```

You can also:
  - Add logging
  - Add Basic Authorization
  - Add yourown Delegating handler


# Integration with .Net Core DI

```csharp
services.RegisterClients(restBuilder =>
    {
        restBuilder.AddClient<TodosApiContract>("https://jsonplaceholder.typicode.com/");
    });
```

And getting from service provider 

```csharp
var client = provider.GetRequiredService<TodosApiContract>();
```
