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
using Microsoft.Win32;



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
        public string currentfilename;


        public MainWindow()
        {
            InitializeComponent();
            mainwindow = this;
        }


        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            if (this.search_tbox.Text == "")
            {
                MessageBox.Show("没有输入搜索值，请输入：");
            }
            else
            {
                view.ShowSearchResult(Convert.ToInt32(this.search_tbox.Text));
                this.highlight_button.IsEnabled = true;
                this.returntoblack_button.IsEnabled = true;
            }
            this.tableitem_searchresult.Focus();
        }

        private void returntoblack_button_Click(object sender, RoutedEventArgs e)
        {
            if (view == null)
            {
                MessageBox.Show("错误操作");
            }
            else
            {
                view.ReturnToBlack();
            }

        }

        private void highlight_button_Click(object sender, RoutedEventArgs e)
        {
            if (view == null)
            {
                MessageBox.Show("错误操作");
            }
            else
            {
                view.ChangeSearchPathColortoRed();
            }

        }

        private void addnewproject_Click(object sender, RoutedEventArgs e)
        {
               addnewwindow = new AddNewWindow();
               addnewwindow.ShowDialog();
               if (addnewwindow.ReturnNumbers()!=0)
               {
                     view = new RectanglesAndInputs(addnewwindow.ReturnNumbers());
                view.Show_All();
                this.currentfilename = addnewwindow.ReturnPath();
                this.tname.Text = addnewwindow.ReturnName();
                this.tnumbers.Text = addnewwindow.ReturnNumbers().ToString();
               }
              
        }

        private void run_button_Click(object sender, RoutedEventArgs e)
        {
            if (view.ReturnIfInportsAllHaveValue() == 1)
            {
                MessageBox.Show("输入不完全");
            }
            else if (view.ReturnIfInportsHaveSameValue() == 1)
            {
                MessageBox.Show("输入中含有相同项，请检查后重新输入：");
            }
            else if (view.ReturnIfInportsValueAboveN() == 1)
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
            OpenFileDialog ofg = new OpenFileDialog();

            if (ofg.ShowDialog() == true)
            {
                view = new RectanglesAndInputs();
                view.OpenProject(ofg.FileName);
                view.GotInputAndRun();

                this.tname.Text = ofg.SafeFileName;
                this.tnumbers.Text = view.ReturnN().ToString();
            }

        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentfilename != null)
            {
                view.SaveAll(currentfilename);
            }
            MessageBox.Show("保存成功");
        }

 
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (view != null)
            {
                LeaveConfirm leaveconfirmwindow = new LeaveConfirm();
                leaveconfirmwindow.ShowDialog();
                if (leaveconfirmwindow.ReturnFlag() == 1)
                {
                    e.Cancel = false;
                }
                else if (leaveconfirmwindow.ReturnFlag() == 2)
                {
                    if (currentfilename != null)
                    {
                        view.SaveAll(currentfilename);
                    }
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }


        }

        private void random_button_Click(object sender, RoutedEventArgs e)
        {
            if (view == null)
            {
                MessageBox.Show("请先创建项目，再设置随机输入");
            }
            else
            {
                view.SetRandomInputs();
            }
        }

        private void print_button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(this.canvas, "Print Test");
            }
        }

        private void closeproject_button_Click(object sender, RoutedEventArgs e)
        {
            view = null;
            currentfilename = null;
            this.canvas.Children.Clear();
            this.datagrid.ItemsSource = null;
            this.datagrid.Items.Clear();

        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            if (view != null)
            {
                LeaveConfirm leaveconfirmwindow = new LeaveConfirm();
                leaveconfirmwindow.ShowDialog();
                if (leaveconfirmwindow.ReturnFlag() == 1)
                {
                    this.Close();
                }
                else if (leaveconfirmwindow.ReturnFlag() == 2)
                {
                    if (currentfilename != null)
                    {
                        view.SaveAll(currentfilename);
                    }
                    this.Close();
                }
                else
                {
                    leaveconfirmwindow.Close();
                }
               
            }
            else
            {
                this.Close();
            }
        }

        private void information_button_Click(object sender, RoutedEventArgs e)
        {
            InformationWindow informationwindow = new InformationWindow();
            informationwindow.ShowDialog();
        }

        private void changesettings_button_Click(object sender, RoutedEventArgs e)
        {

            this.tnumbers.IsReadOnly = false;
            this.savechange_button.IsEnabled = true;
        }

        private void savechange_button_Click(object sender, RoutedEventArgs e)
        {
            this.canvas.Children.Clear();
            view = new RectanglesAndInputs(Convert.ToInt32(this.tnumbers.Text));
            view.Show_All();
            
        }

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {
            this.tableitem_change_settings.Focus();
        }
    }
}
