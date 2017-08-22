using System.Collections.Generic;
using System.Reactive.Linq;
using NUnit.Framework;
using static Genc.Generator;

namespace Tests.GeneratorAPI {
    [TestFixture]
    internal class IncrementerTests {
        private static IEnumerable<int> Expected(int start, int count) {
            var longs = new List<int>();
            var current = 0;
            while (true) {
                longs.Add(start++);
                current++;
                if (current == count) {
                    break;
                }
            }
            return longs;
        }

        private static IEnumerable<long> Expected(long start, int count) {
            var longs = new List<long>();
            var current = 0;
            while (true) {
                longs.Add(start++);
                current++;
                if (current == count) {
                    break;
                }
            }
            return longs;
        }

        [Test]
        public void Int_MinValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Incrementer(int.MinValue)
                .Take(500).ToEnumerable());
        }

        [Test]
        public void Long_MinValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Incrementer(long.MinValue).Take(500).ToListObservable());
        }

        [Test]
        public void Start_Int_MaxValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Incrementer(int.MaxValue).ToListObservable());
        }

        [Test]
        public void Start_Int_MaxValue_Includes_MaxValue_Only() {
            var listObservable = Incrementer(int.MaxValue).ToListObservable();
            Assert.AreEqual(1, listObservable.Count);
            Assert.AreEqual(int.MaxValue, listObservable[0]);
        }

        [Test]
        public void Start_Long_MaxValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Incrementer(long.MaxValue).ToListObservable());
        }

        [Test]
        public void Start_Long_MaxValue_Includes_MaxValue_Only() {
            var listObservable = Incrementer(long.MaxValue).ToListObservable();
            Assert.AreEqual(1, listObservable.Count);
            Assert.AreEqual(long.MaxValue, listObservable[0]);
        }

        [Test]
        public void Start_Long_MaxValue_Minus_Five_Thousand() {
            const long start = long.MaxValue - 5000;
            const int count = 500;

            var enumerable = Expected(start, count);
            var result = Incrementer(start).Take(count).ToListObservable();
            Assert.AreEqual(enumerable, result);
        }

        [Test]
        public void Start_Long_MinValue_Plus_Five_Thousand() {
            const long start = long.MinValue + 5000;
            const int count = 500;


            var enumerable = Expected(start, count);
            var result = Incrementer(start).Take(count).ToListObservable();
            Assert.AreEqual(enumerable, result);
        }

        [Test]
        public void Start_Minus_Twenty() {
            var start = -20;
            var count = 500;
            var result = Incrementer(start).Take(count).ToListObservable();
            var expected = Expected(start, count);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Start_Twenty() {
            var result = Incrementer(20)
                .Take(500)
                .ToEnumerable();
            var expected = Expected(20, 500);
            Assert.AreEqual(expected, result);
        }
    }
}