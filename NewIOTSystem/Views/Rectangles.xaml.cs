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

namespace NewIOTSystem
{
    /// <summary>
    /// Rectangles.xaml 的交互逻辑
    /// </summary>
    public partial class Rectangles : UserControl
    {
        public Rectangles()
        {
            InitializeComponent();
        }

        private void rectangle_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("this is a rectangle");
        }

        public void Set_Origin()
        {
            this.line1.Visibility = Visibility.Hidden;
            this.line2.Visibility = Visibility.Hidden;
            this.line3.Visibility = Visibility.Hidden;
            this.line4.Visibility = Visibility.Hidden;
        }

        public void Set_Straight()
        {
            this.line1.Visibility = Visibility.Hidden;
            this.line2.Visibility = Visibility.Hidden;
            this.line3.Visibility = Visibility.Visible;
            this.line4.Visibility = Visibility.Visible;
        }
        public void Set_Cross()
        {
            this.line1.Visibility = Visibility.Visible;
            this.line2.Visibility = Visibility.Visible;
            this.line3.Visibility = Visibility.Hidden;
            this.line4.Visibility = Visibility.Hidden;
 
        }

    }
}
