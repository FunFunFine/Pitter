namespace Pitter {
    public class ShortenedLink
    {
        public string ShortUrl { get; set; }
        public string Url { get; set; }
        public string Key { get; set; }

        public override string ToString() => $"{ShortUrl}";
    }
}