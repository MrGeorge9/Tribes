using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class ArmyServiceTests : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
               .UseInMemoryDatabase(databaseName: "ArmyServiceTest").Options;
        public ApplicationContext Context;
        public ArmyService ArmyService;
        public Mock<IArmyFactory> ArmyFactory;

        public ArmyServiceTests()
        {        
            Context = new ApplicationContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            ArmyFactory = new Mock<IArmyFactory>();
            ArmyService = new ArmyService(ArmyFactory.Object, Context);
            User user = new User();
            user.Name = "test";
            user.Email = "test";
            user.PasswordHash = "test";
            user.VerificationToken = "test";
            user.ForgottenPasswordToken = "token";
            Context.Users.Add(user);
            World world = new World() { Name = "world"};
            Context.Worlds.Add(world);
            Location location = new Location();
            location.XCoordinate = 0;
            location.YCoordinate = 0;
            Context.Locations.Add(location);
            Kingdom kingdom = new Kingdom();
            kingdom.Location = location;
            kingdom.User = user;
            kingdom.WorldId = world.Id;
            kingdom.World = world;
            kingdom.UserId = user.Id;
            kingdom.Name = "kingdom";
            kingdom.Armies = new List<Army>();
            kingdom.Resources = new List<Resource>();
            Context.Kingdoms.Add(kingdom);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Error1()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            Soldier soldier = new Soldier { Level = 1 };
            kingdom.Resources.Add(soldier);
            Context.SaveChanges();

            Assert.Null(ArmyService.CreateArmy(new List<int> { 0, 1 }, kingdom.Id, 0));
            Assert.Equal("You do not posses any units of requested level", ArmyService.GetError());

        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Error2()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            Soldier soldier = new Soldier { Level = 1 };
            kingdom.Resources.Add(soldier);
            Context.SaveChanges();


            Assert.Null(ArmyService.CreateArmy(new List<int> { 2 }, kingdom.Id, 0));
            Assert.Equal("Not enugh units of level 1", ArmyService.GetError());
        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Army()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            for (int i = 0; i < 12; i++)
            {
                if (i < 6)
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                }
                else
                {
                    Soldier soldier = new Soldier { Level = 2 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                }
            }
            Context.SaveChanges();
            ArmyFactory.Setup(i => i.CrateArmy(soldiers, kingdom, 0)).Returns(new Army { Soldiers = soldiers });

            Army army = ArmyService.CreateArmy(new List<int> { 6, 6 }, kingdom.Id, 0);

            Assert.Equal(army.Soldiers, soldiers);
        }

        [Fact]
        public void ArmyServiceTest_GetAvailableUnits_AvailableUnitsDTO()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            Army army = new Army();
            army.Soldiers = new List<Soldier>();
            army.Kingdom = kingdom;
            for (int i = 0; i < 12; i++)
            {
                if (i < 6)
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army.Soldiers.Add(soldier);
                }
                else
                {
                    Soldier soldier = new Soldier { Level = 2 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                }
            }
            Context.Armies.Add(army);
            Context.SaveChanges();

            AvailableUnitsDTO DTO = ArmyService.GetAvailableUnits(kingdom.Id);

            Assert.Equal(0, DTO.NumberOfUnitsByLevel[0]);
            Assert.Equal(6, DTO.NumberOfUnitsByLevel[1]);
        }

        [Fact]
        public void ArmyServiceTest_RemoveArmy_Presist()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            Army army = new Army();
            army.DisbandsAt = DateTime.Now.AddHours(1);
            army.Soldiers = new List<Soldier>();
            army.Kingdom = kingdom;
            for (int i = 0; i < 12; i++)
            {
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army.Soldiers.Add(soldier);
                }
            }
            Context.Armies.Add(army);
            Context.SaveChanges();

            ArmyService.RemoveArmy();

            Assert.True(Context.Armies.Count() == 1);
        }

        [Fact]
        public void ArmyServiceTest_RemoveArmy_Remove()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            Army army = new Army();
            army.DisbandsAt = DateTime.Now;
            army.Soldiers = new List<Soldier>();
            army.Kingdom = kingdom;
            for (int i = 0; i < 12; i++)
            {
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army.Soldiers.Add(soldier);
                }
            }
            Context.Armies.Add(army);
            Context.SaveChanges();

            ArmyService.RemoveArmy();

            Assert.True(Context.Armies.Count() == 0);
        }

        [Fact]
        public void ArmyServiceTest_GetArmy_Error1()
        {
            Assert.Null(ArmyService.GetArmy(0));
            Assert.Equal("Invalid id", ArmyService.GetError());
        }

        [Fact]
        public void ArmyServiceTest_GetArmy_Error2()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            Army army = new Army();
            army.DisbandsAt = DateTime.Now;
            army.Soldiers = new List<Soldier>();
            army.Kingdom = kingdom;
            for (int i = 0; i < 12; i++)
            {
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army.Soldiers.Add(soldier);
                }
            }
            Context.Armies.Add(army);
            Context.SaveChanges();

            Assert.Null(ArmyService.GetArmy(2));
            Assert.Equal("Army not found", ArmyService.GetError());
        }

        [Fact]
        public void ArmyServiceTest_GetArmy_ArmyDTO()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            Army army = new Army();
            army.DisbandsAt = DateTime.Now;
            army.Soldiers = new List<Soldier>();
            army.Kingdom = kingdom;
            for (int i = 0; i < 12; i++)
            {
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army.Soldiers.Add(soldier);
                }
            }
            Context.Armies.Add(army);
            Context.SaveChanges();

            ArmyDTO armyDTO= ArmyService.GetArmy(army.Id);

            Assert.Equal(army.Id, armyDTO.Id);
            Assert.Equal(kingdom.Id, armyDTO.Owner);
            Assert.Equal(12, armyDTO.NumberOfUnitsByLevel[0]);
        }

        [Fact]
        public void ArmyServiceTest_GetArmies_Empty()
        {
            Assert.Equal(new ArmyDTO[0], ArmyService.GetArmies(0));
        }

        [Fact]
        public void ArmyServiceTest_GetArmies_Array()
        {
            Kingdom kingdom = Context.Kingdoms.FirstOrDefault();
            List<Soldier> soldiers = new List<Soldier>();
            Army army1 = new Army();
            army1.Soldiers = new List<Soldier>();
            Army army2 = new Army();
            army2.Soldiers = new List<Soldier>();
            army1.Kingdom = kingdom;
            army2.Kingdom = kingdom;
            for (int i = 0; i < 12; i++)
            {
                if (i < 6)
                {
                    Soldier soldier = new Soldier { Level = 1 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army1.Soldiers.Add(soldier);
                }
                else
                {
                    Soldier soldier = new Soldier { Level = 2 };
                    kingdom.Resources.Add(soldier);
                    soldiers.Add(soldier);
                    army2.Soldiers.Add(soldier);
                }
            }
            Context.Armies.Add(army1);
            Context.Armies.Add(army2);
            Context.SaveChanges();

            ArmyDTO[] output = ArmyService.GetArmies(kingdom.Id);

            Assert.Equal(6, output[0].NumberOfUnitsByLevel[0]);
            Assert.Equal(6, output[1].NumberOfUnitsByLevel[1]);
        }
    }
}
