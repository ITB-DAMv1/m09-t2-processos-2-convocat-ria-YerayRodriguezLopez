using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex5
{
    public class Battery
    {
        private int capacity;
        private int remaining;
        private readonly object lockObj = new();

        public Battery(int capacity)
        {
            this.capacity = capacity;
            this.remaining = capacity;
        }

        public bool TryCharge(int amount)
        {
            lock (lockObj)
            {
                if (remaining >= amount)
                {
                    remaining -= amount;
                    return true;
                }
                return false;
            }
        }

        public int Remaining
        {
            get
            {
                lock (lockObj)
                {
                    return remaining;
                }
            }
        }

        public bool CanFullyCharge(int amount)
        {
            lock (lockObj)
            {
                return remaining >= amount;
            }
        }
    }
}
