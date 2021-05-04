Redis message queue light
==========================
Implementation message queue based [BackgroundService](https://docs.microsoft.com/dotnet/architecture/microservices/multi-container-microservice-net-applications/subscribe-events) and [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)

### Usage
1. Create DTO model for in\out queue message
```c#
public class MqMessage
{
    public int Id { get; set; }
    public string Message { get; set; }
}
```
2. Create shared worker class for settings queue and handle message. Worker should inherits from abstract class ```RmqlWorker<T>``` (T is dto model).
```c#
public class TestWorker: RmqlWorker<MqMessage>
{
    public override string Key => "TestQueue";
    public override int Frequency => 50;
    public override RedisDbEnum RedisDb => RedisDbEnum.Db0;

    public override Task ActionAsync(IServiceScope serviceScope, MqMessage message)
    {
        Console.WriteLine($"Handling message from {Key}: {message.Id}; {message.Message}");
        return Task.CompletedTask;
    }
}
```
3. Register workers to ```Startup.cs``` in project for queue handle
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddRmql(config =>
    {
        config.UseWorker<TestWorker, MqMessage>();
    });
    
    ...
}
```
4. Use ```RmqlClient``` for send message in queue
```c#
public class HomeController : Controller
{
    private readonly RmqlClient _rmqlClient;
    
    public HomeController(RmqlClient rmqlClient)
    {
        _rmqlClient = rmqlClient;
    }

    public async Task<IActionResult> Index()
    {
        var model = new MqMessage
        {
            Id = 1,
            Message = "Message text"
        };
        await _rmqlClient.PushAsync<TestWorker, MqMessage>(model);
        
        return View();
    }
}
```
5. [Option] Add ```services.AddRmql()``` if only use need ```RmqlClient``` in project.
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddRmql();
    ...
}
```

### Settings
Abstract class ```RmqlWorker<T>``` has properties and methods for override:
* ``[string] Key`` - Unique list name for queue message storage
* ``[QueueType] Type`` - Type of queue. *Default FIFO*
* ``[int] Frequency`` - Delay in milliseconds before check new message in queue. *Default 1000ms*
* ``[RedisDbEnum] RedisDb`` -  Redis db number for queue storage. *Default Db0*
* ``[Task] ActionAsync()`` -  Handling one queue message
* ``[Task] LogErrorAsync()`` -  Handling thrown exception