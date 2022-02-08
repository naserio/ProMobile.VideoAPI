using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;

using VideoChat.Models;

namespace VideoChat.Services
{
    public interface IVideoService
    {
        /// <summary>
        /// Gets the Twilio JSON web token so the client can connect to video services.
        /// </summary>
        string GetTwilioJwt(string identity);

        /// <summary>
        /// Gets all of the current active rooms for the app.
        /// </summary>

        Task<RoomModel> CreateRoom(string roomSID);
        Task<RoomModel> GetRoom(string roomSID);
     
    }
}