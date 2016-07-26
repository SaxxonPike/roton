// From: http://referencesource.microsoft.com/#mscorlib/system/random.cs
// Reason: We want a framework version agnostic random number generator.
//         Microsoft claims they may change on major version changes.
//         So, we just use the one from .NET 4.6.1

using System;

namespace Roton.Emulation.Execution
{
    public sealed class Random
    {
        //
        // Private Constants 
        //
        private const int MBig = int.MaxValue;
        private const int MSeed = 161803398;

        private int _iNext;
        private int _iNextP;
        private readonly int[] _seedArray = new int[56];

        public Random()
            : this(Environment.TickCount)
        {
        }

        public Random(int seed)
        {
            //Initialize our Seed array.
            //This algorithm comes from Numerical Recipes in C (2nd Ed.)
            var subtraction = seed == int.MinValue ? int.MaxValue : Math.Abs(seed);
            var mj = MSeed - subtraction;
            _seedArray[55] = mj;
            var mk = 1;
            for (var i = 1; i < 55; i++)
            {
                //Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
                var ii = 21*i%55;
                _seedArray[ii] = mk;
                mk = mj - mk;
                if (mk < 0) mk += MBig;
                mj = _seedArray[ii];
            }
            for (var k = 1; k < 5; k++)
            {
                for (var i = 1; i < 56; i++)
                {
                    _seedArray[i] -= _seedArray[1 + (i + 30)%55];
                    if (_seedArray[i] < 0) _seedArray[i] += MBig;
                }
            }
            _iNext = 0;
            _iNextP = 21;
        }

        private double Sample()
        {
            //Including this division at the end gives us significantly improved
            //random number distribution.
            return InternalSample()*(1.0/MBig);
        }

        private int InternalSample()
        {
            var locINext = _iNext;
            var locINextp = _iNextP;

            if (++locINext >= 56) locINext = 1;
            if (++locINextp >= 56) locINextp = 1;

            var retVal = _seedArray[locINext] - _seedArray[locINextp];

            if (retVal == MBig) retVal--;
            if (retVal < 0) retVal += MBig;

            _seedArray[locINext] = retVal;

            _iNext = locINext;
            _iNextP = locINextp;

            return retVal;
        }

        public int Next()
        {
            return InternalSample();
        }

        private double GetSampleForLargeRange()
        {
            // The distribution of double value returned by Sample 
            // is not distributed well enough for a large range.
            // If we use Sample for a range [Int32.MinValue..Int32.MaxValue)
            // We will end up getting even numbers only.

            var result = InternalSample();
            // Note we can't use addition here. The distribution will be bad if we do that.
            var negative = InternalSample()%2 == 0; // decide the sign based on second sample
            if (negative)
            {
                result = -result;
            }
            double d = result;
            d += int.MaxValue - 1; // get a number in range [0 .. 2 * Int32MaxValue - 1)
            d /= 2*(uint) int.MaxValue - 1;
            return d;
        }

        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            var range = (long) maxValue - minValue;
            if (range <= int.MaxValue)
            {
                return (int) (Sample()*range) + minValue;
            }
            return (int) ((long) (GetSampleForLargeRange()*range) + minValue);
        }

        public int Next(int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }
            return (int) (Sample()*maxValue);
        }

        public double NextDouble()
        {
            return Sample();
        }

        public void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte) (InternalSample()%(byte.MaxValue + 1));
            }
        }
    }
}