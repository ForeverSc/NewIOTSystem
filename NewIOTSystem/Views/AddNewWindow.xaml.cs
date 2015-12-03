using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// AddNewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewWindow : Window
    {
        private string name=null;
        private string location=null;
        private int n=0;
        public AddNewWindow()
        {
            InitializeComponent();
        }


        //判断是否为2的幂
        private int Pow2n(int n)
        {

            if (n == 2)
            {
                return 0;
            }
            else if (n % 2 == 0)
            {
                n = n / 2;
                Pow2n(n);
            }
            else
            {
                return 1;
            }
            return 0;
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            if (this.tname.Text=="")
            {
                MessageBox.Show("名字不能为空！");
            }
            else if (this.tlocation.Text=="")
            {
                MessageBox.Show("位置不能为空");
            }
            else if (this.tnumbers.Text == "" && Pow2n(Convert.ToInt32(this.tnumbers.Text)) == 1)
            {
                MessageBox.Show("端口数不能为空且必须是2的幂次数，如2，4，8，16!");
            }
            else
            {
                this.name = this.tname.Text;
                this.location = this.tlocation.Text;
                this.n = Convert.ToInt32(this.tnumbers.Text);
                string path=this.location+this.name+".iot";
                FileStream file = new FileStream(path, FileMode.Create);         
                this.Close();
            }
           
        }

        public int ReturnNumbers()
        {
            return this.n;
        }


        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void change_location_button_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if (result==System.Windows.Forms.DialogResult.OK)
            {
                this.tlocation.Text = fbd.SelectedPath;
            }
       
         
        }



     

     
    }
}
