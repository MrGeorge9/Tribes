using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using Eucyon_Tribes.Models.UserModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.EmailTests
{
    [Serializable]
    [Collection("Serialize")]
    public class EmailControllerIntegrationTests : IntegrationTests
    {
        protected readonly IAuthService authService;
        protected readonly IConfiguration configuration;

        public EmailControllerIntegrationTests() : base("emailControllerTests")
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in configuration.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            authService = new JWTService(configuration);
        }

        [Fact]
        public async Task Verify_WithRightToken_ReturnsUserVerified()
        {
            var user = new User() { Id = 1 };
            user.VerificationToken = authService.GenerateToken(user, "verify");
            var response = await _client.GetAsync($"api/email/verify/{user.VerificationToken}");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("User has been verified", result["status"]);
        }

        [Fact]
        public async Task Verify_WithWrongToken_ReturnsUnauthorized()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NTgyMzE5ODEsImV4cCI6MTk3Mzg1MTE4MCwiaWF0IjoxNjU4MjMxOTgxfQ.oYzvDiJ7FaJLWdmkbFxV14gNXGJtUseWAgOdgqhZFDE";
            var response = await _client.GetAsync($"api/email/verify/{token}");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Equal("The token is invalid or has expired. Please request new verification email", result["error"].ToString());
        }

        [Fact]
        public async Task NewToken_WithRightUserAndPassword_ReturnsNewTokenSent()
        {
            UserLoginDto userLoginDto = new UserLoginDto("John", "Johny123");

            var response = await _client.PostAsync("api/email/newToken", JsonContent.Create(userLoginDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("New verification email has been sent", result["status"].ToString());
        }

        [Fact]
        public async Task NewToken_WithWrongUser_ReturnsError()
        {
            UserLoginDto userLoginDto = new UserLoginDto("Shepard", "Johny123");

            var response = await _client.PostAsync("api/email/newToken", JsonContent.Create(userLoginDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("User Shepard is not in database", result["error"].ToString());
        }

        [Fact]
        public async Task NewToken_WithWrongPassword_ReturnsError()
        {
            UserLoginDto userLoginDto = new UserLoginDto("John", "Uhorka");

            var response = await _client.PostAsync("api/email/newToken", JsonContent.Create(userLoginDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("User John wrong password", result["error"].ToString());
        }

        [Fact]
        public async Task NewPasswordRequest_WithRightEmail_ReturnsOk()
        {
            EmailForPasswordResetDto emailForPasswordResetDto = new EmailForPasswordResetDto("george@george.com");

            var response = await _client.PostAsync("api/email/reset-password", JsonContent.Create(emailForPasswordResetDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("Email with password reset verification has been sent", result["status"].ToString());
        }

        [Fact]
        public async Task NewPasswordRequest_WithRightEmailUnverified_ReturnsUnverified()
        {
            EmailForPasswordResetDto emailForPasswordResetDto = new EmailForPasswordResetDto("john@john.com");

            var response = await _client.PostAsync("api/email/reset-password", JsonContent.Create(emailForPasswordResetDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Equal("Email is not verified", result["error"].ToString());
        }

        [Fact]
        public async Task NewPasswordRequest_WithNoEmail_ReturnsBadRequest()
        {
            EmailForPasswordResetDto emailForPasswordResetDto = new EmailForPasswordResetDto(String.Empty);

            var response = await _client.PostAsync("api/email/reset-password", JsonContent.Create(emailForPasswordResetDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Email is required", result["error"].ToString());
        }

        [Fact]
        public async Task NewPasswordVerify_WithRightToken_ReturnsPasswordTokenVerified()
        {
            var user = new User() { Id = 2 };
            user.ForgottenPasswordTokenExpiresAt = DateTime.Now.AddHours(1);
            user.ForgottenPasswordToken = authService.GenerateToken(user, "forgotten password");
            var response = await _client.GetAsync($"api/email/reset-password/{user.ForgottenPasswordToken}");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("User forgotten password token has been verified", result["status"]);
        }

        [Fact]
        public async Task NewPasswordVerify_WithWrongToken_ReturnsWrongToken()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NTgyMzE5ODEsImV4cCI6MTk3Mzg1MTE4MCwiaWF0IjoxNjU4MjMxOTgxfQ.oYzvDiJ7FaJLWdmkbFxV14gNXGJtUseWAgOdgqhZFDE";
            var response = await _client.GetAsync($"api/email/reset-password/{token}");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Equal("The token is invalid or has expired. Please request new verification email", result["error"]);
        }

        [Fact]
        public async Task NewPasswordGeneration_WithRightokenAndPassword_ReturnsNewPasswordSet()
        {
            var user = new User() { Id = 3 };
            user.ForgottenPasswordTokenExpiresAt = DateTime.Now.AddHours(1);
            user.ForgottenPasswordToken = authService.GenerateToken(user, "forgotten password");
            NewPasswordDTO newPasswordDTO = new NewPasswordDTO("salama1234");

            var response = await _client.PostAsync($"api/email/reset-password/{user.ForgottenPasswordToken}", JsonContent.Create(newPasswordDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("New password is set", result["status"]);
        }

        [Fact]
        public async Task NewPasswordGeneration_WithUnverifiedToken_ReturnsUnverifiedToken()
        {
            var user = new User() { Id = 2 };
            user.ForgottenPasswordTokenExpiresAt = DateTime.Now.AddHours(1);
            user.ForgottenPasswordToken = authService.GenerateToken(user, "forgotten password");
            NewPasswordDTO newPasswordDTO = new NewPasswordDTO("salama1234");

            var response = await _client.PostAsync($"api/email/reset-password/{user.ForgottenPasswordToken}", JsonContent.Create(newPasswordDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Password token has not been verified yet. Please check your email", result["error"]);
        }

        [Fact]
        public async Task NewPasswordGeneration_WithWrongToken_ReturnsWrongToken()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NTgyMzE5ODEsImV4cCI6MTk3Mzg1MTE4MCwiaWF0IjoxNjU4MjMxOTgxfQ.oYzvDiJ7FaJLWdmkbFxV14gNXGJtUseWAgOdgqhZFDE";
            NewPasswordDTO newPasswordDTO = new NewPasswordDTO("salama1234");

            var response = await _client.PostAsync($"api/email/reset-password/{token}", JsonContent.Create(newPasswordDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Equal("The token is invalid or has expired. Please request new verification email", result["error"]);
        }

        [Fact]
        public async Task NewPasswordGeneration_WithWrongPassword_ReturnsWrongPassword()
        {
            var user = new User() { Id = 3 };
            user.ForgottenPasswordTokenExpiresAt = DateTime.Now.AddHours(1);
            user.ForgottenPasswordToken = authService.GenerateToken(user, "forgotten password");
            NewPasswordDTO newPasswordDTO = new NewPasswordDTO("sa1234");

            var response = await _client.PostAsync($"api/email/reset-password/{user.ForgottenPasswordToken}", JsonContent.Create(newPasswordDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Password must be at least 8 characters long", result["error"]);
        }
    }
}