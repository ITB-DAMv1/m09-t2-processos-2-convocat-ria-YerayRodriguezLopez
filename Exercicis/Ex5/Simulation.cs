using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex5
{
    public class Simulation
    {
        private readonly Battery battery;
        private readonly List<Device> devices = new();
        private readonly Stopwatch stopwatch = new();

        public Simulation(int batteryCapacity)
        {
            battery = new Battery(batteryCapacity);
        }

        public void AddDevice(int id, int capacity, int consumption)
        {
            devices.Add(new Device(id, capacity, consumption, battery));
        }

        public void Start()
        {
            stopwatch.Start();
            foreach (var device in devices)
            {
                device.Start();
            }

            while (true)
            {
                bool canContinue = false;
                foreach (var device in devices)
                {
                    if (battery.CanFullyCharge(device.BatteryCapacity))
                    {
                        canContinue = true;
                        break;
                    }
                }

                if (!canContinue) break;

                Thread.Sleep(500);
            }

            foreach (var device in devices)
            {
                device.Stop();
            }

            stopwatch.Stop();
            PrintResults();
        }

        private void PrintResults()
        {
            Console.WriteLine($"Temps total de simulació: {stopwatch.Elapsed.TotalSeconds:F2} segons");
            foreach (var device in devices)
            {
                Console.WriteLine($"Dispositiu {device.GetHashCode()} - Càrregues realitzades: {device.ChargesPerformed}");
            }
        }
    }
}
