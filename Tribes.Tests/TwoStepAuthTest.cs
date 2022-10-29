using Eucyon_Tribes.Models.DTOs.TwoStepAuthDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests
{
    public class TwoStepAuthTest
    {
        public IConfiguration Config;
        public TwoStepAuthService service;

        public TwoStepAuthTest()
        {
            Config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in Config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            service = new TwoStepAuthService(Config);
        }

        [Fact]
        public void TwoStepAuthTest_Register_NotNull()
        {
            AuthRegistrationDTO dto=service.AuthRegistration("username");

            Assert.NotNull(dto.ManualCode);
            Assert.NotNull(dto.QR);
        }
    }
}
