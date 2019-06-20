using System;
using Microsoft.AspNetCore.Mvc;

namespace RunPath.WebApi.Models.Hypermedia
{
    public class HypermediaLinkBuilder
    {
        public static ApiDiscovery ForServiceDiscovery(IUrlHelper url)
        {
            return new ApiDiscovery(
                GetLinkForRootDiscovery(url),
                GetLinkForAlbumGet(url)
            );
        }

        public static AlbumDiscovery ForAlbumDiscovery(IUrlHelper url, int albumId, int userId)
        {
            return new AlbumDiscovery(
                GetLinkForAlbumGet(url),
                GetLinkForAlbumPhotosGet(url, albumId),
                GetLinkForUserAlbumGet(url, userId)
            );
        }

        public static AlbumDiscovery ForAlbumDetailsDiscovery(IUrlHelper url, int albumId, int userId)
        {
            return new AlbumDiscovery(
                GetLinkForAlbumDetailsGet(url, albumId),
                GetLinkForAlbumPhotosGet(url, albumId),
                GetLinkForUserAlbumGet(url, userId)
            );
        }

        public static PhotosDiscovery ForPhotosDiscovery(IUrlHelper url, int albumId)
        {
            return new PhotosDiscovery(
                GetLinkForAlbumPhotosGet(url, albumId),
                GetLinkForAlbumDetailsGet(url, albumId)
            );
        }

        public static UserDiscovery ForUsersDiscovery(IUrlHelper url, int userId)
        {
            return new UserDiscovery(
                GetLinkForUserAlbumGet(url, userId)
            );
        }

        private static Link GetLinkForUserAlbumGet(IUrlHelper url, int userId)
        {
            var link = url.Link("Home", new {});
            return ConvertLink(link, $"users/{userId}/albums");
        }

        private static Link GetLinkForAlbumPhotosGet(IUrlHelper url, int albumId)
        {
            var link = url.Link("Home", new {});
            return ConvertLink(link, $"albums/{albumId}/photos");
        }

        private static Link GetLinkForAlbumDetailsGet(IUrlHelper url, int albumId)
        {
            var link = url.Link("Home", new {});
            return ConvertLink(link, $"albums/{albumId}");
        }
        
        private static Link GetLinkForRootDiscovery(IUrlHelper url)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link);
        }

        private static Link GetLinkForAlbumGet(IUrlHelper url)
        {
            var link = url.Link("Home", new { });
            return ConvertLink(link, "albums");
        }

        private static Link ConvertLink(string link, string linkSuffix = "")
        {
            var uriBuilder = LocalhostLink(link);
            return new Link { Href = uriBuilder.Uri + linkSuffix};
        }

        private static UriBuilder LocalhostLink(string link)
        {
            var builder = new UriBuilder(new Uri(link))
            {
                Scheme = Uri.UriSchemeHttp
            };
            return builder;
        }
    }
}
