using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Játék.JátékTér;
using Bomberman.Játék.Szabályok;
using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bomberman.Events;

namespace Bomberman.Játék.Keret
{
    class Keret
    {
        public event EventHandler<VégeredményArgs> JátékVége;
        /// <summary>
        /// PályaGenerálásnál random szám generálásához.
        /// </summary>
        private static Random R;
        /// <summary>
        /// View Model
        /// </summary>
        private GameViewModel vm;
        /// <summary>
        /// Pálya szélessége
        /// </summary>
        private int pályaMéretX;
        /// <summary>
        /// Pálya magassága
        /// </summary>
        private int pályaMéretY;
        /// <summary>
        /// Játékelemeket tartalmazó JátékTér.
        /// </summary>
        private JátékTér.JátékTér tér;
        /// <summary>
        /// 1. játékos.
        /// </summary>
        private Játékos j1;
        /// <summary>
        /// 2. játékos.
        /// </summary>
        private Játékos j2;
        /// <summary>
        /// Játéknak a timere.
        /// </summary>
        private DispatcherTimer játékTimer;
        /// <summary>
        /// Mozgó játékelemek mozgatása
        /// </summary>
        private DispatcherTimer játékelemTimer;
        /// <summary>
        /// Szüneteltetve van-e a játék
        /// </summary>
        private bool pause;
        public GameViewModel VM
        {
            get
            {
                return vm;
            }
        }
        public Keret(int pályaMéretX, int pályaMéretY, int játékelemMéret, int játékIdő)
        {
            this.pályaMéretX = pályaMéretX;
            this.pályaMéretY = pályaMéretY;
            this.játékTimer = new DispatcherTimer();
            this.játékTimer.Interval = TimeSpan.FromSeconds(1);
            this.játékTimer.Tick += JátékTimer_Tick;
            this.játékelemTimer = new DispatcherTimer();
            this.játékelemTimer.Interval = TimeSpan.FromMilliseconds(100);
            this.játékelemTimer.Start();
            R = new Random();
            this.tér = new JátékTér.JátékTér(this.pályaMéretX, this.pályaMéretY, játékelemMéret, this.játékelemTimer);
            this.vm = new GameViewModel(this.pályaMéretX * játékelemMéret, this.pályaMéretY * játékelemMéret, játékelemMéret);
            this.vm.JátékIdő = játékIdő;
            vm.Elemek = this.tér.Elemek;
            PályaGenerálás();
        }
        /// <summary>
        /// Játékosok elhelyezése a pályán
        /// </summary>
        /// <param name="játékIdő"></param>
        /// <param name="játékosok"></param>
        public void Futtatás(string[] játékosok)
        {
            vm.JátékosokNeve = new string[] { játékosok[0], játékosok[1] };
            j1 = new Játékos(játékosok[0], 1, 1, this.tér, Kinézetek(1));
            j2 = new Játékos(játékosok[1], pályaMéretX - 2, pályaMéretY - 2, this.tér, Kinézetek(2));
            j1.JátékosHalál += JátékosHalál;
            j2.JátékosHalál += JátékosHalál;
            this.játékTimer.Start();
        }
        /// <summary>
        /// Játék megállítása, ha pl ablak bezárás történt
        /// </summary>
        public void Megállítás()
        {
            this.játékTimer.Stop();
            this.játékelemTimer.Stop();
            this.játékTimer = null;
            this.játékelemTimer = null;
        }
        /// <summary>
        /// Ha egy játékos meghal véget ér a játék. Az életben maradt játékos nyer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JátékosHalál(object sender, JátékosHalálArgs e)
        {
            this.játékTimer.Stop();
            this.játékelemTimer.Stop();
            string[] nyertes = new string[2];
            string[] vesztes = new string[2];
            if (j1.Equals(e.Játékos))
            {
                nyertes[0] = j2.Név;
                nyertes[1] = j2.AlapértelmezettAlak;
                vesztes[0] = j1.Név;
                vesztes[1] = j1.AlapértelmezettAlak;
            }
            else
            {
                nyertes[0] = j1.Név;
                nyertes[1] = j1.AlapértelmezettAlak;
                vesztes[0] = j2.Név;
                vesztes[1] = j2.AlapértelmezettAlak;
            }
            if (this.JátékVége != null) JátékVége(this, new VégeredményArgs(false, nyertes, vesztes));
        }
        /// <summary>
        /// Játékidő tick. Másodpercenként levon a játékidőből.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JátékTimer_Tick(object sender, EventArgs e)
        {
            if (vm.JátékIdő > 0)
            {
                vm.JátékIdő = --vm.JátékIdő;
            }
            else
            {
                this.játékTimer.Stop();
                this.játékelemTimer.Stop();
                if (this.JátékVége != null) JátékVége(this, new VégeredményArgs(true));
            }
        }
        private void Pause(bool szünet)
        {
            if (szünet)
            {
                this.játékTimer.Stop();
                this.játékelemTimer.Stop();
                this.pause = true;
                VM.Pause = pause;
            }
            else
            {
                this.játékTimer.Start();
                this.játékelemTimer.Start();
                this.pause = false;
                VM.Pause = pause;
            }
        }
        /// <summary>
        /// Játékosok irányítása
        /// </summary>
        /// <param name="e"></param>
        public void Lenyom(Key e)
        {
            if (e.Equals(Key.P))
                Pause(!this.pause);
            if (!pause)
            {
                switch (e)
                {
                    case Key.Left:
                        j2.Megy(Megy.balra);
                        break;
                    case Key.Up:
                        j2.Megy(Megy.fel);
                        break;
                    case Key.Right:
                        j2.Megy(Megy.jobbra);
                        break;
                    case Key.Down:
                        j2.Megy(Megy.le);
                        break;
                    case Key.RightCtrl:
                        j2.BombátLerak();
                        break;
                    case Key.A:
                        j1.Megy(Megy.balra);
                        break;
                    case Key.W:
                        j1.Megy(Megy.fel);
                        break;
                    case Key.D:
                        j1.Megy(Megy.jobbra);
                        break;
                    case Key.S:
                        j1.Megy(Megy.le);
                        break;
                    case Key.LeftCtrl:
                        j1.BombátLerak();
                        break;
                }
            }
        }
        /// <summary>
        /// Játékosok megállítása
        /// </summary>
        /// <param name="e"></param>
        public void Felenged(Key e)
        {
            switch (e)
            {
                case Key.A:
                case Key.W:
                case Key.D:
                case Key.S:
                    j1.Megáll();
                    break;
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    j2.Megáll();
                    break;
            }
        }
        /// <summary>
        /// Mozgó játékelemek kép erőforrásainak elérési útját adó metódus.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string[,] Kinézetek(int i)
        {
            switch (i)
            {
                case -1:
                    return new string[,]{ { "sz_n_l1.png", "sz_n_l2.png", "sz_n_l3.png" },
                                                { "sz_n_u1.png", "sz_n_u2.png", "sz_n_u3.png" },
                                                { "sz_n_r1.png", "sz_n_r2.png", "sz_n_r3.png" },
                                                { "sz_n_n1.png", "sz_n_n2.png", "sz_n_n3.png" }};
                    break;
                case 2:
                    return new string[,]{ { "2bm_n_l.png", "2bm_n_l1.png", "2bm_n_l2.png", "2bm_n_l3.png", "2bm_n_l4.png", "2bm_n_l5.png",  "2bm_n_l6.png" },
                                                { "2bm_n_u.png", "2bm_n_u1.png", "2bm_n_u2.png", "2bm_n_u3.png", "2bm_n_u4.png", "2bm_n_u5.png",  "2bm_n_u6.png" },
                                                { "2bm_n_r.png", "2bm_n_r1.png", "2bm_n_r2.png", "2bm_n_r3.png", "2bm_n_r4.png", "2bm_n_r5.png",  "2bm_n_r6.png" },
                                                { "2bm_n.png", "2bm_n1.png", "2bm_n2.png", "2bm_n3.png", "2bm_n4.png", "2bm_n5.png",  "2bm_n6.png" }};
                    break;
                case 1:
                default:
                    return new string[,]{ { "bm_n_l.png", "bm_n_l1.png", "bm_n_l2.png", "bm_n_l3.png", "bm_n_l4.png", "bm_n_l5.png",  "bm_n_l6.png" },
                                                { "bm_n_u.png", "bm_n_u1.png", "bm_n_u2.png", "bm_n_u3.png", "bm_n_u4.png", "bm_n_u5.png",  "bm_n_u6.png" },
                                                { "bm_n_r.png", "bm_n_r1.png", "bm_n_r2.png", "bm_n_r3.png", "bm_n_r4.png", "bm_n_r5.png",  "bm_n_r6.png" },
                                                { "bm_n.png", "bm_n1.png", "bm_n2.png", "bm_n3.png", "bm_n4.png", "bm_n5.png",  "bm_n6.png" }};
                    break;
            }
        }
        /// <summary>
        /// Vasfalak, kőfalak, szörnyek és képességfejlesztők elhelyezése a pályán.
        /// </summary>
        private void PályaGenerálás()
        {
            List<int[]> kőfalakPozíciója = new List<int[]>();
            for (int i = 0; i < this.pályaMéretX; i++)
            {
                for (int j = 0; j < this.pályaMéretY; j++)
                {
                    if (i == 0 || j == 0 || i == pályaMéretX - 1 || j == pályaMéretY - 1 || (i > 1 && j > 1 && i % 2 == 0 && j % 2 == 0))
                    {
                        new VasFal(i, j, this.tér);
                    }
                    else
                    {
                        if (((i != 1 || j != 1) && (i != 1 || j != 2) && (i != 2 || j != 1)) && ((i != pályaMéretX - 2 || j != pályaMéretY - 2) && (i != pályaMéretX - 2 || j != pályaMéretY - 3) && (i != pályaMéretX - 3 || j != pályaMéretY - 2)) && R.Next(10) % 3 > 0)
                        {
                            new KőFal(i, j, this.tér);
                            kőfalakPozíciója.Add(new int[] { i, j });
                        }
                        else if (i != 1 && j != 1 && i != pályaMéretX - 2 && j != pályaMéretY - 2 && R.Next(10) % 7 == 0) //játékosokkal egy oszlopba/sorba ne kerüljenek szörnyek
                        {
                            new Szörny(i, j, this.tér, Kinézetek(-1));
                        }
                    }
                }
            }
            int növelőkSzáma = kőfalakPozíciója.Count / 5;
            for(int i = növelőkSzáma; i > 0; i--) //kőfalak mögé kerülnek a képességfejlesztők
            {
                int index = R.Next(kőfalakPozíciója.Count);
                int rnd = R.Next(4);
                switch (rnd)
                {
                    case 1:
                        new BombaDbNövelő(kőfalakPozíciója[index][0], kőfalakPozíciója[index][1], this.tér);
                        break;
                    case 2:
                        new BombaHatótávNövelő(kőfalakPozíciója[index][0], kőfalakPozíciója[index][1], this.tér);
                        break;
                    case 3:
                        new MozgásiSebességNövelő(kőfalakPozíciója[index][0], kőfalakPozíciója[index][1], this.tér);
                        break;
                }
            }
            
        }
    }
}
