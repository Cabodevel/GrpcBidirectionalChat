using System.Runtime.CompilerServices;

namespace GrpcChat.Services;

public class LoremChatGenerator
{
    public async IAsyncEnumerable<string> CreateLorem([EnumeratorCancellation]CancellationToken ct)
    {
        var lorem = LoremNETCore.Generate.Words(10, true, true).Split(" ");

        foreach (var word in lorem)
        {
            await Task.Delay(300);
            yield return word;
        }
    } 
}
