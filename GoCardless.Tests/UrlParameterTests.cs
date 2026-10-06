using System;
using NUnit.Framework;

namespace GoCardless.Tests
{
    public class UrlParameterTests
    {
        private static string Escape(object value)
        {
            return GoCardlessClient.Helpers.EscapeUrlParam("identity", value);
        }

        [Test]
        public void ShouldLeaveAWellFormedIdentityUnchanged()
        {
            Assert.That(Escape("CU123"), Is.EqualTo("CU123"));
        }

        // A URL parameter is always a single path segment, so a value carrying path syntax
        // would move the request to an endpoint the caller did not ask for.

        [Test]
        public void ShouldRejectASlash()
        {
            Assert.Throws<ArgumentException>(() => Escape("../mandates"));
        }

        [Test]
        public void ShouldRejectAQuestionMark()
        {
            Assert.Throws<ArgumentException>(() => Escape("?limit=500"));
        }

        [Test]
        public void ShouldRejectAFragmentMarker()
        {
            Assert.Throws<ArgumentException>(() => Escape("CU123#x"));
        }

        [Test]
        public void ShouldRejectAControlCharacter()
        {
            Assert.Throws<ArgumentException>(() => Escape("CU123\n"));
        }

        [Test]
        public void ShouldRejectADotSegment()
        {
            Assert.Throws<ArgumentException>(() => Escape("."));
            Assert.Throws<ArgumentException>(() => Escape(".."));
        }

        [Test]
        public void ShouldRejectAnEmptyOrMissingValue()
        {
            Assert.Throws<ArgumentException>(() => Escape(""));
            Assert.Throws<ArgumentException>(() => Escape(null));
        }
    }
}
