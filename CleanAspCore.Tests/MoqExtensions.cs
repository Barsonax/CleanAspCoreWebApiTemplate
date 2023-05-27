using Mediator;
using Moq;

namespace CleanAspCore.Tests;

public static class MoqExtensions
{
    public static void SetupCommandOrQuery<TResponse, TMessage>(this Mock<ISender> senderMock, List<object> messages, Func<TResponse> returnValue)
        where TMessage : IRequest<TResponse>
    {
        senderMock
            .Setup(x => x.Send(It.IsAny<TMessage>(), It.IsAny<CancellationToken>()))
            .Callback<IBaseRequest, CancellationToken>((x, y) => messages.Add(x))
            .ReturnsAsync(returnValue);
    }
}