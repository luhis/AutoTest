using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupPayment
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, Payment> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.Method).IsRequired();
            entity.Property(e => e.PaidAt).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
        }
    }
}
