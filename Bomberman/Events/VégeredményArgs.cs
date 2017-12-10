using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.Events
{
    public class VégeredményArgs : EventArgs
    {
        public bool Döntetlen { get; set; }
        public string[] Nyertes { get; set; }
        public string[] Vesztes { get; set; }

        public VégeredményArgs(bool döntetlen, string[] nyertes = null, string[] vesztes = null)
        {
            this.Döntetlen = döntetlen;
            this.Nyertes = nyertes;
            this.Vesztes = vesztes;
        }
    }
}
