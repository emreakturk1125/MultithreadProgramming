using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForEachParelelAppRaceCondition
{
    class Program
    {
        // Duruma göre Race Condition olabilir ya da olmayabilir. 1000 de bazn olmadı bazn oldu. 10000 ve sonra sı için Race Condition oldu
        static void Main(string[] args)
        { 
            int deger = 0;

            Parallel.ForEach(Enumerable.Range(1, 1000).ToList(), (x) =>
            {
                deger = x;
            });

            Console.WriteLine(deger);
        }
    }
}
