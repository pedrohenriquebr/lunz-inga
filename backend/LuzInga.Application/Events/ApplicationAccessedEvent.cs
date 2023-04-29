using MediatR;

namespace LuzInga.Application.Events;


public sealed record ApplicationAccessedEvent(
    DateTime Datetime,
    string Url,
    string? Username,
    string Method,
    string RemoteIpAddress
);