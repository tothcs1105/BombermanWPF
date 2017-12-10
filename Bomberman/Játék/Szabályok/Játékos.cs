using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;
using System.Windows.Threading;
using Bomberman.Events;

namespace Bomberman.Játék.Szabályok
{
    
    class Játékos : MozgóJátékElem
    {
        public event EventHandler<JátékosHalálArgs> JátékosHalál;
        /// <summary>
        /// Mozgás és mozgási képek cseréje. Játékosnél
        /// </summary>
        private DispatcherTimer mozogAnimáció;
        /// <summary>
        /// Játékos megjelenítendő neve.
        /// </summary>
        private string név;
        /// <summary>
        /// Lerakott bomba pusztítási hatótávolság, amely képességfejlsztővel növelhető.
        /// </summary>
        private int bombaHatótáv;
        /// <summary>
        /// Lerakható bombák darabszáma, amely képességfejlsztővel növelhető.
        /// </summary>
        private int bombaDbSzám;
        /// <summary>
        /// Lerakott bombák száma, játékos bombalerakása növeli, bomba felrobbanása csökkenti.
        /// </summary>
        private int lerakottBombaDb;
        public string Név { get{ return név; } }
        public string AlapértelmezettAlak { get; private set; }
        public override bool Áthatolható
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Bomba robbanásánál a bomba csökkenteni tudja, hogy új bombát tehessen le a játékos.
        /// </summary>
        public int LerakottBombaDb
        {
            get
            {
                return lerakottBombaDb;
            }

            set
            {
                lerakottBombaDb = value;
            }
        }
        /// <summary>
        /// Bomba robbanásánál a bomba letudja kérni, hogy mekkora hatókörben "tegyen le robbanást".
        /// </summary>
        public int BombaHatótáv
        {
            get
            {
                return bombaHatótáv;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="név"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tér"></param>
        /// <param name="mozgásiAlakok">[0][] : balra, [1][] : fel, [2][] : jobbra, [3][] : le</param>
        public Játékos(string név, int x, int y, JátékTér.JátékTér tér, string[,] mozgásiAlakok) : base(x, y, tér, mozgásiAlakok)
        {
            this.mozogAnimáció = new DispatcherTimer();
            this.mozogAnimáció.Interval = TimeSpan.FromMilliseconds(90);
            this.mozogAnimáció.Tick += this.MozogAnimáció_Tick;
            this.AlapértelmezettAlak = mozgásiAlakok[3, 0];
            this.Alak = this.AlapértelmezettAlak; //alapértelmezett alak beállítása
            this.név = név;
            this.bombaHatótáv = 2;
            this.bombaDbSzám = 1;
            this.LerakottBombaDb = 0;
        }
        /// <summary>
        /// Játékos hozzáad (lerak) egy Bomba objektumot a Játéktérre. A Bomba pozíciója megegyezik a Játékos jelenlegi pozíciójával.
        /// </summary>
        public void BombátLerak()
        {
            if (lerakottBombaDb < bombaDbSzám)
            {
                List<Játékelem> seged = this.tér.MegadottHelyenLévők(this.X, this.Y);
                //egy helyre csak egy bomba kerülhessen
                if (seged.FindIndex(tmp => tmp is Játékelem && !tmp.Equals(this)) == -1) //ha nincs semmilyen elem az aktuális pozíción (kivétel ez, azaz aki leteszi a bombát) akkor...
                {
                    new Bomba((int)Math.Round((double)this.X / this.tér.JátékelemMéret), (int)Math.Round((double)this.Y / this.tér.JátékelemMéret), this.tér, this); //új bomba lehelyezése a játéktérre
                    lerakottBombaDb++;
                }
            }
        }
        /// <summary>
        /// Játékelem mozgatása. Ezt a játékos irányítására lehet használni
        /// </summary>
        /// <param name="irány">Melyik irányba mozogjon</param>
        public void Megy(Megy irány)
        {
            this.mozgásiIrány = irány;
            if (!this.mozogAnimáció.IsEnabled)
                this.mozogAnimáció.Start();
        }
        public override void Megáll()
        {
            this.mozogAnimáció.Stop();
            base.Megáll();
        }
        /// <summary>
        /// Játékelem mozgatása.
        /// </summary>
        /// <param name="rx">X koordináta változása.</param>
        /// <param name="ry">Y koordináta változása.</param>
        protected override void PrivMegy()
        {
            //mozgási sebességgel való növelés
            int rx = 0;
            int ry = 0;
            int játékelemMéret = this.tér.JátékelemMéret;
            switch (this.mozgásiIrány)
            {
                case JátékTér.Megy.balra:
                    rx -= (this.mozgásiSebesség + játékelemMéret / 10); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
                    break;
                case JátékTér.Megy.fel:
                    ry -= (this.mozgásiSebesség + játékelemMéret / 10); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
                    break;
                case JátékTér.Megy.jobbra:
                    rx += (this.mozgásiSebesség + játékelemMéret / 10); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
                    break;
                case JátékTér.Megy.le:
                    ry += (this.mozgásiSebesség + játékelemMéret / 10); //azért mert függ a mozgási sebesség a játékelem mérettől, mivel, nagyobb elemméretnél többet kell menni ugyanolyan sebességhez
                    break;
            }
            //Cella közepéhez való igazítás
            if ((this.X + rx) % játékelemMéret < mozgásiSebesség)
                if (rx > 0)
                    rx -= (this.X + rx) % játékelemMéret;
                else if (rx < 0)
                    rx += (this.X + rx) % játékelemMéret;
            if ((this.Y + ry) % játékelemMéret < mozgásiSebesség)
                if (ry > 0)
                    ry -= (this.Y + ry) % játékelemMéret;
                else if (ry < 0)
                    ry += (this.Y + ry) % játékelemMéret;
            //ha nem sikerül az áthelyezés akkor álljon a legközelebbi cella közepére.
            if (!ÁtHelyez(this.X + rx, this.Y + ry))
            {
                ÁtHelyez((int)Math.Round((double)this.X / játékelemMéret) * játékelemMéret, (int)Math.Round((double)this.Y / játékelemMéret) * játékelemMéret);
            }
        }
        /// <summary>
        /// Játékos ütközése különböző Játékelemekkel. Képességfejlesztők hatására itt módosul a játékos.
        /// </summary>
        /// <param name="elem"></param>
        public override void Ütközés(Játékelem elem)
        {

            if (elem is Robbanás || elem is Szörny)
            {
                this.Megáll();
                this.játékelemIdő.Tick += Felrobban_Tick; //elindítja a felrobbanás animációt, majd törlődik a játéktérről
                if (this.JátékosHalál != null) JátékosHalál(this, new JátékosHalálArgs(this));
            }
            else if (elem is KépességFejlesztő) {
                KépességFejlesztő elemTMP = elem as KépességFejlesztő;
                //ha még nem fejlesztett a képességfejlesztő
                if (elemTMP.Aktív)
                {
                    elemTMP.Aktív = false; //muszáj itt állítani különben nem jól működik
                    if (elem is BombaDbNövelő)
                    {
                        this.bombaDbSzám++;
                    }
                    else if (elem is BombaHatótávNövelő)
                    {
                        this.bombaHatótáv++;
                    }
                    else if (elem is MozgásiSebességNövelő)
                    {
                        //maximális mozgási sebesség: 20
                        if(this.mozgásiSebesség < 20)
                            this.mozgásiSebesség += 2;
                        //egyre gyorsabban animálja a játékos hátterét (80 ms és 8 mozgási sebességgel kezd max mozgási sebesség az 20, minimális timer interval: 32, ezért 4 ms-el csökkenti a timert)
                        if (this.mozogAnimáció.Interval.Milliseconds > 32)
                            this.mozogAnimáció.Interval = TimeSpan.FromMilliseconds(this.mozogAnimáció.Interval.Milliseconds - 4);
                    }
                }
            }
        }
    }
}
