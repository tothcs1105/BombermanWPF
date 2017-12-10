using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék;

namespace Bomberman
{
    public class JátékBeállítás : Bindable
    {
        private string[] játékosNevek;
        private int pályaMéretX;
        private int pályaMéretY;
        private int elemMéret;
        private int másodpercek;
        public string[] JátékosNevek
        {
            get
            {
                return játékosNevek;
            }

            set
            {
                játékosNevek = value;
                OnPropertyChanged();
            }
        }
        public int ElemMéret
        {
            get
            {
                return elemMéret;
            }

            set
            {
                elemMéret = value;
                OnPropertyChanged();
            }
        }
        public int Másodpercek
        {
            get
            {
                return másodpercek;
            }

            set
            {
                másodpercek = value;
                OnPropertyChanged();
            }
        }
        public int PályaMéretX
        {
            get
            {
                return pályaMéretX;
            }

            set
            {
                pályaMéretX = value;
                OnPropertyChanged();
            }
        }
        public int PályaMéretY
        {
            get
            {
                return pályaMéretY;
            }

            set
            {
                pályaMéretY = value;
                OnPropertyChanged();
            }
        }
        public JátékBeállítás()
        {
            this.játékosNevek = new string[2] { "Játékos1", "Játékos2" }; //alapértelmezett nevek
            this.pályaMéretX = 19; //alapértelmezett méret
            this.pályaMéretY= 19; //alapértelmezett méret
            this.elemMéret = 40; //alapértelmezett méret
            this.másodpercek = 120; //alapértelmezett játékidő
        }
    }
}
