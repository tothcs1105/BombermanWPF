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

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for GameSettingsWindow.xaml
    /// </summary>
    public partial class GameSettingsWindow : Window
    {
        public JátékBeállítás Beállítások { get; private set; }
        public GameSettingsWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Beállítások = new JátékBeállítás();
            DataContext = Beállítások;
        }
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void játékMásodPercek_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //ne lehessen beírni betűt, vagy negítív számot
            e.Handled = !char.IsNumber(e.Text[0]);
        }
    }
}
