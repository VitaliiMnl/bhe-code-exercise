namespace Sieve
{
    public interface ISieve
    {
        long NthPrime(long n);
    }

    public class SieveImplementation : ISieve
    {
        // The SmallPrimes array stores the first 11 prime numbers. Predefined small primes for low indices.
        // The formula of the EstimateUpperBound() method uses natural logarithms, which for small 𝑛 do not give
        // sufficiently accurate results. Logarithms grow slowly, and for small numbers the difference between the real
        // number and the approximation is significant. Prime numbers are unevenly distributed, especially at the beginning of
        // the number series. The first few prime numbers (2, 3, 5, 7, 11, etc.) are closer together than the logarithmic approximation
        // would suggest. Therefore, if pass indices of the first ten primes, the formula n ×(log(n)+log(log(n))) will not give a
        // high enough upper bound to include all of those primes. For this the SmallPrimes array is used with values from previously known lists of prime numbers.
        private static readonly long[] SmallPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
        public long NthPrime(long n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Index cannot be negative.");
            }

            // Use predefined primes if n is within their range
            if (n < SmallPrimes.Length)
            {
                return SmallPrimes[n];
            }

            // For larger n, estimate upper bound and generate primes
            long upperBound = EstimateUpperBound(n);
            List<long> primes = SieveOfEratosthenes(upperBound);

            // If primes list is still not large enough, re-calculate with a higher bound
            while (primes.Count <= n)
            {
                upperBound *= 2;  // Increase the upper bound
                primes = SieveOfEratosthenes(upperBound);
            }

            return primes[(int)n];
        }

        private List<long> SieveOfEratosthenes(long limit)
        {
            bool[] isComposite = new bool[limit + 1];
            List<long> primes = new List<long>();

            for (long i = 2; i <= limit; i++)
            {
                if (!isComposite[i])
                {
                    primes.Add(i);
                    for (long j = i * i; j <= limit; j += i)
                    {
                        isComposite[j] = true;
                    }
                }
            }

            return primes;
        }

        public long EstimateUpperBound(long n)
        {
            // Prime number theorem based approximation
            double estimate = n * (Math.Log(n) + Math.Log(Math.Log(n)));
            return (long)Math.Ceiling(estimate);
        }
    }
}
