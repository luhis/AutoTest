namespace AutoTest.Web.Models
{
    public class AccessModel
    {
        public AccessModel(bool canViewClubs)
        {
            this.CanViewClubs = canViewClubs;
        }

        public bool CanViewClubs { get; }
    }
}
