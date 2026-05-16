using Bookly.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Infrastructure.Repositories
{
    internal abstract class Repository<T>
        where T : Entity
    {
        protected readonly ApplicationDbContext _dbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }
    }
}
