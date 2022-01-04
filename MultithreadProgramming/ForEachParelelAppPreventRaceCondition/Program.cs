using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ForEachParelelAppPreventRaceCondition
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagesPath = @"C:\Users\aktur\Desktop\Deneme";
            var files = Directory.GetFiles(imagesPath);

            long fileByte = 0;

            Stopwatch sw = new Stopwatch();

            sw.Start();


            // Buradaki olay farklı threadler paylaşımlı oalrak Deneme klasörü içindeki dosyaların boyutlarını alıp, toplam boyutun yazılacağı fileByte değişkenine ekliyecek Fakat
            // Tek bir değere aynı anda ulaşıp değeri eklemeye çalışacakları için RaceCondition  olacak bunu engellemek için Interlocked kullandık. Böylece bir thread işini bitirene kadar lock olacak 
            Parallel.ForEach(files, (item) =>
            {
                Console.WriteLine("thread no :" + Thread.CurrentThread.ManagedThreadId);
                FileInfo f = new FileInfo(item);
                Interlocked.Add(ref fileByte, f.Length);

            });

            Console.WriteLine("toplam boyut" + fileByte.ToString());

        }
    }
}
