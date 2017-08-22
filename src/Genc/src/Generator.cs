using System;
using System.Reactive.Linq;

namespace Genc {
    /// <summary>
    ///     Provides a set of static members for creating <see cref="IObservable{T}" /> with a generation purpose.
    /// </summary>
    public static class Generator {
        /// <summary>
        ///     <para>
        ///         Creates an infinite <see cref="IObservable{T}" /> whose elements are of the type <see cref="int"/> that are
        ///         greater than or equal to argument <paramref name="min" /> and less than argument <paramref name="max" />.
        ///     </para>
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">
        ///     The exclusive upper bound of the random number returned. Argument <paramref name="max" /> must be greater than or
        ///     equal to
        ///     argument <paramref name="min" />.
        /// </param>
        /// <param name="seed">
        ///     A number used to calculate a starting value for the pseudo-random number sequence. If a negative
        ///     number is specified, the absolute value of the number is used.
        /// </param>
        /// <returns>
        ///   An <see cref="IObservable{T}" /> randomizing integers.
        /// </returns>
        /// <example>
        ///     <code language="C#" region="RandomizerThreeArg" source="Examples\GeneratorFactory.cs" />
        /// </example>
        public static IObservable<int> Randomizer(int min, int max, int? seed = null) {
            if (max > min) {
                return Observable.Generate(
                    CreateRandom(seed),
                    random => true,
                    random => random,
                    random => random.Next(min, max)
                );
            }
            throw new ArgumentOutOfRangeException(nameof(max), @"max must be > min!");
        }

        /// <summary>
        ///     <para>
        ///         Creates an infinite <see cref="IObservable{T}" /> whose elements are of the type <see cref="long"/> that are
        ///         greater than or equal to argument <paramref name="min" /> and less than argument <paramref name="max" />.
        ///     </para>
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">
        ///     The exclusive upper bound of the random number returned. Argument <paramref name="max" /> must be greater than or
        ///     equal to
        ///     argument <paramref name="min" />.
        /// </param>
        /// <param name="seed">
        ///     A number used to calculate a starting value for the pseudo-random number sequence. If a negative
        ///     number is specified, the absolute value of the number is used.
        /// </param>
        /// <returns>
        ///   An <see cref="IObservable{T}" /> randomizing longs.
        /// </returns>
        public static IObservable<long> Randomizer(long min, long max, int? seed = null) {
            if (max <= min)
                throw new ArgumentOutOfRangeException(nameof(max), @"max must be > min!");
            //Working with ulong so that modulo works correctly with values > long.MaxValue
            var uRange = (ulong) (max - min);
            return Observable.Generate(
                CreateRandom(seed),
                rnd => true,
                rnd => rnd,
                rnd => {
                    //Prevent a modulo bias; see http://stackoverflow.com/a/10984975/238419
                    //for more information.
                    //In the worst case, the expected number of calls is 2 (though usually it's
                    //much closer to 1) so this loop doesn't really hurt performance at all.
                    ulong ulongRand;
                    do {
                        var buf = new byte[8];
                        rnd.NextBytes(buf);
                        ulongRand = (ulong) BitConverter.ToInt64(buf, 0);
                    } while (ulongRand > ulong.MaxValue - (ulong.MaxValue % uRange + 1) % uRange);

                    return (long) (ulongRand % uRange) + min;
                }
            );
        }


        /// <summary>
        ///     <para>Creates <see cref="Random" /> with seed if it's not set to null.</para>
        /// </summary>
        private static Random CreateRandom(int? seed) {
            return seed == null
                ? new Random()
                : new Random(seed.Value);
        }


        /// <summary>
        ///     <para>
        ///         An infinite <see cref="IObservable{T}" /> which generates <see cref="System.Guid" />.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IObservable{T}" /> with <see cref="System.Guid" /> as its generic type.
        /// </returns>
        public static IObservable<Guid> Guid() {
            return Observable.Generate
            (new Func<Guid>(System.Guid.NewGuid),
                fun => true,
                fun => fun,
                fun => fun()
            );
        }

        /// <summary>
        ///     <para>
        ///         Creates an finite <see cref="IObservable{T}" /> whose elements are of the type <see cref="int"/> 
        ///         which get incremented by 1 until it reaches the value of <see cref="int.MaxValue"/>.
        ///     </para>
        /// </summary>
        /// <param name="start">The inclusive number to start at.</param>
        /// <returns>
        ///     An <see cref="IObservable{T}" /> with <see cref="int" /> as its generic type.
        /// </returns>
        public static IObservable<int> Incrementer(int start) {
            return start.Equals(int.MaxValue)
                ? throw new OverflowException($"Argument {nameof(start)} cannot be {int.MaxValue}.")
                : Observable.Generate(
                    start - 1,
                    i => i < int.MaxValue,
                    i => i + 1,
                    i => i + 1
                );
        }

        /// <summary>
        ///     <para>
        ///         Creates an finite <see cref="IObservable{T}" /> whose elements are of the type <see cref="long"/> 
        ///         which get incremented by 1 until it reaches the value of <see cref="long.MaxValue"/>.
        ///     </para>
        /// </summary>
        /// <param name="start">The inclusive number to start at.</param>
        /// <returns>
        ///     An <see cref="IObservable{T}" /> with <see cref="long" /> as its generic type.
        /// </returns>
        public static IObservable<long> Incrementer(long start) {
            return start.Equals(int.MaxValue)
                ? throw new OverflowException($"Argument {nameof(start)} cannot be {long.MaxValue}.")
                : Observable.Generate(
                    start - 1,
                    i => i < long.MaxValue,
                    i => i + 1,
                    i => i + 1
                );
        }

        /// <summary>
        ///     <para>
        ///         Creates an finite <see cref="IObservable{T}" /> whose elements are of the type <see cref="int"/> 
        ///         which get decremented by 1 until it reaches the value of <see cref="int.MinValue"/>.
        ///     </para>
        /// </summary>
        /// <param name="start">The inclusive number to start at.</param>
        /// <returns>
        ///     An <see cref="IObservable{T}" /> with <see cref="int" /> as its generic type.
        /// </returns>
        /// <example>
        ///     <code language="C#" region="Decrementer" source="Examples\GeneratorFactory.cs" />
        /// </example>
        public static IObservable<int> Decrementer(int start) {
            return start.Equals(int.MinValue)
                ? throw new OverflowException($"Argument {nameof(start)} cannot be {int.MaxValue}.")
                : Observable.Generate(
                    start + 1,
                    i => i > int.MinValue,
                    i => i - 1,
                    i => i - 1
                );
        }

        /// <summary>
        ///     <para>
        ///         Creates an finite <see cref="IObservable{T}" /> whose elements are of the type <see cref="long"/> 
        ///         which get decremented by 1 until it reaches the value of <see cref="long.MinValue"/>.
        ///     </para>
        /// </summary>
        /// <param name="start">The inclusive number to start at.</param>
        /// <returns>
        ///     An <see cref="IObservable{T}" /> with <see cref="long" /> as its generic type.
        /// </returns>
        public static IObservable<long> Decrementer(long start) {
            return start.Equals(int.MinValue)
                ? throw new OverflowException($"Argument {nameof(start)} cannot be {long.MaxValue}.")
                : Observable.Generate(
                    start + 1,
                    i => i > long.MinValue,
                    i => i - 1,
                    i => i - 1
                );
        }
    }
}