using VideoChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Base;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using ParticipantStatus = Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum;
using VideoChat.Settings;

namespace VideoChat.Services
{
    public class VideoService : IVideoService
    {
        readonly TwilioSettings _twilioSettings;

        public VideoService(Microsoft.Extensions.Options.IOptions<TwilioSettings> twilioOptions)
        {
            _twilioSettings =
                twilioOptions?.Value
             ?? throw new ArgumentNullException(nameof(twilioOptions));

            TwilioClient.Init(_twilioSettings.ApiKey, _twilioSettings.ApiSecret);
        }

        public  string GetTwilioJwt(string identity)
        {
          var authToken =  new Token(_twilioSettings.AccountSid,
                         _twilioSettings.ApiKey,
                         _twilioSettings.ApiSecret,
                         identity ?? GetName(),
                         grants: new HashSet<IGrant> { new VideoGrant() }).ToJwt();
            return  authToken;
        }


        public async Task<RoomModel> CreateRoom(string roomSID)
        {

            var room = await RoomResource.CreateAsync(uniqueName: roomSID);
        return new RoomModel() { roomName = room.UniqueName };
    }
        public async Task<RoomModel> GetRoom(string roomSID)
        {

            var room = await RoomResource.FetchAsync(pathSid: roomSID);
        return new RoomModel() { roomName = room.UniqueName };

    }
    #region Names

    readonly string[] _adjectives =
        {
            "Abrasive", "Brash", "Callous", "Daft", "Eccentric", "Feisty", "Golden",
            "Holy", "Ignominious", "Luscious", "Mushy", "Nasty",
            "OldSchool", "Pompous", "Quiet", "Rowdy", "Sneaky", "Tawdry",
            "Unique", "Vivacious", "Wicked", "Xenophobic", "Yawning", "Zesty"
        };

        readonly string[] _firstNames =
        {
            "Anna", "Bobby", "Cameron", "Danny", "Emmett", "Frida", "Gracie", "Hannah",
            "Isaac", "Jenova", "Kendra", "Lando", "Mufasa", "Nate", "Owen", "Penny",
            "Quincy", "Roddy", "Samantha", "Tammy", "Ulysses", "Victoria", "Wendy",
            "Xander", "Yolanda", "Zelda"
        };

        readonly string[] _lastNames =
        {
            "Anchorage", "Berlin", "Cucamonga", "Davenport", "Essex", "Fresno",
            "Gunsight", "Hanover", "Indianapolis", "Jamestown", "Kane", "Liberty",
            "Minneapolis", "Nevis", "Oakland", "Portland", "Quantico", "Raleigh",
            "SaintPaul", "Tulsa", "Utica", "Vail", "Warsaw", "XiaoJin", "Yale",
            "Zimmerman"
        };

        string GetName() => $"{_adjectives.RandomElement()} {_firstNames.RandomElement()} {_lastNames.RandomElement()}";

        #endregion
    }

    static class StringArrayExtensions
    {
        static readonly Random Random = new Random();

        internal static string RandomElement(this IReadOnlyList<string> array)
            => array[Random.Next(array.Count)];
    }
}