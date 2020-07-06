namespace AutoTest.Domain.StorageModels
{
    public class AuthorisationEmail
    {
        public AuthorisationEmail(string email)
        {
            this.Email = email;
        }

        public string Email { get; }
    }
}
