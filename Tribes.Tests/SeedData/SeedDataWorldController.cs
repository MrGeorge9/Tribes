namespace Tribes.Tests.SeedData
{
    public class SeedDataWorldController
    {
        public static void PopulateTestData(ApplicationContext _db, string worldCount)
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            if (worldCount.Equals("1") || worldCount.Equals("3"))
            {
                var world = new World() { Name = "MyWorld", Kingdoms = new List<Kingdom>(), Locations = new List<Location>() };
                for (int i = 0; i < 3; i++)
                {
                    var user = new User() { Name = $"Johnny0{i}", PasswordHash = $"1234567890{i}", Email = $"hereIsJohnny0{i}@gmail.com", VerificationToken = String.Empty, ForgottenPasswordToken = String.Empty };
                    Kingdom kingdom = new Kingdom() { Name = $"kingdom0{i}", UserId = user.Id, WorldId = world.Id, World = world};
                    user.Kingdom = kingdom;
                    Location location = new Location() { XCoordinate = i, YCoordinate = i, WorldId = world.Id, World = world, KingdomId = kingdom.Id};
                    kingdom.Location = location;
                    world.Locations.Add(location);
                    world.Kingdoms.Add(kingdom);
                    _db.Users.Add(user);
                    _db.Kingdoms.Add(kingdom);
                    _db.Locations.Add(location);
                }
                _db.Worlds.Add(world);
            }
            if (worldCount.Equals("3"))
            {
                for (int i = 1; i <= 2; i++)
                {
                    var world = new World() { Name = $"world{i}", Kingdoms = new List<Kingdom>(), Locations = new List<Location>() };
                    for (int j = 0; j < 3; j++)
                    {
                        var user = new User() { Name = $"Johnny{i}{j}", PasswordHash = $"123456789{i}{j}", Email = $"hereIsJohnny{i}{j}@gmail.com", VerificationToken = String.Empty, ForgottenPasswordToken = String.Empty };
                        Kingdom kingdom = new Kingdom() { Name = $"kingdom{i}{j}", UserId = user.Id, WorldId = world.Id, World = world };
                        user.Kingdom = kingdom;
                        Location location = new Location() { XCoordinate = i, YCoordinate = j, WorldId = world.Id, World = world, KingdomId = kingdom.Id };
                        kingdom.Location = location;
                        world.Locations.Add(location);
                        world.Kingdoms.Add(kingdom);
                        _db.Users.Add(user);
                        _db.Kingdoms.Add(kingdom);
                        _db.Locations.Add(location);
                    }
                    _db.Worlds.Add(world);
                }
            }
            _db.SaveChanges();
        }
    }
}
