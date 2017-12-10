using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.Szabályok;

namespace Bomberman.Events
{
    class JátékosHalálArgs : EventArgs
    {
        public Játékos Játékos { get; set; }

        public JátékosHalálArgs(Játékos játékos)
        {
            this.Játékos = játékos;
        }
    }
}
