namespace RunPath.WebApi.Models.Hypermedia
{
    public class UserDiscovery
    {
        public Link Self { get; set; }

        public UserDiscovery(Link self)
        {
            Self = self;
        }
    }
}