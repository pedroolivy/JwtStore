using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UserCases.Create.Contracts;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.SharedContext.Exceptions;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UserCases.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IRepository _repository;
        private readonly IService _service;

        public Handler(IRepository repository, IService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            #region 01. Valida a requisição

            try
            {
                var res = Specification.Ensure(request);
                if (!res.IsValid)
                    return new Response("Requisição inválida", 400, res.Notifications);
            }
            catch (Exception) 
            {
                throw new InvalidRequestException();
            }

            #endregion

            #region 02. Gerar os Objetos

            Email email;
            Password password;
            User user;

            try
            {
                email = new Email(request.Email);
                password = new Password(request.Password);
                user = new User(request.Name, request.Email, password);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, 400);
            }

            #endregion

            #region 03. Verifica se o usuário existe no banco

            try
            {
                var exists = await _repository.AnyAsync(request.Email, cancellationToken);

                if (exists)
                    return new Response("Esse e-mail já está em uso", 400);
            }
            catch (Exception)
            {
                throw new InvalidRequestException("Falha ao buscar os dados");
            }

            #endregion

            #region 04. Persiste od dados

            try
            {
                await _repository.SaveAsync(user, cancellationToken);
            }
            catch (Exception)
            {
                throw new InvalidRequestException("Falha ao persistir os dados");
            }

            #endregion

            #region Enviar e-mail de ativação

            try
            {
                await _service.SendVerificationEmailAsync(user, cancellationToken);
            }
            catch { }

            #endregion

            return new Response("Conta criada", new ResponseData(user.Id, user.Name, user.Email));
        }
    }
}
