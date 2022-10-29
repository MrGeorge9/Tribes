using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.SeedData
{
    public class BuildingControllerTestsSeedData
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
            var tonwhall = new TownHall()
            {
                Level = 2,
                Hp = 1000,
                StartedAt = DateTime.Today,
                Production = 4,
            };
            var barracks = new Barracks()
            {
                Level = 2,
                Hp = 600,
                StartedAt = DateTime.Today,
                Production = 2,
            };
            _db.Buildings.Add(mine);
            _db.Buildings.Add(sawmill);
            _db.Buildings.Add(tonwhall);
            _db.Buildings.Add(barracks);

            var gold = new Gold() { Amount = 500 };
            var wood = new Wood() { Amount = 500 };
            var food = new Food() { Amount = 500 };
            var soldier = new Soldier() { Amount = 25 };
            var people = new People() { Amount = 500 };
            _db.Resources.Add(gold);
            _db.Resources.Add(wood);
            _db.Resources.Add(food);
            _db.Resources.Add(soldier);
            _db.Resources.Add(people);

            kingdom.Buildings = new List<Building>();
            kingdom.Buildings.Add(mine);
            kingdom.Buildings.Add(sawmill);
            kingdom.Buildings.Add(tonwhall);
            kingdom.Buildings.Add(barracks);
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