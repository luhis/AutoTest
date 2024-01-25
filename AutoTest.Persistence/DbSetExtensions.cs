using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence
{
    public static class DbSetExtensions
    {
        public static async Task<UpdateStatus> Upsert<T>(this DbSet<T> set, T toSave, Expression<Func<T, bool>> search, CancellationToken cancellationToken) where T : class
        {
            if (await set.SingleOrDefaultAsync(search, cancellationToken) == null)
            {
                set.Add(toSave);
                return UpdateStatus.Add;
            }
            else
            {
                set.Attach(toSave);
                set.Update(toSave).State = EntityState.Modified;
                return UpdateStatus.Update;
            }
        }

        public enum UpdateStatus
        {
            Add, Update
        }
    }
}
