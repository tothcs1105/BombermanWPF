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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            GameSettingsWindow gSettings = new GameSettingsWindow()
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            this.Visibility = Visibility.Collapsed; //amíg fut a "főmenü" ne látszódjon
            if (gSettings.ShowDialog() == true)
            {
                GameWindow gWin = new GameWindow(gSettings.Beállítások.Másodpercek, gSettings.Beállítások.PályaMéretX, gSettings.Beállítások.PályaMéretY, gSettings.Beállítások.ElemMéret, gSettings.Beállítások.JátékosNevek);
                gWin.ShowDialog();
            }
            this.Visibility = Visibility.Visible;
        }

        private void about_Button_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aWin = new AboutWindow()
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            aWin.ShowDialog();
        }
    }
}
