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
    abstract class Játékelem : Bindable
    {
        /// <summary>
        /// Felrobbanás animációhoz és egyéb dolgokhoz használatos timer.
        /// </summary>
        protected DispatcherTimer játékelemIdő;
        /// <summary>
        /// Játékelem x pozíciója.
        /// </summary>
        private int x;
        /// <summary>
        /// Játékelem y pozíciója.
        /// </summary>
        private int y;
        /// <summary>
        /// Játékelem hátterének x irányú nyújtása.
        /// </summary>
        private double bgScaleX;
        /// <summary>
        /// Játékelem hátterének y irányú nyújtása.
        /// </summary>
        private double bgScaleY;
        /// <summary>
        /// Játékelem hátterének szöge.
        /// </summary>
        private int bgAngle;
        /// <summary>
        /// Felrobbanás animációhoz képfájlok elérési útjait tárolja.
        /// </summary>
        private string[] robbanásAlakok;
        /// <summary>
        /// Felrobbanás animáció képfájlaihoz tartozó indexelő.
        /// </summary>
        private int jelenlegiRobbanásAlak;
        /// <summary>
        /// Játékelem alapértelmezett megjelenítendő háttere.
        /// </summary>
        private string alak;
        /// <summary>
        /// Játékelemet tartalmaző JátékTér.
        /// </summary>
        protected JátékTér tér;
        /// <summary>
        /// Játékelem x pozíciója.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Játékelem y pozíciója.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Játékelem hátterének x irányú nyújtása.
        /// </summary>
        public double BgScaleX
        {
            get
            {
                return bgScaleX;
            }

            set
            {
                bgScaleX = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Játékelem hátterének y irányú nyújtása.
        /// </summary>
        public double BgScaleY
        {
            get
            {
                return bgScaleY;
            }

            set
            {
                bgScaleY = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Játékelem hátterének szöge.
        /// </summary>
        public int BgAngle
        {
            get
            {
                return bgAngle;
            }

            set
            {
                bgAngle = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Megjelenítendő kép, erőforrás elérési útja.
        /// </summary>
        public string Alak { get { return this.alak; } protected set { this.alak = value; OnPropertyChanged(); } }
        /// <summary>
        /// Játékelem áthatolható-e egy másik játékelem által.
        /// </summary>
        public abstract bool Áthatolható { get; }
        /// <summary>
        /// Játékelemek elhelyezkedhetnek egymás felett (mögött) is, ezért szükség van egy z irányú "koordinátára" is.
        /// </summary>
        public abstract int ZIndex { get; }
        /// <summary>
        /// Játékelem létrehozása és hozzáadása a megadott JátékTérhez.
        /// </summary>
        /// <param name="x">Játékelem x pozíciója a JátékTéren</param>
        /// <param name="y">Játékelem y pozíciója a JátékTéren</param>
        /// <param name="tér">Játékelemet tartalmazó JátékTér</param>
        public Játékelem(int x, int y, JátékTér tér)
        {
            this.tér = tér;
            this.robbanásAlakok = new string[] { "exp1.png", "exp2.png", "exp3.png", "exp4.png", "exp5.png", "exp6.png", "exp7.png", "exp8.png" };
            this.játékelemIdő = this.tér.JátékelemTimer;
            this.x = x;
            this.y = y;
            this.bgScaleX = 1;
            this.BgScaleY = 1;
            this.bgAngle = 0;
            this.jelenlegiRobbanásAlak = 0;
            this.tér.Felvétel(this);
        }
        /// <summary>
        /// Felrobbanás animáció végrehajtása, majd Játékelem törlése a JátékTérről.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Felrobban_Tick(object sender, EventArgs e)
        {
            if (jelenlegiRobbanásAlak < 8)
            {
                this.Alak = robbanásAlakok[this.jelenlegiRobbanásAlak];
                this.jelenlegiRobbanásAlak++;     
            }
            else
                this.tér.Törlés(this);
        } 
        /// <summary>
        /// Mikor a Játékelem találkozik egy másik Játékelemmel.
        /// </summary>
        /// <param name="elem"></param>
        public abstract void Ütközés(Játékelem elem);  
    }
}
