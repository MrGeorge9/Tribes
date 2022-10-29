using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribes.Services;
using TribesTest;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class TimeServiceTest
    {
        public IConfiguration Config;
        public TimeServiceTest()
        {
            Config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in Config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
        }

        [Fact]
        public async Task TimeService_Tick()
        {
            TimeService timeService = new TimeService(Config);
            timeService.StartAsync(new CancellationToken());
            timeService.timer.Interval = 1000;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            await Task.Delay(10500);
            
            string consoleOutput = stringWriter.ToString();
            String[] strings = consoleOutput.Split("\n");
            if (strings[9].EndsWith("\r")) strings[9] = strings[9].Substring(0, strings[9].Length - 1);
            Assert.Equal(10, timeService.tick);
            Assert.Equal(11, strings.Length);
            Assert.Equal("game tick occured", strings[9]);
        }
    }
}
