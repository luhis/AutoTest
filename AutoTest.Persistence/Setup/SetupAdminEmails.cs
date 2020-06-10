namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupAdminEmails
    {
        public static void Setup(EntityTypeBuilder<AdminEmail> entity)
        {
            entity.HasKey(e => e.AdminEmailId);
            entity.HasOne<Club>().WithMany().HasForeignKey(p => p.ClubId);
        }
    }
}