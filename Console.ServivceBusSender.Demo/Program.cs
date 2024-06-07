// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

var connectionString = "Endpoint=sb://azure-course.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VuglUL3sxwhafPWDrhrr5lF8+nL65fnjI+ASbOq/+P4=";
//var queueName = "azure-queue";
var topicName = "azure-course-topic";
var maxNumberOfMessage = 3;

var client = new ServiceBusClient(connectionString);
var sender = client.CreateSender(topicName);
using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();
for (int i = 0; i < maxNumberOfMessage; i++)
{
    if(!batch.TryAddMessage(new ServiceBusMessage($"This is a message - {i} ")))
    {
        Console.WriteLine($"Message - {i} was not added to batch");
    }
}

try
{
    await sender.SendMessagesAsync(batch);
    Console.WriteLine("Message sent");
}
catch(Exception ex)
{
    Console.WriteLine($"Exception - {ex.Message}");
    throw;
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}