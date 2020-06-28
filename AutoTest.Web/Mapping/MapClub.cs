using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models;

namespace AutoTest.Web.Mapping
{
    public static class MapClub
    {
        public static Club Map(ulong clubId, ClubSaveModel model) => new Club(clubId, model.ClubName, model.ClubPaymentAddress, model.Website);
    }
}
