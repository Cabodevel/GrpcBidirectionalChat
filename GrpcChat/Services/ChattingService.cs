using Grpc.Core;
using GrpcChat.Protos;

namespace GrpcChat.Services;

public class ChattingService : ChatService.ChatServiceBase
{
    private readonly LoremChatGenerator _loremChatGenerator;

    public ChattingService(LoremChatGenerator loremChatGenerator) => _loremChatGenerator = loremChatGenerator ?? throw new ArgumentNullException(nameof(loremChatGenerator));

    public override async Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
    {
        while(await requestStream.MoveNext())
        {
            var lorem = _loremChatGenerator.CreateLorem(CancellationToken.None).GetAsyncEnumerator();
            while(await lorem.MoveNextAsync())
            {
                await responseStream.WriteAsync(new ChatMessage { Message = lorem.Current });
            }

            await responseStream.WriteAsync(new ChatMessage { Message = string.Empty, IsFinished = true });
        }
    }
}
