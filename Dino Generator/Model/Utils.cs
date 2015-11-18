using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dino_Generator.Model
{
    public class Utils
    {
        public static int GetRealRandomNumberInRange(int from, int to)
        {
            if (from == to || to <= from)
            {
                throw new Exception("Wrong random number LAWL.");
            }
            byte[] numbers = new byte[to/255 + 1];

            int sum = 0;

            RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();
            do
            {
                sum = 0;
                randomNumberGenerator.GetBytes(numbers);
                foreach (var number in numbers)
                {
                    sum += number;
                }
            }
            while (sum >= to);



            return sum;
        }
    }
}
