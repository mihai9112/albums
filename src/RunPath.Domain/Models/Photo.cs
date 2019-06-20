namespace RunPath.Domain.Models
{
    public class Photo
    {
        public int AlbumId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Url { get; }
        public string ThumbnailUrl { get; }

        public Photo(int albumId, int id, string title, string url, string thumbnailUrl)
        {
            AlbumId = albumId;
            Id = id;
            Title = title;
            Url = url;
            ThumbnailUrl = thumbnailUrl;
        }
    }
}