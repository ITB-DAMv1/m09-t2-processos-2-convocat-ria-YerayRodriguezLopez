using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex5
{
    public class Device
    {
        private readonly int id;
        private readonly int batteryCapacity;
        private readonly int consumptionPerSecond;
        private readonly Battery battery;
        private int chargesPerformed;
        private Thread thread;
        private bool running = true;

        public Device(int id, int batteryCapacity, int consumptionPerSecond, Battery battery)
        {
            this.id = id;
            this.batteryCapacity = batteryCapacity;
            this.consumptionPerSecond = consumptionPerSecond;
            this.battery = battery;
        }
        public int BatteryCapacity => batteryCapacity;
        public void Start()
        {
            thread = new Thread(Run);
            thread.Start();
        }

        private void Run()
        {
            while (running)
            {
                if (!battery.CanFullyCharge(batteryCapacity))
                {
                    break;
                }

                if (battery.TryCharge(batteryCapacity))
                {
                    chargesPerformed++;

                    int secondsToDrain = batteryCapacity / consumptionPerSecond;
                    for (int i = 0; i < secondsToDrain; i++)
                    {
                        if (!running) break;
                        Thread.Sleep(1000); // simulate 1 second
                    }
                }
                else
                {
                    Thread.Sleep(100); // wait and try again
                }
            }
        }

        public void Stop()
        {
            running = false;
            thread?.Join();
        }

        public int ChargesPerformed => chargesPerformed;
    }
}
