using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Musketeer.Extensions
{
    public static class AssertExtensions
    {
        public static void StringEqualsIgnoreCase(this Assert assert, string expected, string actual, string message = null)
        {
            if(message == null)
                Assert.AreEqual(expected.ToLower(), actual.ToLower());
            else
                Assert.AreEqual(expected.ToLower(), actual.ToLower(), message);
        }
    }
}