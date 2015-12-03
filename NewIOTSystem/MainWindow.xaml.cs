﻿using System;
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
         
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            if (this.search_tbox.Text=="")
            {
                MessageBox.Show("没有输入搜索值，请输入：");
            }
            else
            {
                view.ShowSearchResult(Convert.ToInt32(this.search_tbox.Text));            
            }
                   
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
                this.run_button.IsEnabled = true;
            }
            
        }

        private void run_button_Click(object sender, RoutedEventArgs e)
        {
            if (view.ReturnIfInportsAllHaveValue()==1)
            {
                MessageBox.Show("输入不完全");
            }
            else if (view.ReturnIfInportsHaveSameValue()==1)
            {
                MessageBox.Show("输入中含有相同项，请检查后重新输入：");
            }
            else if (view.ReturnIfInportsValueAboveN()==1)
            {
                MessageBox.Show("输入中存在超过范围的项");
            }
            else
            {
                view.GotInputAndRun();
                view.ShowAllPath();
            }
        }

        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            view.RestartAll();
        }

        private void open_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
