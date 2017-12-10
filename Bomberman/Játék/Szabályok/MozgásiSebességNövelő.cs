using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;

namespace Bomberman.Játék.Szabályok
{
    class MozgásiSebességNövelő : KépességFejlesztő
    {
        public MozgásiSebességNövelő(int x, int y, JátékTér.JátékTér tér) : base(x, y, tér)
        {
            this.animáltAlakok = new string[] { "pUpSeb1.jpg", "pUpSeb2.jpg" };
            this.animáltAlakokDb = this.animáltAlakok.Length;
            this.Alak = animáltAlakok[0];
        }
    }
}
