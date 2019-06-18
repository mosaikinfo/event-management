using FluentAssertions;
using Xunit;

namespace EventManagement.UnitTests
{
    public class UriHelperTests
    {
        [Theory]
        [InlineData("http://foo.com/foo", "~/bar", "http://foo.com/foo/bar")]
        [InlineData("http://foo.com/foo", "~/bar/blub", "http://foo.com/foo/bar/blub")]
        [InlineData("http://foo.com/foo", "http://foo.com", "http://foo.com")]
        [InlineData("http://foo.com/foo", "bar", "bar")]
        [InlineData("http://foo.com/foo", "/bar", "/bar")]
        public void MakeAbsoluteUri(string baseUri, string relativeUri, string expected)
        {
            string newUri = UriHelper.MakeAbsoluteUri(baseUri, relativeUri);
            newUri.Should().NotBeNull();
            newUri.Should().Be(expected);
        }
    }
}