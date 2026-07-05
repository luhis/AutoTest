using System.Linq;
using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping;

public static class MapClub
{
    public static Club Map(ulong clubId, ClubSaveModel model)
    {
        var c = new Club(clubId, model.ClubName, model.ClubPaymentAddress, model.Website);
        c.SetAdminEmails(model.AdminEmails.Select(a => new AuthorisationEmail(a.Email)).ToArray());
        return c;
    }
}
