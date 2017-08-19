# Genc #
[![Build status](https://ci.appveyor.com/api/projects/status/b3q113l8lcpne7r1?svg=true)](https://ci.appveyor.com/project/inputfalken/genc)

# What is it? #
A library using `Reactive Extensions` for data generation. For example:
* Randomizing numbers
* Incrementing numbers
* Decrementing numbers

# How do i use it? #
Use the Factory class located at `Genc.Generator`.

# Example #
```cs
using Genc;
using System.Reactive;

namespace Example
{
    class Program
    {
        public static void Main()
        {
            // Creates a generator which starts at 0 and increments by 1.
            IObservable<int> incrementer = Generator.Incrementer(start: 0);
            // A modified incrementer which is doubled for each incrementation.
            IObservable<int> doubledIncrementer = incrementer.Select(i => i * 2);

            // Take 10 numbers from the generator.
            IEnumerable<int> numbers = doubledIncrementer
            .ToEnumerable()
            .Take(10);

            foreach(int number in numbers)
            {
                Console.Write(number); // prints: 024681012141618
            }
        }
    }
}

```

# Install #
[Nuget](https://www.nuget.org/packages/Genc/)
