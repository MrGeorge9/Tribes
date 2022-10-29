using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.Buildings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.BuildingsTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BuildingFactoryTests
    {
        private BuildingFactory _buildingFactory;

        public BuildingFactoryTests()
        {
            _buildingFactory = new BuildingFactory();
        }

        [Fact]
        public void CreateTownHallTest()
        {
            TownHall townHall = new TownHall()
            {
                Level = 1,
                Hp = 500,
                StartedAt = DateTime.Today,
                Production = 2,
            };
            TownHall townHall1 = _buildingFactory.CreateTownHall();

            Assert.Equal(townHall.Level, townHall1.Level);
            Assert.Equal(townHall.Hp, townHall1.Hp);
            Assert.Equal(townHall.Production, townHall1.Production);
        }

        [Fact]
        public void CreateFarmTest()
        {
            Farm farm = new Farm()
            {
                Level = 1,
                Hp = 50,
                StartedAt = DateTime.Today,
                Production = 5,
            };
            Farm farm1 = _buildingFactory.CreateFarm();

            Assert.Equal(farm.Level, farm1.Level);
            Assert.Equal(farm.Hp, farm1.Hp);
            Assert.Equal(farm.Production, farm1.Production);
        }
    }
}