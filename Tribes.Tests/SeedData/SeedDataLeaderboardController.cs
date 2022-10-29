using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;

namespace Tribes.Tests.SeedData
{
    public class SeedDataLeaderboardController
    {
        public static void PopulateTestData(ApplicationContext appContext, bool includeKingdoms)
        {
            appContext.Database.EnsureDeleted();
            appContext.Database.EnsureCreated();
            User user1 = new User() { Name = "abcde", PasswordHash = "12345678", Email = "myemail@gmail.com", VerificationToken = String.Empty, ForgottenPasswordToken = String.Empty };
            User user2 = new User() { Name = "edcba", PasswordHash = "87654321", Email = "emailofmine@gmail.com", VerificationToken = String.Empty, ForgottenPasswordToken = String.Empty };
            User user3 = new User() { Name = "zyxwv", PasswordHash = "18273645", Email = "justemail@gmail.com", VerificationToken = String.Empty, ForgottenPasswordToken = String.Empty };
            World world = new World() { Name = "world"};
            if (includeKingdoms)
            {
                Location location1 = new Location() { XCoordinate = 1, YCoordinate = 1 };
                Location location2 = new Location() { XCoordinate = 3, YCoordinate = 3 };
                Location location3 = new Location() { XCoordinate = 5, YCoordinate = 5 };
                Army army1 = new Army() { Soldiers = new List<Soldier>() };
                Army army2 = new Army() { Soldiers = new List<Soldier>() };
                Army army3 = new Army() { Soldiers = new List<Soldier>() };
                List<Resource> resources1 = new List<Resource>();
                List<Resource> resources2 = new List<Resource>();
                List<Resource> resources3 = new List<Resource>();
                for (int i = 0; i < 10; i++)
                {
                    Soldier soldier = new Soldier() { Amount = 1, UpdatedAt = DateTime.Now, Level = 1, TotalHP = 25, Attack = 10, Defense = 10 };
                    soldier.Army = army1;
                    soldier.ArmyId = army1.Id;
                    army1.Soldiers.Add(soldier);
                    resources1.Add(soldier);
                    Soldier soldier2 = new Soldier() { Amount = 1, UpdatedAt = DateTime.Now, Level = 1, TotalHP = 25, Attack = 15, Defense = 10 };
                    soldier2.Army = army2;
                    soldier2.ArmyId = army2.Id;
                    army2.Soldiers.Add(soldier2);
                    resources2.Add(soldier2);
                    Soldier soldier3 = new Soldier() { Amount = 1, UpdatedAt = DateTime.Now, Level = 1, TotalHP = 25, Attack = 25, Defense = 10 };
                    soldier3.Army = army3;
                    soldier3.ArmyId = army3.Id;
                    army3.Soldiers.Add(soldier3);
                    resources3.Add(soldier3);
                }

                List<Building> buildings1 = new List<Building>
            {
                new Barracks() {Level = 1, Hp = 300, StartedAt = DateTime.Today, Production = 1},
                new TownHall() {Level = 1, Hp = 500, StartedAt = DateTime.Today, Production = 20},
                new Sawmill() {Level = 1, Hp = 75, StartedAt = DateTime.Today, Production = 15},
                new Farm() {Level = 1, Hp = 50, StartedAt = DateTime.Today, Production = 5},
                new Mine() {Level = 1, Hp = 100, StartedAt = DateTime.Today, Production = 7},
            };

                List<Building> buildings2 = new List<Building>
            {
                new Barracks() {Level = 2, Hp = 300, StartedAt = DateTime.Today, Production = 1},
                new TownHall() {Level = 2, Hp = 500, StartedAt = DateTime.Today, Production = 20},
                new Sawmill() {Level = 2, Hp = 75, StartedAt = DateTime.Today, Production = 15},
                new Farm() {Level = 2, Hp = 50, StartedAt = DateTime.Today, Production = 5},
                new Mine() {Level = 2, Hp = 100, StartedAt = DateTime.Today, Production = 7},
            };

                List<Building> buildings3 = new List<Building>
            {
                new Barracks() {Level = 1, Hp = 300, StartedAt = DateTime.Today, Production = 1},
                new TownHall() {Level = 6, Hp = 500, StartedAt = DateTime.Today, Production = 20},
                new Sawmill() {Level = 1, Hp = 75, StartedAt = DateTime.Today, Production = 15},
                new Farm() {Level = 1, Hp = 50, StartedAt = DateTime.Today, Production = 5},
                new Mine() {Level = 1, Hp = 100, StartedAt = DateTime.Today, Production = 7},
            };

                Kingdom kingdom1 = new Kingdom() { Buildings = buildings1, Armies = new List<Army>() { army1 }, Name = "MyKingdom", Location = location1, User = user1, UserId = user1.Id, World = world, WorldId = world.Id };
                Kingdom kingdom2 = new Kingdom() { Buildings = buildings2, Armies = new List<Army>() { army2 }, Name = "MyOtherKingdom", Location = location2, User = user2, UserId = user2.Id, World = world, WorldId = world.Id };
                Kingdom kingdom3 = new Kingdom() { Buildings = buildings3, Armies = new List<Army>() { army3 }, Name = "MyOtherKingdom", Location = location3, User = user3, UserId = user3.Id, World = world, WorldId = world.Id };

                army1.Kingdom = kingdom1;
                army2.Kingdom = kingdom2;
                army3.Kingdom = kingdom3;
                location1.Kingdom = kingdom1;
                location1.KingdomId = kingdom1.Id;
                location2.Kingdom = kingdom2;
                location2.KingdomId = kingdom2.Id;
                location3.Kingdom = kingdom3;
                location3.KingdomId = kingdom3.Id;
                kingdom1.Resources = resources1;
                kingdom2.Resources = resources2;
                kingdom3.Resources = resources3;

                appContext.Kingdoms.AddRange(new List<Kingdom>() { kingdom1, kingdom2, kingdom3 });
                appContext.Resources.AddRange(resources1);
                appContext.Resources.AddRange(resources2);
                appContext.Resources.AddRange(resources3);
                appContext.Buildings.AddRange(buildings1);
                appContext.Buildings.AddRange(buildings2);
                appContext.Buildings.AddRange(buildings3);
                appContext.Armies.AddRange(new List<Army>() { army1, army2, army3 });
                appContext.Locations.AddRange(new List<Location>() { location1, location2, location3 });
            }
            appContext.Users.AddRange(new List<User>() { user1, user2, user3 });
            appContext.Worlds.Add(world);
            appContext.SaveChanges();
        }
    }
}
