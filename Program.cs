//this is using the new .NET 6 features
// --removes the need for boilerplate: No namespace, class or Main method are necessary) 

//Originally from: https://github.com/MicrosoftLearning/AZ-204-DevelopingSolutionsforMicrosoftAzure/blob/7b1e03e98e88aa33fbacc144d78dde0a44a55109/Allfiles/Labs/10/Solution/MessageProcessor/Program.cs

using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=eric71;AccountKey=4PyRIkVc/LYeUiZnllwNYlpJtEcH4zLb6ReUnXmwjARfaVw/j+IIcPrQuGQa/K/H2QH9m9uAQ2h2+AStt8JjcA==;EndpointSuffix=core.windows.net";
string queueName = "docs";

QueueClient client = new QueueClient(storageConnectionString, queueName);
await client.CreateAsync();

Console.WriteLine($"---Account Metdata---");
Console.WriteLine($"Account Uri:\t{client.Uri}");

Console.WriteLine($"---Existing Messages---");
int batchSize = 10;
TimeSpan visibilityTimeout = TimeSpan.FromSeconds(10);

Response<QueueMessage[]> messages = await client.ReceiveMessagesAsync(batchSize, visibilityTimeout);

foreach (QueueMessage message in messages.Value)
{
    Console.WriteLine($"[{message.MessageId}]\t{message.MessageText}");
    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
}

Console.WriteLine($"---New Messages---");
string greeting = $"Hi, Developer! {DateTime.Now.ToLongTimeString()}";
await client.SendMessageAsync(greeting);

Console.WriteLine($"Sent Message:\t{greeting}");
