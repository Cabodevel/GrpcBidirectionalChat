using ChatClient.Protos;
using Grpc.Core;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7217");


var client = new ChatService.ChatServiceClient(channel);
using var call = client.Chat();

Console.WriteLine("Tarea en segundo plano para recibir mensajes");
var readTask = Task.Run(async () =>
{
    while(await call.ResponseStream.MoveNext())
    {
        Console.Write(call.ResponseStream.Current.Message + " ");
        if(call.ResponseStream.Current.IsFinished)
        {
            Console.WriteLine();
        }
    }
});

Console.WriteLine("iniciando envío de mensajes");

var result = "";
Console.WriteLine("Escribe un mensaje y presiona enter.");

while(!result.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
{
    Console.WriteLine();
    result = Console.ReadLine();
    Console.WriteLine("Usuario: " +  result);
    Console.WriteLine();

    await call.RequestStream.WriteAsync(new ChatMessage { Message = result });
    Console.WriteLine("Asistente: ");
}

Console.WriteLine("Disconnecting");
await call.RequestStream.CompleteAsync();
await readTask;
Console.ReadKey();
