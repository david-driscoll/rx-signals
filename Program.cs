using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using static System.Reactive.Linq.Observable;

namespace rx_signals
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var a = new Signal<int>(Return(1));
            var b = Return(2).ToSignal();
            var c = Return(3).ToSignal();

            var d = Merge(a, b, c).ToSignal();

            Report();

            a.Value = 5;

            Report();

            await Task.Delay(500);

            Report();



            void Report()
            {
                Console.WriteLine($"-------------");
                Console.WriteLine($"A: {a.Value}");
                Console.WriteLine($"B: {b.Value}");
                Console.WriteLine($"C: {c.Value}");
                Console.WriteLine($"D: {d.Value}");
            }
        }
    }
}
