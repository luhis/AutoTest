namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupAuthorisationEmails
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, AuthorisationEmail> entity) where T : class
        {
            entity.Property(e => e.Email).IsRequired();
        }
    }
}
