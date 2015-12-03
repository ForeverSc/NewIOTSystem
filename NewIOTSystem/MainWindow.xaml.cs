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
using NewIOTSystem.Views;
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
        public AddNewWindow addnewwindow;


        public MainWindow()
        {
            InitializeComponent();
            mainwindow = this;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            view.GotInputAndRun();
            view.ShowAllPath();
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

        private void addnewproject_Click(object sender, RoutedEventArgs e)
        {
            addnewwindow = new AddNewWindow();
            addnewwindow.ShowDialog();
            if (addnewwindow.ReturnNumbers()!=0)
            {
                view = new RectanglesAndInputs(addnewwindow.ReturnNumbers());
                view.Show_All();
            }
            
        }
    }
}
