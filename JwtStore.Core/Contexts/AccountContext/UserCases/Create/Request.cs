using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UserCases.Create
{
    public record Request(string Name, string Email, string Password) : IRequest<Response>;
}
