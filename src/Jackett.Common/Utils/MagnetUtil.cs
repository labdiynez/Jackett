using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Jackett.Common.Helpers;

namespace Jackett.Common.Utils
{
    public static class MagnetUtil
    {
        // Update best trackers from https://github.com/ngosang/trackerslist
        private static readonly NameValueCollection _Trackers = new NameValueCollection
        {
            {"tr", "http://tracker.opentrackr.org:1337/announce"},
            {"tr", "udp://tracker.auctor.tv:6969/announce"},
            {"tr", "udp://opentracker.i2p.rocks:6969/announce"},
            {"tr", "https://opentracker.i2p.rocks:443/announce"},
            {"tr", "udp://open.demonii.com:1337/announce"},
            {"tr", "udp://tracker.openbittorrent.com:6969/announce"},
            {"tr", "http://tracker.openbittorrent.com:80/announce"},
            {"tr", "udp://open.stealth.si:80/announce"},
            {"tr", "udp://tracker.torrent.eu.org:451/announce"},
            {"tr", "udp://tracker.moeking.me:6969/announce"}
        };

        private static readonly string _TrackersEncoded = _Trackers.GetQueryString(null, true);

        public static Uri InfoHashToPublicMagnet(string infoHash, string title)
        {
            if (string.IsNullOrWhiteSpace(infoHash) || string.IsNullOrWhiteSpace(title))
                return null;
            return new Uri($"magnet:?xt=urn:btih:{infoHash}&dn={WebUtilityHelpers.UrlEncode(title, Encoding.UTF8)}&{_TrackersEncoded}");
        }

        public static string MagnetToInfoHash(Uri magnet)
        {
            try
            {
                var xt = ParseUtil.GetArgumentFromQueryString(magnet.ToString(), "xt");
                return xt.Split(':').Last(); // remove prefix urn:btih:
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
