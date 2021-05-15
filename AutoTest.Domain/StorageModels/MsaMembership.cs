namespace AutoTest.Domain.StorageModels
{
    public class MsaMembership
    {
        public MsaMembership()
        {
        }

        public MsaMembership(string msaLicenseType, uint msaLicense)
        {
            MsaLicenseType = msaLicenseType;
            MsaLicense = msaLicense;
        }


        public string MsaLicenseType { get; } = string.Empty;

        public uint MsaLicense { get; }
    }
}
