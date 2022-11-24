namespace Generator.Elements
{
    public class SendMessage : IMessage
    {
        public string Message { get; set; }

        public SendMessage(string message)
        {
            Message = message;
        }
    }
}
