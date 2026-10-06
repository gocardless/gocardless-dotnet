using System;
using NUnit.Framework;

namespace GoCardless.Tests
{
    public class UrlResolutionTests
    {
        private static readonly Uri BaseUrl = new Uri("https://api.gocardless.com");

        private static Uri Resolve(string path)
        {
            return GoCardlessClient.Helpers.ResolveAgainstBaseUrl(BaseUrl, path);
        }

        [Test]
        public void ShouldResolveRelativePathAgainstBaseUrl()
        {
            Assert.That(
                Resolve("/customers/CU123").ToString(),
                Is.EqualTo("https://api.gocardless.com/customers/CU123")
            );
        }

        [Test]
        public void ShouldKeepQueryString()
        {
            Assert.That(
                Resolve("/customers?limit=50").ToString(),
                Is.EqualTo("https://api.gocardless.com/customers?limit=50")
            );
        }

        [Test]
        public void ShouldKeepDotSegmentsOnTheBaseUrl()
        {
            // Dot segments resolve against the base URL, so they can reach another path on
            // the same origin but cannot leave it. They are allowed through.
            Assert.That(Resolve("/customers/../other").Host, Is.EqualTo(BaseUrl.Host));
        }

        // An absolute or scheme-relative path would replace the configured base URL while the
        // Authorization header is still attached, handing the access token to whichever host
        // the path names.

        [Test]
        public void ShouldRejectAbsoluteUrl()
        {
            Assert.Throws<ArgumentException>(() =>
                Resolve("https://elsewhere.example.com/capture")
            );
        }

        [Test]
        public void ShouldRejectAbsoluteUrlWithADifferentScheme()
        {
            Assert.Throws<ArgumentException>(() => Resolve("http://api.gocardless.com/capture"));
        }

        [Test]
        public void ShouldRejectSchemeRelativeUrl()
        {
            Assert.Throws<ArgumentException>(() => Resolve("//elsewhere.example.com/capture"));
        }

        [Test]
        public void ShouldRejectAbsoluteUrlOnADifferentPortOfTheSameHost()
        {
            Assert.Throws<ArgumentException>(() =>
                Resolve("https://api.gocardless.com:8443/capture")
            );
        }

        [Test]
        public void ShouldNeverResolveBackslashAuthorityOffTheBaseUrl()
        {
            // Uri normalises backslashes to slashes for hierarchical URIs, which makes this
            // authority syntax rather than a path - but how far that goes has varied between
            // the frameworks this package targets, so the assertion is the property that has
            // to hold rather than the mechanism: either the path is rejected, or it stays on
            // the configured origin. What must never happen is a request to the named host.
            Uri resolved = null;

            try
            {
                resolved = Resolve("\\\\elsewhere.example.com/capture");
            }
            catch (ArgumentException)
            {
                Assert.Pass("rejected before resolution");
            }

            Assert.That(resolved.Host, Is.EqualTo(BaseUrl.Host));
        }
    }
}
