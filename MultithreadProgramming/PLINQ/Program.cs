using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQ
{
    class Program
    {
        // PLINQ linq sorgularını paralel bir şekilde çalıştırılmasını sağlar. AsParallel() kullanılır. Çok verisi olan ve çok işlem gerektiren yani performans gerektiren sorgularda kullanılabilir. 
        // ForAll() ile kullanmak gerekir farklı threadlerde çalışması için
        // Sorgular eş zamanlı olarak farklı threadler üzerinde çalıştırılmasıdır.
        // Farklı threadler üzerinde alınan sonuçlar birleştirilip sonuç döner.
        // LINQ array lar üzerinde sorgulamak içindir.
        static void Main(string[] args)
        {
            var array = Enumerable.Range(1, 99999999).ToList();
            Stopwatch sw = new Stopwatch();


            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("İşlem - 1");

            sw.Start();
            var newArray = array.AsParallel().Where(Islem);
            newArray.ForAll(x =>                                    //  ForAll  farklı threadlerde yani multithread olarak çalışmasını sağlamak içindir.
            {
                int result = x * 10 / 15;
                double rsl = result * 0.1;
            });
            sw.Stop();
            Console.WriteLine("Toplam zaman: " + sw.ElapsedMilliseconds.ToString());

            sw.Reset();


            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("İşlem - 2");

            sw.Start();
            var newArray2 = array.Where(Islem);
            newArray2.ToList().ForEach(x =>                      // ToList() varsa senkron olur, tek threadli çalışır
            {
                int result = x * 10 / 15;
                double rsl = result * 0.1;

            });
            sw.Stop();
            Console.WriteLine("Toplam zaman: " + sw.ElapsedMilliseconds.ToString());

        }

        public static bool Islem(int x)
        {
            return x % 2 == 0;
        }

    }
}
