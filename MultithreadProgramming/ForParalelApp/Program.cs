using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ForParalelApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagesPath = @"C:\Users\aktur\Desktop\Deneme";
            var files = Directory.GetFiles(imagesPath);

            long totalByte = 0;
              
            // Buradaki olay farklı threadler paylaşımlı oalrak Deneme klasörü içindeki dosyaların boyutlarını alıp, toplam boyutun yazılacağı fileByte değişkenine ekliyecek Fakat
            // Tek bir değere aynı anda ulaşıp değeri eklemeye çalışacakları için RaceCondition  olacak bunu engellemek için Interlocked kullandık. Böylece bir thread işini bitirene kadar lock olacak 
            Parallel.For(0,files.Length, (index) =>
            {
                Console.WriteLine("thread no :" + Thread.CurrentThread.ManagedThreadId);
                FileInfo f = new FileInfo(files[index]);
                Interlocked.Add(ref totalByte, f.Length);

            });

            Console.WriteLine("toplam boyut" + totalByte.ToString());
        }
    }
}
