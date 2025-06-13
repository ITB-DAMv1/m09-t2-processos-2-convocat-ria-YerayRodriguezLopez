using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Ex5
{
    public class Program
    {
        public static void Main()
        {
            RunScenario1();
            Console.WriteLine("\n---\n");
            RunScenario2();
        }

        private static void RunScenario1()
        {
            Console.WriteLine("Escenari 1");
            var simulation = new Simulation(100_000);
            simulation.AddDevice(1, 30_000, 10_000);
            simulation.AddDevice(2, 20_000, 12_000);
            simulation.AddDevice(3, 5_000, 1_000);
            simulation.Start();
        }

        private static void RunScenario2()
        {
            Console.WriteLine("Escenari 2");
            var simulation = new Simulation(100_000);
            simulation.AddDevice(1, 25_000, 23_000);
            simulation.AddDevice(2, 20_000, 12_000);
            simulation.AddDevice(3, 8_000, 1_000);
            simulation.AddDevice(4, 10_000, 1_000);
            simulation.Start();
        }
    }
}
