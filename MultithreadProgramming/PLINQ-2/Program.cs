using PLINQ_2.Models;
using System;
using System.Linq;

namespace PLINQ_2
{
    class Program
    {
         // Exception

        private static bool IsControl(Product p)
        {
            try
            {
                return p.Name[2] == 'a';
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata : Array sınırları aşıldı.");
                return false;
            }
        }
        static void Main(string[] args)
        {
            AdventureWorks2019Context context = new AdventureWorks2019Context();
            var products = context.Products.Take(100).ToArray();
            products[3].Name = "##";
            products[5].Name = "##";

            var query = products.AsParallel().Where(IsControl);

            // Farklı threadler çalışrıken hata alınabilir.
            try
            {
                query.ForAll(x =>
                {
                    Console.WriteLine($"{x.Name}");
                });
            }
            catch (AggregateException ex)  // AggregateException birden fazla exception ı yakalayabilir.
            {

                ex.InnerExceptions.ToList().ForEach(x =>
                {
                    if (x is IndexOutOfRangeException)
                    {
                        Console.WriteLine($"Hata : Array sınırları dışına çıkıldı.");
                    }

                });
            }
          

        }

        // Not : Parallel Linq amacı elimizde var olan array üzerinde paralel işlem yapmak istiyorsak

        public static void Plinq()
        {
            AdventureWorks2019Context context = new AdventureWorks2019Context();

            // Sonuna ToList eklemediğin sürece sorgu db'ye gitmez. ToList eklersen db'ye gider
            var deneme3 = context.Products.ToList();

            // Sonuna ToList eklemediğin sürece sorgu db'ye gitmez. ToList eklersen db'ye gider ve Bütün veri çekilir Sql Profiler da sorguda şart kısmı olmaz, çünkü AsEnumerable() da bütün veri çekildikten sonra filtreleme yapılır
            var deneme1 = context.Products.AsEnumerable().Where(x => x.Name.StartsWith("a")).ToList();

            // Sonuna ToList eklemediğin sürece sorgu db'ye gitmez. ToList eklersen db'ye gider ve şartlı bir şekilde gider.  Bütün veri şartlı bir şekilde çekilir Sql Profiler da sorguda şart kısmı olur, çünkü AsQueryable() da mantık böyledir
            var deneme2 = context.Products.AsQueryable().Where(x => x.Name.StartsWith("A")).ToList();


            var product = (from p in context.Products.AsParallel()        // AsParallel() den sonraki kısım
                           where p.ListPrice > 10m
                           select p).Take(10);    // tüm data çekildikten sonra bu kısım paralel bir şekilde çalışacak fakat bu haliyle henüz db'ye istek yapmayacak

            // 1. Yol

            product.ForAll(x =>                                           // ToList, ForAll koyduğumuz zaman db'ye sorgu gidecek. ForAll kullanabilmemiz için AsParallel  sorgusu olmasi gerek
            {
                // içinde işlemler yapabilirsin
                WriteLog(x);
            });

            // 2. Yol

            context.Products.AsParallel().Where(x => x.ListPrice > 10m).Take(10).ForAll(x =>
            {
                WriteLog(x);

            });
        }

        public static void WriteLog(Product p)
        {
            //Log'a yazılacak
            Console.WriteLine(p.Name);
        }
    }
}
