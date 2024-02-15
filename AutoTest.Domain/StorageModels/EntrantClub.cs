namespace AutoTest.Domain.StorageModels
{
    public class EntrantClub
    {
        public EntrantClub() { }

        public EntrantClub(string club, string clubNumber)
        {
            Club = club;
            ClubNumber = clubNumber;
        }


        public string Club { get; } = string.Empty;

        public string ClubNumber { get; } = string.Empty;
    }
}
