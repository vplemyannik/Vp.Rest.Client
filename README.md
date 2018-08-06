# Vp.Rest.Client
Is open source project that allow you create rest client. How it works?
You should defined the api contract by interface. Thats all!
The library implements your interface at runtime.

# Example

1. Interface definition
```
    public interface TodosApiContract
    {
        [Rest(RestMethod.GET, "todos/{todosId}")]
        Task<Todos> GetTodos(int todosId);
    }
```
2. Define Settings for client
```
 var restFactory = new RestImplementationBuilder()
                .WithBaseUrl("https://jsonplaceholder.typicode.com/")
                .WithTimeout(TimeSpan.FromSeconds(60))
                .Build();
```

3. Usage
```
 var  apiClient = restFactory.Create<TodosApiContract>();
                var result = await apiClient.GetTodos(1);
```

You can also:
  - Add logging
  - Add Basic Authorization
  - Add yourown Delegating handler


# Integration with .NEt core DI

```
            services.RegisterClients(restBuilder =>
                {
                    restBuilder.AddClient<TodosApiContract>("https://jsonplaceholder.typicode.com/");
                });
```

And getting from service provider 

```
        var client = provider.GetRequiredService<TodosApiContract>();
```
