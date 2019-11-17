using System.Net.Http;
using System.Threading.Tasks;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleStateTracker.Integration.Tests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace BattleStateTracker.Integration.Tests.Controllers
{
    public class BattleControllerTest : IClassFixture<BattleStateTrackerAppFactory>
    {
        private readonly HttpClient _client;

        public BattleControllerTest(BattleStateTrackerAppFactory factory)
        {
            _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task InitiateNewBattle()
        {
            var request = new
            {
                Url = "/battle",
                Body = new
                {
                    ShipLength = 3,
                    Dimension = 10,
                    NoOfShips = 5
                }
            };
            var httpResponse = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var battle = JsonConvert.DeserializeObject<Battle>(stringResponse);
             
            Assert.Equal(BattleStatus.Initialized ,battle.Status); 
        }
        
        //TODO: Write more integration tests
    }
}