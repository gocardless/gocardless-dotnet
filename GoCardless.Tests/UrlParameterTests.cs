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
            Assert.Throws<ArgumentException>((Action)(() => Escape("../mandates")));
        }

        [Test]
        public void ShouldRejectAQuestionMark()
        {
            Assert.Throws<ArgumentException>((Action)(() => Escape("?limit=500")));
        }

        [Test]
        public void ShouldRejectAFragmentMarker()
        {
            Assert.Throws<ArgumentException>((Action)(() => Escape("CU123#x")));
        }

        [Test]
        public void ShouldRejectAControlCharacter()
        {
            Assert.Throws<ArgumentException>((Action)(() => Escape("CU123\n")));
        }

        [Test]
        public void ShouldRejectADotSegment()
        {
            Assert.Throws<ArgumentException>((Action)(() => Escape(".")));
            Assert.Throws<ArgumentException>((Action)(() => Escape("..")));
        }

        [Test]
        public void ShouldRejectAnEmptyOrMissingValue()
        {
            Assert.Throws<ArgumentException>((Action)(() => Escape("")));
            Assert.Throws<ArgumentException>((Action)(() => Escape(null)));
        }
    }
}
