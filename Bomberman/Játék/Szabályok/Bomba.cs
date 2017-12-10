using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;
using System.Windows.Threading;

namespace Bomberman.Játék.Szabályok
{
    class Bomba : RögzítettJátékElem
    {
        /// <summary>
        /// Detonáció visszaszámlálás.
        /// </summary>
        private DispatcherTimer detonáció;
        /// <summary>
        /// Háttér nyújtásának növelése vagy csökkentése. "Pulzáló" animáció.
        /// </summary>
        private bool nővelés; 
        /// <summary>
        /// Ennyi másodperc után robban a bomba.
        /// </summary>
        private int detonációsIdő;
        /// <summary>
        /// Azért van külön addatag, mert neki változtatni kell az áthatolhatóságát. Mikor lerakja a játékos még áthatolható majd mikor már nincs "fölötte" áthatolhatatlan.
        /// </summary>
        private bool áthatolható;
        /// <summary>
        /// Gazdajátékos refernciája. Ebből kinyerhető a bombahatótávolsága és csökkenthető a lerakott bombák darabszáma miután a bomba felrobbant.
        /// </summary>
        private Játékos gazdaJátékos;
        public override bool Áthatolható
        {
            get
            {
                return áthatolható;
            }
        }
        /// <summary>
        /// Bomba objektum, mely pár másodperc után Robbanás objektumokat helyez le a Játéktérre.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tér"></param>
        /// <param name="gazdaJátékos">Bombát lerakó játékos</param>
        public Bomba(int x, int y, JátékTér.JátékTér tér, Játékos gazdaJátékos) : base(x, y, tér)
        {
            this.Alak = "bomba.png";
            this.gazdaJátékos = gazdaJátékos;
            this.áthatolható = true; //alapesetben mikor leteszi a játékos még áthatolhat rajta, majd miután már nincs rajta áthatolhatatlanná válik
            this.játékelemIdő.Tick += HelyetVizsgál_Tick;
            this.játékelemIdő.Tick += KepAnimálás_Tick;
            this.detonációsIdő = 3; //ennyi másodperc után robban a bomba
            this.detonáció = new DispatcherTimer();
            this.detonáció.Interval = TimeSpan.FromSeconds(1); //másodpercenként számoljon (vissza)
            this.detonáció.Tick += VisszaSzámlálás_Tick;
            this.detonáció.Start();
        } 
        /// <summary>
        /// Robbanás objektumok elhelyezése a JátékTéren, a gazdajátékosban tárolt hatótávolság alapján.
        /// </summary>
        private void Robbanás()
        {
            this.detonáció.Stop(); //detonációs timer leállítása
            this.játékelemIdő.Tick -= HelyetVizsgál_Tick; //robbanásnál már nem kell a bombának gazdajátékost keresni
            this.játékelemIdő.Tick -= KepAnimálás_Tick; //robbanásnál már nem kell a bombának "pulzálnia"
            this.gazdaJátékos.LerakottBombaDb--; //újra letudjon rakni bombát a gazdajátékos
            this.tér.Törlés(this); //törölje magát a játéktérről a bomba
            int hatótáv = this.gazdaJátékos.BombaHatótáv; //kimentve, mert ciklusban lesz használva
            bool[] irányTalálat = new bool[4]; //0-bal, 1-fent, 2-jobb, 3-lent (különböző irányokba hatótávon belül talált-e már kő/vasfal objektumot, ha igen akkor abba az irányba nem tárol el több pozíciót)
            int játékelemMéret = this.tér.JátékelemMéret; //kimentve, mert sokszor lesz használva
            Robbanás center = new Robbanás((int)Math.Round((double)(this.X / játékelemMéret)), (int)Math.Round((double)(this.Y / játékelemMéret)), this.tér, "exp_center.png", 0, true); //a bomba jelenlegi pozíciójába biztos hogy kell rakni egy robbanást
            for (int i = 1; i <= hatótáv; i++) //bombának van hatótávolsága
            {
                if (!irányTalálat[0])
                {
                    int[] újPozíció = { (int)Math.Round((double)(this.X / játékelemMéret)) - i, (int)Math.Round((double)(this.Y / játékelemMéret)) };
                    foreach (Játékelem akt in this.tér.MegadottHelyenLévők(this.X - játékelemMéret * i, this.Y))
                    {
                        if (akt is VasFal) //ha vasfal van akkor ne adjon hozzá több pozíciót ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[0] = true;
                        }
                        else if (akt is KőFal) //ha kőfal van akkor ezt a pozíciót még adja hozzá de többet már ne ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[0] = true;
                            new Robbanás(újPozíció[0], újPozíció[1], this.tér, "exp_end.png", 0, false);

                        }
                    }
                    if(!irányTalálat[0]) //ha nincs fal akkor simán adja hozzá a pozíciót
                    {
                        new Robbanás(újPozíció[0], újPozíció[1], this.tér, i == this.gazdaJátékos.BombaHatótáv ? "exp_end.png" : "exp_in.png", 0, false).AnimációVége += center.Törlés;
                    }
                }
                if (!irányTalálat[1])
                {
                    int[] újPozíció = { (int)Math.Round((double)(this.X / játékelemMéret)), (int)Math.Round((double)(this.Y / játékelemMéret)) - i };
                    foreach (Játékelem akt in this.tér.MegadottHelyenLévők(this.X, this.Y - játékelemMéret * i))
                    {
                        if (akt is VasFal) //ha vasfal van akkor ne adjon hozzá több pozíciót ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[1] = true;
                        }
                        else if (akt is KőFal) //ha kőfal van akkor ezt a pozíciót még adja hozzá de többet már ne ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[1] = true;
                            new Robbanás(újPozíció[0], újPozíció[1], this.tér, "exp_end.png", 90, false);
                        }
                    }
                    if (!irányTalálat[1]) //ha nincs fal akkor simán adja hozzá a pozíciót
                    {
                        new Robbanás(újPozíció[0], újPozíció[1], this.tér, i == this.gazdaJátékos.BombaHatótáv ? "exp_end.png" : "exp_in.png", 90, false).AnimációVége += center.Törlés;
                    }
                }
                if (!irányTalálat[2])
                {
                    int[] újPozíció = { (int)Math.Round((double)(this.X / játékelemMéret)) + i, (int)Math.Round((double)(this.Y / játékelemMéret)) };
                    foreach (Játékelem akt in this.tér.MegadottHelyenLévők(this.X + játékelemMéret * i, this.Y))
                    {
                        if (akt is VasFal) //ha vasfal van akkor ne adjon hozzá több pozíciót ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[2] = true;
                        }
                        else if (akt is KőFal) //ha kőfal van akkor ezt a pozíciót még adja hozzá de többet már ne ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[2] = true;
                            new Robbanás(újPozíció[0], újPozíció[1], this.tér, "exp_end.png", 180, false);
                        }
                    }
                    if (!irányTalálat[2]) //ha nincs fal akkor simán adja hozzá a pozíciót
                    {
                        new Robbanás(újPozíció[0], újPozíció[1], this.tér, i == this.gazdaJátékos.BombaHatótáv ? "exp_end.png" : "exp_in.png", 180, false).AnimációVége += center.Törlés;
                    }
                }
                if (!irányTalálat[3])
                {
                    int[] újPozíció = { (int)Math.Round((double)(this.X / játékelemMéret)), (int)Math.Round((double)(this.Y / játékelemMéret)) + i };
                    foreach (Játékelem akt in this.tér.MegadottHelyenLévők(this.X, this.Y + játékelemMéret * i))
                    {
                        if (akt is VasFal) //ha vasfal van akkor ne adjon hozzá több pozíciót ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[3] = true;
                        }
                        else if (akt is KőFal) //ha kőfal van akkor ezt a pozíciót még adja hozzá de többet már ne ebben az irányban, függetlenül a hatótávtól
                        {
                            irányTalálat[3] = true;
                            new Robbanás(újPozíció[0], újPozíció[1], this.tér, "exp_end.png", 270, false);
                        }
                    }
                    if (!irányTalálat[3]) //ha nincs fal akkor simán adja hozzá a pozíciót
                    {
                        new Robbanás(újPozíció[0], újPozíció[1], this.tér, i == this.gazdaJátékos.BombaHatótáv ? "exp_end.png" : "exp_in.png", 270, false).AnimációVége += center.Törlés;
                    }
                }
            }
        }
        /// <summary>
        /// Megadott idő után robbanás.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisszaSzámlálás_Tick(object sender, EventArgs e)
        {
            //ha még nem telt le az idő és megy a játékelemTimer
            if(detonációsIdő > 0 && this.játékelemIdő.IsEnabled)
            {
                detonációsIdő--; //visszafelé számoljon
            }
            else if(detonációsIdő == 0)
            {
                Robbanás(); //robbanás indítása
            }
        }
        /// <summary>
        /// Bomba pulzálási animációja.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KepAnimálás_Tick(object sender, EventArgs e)
        {
            if (this.nővelés)
            {
                this.BgScaleX += 0.1;
                this.BgScaleY += 0.1;
                if (this.BgScaleY > 0.99)
                    this.nővelés = false;
            }
            else
            {
                this.BgScaleX -= 0.1;
                this.BgScaleY -= 0.1;
                if (this.BgScaleY < 0.7)
                {
                    this.nővelés = true;
                }
            }
        }
        /// <summary>
        /// Ha már nincs a bomba "felett" a gazdajátékos akkor a bomba áthatolhatatlan lesz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelyetVizsgál_Tick(object sender, EventArgs e)
        {
            List<Játékelem> seged = this.tér.MegadottHelyenLévők(this.X, this.Y);
            if(!seged.Contains(this.gazdaJátékos))
            {
                this.áthatolható = false;
                this.játékelemIdő.Tick -= HelyetVizsgál_Tick;
            }
        }
        public override void Ütközés(Játékelem elem)
        {
            if (elem is Robbanás) //fel tudja robbantani egyik bomba a másikat
            {
                this.Robbanás();
            }
        }
    }
}
