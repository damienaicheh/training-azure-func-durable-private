using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class HelloOrchestration
{
    [Function(nameof(HelloOrchestration))]
    public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(HelloOrchestration));
        logger.LogInformation("Saying hello.");
        var outputs = new List<string>();

        var cities = await context.CallActivityAsync<List<City>>(nameof(GetCities));

        foreach (var city in cities)
        {
            logger.LogInformation("Processing city: {CityName}", city.Name);
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), city.Name));
        }

        // Replace name and input with values relevant for your Durable Functions Activity
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

        return outputs;
    }

    [Function(nameof(GetCities))]
    public static async Task<IEnumerable<City>> GetCities(
        [ActivityTrigger] string input,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("GetCities");
        logger.LogInformation("Getting cities from the service.");

        var citiesService = executionContext.InstanceServices.GetRequiredService<ICitiesService>();
        return await citiesService.GetCitiesAsync();
    }

    [Function(nameof(SayHello))]
    public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("SayHello");
        logger.LogInformation("Saying hello to {name}.", name);
        return $"Hello {name}!";
    }

    [Function("HelloOrchestration_HttpStart")]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("HelloOrchestration_HttpStart");

        // Function input comes from the request content.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(HelloOrchestration));

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        // Returns an HTTP 202 response with an instance management payload.
        // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }

    [Function("HelloOrchestration_StorageQueueStart")]
    public static async Task StorageQueueStart(
        [QueueTrigger("%STORAGE_QUEUE_NAME%", Connection = "StorageQueueConnection")] string queueMessage,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("HelloOrchestration_StorageQueueStart");

        logger.LogInformation($"Processing queue message {queueMessage}");

        // Function input comes from the request content.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(HelloOrchestration), queueMessage);

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
    }
}