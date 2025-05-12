using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UserCases.Create.Contracts;
using JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Create
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _appDbContext;
        public Repository(AppDbContext appDbContext) 
            => _appDbContext = appDbContext;

        public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken) 
            => await _appDbContext
                .Users
                .AsNoTracking()
                .AnyAsync(x => x.Email == email, cancellationToken);

        public async Task SaveAsync(User user, CancellationToken cancellationToken)
        {
            await _appDbContext
                .Users
                .AddAsync(user, cancellationToken);

            await _appDbContext
                .SaveChangesAsync(cancellationToken);
        }
    }
}
