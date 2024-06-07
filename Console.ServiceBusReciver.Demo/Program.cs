// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

var connectionString = "Endpoint=sb://azure-course.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VuglUL3sxwhafPWDrhrr5lF8+nL65fnjI+ASbOq/+P4=";
//var queueName = "azure-queue";
var topicName = "azure-course-topic";
var sub1Name = "Sub1";

ServiceBusClient client;
ServiceBusProcessor processor = default!;

async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"{body} - Subscription: {sub1Name}");
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs processErrorEventArgs)
{
    Console.WriteLine(processErrorEventArgs.Exception.ToString());
    return Task.CompletedTask;
}
client = new ServiceBusClient(connectionString);
processor = client.CreateProcessor(topicName, sub1Name, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("\n Stopping the receiver ...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
catch(Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}
