using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Coflnet.Sky.Core;
using Newtonsoft.Json;
using RestSharp;

namespace Coflnet.Sky.Commands.Shared
{
    public class IndexerClient
    {
        public static RestClient Client = new RestClient(SimplerConfig.SConfig.Instance["INDEXER_BASE_URL"] ?? "http://" + SimplerConfig.SConfig.Instance["INDEXER_HOST"]);
        public static Task<RestSharp.RestResponse<Player>> TriggerNameUpdate(string uuid)
        {
            if(uuid.Length != 32)
                throw new System.ArgumentException("uuid must be 32 characters long", nameof(uuid));
            return Client.ExecuteAsync<Player>(new RestRequest("player/{uuid}", Method.Patch).AddUrlSegment("uuid", uuid));
        }
        public static async Task<IEnumerable<KeyValuePair<string, short>>> LowSupply()
        {
            var response = await Client.ExecuteAsync(new RestRequest("supply/low", Method.Get));
            return JsonConvert.DeserializeObject<List<KeyValuePair<string, short>>>(response.Content);
        }
        public static async Task<IEnumerable<AuctionResult>> RecentlyEnded()
        {
            var response = await Client.ExecuteAsync(new RestRequest("auctions/ended", Method.Get));
            return JsonConvert.DeserializeObject<List<AuctionResult>>(JsonConvert.DeserializeObject<string>(response.Content));
        }
        public static async Task<string> DeleteUser(string email, string googleId)
        {
            var response = await Client.ExecuteAsync(new RestRequest($"user/{email}?id={googleId}", Method.Delete));
            return JsonConvert.DeserializeObject<string>(JsonConvert.DeserializeObject<string>(response.Content));
        }
    }
}