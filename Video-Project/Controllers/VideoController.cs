using Microsoft.AspNetCore.Mvc;
using Video_Project.Interfaces;
using Video_Project.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Video_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        private readonly IVideo _videoRepo;

        public VideoController(
        IVideo videoRepository)

        {
            _videoRepo = videoRepository;

        }

        // GET: api/<VideoController>/5
        [HttpGet("GetVideoById/{id}")]
        public ActionResult GetVideoById(int id)
        {
            var video = _videoRepo.GetVideoById(id);
            return Ok(video);
        }

        // GET api/<VideoController>
        [HttpGet]
        public ActionResult GetAllVideos()
        {
            var videos = _videoRepo.GetAllVideos();
            return Ok(videos);
        }

        // POST api/<VideoController>
        [HttpPost("PostVideo")]
        public ActionResult PostVideo(Video video)
        {
            var newVideo = _videoRepo.PostVideo(video);
            return Ok(newVideo);
        }

        // PUT api/<VideoController>/5
        [HttpPut("{id}")]
        public void ArchiveVideo(Video video)
        {
            _videoRepo.ArchiveVideo(video);
        }

        // DELETE api/<VideoController>/5
        [HttpDelete("{id}")]
        public void DeleteVideo(int id)
        {
            _videoRepo.DeleteVideo(id);
        }
    }
}
