using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Bomberman.Játék.JátékTér
{
    public enum Megy { balra, fel, jobbra, le }
    abstract class MozgóJátékElem : Játékelem
    {
        public override int ZIndex
        {
            get
            {
                return 3;
            }
        }
        /// <summary>
        /// Megfelelő mozgási animáció betöltéséhez, jelenlegi mozgási irány.
        /// 0 : bal
        /// 1 : fel
        /// 2 : jobb
        /// 3 : le
        /// </summary>
        protected Megy mozgásiIrány;
        /// <summary>
        /// Jelenleg megjelenítendő alak indexe.
        /// </summary>
        private int jelenlegiAlak;
        /// <summary>
        /// Mozgási animáció során megjelenítendő kép erőforrások elérési útjaik.
        /// [0][] : balra
        /// [1][] : fel
        /// [2][] : jobbra
        /// [3][] : le
        /// </summary>
        private string[,] mozgásiAlakok;
        /// <summary>
        /// Mozgási alakok darabszáma egy irányba. Így bármennyi mozgázianimáció kép lehetséges, lényeg hogy minden irányba azonos dbszámú legyen.
        /// </summary>
        private int mozgásiAlakokDb;
        /// <summary>
        /// Mozgási sebesség, amely képességfejlsztővel növelhető.
        /// </summary>
        protected int mozgásiSebesség;
        public MozgóJátékElem(int x, int y, JátékTér tér, string[,] mozgásiAlakok) : base(x, y, tér)
        {
            this.mozgásiSebesség = 1;
            this.jelenlegiAlak = 0;
            this.mozgásiAlakok = mozgásiAlakok;
            this.mozgásiAlakokDb = mozgásiAlakok.GetLength(1);
        }
        /// <summary>
        /// Játéktéren való áthelyezése adott pozícióról újra.
        /// </summary>
        /// <param name="újX"></param>
        /// <param name="újY"></param>
        /// <returns>Sikeres volt-e az áthelyezés.</returns>
        protected bool ÁtHelyez(int újX, int újY)
        {
            List<Játékelem> seged = tér.MegadottHelyenLévők(újX, újY);
            for (int i = 0; i < seged.Count; i++)
            {
                seged[i].Ütközés(this);
                this.Ütközés(seged[i]);
            }
            if (seged.FindIndex(x => !x.Áthatolható) == -1) //léphet az új koordinátákra
            {
                this.X = újX;
                this.Y = újY;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Játékelem alakjának alaphelyzetbe állítása. És megállítása.
        /// </summary>
        public virtual void Megáll()
        {
            //azért hogy ne mozogjanak a játékelemek mikor szüneteltetve van a játék
            if (this.játékelemIdő.IsEnabled)
            {
                this.jelenlegiAlak = 0;
                this.Alak = this.mozgásiAlakok[(int)this.mozgásiIrány, this.jelenlegiAlak];
            }
        }
        /// <summary>
        /// Mozgás során váltogatja az alakot, mozgási iránynak megfelelően.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MozogAnimáció_Tick(object sender, EventArgs e)
        {
            //azért hogy ne mozogjanak a játékelemek mikor szüneteltetve van a játék
            if (this.játékelemIdő.IsEnabled)
            {
                if (this.jelenlegiAlak < mozgásiAlakokDb - 1)
                {
                    this.jelenlegiAlak++;
                }
                else
                {
                    this.jelenlegiAlak = 1;
                }
                this.Alak = this.mozgásiAlakok[(int)this.mozgásiIrány, this.jelenlegiAlak];
                this.PrivMegy();
            }
        }
        /// <summary>
        /// Ezt a metódust hívja meg a timer időközönként. Minden leszármazottnak implementálnia kell.
        /// </summary>
        protected abstract void PrivMegy();
    }
}
