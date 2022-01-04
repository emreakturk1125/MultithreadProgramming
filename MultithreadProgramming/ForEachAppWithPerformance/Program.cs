using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForEachAppWithPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            int total = 0;

            // Aşağıdaki işlemde her thread kendi toplama işlemini yapar ve en son total değişkenini Interlocked üzerinden atar

            // 1 den 100 e kadar olan değerleri arka arkaya topla ve total değişkenine yazdırma işleminde
            // Normal şartlarda 4 thread bu işlemi yapacak olsun a(1-25), b(26-50), c(51-75), d(76-100) 
            // Aşağıdaki kullanımda threadlerin sorumlu olduğu rakamları kendi arasında toplayıp en son total'e setlesin böyle daha performanslıdır.
            // Böylece threadlerden shared dataya erişim daha az olacaktır.

            Parallel.ForEach(Enumerable.Range(1, 100).ToList(), () => 0, (x, loop, subtotal) =>
            {
                subtotal += x;
                return subtotal;

            }, (y) => Interlocked.Add(ref total, y));

            Console.WriteLine(total);
        }
    }
}
