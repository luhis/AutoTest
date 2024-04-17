using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupEntrantClub
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, EntrantClub> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.Club).IsRequired();
            entity.Property(e => e.ClubNumber).IsRequired();
        }
    }
}
