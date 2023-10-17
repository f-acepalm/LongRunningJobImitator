using MediatR;

namespace LongRunningJobImitator.Api.Mediator.Requests
{
    public record ConversionRequestedEvent(Guid JobId, string Text) : IRequest;
}
