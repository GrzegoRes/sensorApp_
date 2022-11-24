using System.Threading;
using System.Threading.Tasks;

namespace Generator.Elements
{
    public class SendMessageHandler : IHandler<SendMessage>
    {
        public Task HandleAsync(SendMessage message, CancellationToken token)
        {
            System.Console.WriteLine($"Send {message}");
            return Task.CompletedTask;
        }
    }
}
