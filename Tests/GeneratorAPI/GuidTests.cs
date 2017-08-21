using System;
using System.Reactive.Linq;
using Genc;
using NUnit.Framework;

namespace Tests.GeneratorAPI {
    [TestFixture]
    internal class GuidTests {
        [Test(
            Description = "Verify that GUID generator does not return null"
        )]
        public void Is_Not_Empty() {
            Generator.Guid()
                .FirstAsync()
                .Subscribe(guid => Assert.AreNotEqual(guid, Guid.Empty));
        }

        [Test(
            Description = "Verify that GUID generator does not return null"
        )]
        public void Is_Not_Null() {
            var result = Generator.Guid();
            Assert.IsNotNull(result);
        }
    }
}