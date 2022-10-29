using Eucyon_Tribes.Models.Resources;
using Eucyon_Tribes.Factories;

namespace Tribes.Tests.ResourceTests
{
    [Serializable]
    [Collection("Serialize")]
    public class ResourceFactoryTest
    {
        ResourceFactory factory = new ResourceFactory();

        [Fact]
        public void CreatesCorrectWoodResource()
        {
            var myResource = factory.GetWoodResource();
            Assert.True(myResource is Wood);
            Assert.Equal(0, myResource.Amount);
        }

        [Fact]
        public void CreatesCorrectGoldResource()
        {
            var myResource = factory.GetGoldResource();
            Assert.True(myResource is Gold);
            Assert.Equal(0, myResource.Amount);
        }

        [Fact]
        public void CreatesCorrectFoodResource()
        {
            var myResource = factory.GetFoodResource();
            Assert.True(myResource is Food);
            Assert.Equal(0, myResource.Amount);
        }

        [Fact]
        public void CreatesCorrectPeopleResource()
        {
            var myResource = factory.GetPeopleResource();
            Assert.True(myResource is People);
            Assert.Equal(0, myResource.Amount);
        }

        [Fact]
        public void CreatesCorrectSoldierResource()
        {
            var myResource = factory.GetSoldierResource();
            Assert.True(myResource is Soldier);
            Assert.Equal(1, myResource.Amount);
            Assert.Equal(1, myResource.Level);
            Assert.Equal(10, myResource.Attack);
        }
    }
}

