using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.SeedData
{
    public class BuildingControllerTestsSeedDataNoResources
    {
        public static void PopulateTestData(ApplicationContext _db)
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user = new User() { Email = "john@john.com", PasswordHash = "Johny123", Name = "John", ForgottenPasswordToken = String.Empty, VerificationToken = String.Empty };
            _db.Users.Add(user);

            var kingdom = new Kingdom() { Name = "Aden" };
            _db.Kingdoms.Add(kingdom);

            user.Kingdom = kingdom;

            var mine = new Mine()
            {
                Level = 1,
                Hp = 100,
                StartedAt = DateTime.Today,
                Production = 7,
            };
            var sawmill = new Sawmill()
            {
                Level = 1,
                Hp = 75,
                StartedAt = DateTime.Today,
                Production = 15,
            };
            _db.Buildings.Add(mine);
            _db.Buildings.Add(sawmill);

            var gold = new Gold() { Amount = 0 };
            var wood = new Wood() { Amount = 0 };
            var food = new Food() { Amount = 0 };
            var soldier = new Soldier() { Amount = 0 };
            var people = new People() { Amount = 0 };
            _db.Resources.Add(gold);
            _db.Resources.Add(wood);
            _db.Resources.Add(food);
            _db.Resources.Add(soldier);
            _db.Resources.Add(people);

            kingdom.Buildings = new List<Building>();
            kingdom.Buildings.Add(mine);
            kingdom.Buildings.Add(sawmill);
            kingdom.Resources = new List<Resource>();
            kingdom.Resources.Add(gold);
            kingdom.Resources.Add(wood);
            kingdom.Resources.Add(food);
            kingdom.Resources.Add(soldier);
            kingdom.Resources.Add(people);
            _db.SaveChanges();
        }
    }
}
