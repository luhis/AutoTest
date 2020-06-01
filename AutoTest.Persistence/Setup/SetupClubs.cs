namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupClubs
    {
        public static void Setup(EntityTypeBuilder<Club> entity)
        {
            entity.HasKey(e => e.ClubId);
            entity.Property(e => e.ClubId).ValueGeneratedNever().IsRequired();
        }
    }
}
