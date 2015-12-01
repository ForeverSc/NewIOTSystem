using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Data;
using System.Windows.Data;




namespace NewIOTSystem.ViewModels
{
    public class AddListView
    {

        public void AddListViewItem(int n)
        {

            DataTable dt = new DataTable();
            for (int i = 0; i < n; i++)
            {
               
            }
            dt.Columns.Add(new DataColumn("Column1"));
            dt.Columns.Add(new DataColumn("Column2"));
            dt.Rows.Add("12", "21");


            MainWindow.mainwindow.datagrid.ItemsSource = dt.DefaultView;
         
           
           
          
      
        }

    }
}
