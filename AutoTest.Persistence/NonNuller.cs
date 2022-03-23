using System;
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
}
