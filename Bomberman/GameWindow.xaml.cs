using Bomberman.Játék.Keret;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bomberman.Events;

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Keret játék;
        List<Key> lenyomottGombok; //azért, hogy működjön a multiplayer irányítás
        public GameWindow(int játékIdő, int pályaMéretX, int pályaMéretY, int játékelemMéret, string[] játékosok)
        {
            InitializeComponent();
            this.lenyomottGombok = new List<Key>();
            this.játék = new Keret(pályaMéretX, pályaMéretY, játékelemMéret, játékIdő);
            this.játék.JátékVége += JátékVége;
            this.játék.Futtatás(játékosok);
        }

        private void JátékVége(object sender, VégeredményArgs e)
        {
            if (e.Döntetlen)
                MessageBox.Show("Döntetlen!", "Eredmény", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                EredményWindow eWin = new EredményWindow(e);
                eWin.ShowDialog();
            }
            if (DialogResult == null) //Azért kell, mert ha esetleg egyik játékos halála után meghal a másik is akkor az is beállítaná ezt true-ra, de az ablak már bezárult ezért exceptiont dobna. (az a játékos nyer aki tovább bírja a pályán = később hal meg)
                DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = játék.VM;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(!lenyomottGombok.Contains(e.Key)) //azért, hogy működjön a multiplayer irányítás
                lenyomottGombok.Add(e.Key);

            foreach(Key akt in lenyomottGombok) //azért, hogy működjön a multiplayer irányítás
                játék.Lenyom(akt);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            lenyomottGombok.Remove(e.Key);
            játék.Felenged(e.Key);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.játék.Megállítás();
            this.játék = null;
        }
    }
}
