namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupMarshal
    {
        public static void Setup(EntityTypeBuilder<Marshal> entity)
        {
            entity.HasKey(e => e.MarshalId);
            entity.Property(e => e.MarshalId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.RegistrationNumber).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.GivenName).IsRequired();
            entity.Property(e => e.FamilyName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
            entity.OwnsOne(a => a.EmergencyContact, SetupEmergencyContact.Setup);
            entity.OwnsOne(a => a.AcceptDeclaration, SetupAcceptDeclaration.Setup);
        }
    }
}
