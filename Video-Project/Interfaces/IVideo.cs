using Video_Project.Models;
using Video_Project.Repositories;

namespace Video_Project.Interfaces
{
    public interface IVideo
    {
        public Video GetVideoById(int id);
        public List<Video> GetAllVideos();
        public Video PostVideo(Video video);
        public void ArchiveVideo(Video video);
        public void DeleteVideo(int id);

    }
}
