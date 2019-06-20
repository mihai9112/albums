using System.Collections.Generic;

namespace RunPath.Domain.Models
{
    public class Album
    {
        public int UserId { get; }
        public int Id { get ; }
        public string Title { get; }
        public List<Photo> Photos { get; }

        public Album(int userId, int id, string title)
        {
            UserId = userId;
            Id = id;
            Title = title;
        }

        public Album(int userId, int id, string title, List<Photo> photos)
        {
            UserId = userId;
            Id = id;
            Title = title;
            Photos = photos;
        }
    }
}