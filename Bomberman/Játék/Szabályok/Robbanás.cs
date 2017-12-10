using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;
using System.Windows.Threading;

namespace Bomberman.Játék.Szabályok
{
    class Robbanás : RögzítettJátékElem
    {
        /// <summary>
        /// Robbanás animáció.
        /// </summary>
        private DispatcherTimer robbanás;
        /// <summary>
        /// Robbanás animáció során háttér nyújtása vagy zsugorítása.
        /// </summary>
        private bool robbanNövel;
        public override bool Áthatolható
        {
            get
            {
                return false;
            }
        }
        public event EventHandler AnimációVége; //mikor véget ér az animáció, azaz eltüntek a nem középső robbanások akkor a középső robbanás eltüntetése.
        /// <summary>
        /// Ha ütközik nem vasfal játékelemmel akkor azt letörli a JátékTérről.
        /// </summary>
        /// <param name="x">Robbanás x pozíciója a JátékTéren</param>
        /// <param name="y">Robbanás y pozíciója a JátékTéren</param>
        /// <param name="tér">Robbanást tartalmazó JátékTér</param>
        /// <param name="alak">Megjelenítendő kép erőforrásának elérési útja</param>
        /// <param name="angle">Megjelenítendő kép forgatásához szükséges szög</param>
        /// <param name="center">Középső robbanási elem-e</param>
        public Robbanás(int x, int y, JátékTér.JátékTér tér, string alak, int angle, bool center) : base(x, y, tér)
        {
            this.Alak = alak;
            this.BgAngle = angle;
            this.robbanNövel = true;
            this.robbanás = new DispatcherTimer();
            this.robbanás.Interval = TimeSpan.FromMilliseconds(20);
            if (center)
            {
                this.robbanás.Tick += Csökken_Tick;
            }
            else
            {
                this.BgScaleY = 0.5;
                this.robbanás.Tick += Robbanás_Tick;
                this.robbanás.Start();
            }
            List<Játékelem> seged = tér.MegadottHelyenLévők(x * this.tér.JátékelemMéret, y * this.tér.JátékelemMéret);
            foreach(Játékelem akt in seged)
            {
                akt.Ütközés(this);
                this.Ütközés(akt);
            }   
        }
        /// <summary>
        /// Középső robbanás elem ezt a metódust indítja mikor a nem középső robbanások eltűntek.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Törlés(object sender, EventArgs e)
        {
            this.robbanás.Start();
        }
        /// <summary>
        /// Középső robbanás animálása.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Csökken_Tick(object sender, EventArgs e)
        {
            //ha szüneteltetve van a játék ne csináljon semmit
            if (this.játékelemIdő.IsEnabled)
            {
                this.BgScaleY -= 0.1;
                this.BgScaleX -= 0.1;
                if (this.BgScaleY < 0.1)
                {
                    this.robbanás.Stop();
                    this.tér.Törlés(this);
                }
            }
        }
        /// <summary>
        /// Nem középső robbanások animálása.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Robbanás_Tick(object sender, EventArgs e)
        {
            //ha szüneteltetve van a játék ne csináljon semmit
            if(this.játékelemIdő.IsEnabled)
            {
                if (this.robbanNövel)
                {
                    this.BgScaleY += 0.1;
                    if (this.BgScaleY > 0.99)
                        this.robbanNövel = false;
                }
                else
                {
                    this.BgScaleY -= 0.1;
                    if (this.BgScaleY < 0.1)
                    {
                        this.robbanás.Stop();
                        this.tér.Törlés(this);
                    }
                    if (this.BgScaleY < 0.5) //ilyenkor induljon el a robbanás középpontjának csökkentése
                        if (AnimációVége != null) AnimációVége(this, new EventArgs());
                }
            }
        }
        /// <summary>
        /// A rögzítettjátékelem osztályban lévő érték felülírása kivételesen. Had látszódjon minden elem robbanás animációja.
        /// </summary>
        public override int ZIndex
        {
            get
            {
                return 0;
            }
        }
        public override void Ütközés(Játékelem elem)
        {

        }
    }
}
