using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipAPI.SignalRHubs
{
    public class InitializationGameHub: Hub
    {
        public async Task InitGame(string user, string message)
        {

            await Clients.Caller.SendAsync("gameReadyRequest", user, message);
        }
    }
}
