using System;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service
{
    public static class NonNuller
    {
        public static DbSet<T> ThrowIfNull<T>(DbSet<T>? nullable) where T : class
        {
            return nullable == null ? throw new NullReferenceException() : nullable;
        }
    }
}
