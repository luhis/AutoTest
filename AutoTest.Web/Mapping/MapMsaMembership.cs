using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping
{
    public static class MapMsaMembership
    {

        public static MsaMembership Map(MsaMembershipSaveModel membership)
        {
            return new MsaMembership(membership.MsaLicenseType, membership.MsaLicense);
        }
    }
}
