using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipAPI.SignalRHubs
{
    public class InitializationGameHub: Hub
    {
        public async Task InitGame(List<List<string>> coordinates)
        {
            await Clients.Caller.SendAsync("gameReadyRequest", true);
        }
    }
}
