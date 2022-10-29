using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.SeedData
{
    public class EmailControllerTestSeedData
    {
        public static void PopulateTestData(ApplicationContext _db)
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user = new User() 
            { 
                Email = "john@john.com", 
                PasswordHash = "Johny123", 
                Name = "John", 
                ForgottenPasswordToken = String.Empty 
            };
            user.VerificationToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NTgyMzE5ODEsImV4cCI6MTk3Mzg1MTE4MCwiaWF0IjoxNjU4MjMxOTgxfQ.oYzvDiJ7FaJLWdmkbFxV14gNXGJtUseWAgOdgqhZFxc";
            _db.Users.Add(user);

            var user1 = new User()
            {
                Email = "george@george.com",
                PasswordHash = "George123",
                Name = "George",
                VerificationToken = String.Empty,
                ForgottenPasswordToken = String.Empty,
                VerifiedAt = DateTime.Now,
                ForgottenPasswordTokenExpiresAt = DateTime.Now.AddHours(1)
            };
            _db.Users.Add(user1);

            var user2 = new User()
            {
                Email = "tobi@tobi.com",
                PasswordHash = "Tobis123",
                Name = "Tobi",
                VerificationToken = String.Empty,
                ForgottenPasswordToken = String.Empty,
                VerifiedAt = DateTime.Now,
                ForgottenPasswordTokenVerifiedAt = DateTime.Now,
                ForgottenPasswordTokenExpiresAt = DateTime.MinValue
            };
            _db.Users.Add(user2);

            var kingdom = new Kingdom() { Name = "Aden" };
            _db.Kingdoms.Add(kingdom);

            var kingdom1 = new Kingdom() { Name = "Giran" };
            _db.Kingdoms.Add(kingdom1);

            var kingdom2 = new Kingdom() { Name = "Dion" };
            _db.Kingdoms.Add(kingdom2);

            user.Kingdom = kingdom;
            user1.Kingdom = kingdom1;
            user2.Kingdom = kingdom2;

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






