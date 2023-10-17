using MediatR;

namespace LongRunningJobImitator.Api.Mediator.Requests
{
    public record ConversionCanceledEvent(Guid JobId) : IRequest;
}
