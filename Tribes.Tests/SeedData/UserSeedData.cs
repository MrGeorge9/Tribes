namespace Tribes.Tests.SeedData
{
    public class UserSeedData
    {

        public static void PopulateTestData(ApplicationContext _db, string dbFillCode)
        {

            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user1 = new User()
            {
                Name = "Matilda",
                PasswordHash = "m",
                Email = "matilda@gmail.com",
                VerificationToken = String.Empty,
                ForgottenPasswordToken = String.Empty

            };

            var user2 = new User()
            {
                Name = "Klotilda",
                PasswordHash = "k",
                Email = "klotilda@gmail.com",
                VerificationToken = String.Empty,
                ForgottenPasswordToken = String.Empty
            };

            try
            {
                int worlds = Int32.Parse(dbFillCode.Substring(dbFillCode.Length - 1));
                if (worlds > 0)
                    for (int i = 0; i < worlds; i++)
                        _db.Worlds.Add(new World() { Name = $"world{i}"});
              
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{dbFillCode}'");
            }

            _db.Users.Add(user1);
            _db.Users.Add(user2);
            _db.SaveChanges();
        }
    }
}