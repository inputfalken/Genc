using System;
using System.Reactive.Linq;
using System.Threading;
using Genc;
using NUnit.Framework;

namespace Tests.GeneratorAPI {
    [TestFixture]
    internal class RandomizerTests {
        [Test]
        public void Double_Max_Greater_Than_Min_Does_Not_Throw() {
            const double min = 0.5;
            const double max = 0.7;
            Assert.DoesNotThrow(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Double_Min_Equal_To_Max_Throws() {
            const double min = 0.7;
            const double max = 0.7;
            Assert.Throws<ArgumentOutOfRangeException>(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Double_Min_Greater_Than_Max_Throws() {
            const double min = 0.8;
            const double max = 0.7;
            Assert.Throws<ArgumentOutOfRangeException>(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Double_Using_Seed_Gives_Same_Result() {
            const int seed = 100;
            const int length = 1000000;
            const double maxValue = 1.9;
            const double minValue = 0.8;
            var result = Generator.Randomizer(minValue, maxValue, seed)
                .Take(length)
                .ToEnumerable();
            // So the seed could change
            Thread.Sleep(200);
            var expected = Generator.Randomizer(minValue, maxValue, seed)
                .Take(length)
                .ToEnumerable();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Double_With_Different_Seed_Does_Not_Gives_Same_Result() {
            const int length = 1000000;
            const double maxValue = 1.9;
            const double minValue = 0.8;
            var result = Generator.Randomizer(minValue, maxValue, 100)
                .Take(length)
                .ToEnumerable();
            var expected = Generator.Randomizer(minValue, maxValue, 200)
                .Take(length)
                .ToEnumerable();

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void Double_Without_Seed_Does_Not_Give_Same_Result() {
            const int length = 1000000;
            const double maxValue = 1.9;
            const double minValue = 0.8;
            var result = Generator
                .Randomizer(minValue, maxValue)
                .Take(length)
                .ToEnumerable();
            // So the seed can change
            Thread.Sleep(200);
            var expected = Generator
                .Randomizer(minValue, maxValue)
                .Take(length)
                .ToEnumerable();

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void Int32_Max_Greater_Than_Min_Does_Not_Throw() {
            const int min = 0;
            const int max = 1;
            Assert.DoesNotThrow(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Int32_Min_Equal_To_Max_Throws() {
            const int min = 1;
            const int max = 1;
            Assert.Throws<ArgumentOutOfRangeException>(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Int32_Min_Greater_Than_Max_Throws() {
            const int min = 1;
            const int max = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Int32_Using_Seed_Gives_Same_Result() {
            const int seed = 100;
            const int length = 1000000;
            const int maxValue = 100000;
            const int minValue = 1000;
            var result = Generator
                .Randomizer(minValue, maxValue, seed)
                .Take(length)
                .ToEnumerable();
            // So the seed could change
            Thread.Sleep(200);
            var expected = Generator
                .Randomizer(minValue, maxValue, seed)
                .Take(length)
                .ToEnumerable();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Int32_With_Different_Seed_Does_Not_Gives_Same_Result() {
            const int length = 1000000;
            const int maxValue = 100000;
            const int minValue = 1000;
            var result = Generator
                .Randomizer(minValue, maxValue, 100)
                .Take(length)
                .ToEnumerable();
            var expected = Generator
                .Randomizer(minValue, maxValue, 200)
                .Take(length)
                .ToEnumerable();

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void Int32_Without_Seed_Does_Not_Gives_Same_Result() {
            const int length = 1000000;
            const int maxValue = 100000;
            const int minValue = 1000;
            var result = Generator
                .Randomizer(minValue, maxValue)
                .Take(length)
                .ToEnumerable();
            // So the seed can change
            Thread.Sleep(200);
            var expected = Generator
                .Randomizer(minValue, maxValue)
                .Take(length)
                .ToEnumerable();

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void Int64_Max_Greater_Than_Min_Does_Not_Throw() {
            const long min = 0;
            const long max = 1;
            Assert.DoesNotThrow(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Int64_Min_Equal_To_Max_Throws() {
            const long min = 1;
            const long max = 1;
            Assert.Throws<ArgumentOutOfRangeException>(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Int64_Min_Greater_Than_Max_Throws() {
            const long min = 1;
            const long max = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => Generator.Randomizer(min, max));
        }

        [Test]
        public void Int64_Using_Seed_Gives_Same_Result() {
            const int seed = 100;
            const int length = 1000000;
            const long maxValue = long.MaxValue - 20000000;
            const long minValue = long.MaxValue - 30000000;
            var result = Generator
                .Randomizer(minValue, maxValue, seed)
                .Take(length)
                .ToEnumerable();
            // So the seed can change
            Thread.Sleep(200);
            var expected = Generator.Randomizer(minValue, maxValue, seed)
                .Take(length)
                .ToEnumerable();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Int64_With_Different_Seed_Does_Not_Gives_Same_Result() {
            const int length = 1000000;
            const long maxValue = long.MaxValue - 20000000;
            const long minValue = long.MaxValue - 30000000;
            var result = Generator
                .Randomizer(minValue, maxValue, 100)
                .Take(length)
                .ToEnumerable();

            var expected = Generator
                .Randomizer(minValue, maxValue, 200)
                .Take(length)
                .ToEnumerable();

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void Int64_Without_Seed_Does_Not_Gives_Same_Result() {
            const int length = 1000000;
            const long maxValue = long.MaxValue - 20000000;
            const long minValue = long.MaxValue - 30000000;
            var result = Generator
                .Randomizer(minValue, maxValue)
                .Take(length)
                .ToEnumerable();
            // So the seed can change
            Thread.Sleep(200);
            var expected = Generator
                .Randomizer(minValue, maxValue)
                .Take(length)
                .ToEnumerable();

            Assert.AreNotEqual(expected, result);
        }
    }
}