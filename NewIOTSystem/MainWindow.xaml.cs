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
using NewIOTSystem.ViewModels;
using System.Data;


namespace NewIOTSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainwindow;
        public RectanglesAndInputs view;

        public MainWindow()
        {
            InitializeComponent();
            mainwindow = this;
        }

       



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            view.GotInputAndRun();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            view = new RectanglesAndInputs(8);
            view.Show_All();
            
            
           
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            view.ShowSearchResult(Convert.ToInt32(this.search_tbox.Text));
                   
        }

        private void returntoblack_button_Click(object sender, RoutedEventArgs e)
        {
            view.ReturnToBlack();
        }

        private void highlight_button_Click(object sender, RoutedEventArgs e)
        { 
            view.ChangeSearchPathColortoRed();
        }
    }
}
