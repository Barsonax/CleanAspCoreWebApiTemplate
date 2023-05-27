using Mediator;
using Moq;

namespace CleanAspCore.Tests;

public class SenderMockBuilder
{
    public IReadOnlyList<object> Messages => _messages;
    public ISender Sender => _senderMock.Object;
	
    private readonly Mock<ISender> _senderMock;
    private readonly List<object> _messages;

    public SenderMockBuilder()
    {
        _senderMock = new Mock<ISender>(MockBehavior.Strict);
        _messages = new List<object>();
    }

    public void SetupCommandOrQuery<TResponse, TMessage>(Func<TResponse> returnValue)
        where TMessage : IRequest<TResponse>
    {
        _senderMock.SetupCommandOrQuery<TResponse, TMessage>(_messages, returnValue);
    }
}