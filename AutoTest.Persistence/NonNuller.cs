using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence
{
    public static class NonNuller
    {
        public static DbSet<T> ThrowIfNull<T>(this DbSet<T>? nullable) where T : class
        {
            return nullable == null ? throw new NullReferenceException() : nullable;
        }
    }

    public static class DbSetExtensions
    {
        public static async Task Upsert<T>(this DbSet<T> set, T toSave, Expression<Func<T, bool>> search, CancellationToken cancellationToken) where T : class
        {
            if (await set.SingleOrDefaultAsync(search, cancellationToken) == null)
            {
                set.Add(toSave);
            }
            else
            {
                set.Add(toSave);
                set.Update(toSave).State = EntityState.Modified;
            }
        }
    }

    public static class QueryableExtensions
    {
        public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this IOrderedQueryable<T> q, CancellationToken cancellationToken)
        {
            return await q.ToArrayAsync(cancellationToken);
        }
    }
}
