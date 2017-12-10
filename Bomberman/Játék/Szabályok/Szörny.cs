using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;
using System.Windows.Threading;

namespace Bomberman.Játék.Szabályok
{
    class Szörny : MozgóJátékElem
    {
        /// <summary>
        /// Véletlen irány generálásához.
        /// </summary>
        private Random r;
        /// <summary>
        /// Lehetséges irányok. [0] : bal, [1] : fel, [2] : jobb, [3] : le
        /// </summary>
        private List<int> lehetségesIrányok;
        /// <summary>
        /// Ezekbe az irányokba lép el a játékelem. Konkrétan ez adódik hozzá a pozíciójához. [0] : bal, [1] : fel, [2] : jobb, [3] : le
        /// </summary>
        private List<int[]> irányok;
        /// <summary>
        /// Jelenlegi irány, véletlen szerűen generált.
        /// </summary>
        int irány;
        public Szörny(int x, int y, JátékTér.JátékTér tér, string[,] mozgásiAlakok) : base(x, y, tér, mozgásiAlakok)
        {
            this.r = new Random();
            this.lehetségesIrányok = new List<int>(); //fontos a feltöltési sorrend
            this.lehetségesIrányok.Add(0);
            this.lehetségesIrányok.Add(1);
            this.lehetségesIrányok.Add(2);
            this.lehetségesIrányok.Add(3);
            this.irányok = new List<int[]>(); //fontos a feltöltési sorrend
            this.irányok.Add(new int[] { - this.tér.JátékelemMéret / 10, 0 }); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
            this.irányok.Add(new int[] { 0, - this.tér.JátékelemMéret / 10 }); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
            this.irányok.Add(new int[] { this.tér.JátékelemMéret / 10, 0 }); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
            this.irányok.Add(new int[] { 0, this.tér.JátékelemMéret / 10 }); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
            this.irány = r.Next(4);
            this.mozgásiIrány = (JátékTér.Megy)lehetségesIrányok[irány];
            this.BgScaleX = 0.9; //kisebb legyen a szörny
            this.BgScaleY = 0.9; //kisebb legyen a szörny
            this.Alak = mozgásiAlakok[3,0]; //alapértelmezett alak beállítása
            this.játékelemIdő.Tick += this.MozogAnimáció_Tick;
        }
        public override bool Áthatolható
        {
            get
            {
                return true;
            }
        }
        public override void Ütközés(Játékelem elem)
        {
            if (elem is Robbanás)
            {
                this.Megáll(); //azért kell hogy a felrobbanás animáció látszódjon, különben cserélgetni a mozgási timer és a felrobbanás timer is a képeket
                this.játékelemIdő.Tick += Felrobban_Tick;
            }
        }
        /// <summary>
        /// Véletlenszerűen választ irányt, addig megy egy irányba amíg tud, majd új irányt választ véletlen szerűen.
        /// </summary>
        protected override void PrivMegy()
        {
            if (lehetségesIrányok.Count != 0 && !ÁtHelyez(this.X + irányok[lehetségesIrányok[irány]][0], this.Y + irányok[lehetségesIrányok[irány]][1]))
            {
                lehetségesIrányok.RemoveAt(irány);
                irány = r.Next(lehetségesIrányok.Count);      
            }
            else if(lehetségesIrányok.Count != 0)
            {
                this.mozgásiIrány = (JátékTér.Megy)lehetségesIrányok[irány];
            }    
            else if(lehetségesIrányok.Count == 0)
            {
                lehetségesIrányok.Add(0);
                lehetségesIrányok.Add(1);
                lehetségesIrányok.Add(2);
                lehetségesIrányok.Add(3);
            }
        }
        public override void Megáll()
        {
            this.játékelemIdő.Tick -= this.MozogAnimáció_Tick;
            base.Megáll();
        }
    }
}
