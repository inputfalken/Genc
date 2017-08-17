using System;
using System.Linq;
using System.Reactive.Linq;
using NUnit.Framework;
using static Genc.Generator;

namespace Tests.GeneratorAPI {
    [TestFixture]
    internal class IncrementerTests {
        [Test]
        public void Int_MinValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Incrementer(int.MinValue)
                .Take(500).ToEnumerable());
        }

        [Test]
        public void Start_Below_Int_MinValue_Throws() {
            var start = int.MinValue;
            Assert.Throws<OverflowException>(() => Incrementer(start - 1)
                .Take(1)
                .ToEnumerable()
            );
        }

        [Test]
        public void Start_Int_MaxValue_Throws() {
            Assert.Throws<OverflowException>(() => Incrementer(int.MaxValue)
                .Take(500).ToEnumerable()
            );
        }

        [Test]
        public void Start_Minus_Twenty() {
            var start = -20;
            var result = Incrementer(start)
                .Take(500)
                .ToEnumerable();
            var expected = Enumerable.Range(start, 500);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Start_Twenty() {
            var result = Incrementer(20)
                .Take(500)
                .ToEnumerable();
            var expected = Enumerable.Range(20, 500);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void To_Int_Max_Value() {
            const int start = int.MaxValue - 500;
            var result = Incrementer(start)
                .Take(500)
                .ToEnumerable();
            var expected = Enumerable.Range(start, 500);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void To_More_Than_Int_Max_Value() {
            const int start = int.MaxValue - 500;
            var count = Incrementer(start)
                .Take(502)
                .ToEnumerable()
                .Count();
            Assert.AreEqual(501, count);
        }
    }
}