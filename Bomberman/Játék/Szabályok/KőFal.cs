using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;

namespace Bomberman.Játék.Szabályok
{
    class KőFal : RögzítettJátékElem
    {
        public KőFal(int x, int y, JátékTér.JátékTér tér) : base(x, y, tér)
        {
            this.Alak = "kofal.jpg";
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
            if (elem is Robbanás)
                this.játékelemIdő.Tick += Felrobban_Tick;
        }
    }
}
