using Bomberman.Játék.JátékTér;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.Játék
{
    class GameViewModel : Bindable
    {
        private BindingList<Játékelem> elemek;
        private int játékIdő;
        private bool pause;
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
        public int JátékIdő
        {
            get
            {
                return játékIdő;
            }

            set
            {
                játékIdő = value;
                OnPropertyChanged();
            }
        }
        public bool Pause
        {
            get
            {
                return pause;
            }

            set
            {
                pause = value;
                OnPropertyChanged();
            }
        }
        public int ElemMéret //soha nem fog változni kötés után, futás alatt
        {
            get; private set;
        }
        public int PályaMéretX //akkora legyen az ablak mint a pálya
        {
            get; private set;
        }
        public int PályaMéretY //akkora legyen az ablak mint a pálya
        {
            get; private set;
        }
        public string[] JátékosokNeve //soha nem fog változni kötés után, futás alatt
        { get; set; }
        public GameViewModel(int pályaX, int pályaY, int elemMéret)
        {
            this.elemek = new BindingList<Játékelem>();
            this.PályaMéretX = pályaX;
            this.PályaMéretY = pályaY;
            this.ElemMéret = elemMéret;
        }
    }
}
