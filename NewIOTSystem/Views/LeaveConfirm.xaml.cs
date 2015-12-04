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
using System.Windows.Shapes;

namespace NewIOTSystem.Views
{
    /// <summary>
    /// LeaveConfirm.xaml 的交互逻辑
    /// </summary>
    public partial class LeaveConfirm : Window
    {
        public int flag = 0;
        public LeaveConfirm()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void leave_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;
            this.Close();
        }

        private void saveandleave_button_Click(object sender, RoutedEventArgs e)
        {
            flag = 2;
            this.Close();
        }

        public int ReturnFlag()
        {
            return flag;
        }
    }
}
