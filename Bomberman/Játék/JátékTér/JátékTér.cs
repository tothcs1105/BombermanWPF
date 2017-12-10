using Bomberman.Játék.Szabályok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Bomberman.Játék.JátékTér
{
    class JátékTér : Bindable
    {
        private DispatcherTimer játékelemTimer;
        /// <summary>
        /// Játékelemek lista.
        /// </summary>
        private BindingList<Játékelem> elemek;
        /// <summary>
        /// Pálya mérete x irányba.
        /// </summary>
        private int meretX;
        /// <summary>
        /// Pálya mérete y irányba.
        /// </summary>
        private int meretY;
        /// <summary>
        /// Alapértelmezett játékelem méret. Pályán elfoglalt (pixel)méret.
        /// </summary>
        private int játékelemMéret;
        /// <summary>
        /// Mivel szögletesek a játékelemek nehéz (pixelre) pontosan eltalálni az áthatolható helyet két fal között, ezért van ez könnyítésként. Min.: 1
        /// </summary>
        private const int POZÍCIÓ_HIBATŰRÉS = 1;
        /// <summary>
        /// Pálya mérete x irányba.
        /// </summary>
        public int MeretX
        {
            get
            {
                return meretX;
            }

        }
        /// <summary>
        /// Pálya mérete y irányba.
        /// </summary>
        public int MeretY
        {
            get
            {
                return meretY;
            }
        }
        /// <summary>
        /// Hogy a játékelemek elérjék
        /// </summary>
        public DispatcherTimer JátékelemTimer
        {
            get
            {
                return játékelemTimer;
            }
        }
        /// <summary>
        /// JátékTéren lévő Játékelemek.
        /// </summary>
        public BindingList<Játékelem> Elemek
        {
            get
            {
                return elemek;
            }

            set
            {
                elemek = value;
                OnPropertyChanged();
            }
        }
        public int JátékelemMéret
        {
            get
            {
                return játékelemMéret;
            }
        }
        /// <summary>
        /// Játékelemeket tartalmazó JátékTér.
        /// </summary>
        /// <param name="x">Játéktér mérete x irányba, játékelem mérettel beszorzásra kerül.</param>
        /// <param name="y">Játéktér mérete y irányba, játékelem mérettel beszorzásra kerül.</param>
        /// <param name="játékelemMéret">Játékelemek alapértelmezett mérete.</param>
        public JátékTér(int x, int y, int játékelemMéret, DispatcherTimer játékelemTimer)
        {
            this.játékelemTimer = játékelemTimer;
            this.meretX = x * játékelemMéret;
            this.meretY = y * játékelemMéret;
            this.játékelemMéret = játékelemMéret;
            this.Elemek = new BindingList<Játékelem>();
        }
        /// <summary>
        /// Játékelem felvétele a JátékTérre. Pozíciója beszorzásra kerül az alapértelmezett JátékelemMérettel.
        /// </summary>
        /// <param name="elem">Új Játékelem</param>
        public void Felvétel(Játékelem elem)
        {
            Játékelem seged = elem;
            seged.X *= JátékelemMéret;
            seged.Y *= JátékelemMéret;
            elemek.Add(seged);
        }
        /// <summary>
        /// Játékelem törlése a JátékTérről.
        /// </summary>
        /// <param name="elem">Törlendő Játékelem</param>
        public void Törlés(Játékelem elem)
        {
            elemek.Remove(elem);
        }
        /// <summary>
        /// Megadott pozíción lévő Játékelemek listáját adja. Figyelembe veszi, hogy ha egy fal mögött képességfejlesző van akkor csak a falat adja vissza.
        /// </summary>
        /// <param name="x">Pozíció x komponense</param>
        /// <param name="y">Pozíció y komponense</param>
        /// <returns>Megadott pozíción lévő Játékelemek</returns>
        public List<Játékelem> MegadottHelyenLévők(int x, int y)
        {
            List<Játékelem> kimenetiElemek = new List<Játékelem>();
            foreach (Játékelem akt in elemek)
            {
                if (Math.Abs((akt.X + (JátékelemMéret >> 1)) - (x + (JátékelemMéret >> 1))) <= JátékelemMéret - POZÍCIÓ_HIBATŰRÉS && Math.Abs((akt.Y + (JátékelemMéret >> 1)) - (y + (JátékelemMéret >> 1))) <= JátékelemMéret - POZÍCIÓ_HIBATŰRÉS)
                {
                    //ha kőfal mögött van egy képességfejlesztő akkor a képességfejlesztő ne legyen figyelembe véve (ne adja vissza)
                    if (akt is KépességFejlesztő){
                        int i = 0;
                        int elemekDb = elemek.Count;
                        while (i < elemekDb && (!(elemek[i] is KőFal) || elemek[i].X != akt.X || elemek[i].Y != akt.Y)) { i++; }
                        if (i == elemekDb)
                            kimenetiElemek.Add(akt);
                    }
                    else if(!(akt is KépességFejlesztő))
                    {
                        kimenetiElemek.Add(akt);
                    }
                }
            }
            return kimenetiElemek;
        }
    }
}
