namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class SocialMediaBlogs : ISocialMedia, ITechnology
    {
        private readonly ITrending trending;

        public SocialMediaBlogs(ITrending trending)
        {
            this.trending = trending;
        }
    }
}