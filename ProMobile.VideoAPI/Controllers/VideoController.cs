using System.Threading.Tasks;
using VideoChat.Services;
using Microsoft.AspNetCore.Mvc;
using VideoChat.Models;
using Microsoft.AspNetCore.Cors;
using VideoChat.Settings;
using ProMobile.VideoAPI.Models;

namespace VideoChat.Controllers
{
    [EnableCors(Constants.MyAllowSpecificOrigins)]

    [
    ApiController,
        Route("api/video")
    ]
    public class VideoController : ControllerBase
    {
        readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
            => _videoService = videoService;

        [HttpGet("token")]
        public  ActionResult<string> GetToken()
        {
            var token =   _videoService.GetTwilioJwt(User.Identity.Name);
            return  token;
        }
            
      


        [HttpPost("createRoom")]
        public async Task<IActionResult> createRoom([FromBody] RoomModel roomID )
            => new JsonResult(await _videoService.CreateRoom(roomID.roomName));


        [HttpPost("getRoom")]
        public async Task<IActionResult> GetRoom([FromBody] RoomModel roomID)
            => new JsonResult(await _videoService.GetRoom(roomID.roomName));


    }
}