using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence
{
    public static class QueryableExtensions
    {
        public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this IQueryable<T> q, CancellationToken cancellationToken)
        {
            return await q.ToArrayAsync(cancellationToken);
        }
    }
}
