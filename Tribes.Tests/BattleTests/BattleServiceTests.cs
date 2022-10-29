using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.BattleServiceTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BattleServiceTests : IDisposable
    {
        public BattleService BattleService;
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
               .UseInMemoryDatabase(databaseName: "ArmyServiceTest").Options;
        public ApplicationContext Context;
        public Mock<IArmyFactory> ArmyFactory;
        public Mock<IArmyService> ArmyService;
        public IConfiguration Config;
        public IBattleFactory BattleFactory;
        public RuleService RuleService;

        public BattleServiceTests()
        {
            Config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in Config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            Context = new ApplicationContext(options);
            ArmyFactory = new Mock<IArmyFactory>();
            ArmyService = new Mock<IArmyService>();
            BattleFactory = new BattleFactory(Config);
            RuleService = new ConfigRuleService(Context,Config);
            BattleService = new BattleService(Context, ArmyFactory.Object, ArmyService.Object, RuleService, BattleFactory);
            World world = new World();
            world.Name = "Worl1";
            Context.Worlds.Add(world);
            User user1 = new User
            {
                Name = "user1",
                PasswordHash = "password1",
                ForgottenPasswordToken = "token1",
                VerificationToken = "token1",
                Email = "email1"
            };
            Kingdom kingdom1 = new Kingdom
            {
                Name = "name1",
                Resources = new List<Resource>(),
                Armies = new List<Army>()
            };

            Location location1 = new Location()
            {
                XCoordinate = 0,
                YCoordinate = 0
            };
            kingdom1.Location = location1;
            kingdom1.World = world;
            kingdom1.User = user1;
            Army army1 = new Army()
            {
                Soldiers = new List<Soldier>(),
            };
            army1.Kingdom = kingdom1;
            Context.Users.Add(user1);
            Context.Kingdoms.Add(kingdom1);
            Context.Locations.Add(location1);
            Context.Armies.Add(army1);
            for (int i = 0; i < 19; i++)
            {
                Soldier soldier = new Soldier()
                {
                    Attack = 20,
                    Defense = 20,
                    TotalHP = 50,
                    CurrentHP = 50,
                    Level = 1
                };
                if (i < 13)
                {
                    soldier.Army = army1;
                }
                soldier.Kingdom = kingdom1;
                Context.Resources.Add(soldier);
            }
            kingdom1.Resources.Add(new Food() { Amount = 0 });
            Context.SaveChanges();
        }

        [Fact]
        public void BattleService_Battle_DBChange()
        {
            Battle battle = new Battle();
            Kingdom attacker = Context.Kingdoms.FirstOrDefault();
            User user2 = new User
            {
                Name = "user2",
                PasswordHash = "password2",
                ForgottenPasswordToken = "token2",
                VerificationToken = "token2",
                Email = "email2"
            };
            Kingdom kingdom2 = new Kingdom
            {
                Name = "name2",
                Resources = new List<Resource>(),
                Armies = new List<Army>()
            };

            Location location2 = new Location()
            {
                XCoordinate = 15,
                YCoordinate = 15
            };
            kingdom2.Location = location2;
            kingdom2.World = Context.Worlds.FirstOrDefault();
            kingdom2.User = user2;
            Context.Users.Add(user2);
            Context.Kingdoms.Add(kingdom2);
            Context.Locations.Add(location2);
            for (int i = 0; i < 12; i++)
            {
                Soldier soldier = new Soldier()
                {
                    Attack = 20,
                    Defense = 20,
                    TotalHP = 50,
                    CurrentHP = 50,
                    Level = 1
                };
                soldier.Kingdom = kingdom2;
                Context.Resources.Add(soldier);
            }
            kingdom2.Resources.Add(new Food() { Amount = 1000 });
            Kingdom defender = kingdom2;
            battle.Attacker = attacker;
            battle.AttackerId = attacker.Id;
            battle.Defender = defender;
            battle.DefenderId = defender.Id;
            battle.AttackerArmyId = Context.Armies.FirstOrDefault().Id;
            battle.RequestedAt = DateTime.Now;
            battle.WinnerId = null;
            Context.Battles.Add(battle);
            Context.SaveChanges();
            ArmyFactory.Setup(i => i.CrateArmy(It.IsAny<List<Soldier>>(), kingdom2, 0))
                .Returns(new Army { Soldiers = kingdom2.Resources.Where(r => r.GetType() == typeof(Soldier))
                .Select(r => (Soldier)r).ToList(), Kingdom = kingdom2 });

            BattleService.Battle(battle);

            Assert.True(Context.Battles.FirstOrDefault().WinnerId != null);
            Assert.Equal(700, Context.Resources.FirstOrDefault(r => r.Kingdom == kingdom2).Amount);
            Assert.Equal(300, Context.Resources.FirstOrDefault(r => r.Kingdom == attacker).Amount);
            Assert.True(Context.Armies.FirstOrDefault().Soldiers.All(s => s.TotalHP == s.CurrentHP));
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        [Fact]
        public void BattleService_BattleCost_Error1()
        {
            BattleCostDTO battleCostDTO = BattleService.BattleCost(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), 10);

            Assert.Equal("Kingdom not found", BattleService.GetError());
            Assert.Null(battleCostDTO);
        }

        [Fact]
        public void BattleService_BattleCost_Error2()
        {
            BattleCostDTO battleCostDTO = BattleService.BattleCost(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), 0);

            Assert.Equal("Invalid kingdom Id", BattleService.GetError());
            Assert.Null(battleCostDTO);
        }

        [Fact]
        public void BattleService_BattleCost_Error3()
        {
            ArmyService.Setup(i => i.CreateArmy(It.IsAny<List<int>>(), Context.Kingdoms.FirstOrDefault().Id, 0))
                .Returns(new Army
                {
                    Soldiers = Context.Resources.ToList().Where(r => r.GetType() == typeof(Soldier))
                .Select(r => (Soldier)r).ToList(),
                    Kingdom = Context.Kingdoms.FirstOrDefault()
                });
            BattleCostDTO battleCostDTO = BattleService.BattleCost(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), 1);

            Assert.Equal("You cannot attack yourself", BattleService.GetError());
            Assert.Null(battleCostDTO);
        }

        [Fact]
        public void BattleService_BattleCost_Error4()
        {
            Kingdom attacker = Context.Kingdoms.FirstOrDefault();
            User user2 = new User
            {
                Name = "user2",
                PasswordHash = "password2",
                ForgottenPasswordToken = "token2",
                VerificationToken = "token2",
                Email = "email2"
            };
            Kingdom kingdom2 = new Kingdom
            {
                Name = "name2",
                Resources = new List<Resource>(),
                Armies = new List<Army>()
            };

            Location location2 = new Location()
            {
                XCoordinate = 15,
                YCoordinate = 15
            };
            kingdom2.Location = location2;
            kingdom2.World = Context.Worlds.FirstOrDefault();
            kingdom2.User = user2;
            Context.Users.Add(user2);
            Context.Kingdoms.Add(kingdom2);
            Context.Locations.Add(location2);
            Kingdom defender = kingdom2;
            kingdom2.CanBeAttackedAt = DateTime.Now.AddHours(1);
            Context.SaveChanges();
            ArmyService.Setup(i => i.CreateArmy(It.IsAny<List<int>>(),attacker.Id , 21))
                .Returns(new Army
                {
                    Soldiers = Context.Resources.ToList().Where(r => r.GetType() == typeof(Soldier))
                .Select(r => (Soldier)r).ToList(),
                    Kingdom = Context.Kingdoms.FirstOrDefault()
                });

            BattleCostDTO battleCostDTO = BattleService.BattleCost(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), kingdom2.Id);

            Assert.Equal("Kingdom can not be attacked yet, as it was attacked recently", BattleService.GetError());
            Assert.Null(battleCostDTO);
        }

        [Fact]
        public void BattleService_BattleCost_BattleCostDTO()
        {
            Kingdom attacker = Context.Kingdoms.FirstOrDefault();
            User user2 = new User
            {
                Name = "user2",
                PasswordHash = "password2",
                ForgottenPasswordToken = "token2",
                VerificationToken = "token2",
                Email = "email2"
            };
            Kingdom kingdom2 = new Kingdom
            {
                Name = "name2",
                Resources = new List<Resource>(),
                Armies = new List<Army>()
            };

            Location location2 = new Location()
            {
                XCoordinate = 15,
                YCoordinate = 15
            };
            kingdom2.Location = location2;
            kingdom2.World = Context.Worlds.FirstOrDefault();
            kingdom2.User = user2;
            Context.Users.Add(user2);
            Context.Kingdoms.Add(kingdom2);
            Context.Locations.Add(location2);
            Kingdom defender = kingdom2;
            Context.SaveChanges();
            ArmyService.Setup(i => i.CreateArmy(It.IsAny<List<int>>(), attacker.Id, 21))
               .Returns(new Army
               {
                   Soldiers = Context.Resources.ToList().Where(r => r.GetType() == typeof(Soldier))
               .Select(r => (Soldier)r).ToList(),
                   Kingdom = Context.Kingdoms.FirstOrDefault()
               });

            BattleCostDTO battleCostDTO = BattleService.BattleCost(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), kingdom2.Id);

            Assert.Equal(806, battleCostDTO.food);
        }

        [Fact]
        public void BattleService_CreateBattle_Error()
        {
            Kingdom attacker = Context.Kingdoms.FirstOrDefault();
            User user2 = new User
            {
                Name = "user2",
                PasswordHash = "password2",
                ForgottenPasswordToken = "token2",
                VerificationToken = "token2",
                Email = "email2"
            };
            Kingdom kingdom2 = new Kingdom
            {
                Name = "name2",
                Resources = new List<Resource>(),
                Armies = new List<Army>()
            };

            Location location2 = new Location()
            {
                XCoordinate = 15,
                YCoordinate = 15
            };
            kingdom2.Location = location2;
            kingdom2.World = Context.Worlds.FirstOrDefault();
            kingdom2.User = user2;
            Context.Users.Add(user2);
            Context.Kingdoms.Add(kingdom2);
            Context.Locations.Add(location2);
            Kingdom defender = kingdom2;
            Context.SaveChanges();
            ArmyService.Setup(i => i.CreateArmy(It.IsAny<List<int>>(), attacker.Id, 21))
              .Returns(new Army
              {
                  Soldiers = Context.Resources.ToList().Where(r => r.GetType() == typeof(Soldier))
              .Select(r => (Soldier)r).ToList(),
                  Kingdom = Context.Kingdoms.FirstOrDefault()
              });

            Assert.Null(BattleService.CreateBattle(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), kingdom2.Id));
            Assert.Equal("Insufficient food to attack", BattleService.GetError());
        }

        [Fact]
        public void BattleService_CreateBattle_StatusDTO()
        {
            Kingdom attacker = Context.Kingdoms.FirstOrDefault();
            User user2 = new User
            {
                Name = "user2",
                PasswordHash = "password2",
                ForgottenPasswordToken = "token2",
                VerificationToken = "token2",
                Email = "email2"
            };
            Kingdom kingdom2 = new Kingdom
            {
                Name = "name2",
                Resources = new List<Resource>(),
                Armies = new List<Army>()
            };

            Location location2 = new Location()
            {
                XCoordinate = 15,
                YCoordinate = 15
            };
            kingdom2.Location = location2;
            kingdom2.World = Context.Worlds.FirstOrDefault();
            kingdom2.User = user2;
            Context.Users.Add(user2);
            Context.Kingdoms.Add(kingdom2);
            Context.Locations.Add(location2);
            Kingdom defender = kingdom2;
            attacker.Resources.FirstOrDefault(r => r.GetType() == typeof(Food)).Amount = 1000;
            Context.SaveChanges();
            ArmyService.Setup(i => i.CreateArmy(It.IsAny<List<int>>(), attacker.Id, 21))
              .Returns(new Army
              {
                  Soldiers = Context.Resources.ToList().Where(r => r.GetType() == typeof(Soldier))
              .Select(r => (Soldier)r).ToList(),
                  Kingdom = Context.Kingdoms.FirstOrDefault()
              });

            StatusDTO statusDTO = BattleService.CreateBattle(new BattleRequestDTO(Context.Kingdoms.FirstOrDefault().Id, new List<int> { 6 }), kingdom2.Id);

            Assert.Equal("Attack order issued", statusDTO.status);
            Assert.Equal(1, Context.Battles.Count());
            Assert.Equal(attacker.Id, Context.Battles.FirstOrDefault().AttackerId);
            Assert.Equal(defender.Id, Context.Battles.FirstOrDefault().DefenderId);
            Assert.Null(Context.Battles.FirstOrDefault().WinnerId);
            Assert.True(DateTime.Now.AddMinutes(421) > Context.Battles.FirstOrDefault().Fought_at);
            Assert.True(DateTime.Now.AddMinutes(419) < Context.Battles.FirstOrDefault().Fought_at);
        }
    }
}
