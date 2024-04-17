using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupAcceptDeclaration
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, AcceptDeclaration> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.IsAccepted).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
        }
    }
}
