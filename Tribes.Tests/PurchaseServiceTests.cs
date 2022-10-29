using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests
{
    public class PurchaseServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "PurchaseTests").Options;
        private readonly ApplicationContext _db;
        private readonly PurchaseService _purchaseService;

        public PurchaseServiceTests()
        {
            _db = new ApplicationContext(options);
            _purchaseService = new PurchaseService(_db);
        }

        public void SeedDatabase()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user = new User() { Email = "john@john.com", PasswordHash = "Johny123", Name = "John", ForgottenPasswordToken = "", VerificationToken = "" };
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
            var farm = new Farm()
            {
                Level = 1,
                Hp = 50,
                StartedAt = DateTime.Today,
                Production = 5,
            };
            var barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };
            _db.Buildings.Add(mine);
            _db.Buildings.Add(sawmill);
            _db.Buildings.Add(farm);
            _db.Buildings.Add(barracks);

            var gold = new Gold() { Amount = 200 };
            var wood = new Wood() { Amount = 200 };
            var food = new Food() { Amount = 200 };
            var soldier = new Soldier() { Amount = 200 };
            var people = new People() { Amount = 200 };
            _db.Resources.Add(gold);
            _db.Resources.Add(wood);
            _db.Resources.Add(food);
            _db.Resources.Add(soldier);
            _db.Resources.Add(people);

            kingdom.Buildings = new List<Building>();
            kingdom.Buildings.Add(mine);
            kingdom.Buildings.Add(sawmill);
            kingdom.Buildings.Add(farm);
            kingdom.Buildings.Add(barracks);
            kingdom.Resources = new List<Resource>();
            kingdom.Resources.Add(gold);
            kingdom.Resources.Add(wood);
            kingdom.Resources.Add(food);
            kingdom.Resources.Add(soldier);
            kingdom.Resources.Add(people);
            _db.SaveChanges();
        }
        [Fact]
        public void GetResourceTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();

            _purchaseService.kingdom = kingdom;
            Assert.Equal("Wood", _purchaseService.Wood().GetType().Name);
        }

        [Fact]
        public void EnoughResourcesForCreatingFarmTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();
            _purchaseService.kingdom = kingdom;

            Assert.True(_purchaseService.EnoughResourcesForCreatingFarm());

            var gold = kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
            gold.Amount -= 200;
            _db.SaveChanges();

            Assert.False(_purchaseService.EnoughResourcesForCreatingFarm());
        }

        [Fact]
        public void EnoughResourcesForUpdatingFarmTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();
            _purchaseService.kingdom = kingdom;

            var farm = kingdom.Buildings.FirstOrDefault(r => r.GetType().Name.Equals("Farm"));

            Assert.True(_purchaseService.EnoughResourcesForUpgradingFarm(farm));

            farm.Level += 5;
            _db.SaveChanges();

            Assert.False(_purchaseService.EnoughResourcesForUpgradingFarm(farm));
        }

        [Fact]
        public void EnoughResourcesForCreatingBarracksTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();
            _purchaseService.kingdom = kingdom;

            Assert.True(_purchaseService.EnoughResourcesForCreatingBarracks());

            var gold = kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
            gold.Amount -= 200;
            _db.SaveChanges();

            Assert.False(_purchaseService.EnoughResourcesForCreatingBarracks());
        }

        [Fact]
        public void EnoughResourcesForUpdatingBarracksTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();
            _purchaseService.kingdom = kingdom;

            var barracks = kingdom.Buildings.FirstOrDefault(r => r.GetType().Name.Equals("Barracks"));

            Assert.True(_purchaseService.EnoughResourcesForUpgradingFarm(barracks));

            barracks.Level += 5;
            _db.SaveChanges();

            Assert.False(_purchaseService.EnoughResourcesForUpgradingFarm(barracks));
        }

        [Fact]
        public void EnoughResourcesForCreatingSoldierTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();
            _purchaseService.kingdom = kingdom;

            Assert.True(_purchaseService.EnoughResourcesForCreatingSoldier());

            var gold = kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
            gold.Amount -= 200;
            _db.SaveChanges();

            Assert.False(_purchaseService.EnoughResourcesForCreatingSoldier());
        }

        [Fact]
        public void EnoughResourcesForUpdatingSoldierTest()
        {
            SeedDatabase();
            var kingdom = _db.Kingdoms.FirstOrDefault();
            _purchaseService.kingdom = kingdom;

            var soldier = (Soldier)kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Soldier"));

            Assert.True(_purchaseService.EnoughResourcesForUpgradingSoldier(soldier));

            soldier.Level += 50;
            _db.SaveChanges();

            Assert.False(_purchaseService.EnoughResourcesForUpgradingSoldier(soldier));
        }

    }
}
