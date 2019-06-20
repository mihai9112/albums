using System;
using NUnit.Framework;
using RunPath.Domain.Extensions;

namespace RunPath.Tests.Unit
{
    public class ThrowExtensionsTests
    {
        [Test]
        public void Throw_If_Null()
        {
            string defaultString = default;
            var exception = Assert.Throws<ArgumentNullException>(() => 
                defaultString.ThrowIfNull(nameof(defaultString)));
            
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: defaultString"));
        }

        [Test]
        public void Return_Value_If_Not_Null()
        {
            string defaultString = "default";
            var checkedValue = defaultString.ThrowIfNull(nameof(defaultString));

            Assert.That(checkedValue, Is.EqualTo(defaultString));
        }
    }
}