using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;

namespace Bomberman.Játék.Szabályok
{
    class BombaDbNövelő : KépességFejlesztő
    {
        public BombaDbNövelő(int x, int y, JátékTér.JátékTér tér) : base(x, y, tér)
        {
            this.animáltAlakok = new string[] { "pUpBDB1.jpg", "pUpBDB2.jpg" };
            this.animáltAlakokDb = this.animáltAlakok.Length;
            this.Alak = animáltAlakok[0];
        }
    }
}
