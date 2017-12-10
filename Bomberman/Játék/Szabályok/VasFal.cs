using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;

namespace Bomberman.Játék.Szabályok
{
    class VasFal : RögzítettJátékElem
    {
        public VasFal(int x, int y, JátékTér.JátékTér tér) : base (x, y, tér)
        {
            this.Alak = "vasfal.png";
        }
        public override bool Áthatolható
        {
            get
            {
                return false;
            }
        }
        public override void Ütközés(Játékelem elem)
        {

        }
    }
}
