// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Numerics.Tests
{
    public class IsEvenTest
    {
        private const int MaxDigits = 400;
        private const int Reps = 500;
        private static int s_seed = 0;

        [Fact]
        public static void RunIsEvenTests()
        {
            Random random = new Random(s_seed);

            // Just basic tests
            // Large Even Number
            VerifyIsEven((BigInteger)Int64.MaxValue + 1, true);

            // Large Odd Number
            VerifyIsEven((BigInteger)Int64.MaxValue + 2, false);

            // Large Random Even Number

            for (int i = 0; i < Reps; i++)
            {
                string bigInt1 = BigIntTools.Utils.BuildRandomNumber(random.Next() % MaxDigits + 1, random.Next());
                VerifyIsEven(BigInteger.Parse(bigInt1) * 2, true);
            }

            // Large Random Odd Number

            for (int i = 0; i < Reps; i++)
            {
                string bigInt1 = BigIntTools.Utils.BuildRandomNumber(random.Next() % MaxDigits + 1, random.Next());
                VerifyIsEven((BigInteger.Parse(bigInt1) * 2) - 1, false);
            }

            // Small Even Number
            VerifyIsEven((BigInteger)Int16.MaxValue - 1, true);

            // Small Odd Number
            VerifyIsEven((BigInteger)Int16.MaxValue - 2, false);


            //Negative tests
            // Large Negative Even Number
            VerifyIsEven(((BigInteger)Int64.MaxValue + 1) * -1, true);

            // Large Negative Odd Number
            VerifyIsEven(((BigInteger)Int64.MaxValue + 2) * -1, false);
            

            // Large Negative Random Even Number
            for (int i = 0; i < Reps; i++)
            {
                string bigInt2 = BigIntTools.Utils.BuildRandomNumber(random.Next() % MaxDigits + 1, random.Next());
                VerifyIsEven(BigInteger.Parse(bigInt2) * -2, true);
            }

            // Small Negative Even Number
            VerifyIsEven(((BigInteger)Int16.MaxValue - 1) * -1, true);

            // Small Negative Odd Number
            VerifyIsEven(((BigInteger)Int16.MaxValue - 2) * -1, false);


            //Zero Case, 1, -1
            // Zero
            VerifyIsEven(BigInteger.Zero, true);

            // One
            VerifyIsEven(BigInteger.One, false);

            // Negative One
            VerifyIsEven(BigInteger.MinusOne, false);
        }
        
        private static void VerifyIsEven(BigInteger bigInt, bool expectedAnswer)
        {
            Assert.Equal(expectedAnswer, bigInt.IsEven);
        }
    }
}