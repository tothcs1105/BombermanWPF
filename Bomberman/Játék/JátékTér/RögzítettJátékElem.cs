using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.Játék.JátékTér
{
    abstract class RögzítettJátékElem : Játékelem
    {
        public RögzítettJátékElem(int x, int y, JátékTér tér) : base(x, y, tér) { }

        public override int ZIndex
        {
            get
            {
                return 2;
            }
        }
    }
}
