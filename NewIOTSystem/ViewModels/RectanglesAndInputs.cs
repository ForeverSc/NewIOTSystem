using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;


namespace NewIOTSystem.ViewModels
{
    public class RectanglesAndInputs
    {
        private List<List<Rectangles>> rectangles_list;
        private List<TextBox> input_list;
        private List<TextBox> output_list;
        private int n;
        private int floors;
        private int[,] rtag;
        private int[,] inports;
        private int[,] outports;
        private int[,] swtichs;
        private Benes benes;
        private CreateView view;
        private List<int> search_list;
        private List<Line> redline_list;
        private double betweenspace = 50;
        private double startleftspace = 40;
        private double starttopspace = 20;
        private double tboxwidth = 50;
        private double tboxheight = 20;
        private SolidColorBrush color = new SolidColorBrush(Colors.Red);
        private double thickness = 2;

        public RectanglesAndInputs()
        {
            
        }

        public RectanglesAndInputs(int n)
        {
            this.n = n;
            input_list = new List<TextBox>();
            output_list = new List<TextBox>();
            rectangles_list = new List<List<Rectangles>>();
            floors = Convert.ToInt32(2 * Math.Log((double)n, 2) - 1);
            rtag = new int[floors - 1, n];
            inports = new int[floors, n];
            outports = new int[floors, n];
            swtichs = new int[floors, n/2];
            benes = new Benes();
            view = new CreateView(n, input_list, output_list, rectangles_list);
        }


        //显示inputbox,rectangles,outputboxs
        public void Show_All()
        {
                   
            benes.SetTag(n, rtag);  
            view.Show_Inputs();
            view.Show_All_RectanglesAndConnections(rtag);
            view.Show_outputs();
            view.Show_OtherConnections();
         
        }

        //接收输入并且运行
        public void GotInputAndRun()
        {
            for (int i = 0; i < n; i++)
            {
                inports[0, i] = Convert.ToInt32(input_list[i].Text);
            }
            benes.SetBenes(n, floors, rtag, inports, outports, swtichs);


            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < floors; j++)
                {
                    view.Set_Status(swtichs[j, i], rectangles_list[i][j]);
                }

            }

            for (int i = 0; i < n; i++)
            {
                output_list[i].Text = outports[floors - 1, i].ToString();
            }
        }


        //将所有线路结果显示出来
        public void ShowAllPath()
        {
            int search;
            DataTable datatable = new DataTable();
            for (int z = 0; z < n; z++)
            {
                List<int> all_search_list = new List<int>();
                all_search_list.Add(z);
                search = z;
                for (int i = 0; i < floors - 1; i++)
                {
                    if (swtichs[i, search / 2] == 1)//直通
                    {
                        search = rtag[i, search];
                        all_search_list.Add(search);
                    }
                    else//交叉
                    {
                        if (search % 2 == 0)
                        {
                            search = rtag[i, search + 1];
                        }
                        else
                        {
                            search = rtag[i, search - 1];
                        }
                        all_search_list.Add(search);
                    }
                }
                if (swtichs[floors - 1, search / 2] == 1)
                {
                    all_search_list.Add(search);
                }
                else
                {
                    if (search % 2 == 0)
                    {
                        search += 1;
                    }
                    else
                    {
                        search -= 1;
                    }
                    all_search_list.Add(search);
                }
                if (search==0)
                {
                 DataColumn incolumn = new DataColumn("输入端口");
                 incolumn.ReadOnly = true;
                datatable.Columns.Add(incolumn);
                for (int j = 0; j < floors - 1; j++)
                {
                    DataColumn datacolumn = new DataColumn("第" + (j + 1).ToString() + "步");
                    datatable.Columns.Add(datacolumn);
                    datacolumn.ReadOnly = true;
                }
                DataColumn outcolumn = new DataColumn("输出端口");
                datatable.Columns.Add(outcolumn);
                outcolumn.ReadOnly = true;
                }
             
                DataRow datarow = datatable.NewRow();
                for (int j = 0; j < floors + 1; j++)
                {
                    datarow[j] = all_search_list[j].ToString();
                }
                datatable.Rows.Add(datarow);               
            }
            MainWindow.mainwindow.datagrid.ItemsSource = datatable.DefaultView;   
        }



        //显示搜索结果
        public void ShowSearchResult(int search)
        {
                search_list = new List<int>();
                search_list.Add(search);
                for (int i = 0; i < floors-1; i++)
                {
                    if (swtichs[i,search/2]==1)//直通
                    {
                        search = rtag[i, search];
                        search_list.Add(search);
                    }
                    else//交叉
                    {
                        if (search%2==0)
                        {
                            search = rtag[i, search + 1];
                        }
                        else
                        {
                            search = rtag[i, search - 1];
                        }
                        search_list.Add(search);
                    }            
                }
                if (swtichs[floors-1,search/2]==1)
                {        
                     search_list.Add(search);
                }
                else
                {
                    if (search % 2 == 0)
                    {
                        search+=1;
                    }
                    else
                    {
                        search-=1;
                    }
                    search_list.Add(search);
                }

            DataTable datatable=new DataTable();
            DataColumn incolumn=new DataColumn("输入端口");
            datatable.Columns.Add(incolumn);
            incolumn.ReadOnly = true;
            for (int i = 0; i < floors-1; i++)
			{
			    DataColumn datacolumn=new DataColumn("第"+(i+1).ToString()+"步");
                datatable.Columns.Add(datacolumn);
                datacolumn.ReadOnly = true;
			}
             DataColumn outcolumn=new DataColumn("输出端口");
            datatable.Columns.Add(outcolumn);
            outcolumn.ReadOnly = true;
            DataRow datarow = datatable.NewRow();
            for (int i = 0; i < floors+1; i++)
            {
                datarow[i] = search_list[i].ToString();
            }
            datatable.Rows.Add(datarow);
            MainWindow.mainwindow.search_datagrid.ItemsSource = datatable.DefaultView;
                
            
 
        }

        //根据搜索结果，将这条线路变成红色
        public void ChangeSearchPathColortoRed()
        {
            redline_list = new List<Line>();
            Line inredline = new Line();
            inredline.Stroke = color;
            inredline.StrokeThickness = thickness;
            inredline.Y1=(double)input_list[search_list[0]].GetValue(Canvas.TopProperty)+tboxheight/2;
            inredline.X1 = (double)input_list[search_list[0]].GetValue(Canvas.LeftProperty) + tboxwidth;
            inredline.X2 = inredline.X1 + betweenspace;
            inredline.Y2 = inredline.Y1;
            redline_list.Add(inredline);
            MainWindow.mainwindow.canvas.Children.Add(inredline);

            for (int i = 0; i < floors-1; i++)
            {
                Line line1 = new Line();
                line1.Stroke = color;
                line1.StrokeThickness = thickness;
                line1.X1 = (double)rectangles_list[search_list[i]/2][i].GetValue(Canvas.LeftProperty);
                
                    if (search_list[i]%2==0)
                    {
                        line1.Y1 = (double)rectangles_list[search_list[i] / 2][i].GetValue(Canvas.TopProperty) + 10;
                    }
                    else
                    {
                        line1.Y1 = (double)rectangles_list[search_list[i] / 2][i].GetValue(Canvas.TopProperty) + 40;
                    }
                
               
               
                line1.X2 = (double)rectangles_list[search_list[i]/2][i].GetValue(Canvas.LeftProperty) + betweenspace;
                if (swtichs[i,search_list[i]/2]==1)//直通
                {                   
                    line1.Y2 = line1.Y1;
                }
                else
                {
                    if (search_list[i]%2==0)
                    {
                        line1.Y2 = line1.Y1 + 30;
                    }
                    else
                    {
                        line1.Y2 = line1.Y1 - 30;
                    }
                    
                }
                redline_list.Add(line1);
                MainWindow.mainwindow.canvas.Children.Add(line1);

                Line line2 = new Line();
                line2.Stroke = color;
                line2.StrokeThickness = thickness;
                line2.X1 = line1.X2;
                line2.Y1 = line1.Y2;
                line2.X2 = line2.X1 + betweenspace;
                if (search_list[i+1]%2==0)
                {
                     line2.Y2=(double)rectangles_list[search_list[i+1]/2][i+1].GetValue(Canvas.TopProperty)+10;
                }
                else
                {
                     line2.Y2 = (double)rectangles_list[search_list[i + 1]/2][i + 1].GetValue(Canvas.TopProperty)+40;
                }
                redline_list.Add(line2);
                MainWindow.mainwindow.canvas.Children.Add(line2);
                
            }

            Line outline1 = new Line();
            outline1.Stroke = color;
            outline1.StrokeThickness = thickness;
            outline1.X1 = redline_list[2 * (floors - 1)].X2;
            outline1.Y1 = redline_list[2 * (floors - 1)].Y2;
            outline1.X2 = outline1.X1 + betweenspace;
            if (swtichs[floors-1,search_list[floors-1]/2]==1)
            {
                outline1.Y2 = outline1.Y1;
            }
            else
            {
                if (search_list[floors-1]%2==0)
                {
                    outline1.Y2=outline1.Y1+30;
                }
                else
                {
                    outline1.Y2 = outline1.Y1 - 30;
                }
            }
            redline_list.Add(outline1);
            MainWindow.mainwindow.canvas.Children.Add(outline1);

            Line outline2 = new Line();
            outline2.Stroke = color;
            outline2.StrokeThickness = thickness;
            outline2.X1 = redline_list[2 * (floors - 1) + 1].X2;
            outline2.Y1 = redline_list[2 * (floors - 1) + 1].Y2;
            outline2.X2 = outline2.X1 + betweenspace;
            outline2.Y2 = outline2.Y1;
            redline_list.Add(outline2);
            MainWindow.mainwindow.canvas.Children.Add(outline2);
            
            
 
        }

        //取消红色显示
        public void ReturnToBlack()
        {
            for (int i = 0; i < redline_list.Count; i++)
            {
                MainWindow.mainwindow.canvas.Children.Remove(redline_list[i]);
            }
        }


    }
}
