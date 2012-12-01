using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dictionary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void mouse_clicked(object sender, RoutedEventArgs e) {
            Button b = (Button)sender;
            b.Content = "clicked";
            Dictionary.DictionaryDisplay dv = new Dictionary.DictionaryDisplay("define");

            Grid.SetColumn(dv,1);
            this.grid.Children.Add(dv);
        }
    }
    
}
