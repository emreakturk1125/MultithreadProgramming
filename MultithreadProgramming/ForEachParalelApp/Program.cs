using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForEachParalelApp
{
    class Program
    {
        //Resimleri küçük boyutlu hale getirmek
        static void Main(string[] args)
        {
            string imagesPath = @"C:\Users\aktur\Desktop\Deneme";  // local pc deki herhangibir yerde olan resimlerin yolunu verebilirsin
            var files = Directory.GetFiles(imagesPath);

            Stopwatch sw = new Stopwatch();


            sw.Start();

            // Datayı paylaşımlı  olarak farklı threadlerde çalıştrmaya yarar
            // Küçük datalaı işlemlerde gerek yoktur. Çok datalı işlemlerde yapılabilir

            // Multithread thread üzerinde işlemleri yaptığımızda daha kısa sürede oluyor : 230 
            Parallel.ForEach(files, (item) =>
             {
                 Console.WriteLine("thread no :" + Thread.CurrentThread.ManagedThreadId);

                 Image img = new Bitmap(item);
                 var thumbnail = img.GetThumbnailImage(50, 50, () => false, IntPtr.Zero);
                 thumbnail.Save(Path.Combine(imagesPath, "thumbnail1", Path.GetFileName(item)));
             });
            sw.Stop();
            Console.WriteLine("----> thumbnail1 için İşlem bitti! : " + sw.ElapsedMilliseconds);

            sw.Reset();


            sw.Start();

            // Senkron olarak tek thread üzerinde işlemleri yaptığımızda daha uzun sürede oluyor : 336 
            files.ToList().ForEach(x =>
            {
                Console.WriteLine("thread no : " + Thread.CurrentThread.ManagedThreadId);

                Image img = new Bitmap(x);
                var thumbnail = img.GetThumbnailImage(50, 50, () => false, IntPtr.Zero);
                thumbnail.Save(Path.Combine(imagesPath, "thumbnail2", Path.GetFileName(x)));
            });
            sw.Stop();
            Console.WriteLine("---> thumbnail2 için İşlem bitti! : " + sw.ElapsedMilliseconds);
        }
    }
}
