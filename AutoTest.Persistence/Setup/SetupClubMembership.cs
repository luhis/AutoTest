using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupClubMembership
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, ClubMembership> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.ClubName).IsRequired();
            entity.Property(e => e.MembershipNumber).IsRequired();
            entity.Property(e => e.Expiry).IsRequired();
        }
    }
}
