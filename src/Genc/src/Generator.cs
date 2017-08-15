using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Genc.Implementations;
using Genc.Linq;

namespace Genc {
    /// <summary>
    ///     Provides a set of static members for creating <see cref="IGenerator{T}" />.
    /// </summary>
    public static class Generator {
        /// <summary>
        ///     <para>
        ///         Creates a <see cref="IGenerator{T}" /> whose generations will use the same
        ///         <typeparamref name="TSource" />.
        ///     </para>
        /// </summary>
        /// <param name="source">The type to generate from.</param>
        /// <typeparam name="TSource">The type of argument <paramref name="source" /></typeparam>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> whose generations will use argument <paramref name="source" />.
        /// </returns>
        public static IGenerator<TSource> Create<TSource>(TSource source) {
            return new Fun<TSource>(() => source);
        }

        /// <summary>
        ///     <para>
        ///         Creates a <see cref="IGenerator{T}" /> whose generations will use the same
        ///         <typeparamref name="TSource" />.
        ///     </para>
        /// </summary>
        /// <param name="lazy">The lazy evaluated value.</param>
        /// <typeparam name="TSource">The type of <paramref name="lazy" />.</typeparam>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> whose generations will use the lazy evaluated value from <paramref name="lazy" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="lazy" /> is null.</exception>
        public static IGenerator<TSource> Lazy<TSource>(Lazy<TSource> lazy) {
            if (lazy == null) throw new ArgumentNullException(nameof(lazy));
            return new Fun<TSource>(() => lazy.Value);
        }

        /// <summary>
        ///     <para>
        ///         Creates a <see cref="IGenerator{T}" /> whose generations will use the same
        ///         <typeparamref name="TSource" />.
        ///     </para>
        /// </summary>
        /// <param name="fn">The lazy evaluated value.</param>
        /// <typeparam name="TSource">The type of <paramref name="fn" />.</typeparam>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> whose generations will use the lazy evaluated value from <paramref name="fn" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="fn" /> is null.</exception>
        public static IGenerator<TSource> Lazy<TSource>(Func<TSource> fn) {
            return Lazy(new Lazy<TSource>(fn));
        }


        /// <summary>
        ///     <para>
        ///         Creates a <see cref="IGenerator{T}" /> whose generations will
        ///         invoke the argument <paramref name="fn" />.
        ///     </para>
        /// </summary>
        /// <param name="fn">The function to invoke for each generation.</param>
        /// <typeparam name="TSource">The type returned from argument <paramref name="fn" />.</typeparam>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> whose generations will invoke argument <paramref name="fn" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="fn" /> is null.</exception>
        public static IGenerator<TSource> Function<TSource>(Func<TSource> fn) {
            return new Fun<TSource>(fn);
        }

        /// <summary>
        ///     <para>
        ///         Creates a <see cref="IGenerator{T}" /> based on an <see cref="IEnumerable{T}" /> which resets if the end is
        ///         reached.
        ///     </para>
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to create a <see cref="IGenerator{T}" /> from.</param>
        /// <typeparam name="TSource">The type of the <paramref name="enumerable" />.</typeparam>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> whose elements comes from argument <paramref name="enumerable" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="enumerable" /> is null.</exception>
        public static IGenerator<TSource> CircularSequence<TSource>(IEnumerable<TSource> enumerable) {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            return new Seq<TSource>(enumerable);
        }

        /// <summary>
        ///     <para>
        ///         Creates a <see cref="IGenerator{T}" /> with object as its type parameter; based on an
        ///         <see cref="IEnumerable" /> which resets if the end is reached.
        ///     </para>
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable" /> to create the <see cref="IGenerator{T}" /> with
        ///     <see cref="object" /> as its type parameter.
        /// </param>
        /// <returns>
        ///     <see cref="IGenerator" /> whose elements comes from argument <paramref name="enumerable" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="enumerable" /> is null.</exception>
        public static IGenerator<object> CircularSequence(IEnumerable enumerable) {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            // TODO add proper implementation so cast can be skipped.
            return new Seq<object>(enumerable.Cast<object>());
        }


        /// <summary>
        ///     <para>
        ///         Returns a <see cref="IGenerator{T}" /> where each invocation of <see cref="IGenerator{T}.Generate" /> will
        ///         return a randomized <see cref="int" /> that is
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
        ///     A <see cref="IGenerator{T}" /> who randomizes a <see cref="int" /> that is greater than or equal to argument
        ///     <paramref name="min" /> and less than argument <paramref name="max" /> when invoking
        ///     <see cref="IGenerator{T}.Generate" />.
        /// </returns>
        /// <example>
        ///     <code language="C#" region="RandomizerThreeArg" source="Examples\GeneratorFactory.cs" />
        /// </example>
        public static IGenerator<int> Randomizer(int min, int max, int? seed = null) {
            if (max > min) return Create(CreateRandom(seed)).Select(rnd => rnd.Next(min, max));
            throw new ArgumentOutOfRangeException(nameof(max), @"max must be > min!");
        }

        /// <summary>
        ///     <para>
        ///         Returns a <see cref="IGenerator{T}" /> where each invocation of <see cref="IGenerator{T}.Generate" /> will
        ///         return a randomized <see cref="int" /> that is
        ///         greater than or equal to 0 and less than argument <paramref name="max" />.
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
        ///     A <see cref="IGenerator{T}" /> who randomizes a <see cref="long" /> that is greater than or equal to argument
        ///     <paramref name="min" /> and less than argument <paramref name="max" /> when invoking
        ///     <see cref="IGenerator{T}.Generate" />.
        /// </returns>
        public static IGenerator<long> Randomizer(long min, long max, int? seed = null) {
            if (max <= min)
                throw new ArgumentOutOfRangeException(nameof(max), @"max must be > min!");
            //Working with ulong so that modulo works correctly with values > long.MaxValue
            var uRange = (ulong) (max - min);
            var random = CreateRandom(seed);
            return Function(() => {
                //Prevent a modulo bias; see http://stackoverflow.com/a/10984975/238419
                //for more information.
                //In the worst case, the expected number of calls is 2 (though usually it's
                //much closer to 1) so this loop doesn't really hurt performance at all.
                ulong ulongRand;
                do {
                    var buf = new byte[8];
                    random.NextBytes(buf);
                    ulongRand = (ulong) BitConverter.ToInt64(buf, 0);
                } while (ulongRand > ulong.MaxValue - (ulong.MaxValue % uRange + 1) % uRange);

                return (long) (ulongRand % uRange) + min;
            });
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
        ///         Returns a <see cref="IGenerator{T}" /> where each invocation of <see cref="IGenerator{T}.Generate" /> will
        ///         return a new <see cref="System.Guid" />.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> where each invocation of <see cref="IGenerator{T}.Generate" /> will
        ///     return a new <see cref="System.Guid" />.
        /// </returns>
        /// <example>
        ///     <code language="C#" region="Guid" source="Examples\GeneratorFactory.cs" />
        /// </example>
        public static IGenerator<Guid> Guid() {
            return Function(System.Guid.NewGuid);
        }

        /// <summary>
        ///     <para>
        ///         Returns a <see cref="IGenerator{T}" /> who starts on argument <paramref name="start" /> and
        ///         returns a new <see cref="int" /> which is incremented by 1 for each invocation of
        ///         <see cref="IGenerator{T}.Generate" />.
        ///     </para>
        /// </summary>
        /// <exception cref="OverflowException">If the value exceeds <see cref="int" />.<see cref="int.MaxValue" />.</exception>
        /// <param name="start">The inclusive number to start at.</param>
        /// <returns>
        ///     A incrementation <see cref="IGenerator{T}" /> who starts on argument <paramref name="start" /> and returns a new
        ///     <see cref="int" /> which is incremented by 1 for each invocation of <see cref="IGenerator{T}.Generate" />.
        /// </returns>
        /// <example>
        ///     <code language="C#" region="Incrementer" source="Examples\GeneratorFactory.cs" />
        /// </example>
        public static IGenerator<int> Incrementer(int start) {
            return Function(() => checked(start++));
        }

        /// <summary>
        ///     <para>
        ///         Returns a <see cref="IGenerator{T}" /> who starts on argument <paramref name="start" /> and
        ///         returns a new <see cref="int" /> which is decremented by 1 for each invocation of
        ///         <see cref="IGenerator{T}.Generate" />.
        ///     </para>
        /// </summary>
        /// <exception cref="OverflowException">If the value gets below <see cref="int" />.<see cref="int.MinValue" />.</exception>
        /// <param name="start">The inclusive number to start at.</param>
        /// <returns>
        ///     A <see cref="IGenerator{T}" /> who starts on argument <paramref name="start" /> and returns a new
        ///     <see cref="int" /> which is decremented by 1 for each invocation of <see cref="IGenerator{T}.Generate" />.
        /// </returns>
        /// <example>
        ///     <code language="C#" region="Decrementer" source="Examples\GeneratorFactory.cs" />
        /// </example>
        public static IGenerator<int> Decrementer(int start) {
            return Function(() => checked(start--));
        }
    }
}