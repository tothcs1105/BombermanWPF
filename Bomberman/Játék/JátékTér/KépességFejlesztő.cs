using Bomberman.Játék.Szabályok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Bomberman.Játék.JátékTér
{
    abstract class KépességFejlesztő : RögzítettJátékElem
    {
        /// <summary>
        /// Felvesz animáció futtatása. Az alap játékidőTimer lassú.
        /// </summary>
        private DispatcherTimer felvesz;
        /// <summary>
        /// Megjelenítendő képek.
        /// </summary>
        protected string[] animáltAlakok;
        /// <summary>
        /// Cserélendő képek száma, így bármennyi kép lehet és gyorsabb mint minden Ticknél lekérni a méretet.
        /// </summary>
        protected int animáltAlakokDb;
        /// <summary>
        /// Jelenleg megjelnítendő alak.
        /// </summary>
        private int jelenlegiAlak;
        /// <summary>
        /// Képességfejlesztő felvétele közben amíg "eltűnik" csak egyszer fejlesszen képességet.
        /// </summary>
        private bool aktív;
        /// <summary>
        /// Képességfejlesztő áthatolható, hogy felvehető legyen.
        /// </summary>
        public override bool Áthatolható
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Legalul helyezkedik el, így fal mögé kerülhet.
        /// </summary>
        public override int ZIndex
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        /// Képességfejlesztő felvétele közben amíg "eltűnik" csak egyszer fejlesszen képességet.
        /// </summary>
        public bool Aktív
        {
            get
            {
                return aktív;
            }

            set
            {
                aktív = value;
            }
        }
        /// <summary>
        /// Képességfejlesztő, felvételével a Jétékos különböző képességei módsíthatók, képességfejlesztő fajtától függően.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tér"></param>
        public KépességFejlesztő(int x, int y, JátékTér tér) : base(x, y, tér)
        {
            this.aktív = true;
            this.játékelemIdő.Tick += Villog_Tick;
            this.felvesz = new DispatcherTimer();
            this.felvesz.Interval = TimeSpan.FromMilliseconds(30);
            this.felvesz.Tick += Felvesz_Tick;
        }
        /// <summary>
        /// Képességfejlesztő villogtatási animációja.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Villog_Tick(object sender, EventArgs e)
        {
            if (jelenlegiAlak < animáltAlakokDb - 1)
                this.jelenlegiAlak++;
            else
                this.jelenlegiAlak = 0;
            this.Alak = this.animáltAlakok[jelenlegiAlak];
        }
        /// <summary>
        /// Felvétel animáció.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Felvesz_Tick(object sender, EventArgs e)
        {
            //csak akkor ha a játékelemTimer megy
            if (this.játékelemIdő.IsEnabled)
            {
                this.játékelemIdő.Tick -= this.Villog_Tick;
                if (this.BgScaleX > 0.2 && this.BgScaleY > 0.2)
                {
                    this.BgScaleX -= 0.2;
                    this.BgScaleY -= 0.2;
                }
                else
                {
                    felvesz.Stop();
                    this.tér.Törlés(this);
                }
            }
        }
        public override void Ütközés(Játékelem elem)
        {
            if (elem is Játékos || elem is Szörny)
            {
                this.felvesz.Start();
            }
            else if (elem is Robbanás)
            {
                this.Aktív = false;
                this.játékelemIdő.Tick -= this.Villog_Tick;
                this.játékelemIdő.Tick += Felrobban_Tick;
            }
        }
    }
}
