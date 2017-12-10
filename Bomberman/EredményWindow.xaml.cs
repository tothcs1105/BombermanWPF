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
    /// Interaction logic for EredményWindow.xaml
    /// </summary>
    public partial class EredményWindow : Window
    {
        public VégeredményArgs Eredmény { get; private set; }
        public EredményWindow(VégeredményArgs erdmény)
        {
            InitializeComponent();
            this.Eredmény = erdmény;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this.Eredmény;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
