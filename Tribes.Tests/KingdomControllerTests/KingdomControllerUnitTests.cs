using Eucyon_Tribes.Context;
using Eucyon_Tribes.Controllers;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Services;
using Eucyon_Tribes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;

namespace Tribes.Tests.KingdomControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomControllerUnitTests : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
               .UseInMemoryDatabase(databaseName: "KingdomServiceTest").Options;
        public ApplicationContext Context;
        public Mock<IKingdomService> Service;
        public Mock<IBattleService> Service2;
        public KingdomRestController Controller;

        public void Dispose()
        {
            foreach (var kingdom in Context.Kingdoms)
                Context.Kingdoms.Remove(kingdom);
            foreach (var world in Context.Worlds)
                Context.Worlds.Remove(world);
            foreach (var user in Context.Users)
                Context.Users.Remove(user);
            Context.SaveChanges();
        }

        public KingdomControllerUnitTests()
        {
            Context = new ApplicationContext(options);
            Service = new Mock<IKingdomService>();
            Service2 = new Mock<IBattleService>();
            Controller = new KingdomRestController(Service.Object,Service2.Object);
            User User = new User();
            User.Name = "test";
            User.Email = "test";
            User.PasswordHash = "test";
            User.ForgottenPasswordToken = "test";
            User.VerificationToken = "test";
            Context.Users.Add(User);
            User User2 = new User();
            User2.Name = "test";
            User2.Email = "test";
            User2.PasswordHash = "test";
            User2.ForgottenPasswordToken = "test";
            User2.VerificationToken = "test";
            Context.Users.Add(User2);
            User User3 = new User();
            User3.Name = "test";
            User3.Email = "test";
            User3.PasswordHash = "test";
            User3.ForgottenPasswordToken = "test";
            User3.VerificationToken = "test";
            Context.Users.Add(User3);
            World World = new World() { Name = "world" };
            Context.Worlds.Add(World);
            Context.SaveChanges();
        }


        [Fact]
        public async Task KingdomController_Index_List()
        {
            Service.Setup(i => i.GetKingdoms(0,0)).Returns(new KingdomsDTO[0]);

            var result = (ObjectResult)Controller.Index(0,0);

            Assert.True(result.StatusCode == (int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task KingdomController_Store_Add()
        {
            CreateKingdomDTO DTO = new CreateKingdomDTO(1, 1, "memes");
            Service.Setup(i => i.AddKingdom(DTO)).Returns(true);

            var result = (StatusCodeResult)Controller.Store(DTO);

            Assert.Equal(200, result.StatusCode);

        }

        [Fact]
        public async Task KingdomController_Store_Error()
        {
            CreateKingdomDTO DTO = new CreateKingdomDTO(1, 1, "memes");
            Service.Setup(i => i.AddKingdom(DTO)).Returns(false);
            Service.Setup(i => i.GetError()).Returns("Invalid kingdom Id");

            var result = (JsonResult)Controller.Store(DTO);

            Assert.Equal(400, result.StatusCode);
        }


        [Fact]
        public async Task KingdomController_Show_KingdomDTO()
        {
            Service.Setup(i => i.GetKindom(0)).Returns(new KingdomDTO(1, 1, 1, 1, 1));

            var result = (ObjectResult)Controller.Show(0);

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task KingdomController_Show_Error()
        {
            Service.Setup(i => i.GetKindom(0)).Returns<KingdomDTO>(null);
            Service.Setup(i => i.GetError()).Returns("Invalid kingdom Id");

            var result = (JsonResult)Controller.Show(0);

            Assert.Equal(400, result.StatusCode);
        }
    }
}
