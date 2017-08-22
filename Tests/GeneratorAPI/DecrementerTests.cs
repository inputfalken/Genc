using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Genc;
using NUnit.Framework;

namespace Tests.GeneratorAPI {
    [TestFixture]
    internal class DecrementerTests {
        private static IEnumerable<int> Expected(int start, int count) {
            var longs = new List<int>();
            var current = 0;
            while (true) {
                longs.Add(start--);
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
                longs.Add(start--);
                current++;
                if (current == count) {
                    break;
                }
            }
            return longs;
        }

        [Test]
        public void Start_Int_MinValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Generator.Decrementer(int.MinValue)
                .Take(500)
                .ToEnumerable()
            );
        }

        [Test]
        public void Start_Long_MinValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Generator.Decrementer(long.MinValue)
                .Take(500)
                .ToEnumerable()
            );
        }

        [Test]
        public void Start_Int_MinValue_Includes_MinValue_Only() {
            var listObservable = Generator.Decrementer(int.MinValue).ToListObservable();
            Assert.AreEqual(1, listObservable.Count);
            Assert.AreEqual(int.MinValue, listObservable[0]);
        }

        [Test]
        public void Start_Long_MinValue_Includes_MinValue_Only() {
            var listObservable = Generator.Decrementer(long.MinValue).ToListObservable();
            Assert.AreEqual(1, listObservable.Count);
            Assert.AreEqual(long.MinValue, listObservable[0]);
        }

        [Test]
        public void Start_Int_MaxValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Generator.Decrementer(int.MaxValue)
                .Take(500)
                .ToEnumerable()
            );
        }

        [Test]
        public void Start_Long_MaxValue_Does_Not_Throw() {
            Assert.DoesNotThrow(() => Generator.Decrementer(long.MaxValue)
                .Take(500)
                .ToEnumerable()
            );
        }

        [Test]
        public void Start_Minus_Twenty() {
            const int start = -20;
            const int count = 500;
            var result = Generator.Decrementer(start)
                .Take(count)
                .ToEnumerable();

            var expected = Expected(start, count);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Start_Long_MinValue_Plus_Five_Thousand() {
            const long start = long.MinValue + 5000;
            const int count = 500;

            var enumerable = Expected(start, count);
            var result = Generator.Decrementer(start).Take(count).ToListObservable();
            Assert.AreEqual(enumerable, result);
        }

        [Test]
        public void Start_Long_MaxValue_Minus_Five_Thousand() {
            const long start = long.MaxValue - 5000;
            const int count = 500;

            var enumerable = Expected(start, count);
            var result = Generator.Decrementer(start).Take(count).ToListObservable();
            Assert.AreEqual(enumerable, result);
        }

        [Test]
        public void Start_Twenty() {
            const int start = 20;
            const int count = 500;
            var result = Generator.Decrementer(start)
                .Take(count)
                .ToEnumerable();
            var expected = Expected(start, count);
            Assert.AreEqual(expected, result);
        }


        [Test]
        public void To_Int_Min_Value() {
            const int count = 500;
            const int start = int.MinValue + 500;
            var result = Generator.Decrementer(start).ToListObservable();
            // This is because Int.MinValue is inclusive
            var expected = Expected(start, count + 1);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void To_Long_Min_Value() {
            const int count = 500;
            const long start = long.MinValue + 500;
            var result = Generator.Decrementer(start).ToListObservable();
            // This is because Long.MinValue is inclusive
            var expected = Expected(start, count + 1);
            Assert.AreEqual(expected, result);
        }
    }
}