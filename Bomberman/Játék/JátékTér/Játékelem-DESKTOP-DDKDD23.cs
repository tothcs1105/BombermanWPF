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
    abstract class JátékElem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //itt kell lennie mivel helyzet változtatásnál itt van az ami módosul és ez kötve lesz a UIhoz
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        protected DispatcherTimer felrobban;
        private bool aktív;
        private int x;
        private int y;
        private double bgScaleX;
        private double bgScaleY;
        private int bgAngle;
        private string[] robbanásAlakok;
        private int jelenlegiRobbanásAlak;
        private string alak; //alapértelmezett alak
        protected JátékTér tér;
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

        public JátékElem(int x, int y, JátékTér tér)
        {
            this.robbanásAlakok = new string[] { "exp1.png", "exp2.png", "exp3.png", "exp4.png", "exp5.png", "exp6.png", "exp7.png", "exp8.png" };
            this.felrobban = new DispatcherTimer();
            this.felrobban.Interval = TimeSpan.FromMilliseconds(100);
            this.felrobban.Tick += Felrobban_Tick;
            this.aktív = true;
            this.x = x;
            this.y = y;
            this.bgScaleX = 1;
            this.BgScaleY = 1;
            this.bgAngle = 0;
            this.jelenlegiRobbanásAlak = 0;
            this.tér = tér;
            this.tér.Felvétel(this);
        }

        private void Felrobban_Tick(object sender, EventArgs e)
        {
            if (jelenlegiRobbanásAlak < 8)
            {
                this.Alak = robbanásAlakok[this.jelenlegiRobbanásAlak];
                this.jelenlegiRobbanásAlak++;
                
            }
            else
                this.tér.Törlés(this);
        }
        public string Alak{ get { return this.alak; } protected set { this.alak = value; OnPropertyChanged(); } } //a megjelenítendő kép (erőforrás) neve
        public abstract int ZIndex { get; } //az egymás alatt (mögött) lévő elemekhez szükséges
        public abstract bool Áthatolható { get; } //áthatolható-e a játékelem a mozgójátékelemek által
        public abstract void Ütközés(JátékElem elem);
    }
}
