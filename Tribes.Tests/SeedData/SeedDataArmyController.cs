using Eucyon_Tribes.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.SeedData
{
    public class SeedDataArmyController
    {
        public static void PopulateDataForArmyControllerTest(ApplicationContext applicationContext) 
        {
            applicationContext.Database.EnsureDeleted();
            applicationContext.Database.EnsureCreated();
            User user1 = new User();
            user1.Name = "User1";
            user1.PasswordHash = "Password1";
            user1.Email = "Email1";
            user1.ForgottenPasswordToken = "token1";
            user1.VerificationToken = "token1";
            user1.ForgottenPasswordToken = "token1";
            User user2 = new User();
            user2.Name = "User2";
            user2.VerificationToken = "token2";
            user2.ForgottenPasswordToken = "token2";
            user2.PasswordHash = "Password2";
            user2.Email = "Email2";
            user2.ForgottenPasswordToken = "token2";
            World world = new World() { Name = "world"};
            Location location1 = new Location();
            Location location2 = new Location();
            location1.YCoordinate = 0;
            location1.XCoordinate = 0;
            location2.XCoordinate = 10;
            location2.YCoordinate = 10;
            Kingdom kingdom1 = new Kingdom();
            Kingdom kingdom2 = new Kingdom();
            location1.Kingdom = kingdom1;
            location1.KingdomId = kingdom1.Id;
            location2.KingdomId= kingdom2.Id;
            location2.Kingdom = kingdom2;
            kingdom1.World = world;
            kingdom1.Armies = new List<Army>();
            kingdom1.Resources = new List<Resource>();
            kingdom2.Resources = new List<Resource>();
            Army army1 = new Army();
            Army army2 = new Army();
            kingdom1.Armies.Add(army1);
            kingdom1.Armies.Add(army2);
            army1.Kingdom = kingdom1;
            army1.Soldiers = new List<Soldier>();
            army2.Soldiers = new List<Soldier>();
            army2.Kingdom = kingdom1;
            for (int i = 0; i < 12; i++) 
            {
                Soldier soldier = new Soldier();
                soldier.TotalHP = 30;
                soldier.CurrentHP = 30;
                if (i <6 ) 
                {
                    soldier.Army = army1;
                    soldier.Level = 2;
                    army1.Soldiers.Add(soldier);
                }
                if (i >=6)
                {
                    soldier.Level = 1;
                    soldier.Army = army1;
                    kingdom1.Resources.Add(soldier);
                }
                applicationContext.Resources.Add(soldier);
            }
            kingdom1.Name = "kingdom1";
            kingdom2.Name = "kingdom2";
            kingdom1.WorldId = world.Id;
            kingdom2.WorldId = world.Id;
            kingdom2.World = world;
            kingdom1.Location = location1;
            kingdom2.Location = location2;
            kingdom1.User = user1;
            kingdom2.User = user2;
            kingdom1.UserId = user1.Id;
            kingdom2.UserId = user2.Id;
            applicationContext.Armies.Add(army1);
            applicationContext.Armies.Add(army2);
            applicationContext.Worlds.Add(world);
            applicationContext.Users.Add(user1);
            applicationContext.Users.Add(user2);
            applicationContext.Locations.Add(location1);
            applicationContext.Locations.Add(location2);
            applicationContext.Kingdoms.Add(kingdom1);
            applicationContext.Kingdoms.Add(kingdom2);
            applicationContext.SaveChanges();
        }
    }
}
