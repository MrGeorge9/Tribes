namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class UserModelTest
    {
        [Fact]
        public void TestUserCreation()
        {
            var User = new User();
            Assert.NotNull(User);
            Assert.True(User.CreatedDate < DateTime.Now);
            Assert.True(User.CreatedDate > DateTime.Now.AddMinutes(-1));
        }
    }
}
