using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;


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
            swtichs = new int[floors, n / 2];
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


        //返回输入是否完全，是否能够运行
        public int ReturnIfInportsAllHaveValue()
        {
            int flag = 0;
            for (int i = 0; i < n; i++)
            {
                if (input_list[i] != null)
                {
                    continue;
                }
                else
                {
                    flag = 1;
                }

            }
            return flag;
        }

        //返回输入是否存在相同项
        public int ReturnIfInportsHaveSameValue()
        {
            int compare;
            for (int i = 0; i < n; i++)
            {
                compare = Convert.ToInt32(input_list[i].Text);
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else if (compare == Convert.ToInt32(input_list[j].Text))
                    {
                        return 1;
                    }
                    else
                        continue;
                }
            }
            return 0;
        }

        //返回输入是否超过范围
        public int ReturnIfInportsValueAboveN()
        {
            for (int i = 0; i < n; i++)
            {
                if (Convert.ToInt32(input_list[i].Text) > n - 1)
                {
                    return 1;
                }
                else
                    continue;
            }
            return 0;
        }


        //将所有线路结果显示出来
        public void ShowAllPath()
        {
            try
            {
                int search;
                DataTable datatable = new DataTable();
                List<int> all_search_list = new List<int>();
                for (int j = 0; j < n; j++)
                {
                    search = j;
                    all_search_list.Add(search);
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
                    if (j==0)
                    {
                       DataColumn incolumn = new DataColumn("输入端口");
                    datatable.Columns.Add(incolumn);
                    incolumn.ReadOnly = true;
                    for (int i = 0; i < floors - 1; i++)
                    {
                        DataColumn datacolumn = new DataColumn("第" + (i + 1).ToString() + "步");
                        datatable.Columns.Add(datacolumn);
                        datacolumn.ReadOnly = true;
                    }
                    DataColumn outcolumn = new DataColumn("输出端口");
                    datatable.Columns.Add(outcolumn);
                    outcolumn.ReadOnly = true; 
                    }
                    
                    DataRow datarow = datatable.NewRow();
                    for (int i = 0; i < floors + 1; i++)
                    {
                        datarow[i] = all_search_list[i].ToString();
                    }
                    datatable.Rows.Add(datarow);
                    all_search_list.Clear();
                }
                
                MainWindow.mainwindow.datagrid.ItemsSource = datatable.DefaultView;

            }
            catch (Exception)
            {

                throw;
            }
            
        }


        //清空所有，重置项目
        public void RestartAll()
        {
            for (int i = 0; i < n; i++)
            {
                input_list[i].Text ="";
                output_list[i].Text = "";
            }
            for (int i = 0; i < floors; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inports[i, j] = 0;
                    outports[i, j] = 0;   
                }
            }
            for (int i = 0; i < floors; i++)
            {
                for (int j = 0; j < n/2; j++)
                {
                    swtichs[i, j] = 0;                    
                }
            }


            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < floors; j++)
                {
                    rectangles_list[i][j].Set_Origin();
                }
            }

            if (redline_list != null)
            {
                for (int i = 0; i < redline_list.Count; i++)
                {
                    MainWindow.mainwindow.canvas.Children.Remove(redline_list[i]);
                }
            }



        }


        //显示搜索结果
        public void ShowSearchResult(int search)
        {
            search_list = new List<int>();
            search_list.Add(search);
            for (int i = 0; i < floors - 1; i++)
            {
                if (swtichs[i, search / 2] == 1)//直通
                {
                    search = rtag[i, search];
                    search_list.Add(search);
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
                    search_list.Add(search);
                }
            }
            if (swtichs[floors - 1, search / 2] == 1)
            {
                search_list.Add(search);
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
                search_list.Add(search);
            }

            DataTable datatable = new DataTable();
            DataColumn incolumn = new DataColumn("输入端口");
            datatable.Columns.Add(incolumn);
            incolumn.ReadOnly = true;
            for (int i = 0; i < floors - 1; i++)
            {
                DataColumn datacolumn = new DataColumn("第" + (i + 1).ToString() + "步");
                datatable.Columns.Add(datacolumn);
                datacolumn.ReadOnly = true;
            }
            DataColumn outcolumn = new DataColumn("输出端口");
            datatable.Columns.Add(outcolumn);
            outcolumn.ReadOnly = true;
            DataRow datarow = datatable.NewRow();
            for (int i = 0; i < floors + 1; i++)
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
            inredline.Y1 = (double)input_list[search_list[0]].GetValue(Canvas.TopProperty) + tboxheight / 2;
            inredline.X1 = (double)input_list[search_list[0]].GetValue(Canvas.LeftProperty) + tboxwidth;
            inredline.X2 = inredline.X1 + betweenspace;
            inredline.Y2 = inredline.Y1;
            redline_list.Add(inredline);
            MainWindow.mainwindow.canvas.Children.Add(inredline);

            for (int i = 0; i < floors - 1; i++)
            {
                Line line1 = new Line();
                line1.Stroke = color;
                line1.StrokeThickness = thickness;
                line1.X1 = (double)rectangles_list[search_list[i] / 2][i].GetValue(Canvas.LeftProperty);

                if (search_list[i] % 2 == 0)
                {
                    line1.Y1 = (double)rectangles_list[search_list[i] / 2][i].GetValue(Canvas.TopProperty) + 10;
                }
                else
                {
                    line1.Y1 = (double)rectangles_list[search_list[i] / 2][i].GetValue(Canvas.TopProperty) + 40;
                }



                line1.X2 = (double)rectangles_list[search_list[i] / 2][i].GetValue(Canvas.LeftProperty) + betweenspace;
                if (swtichs[i, search_list[i] / 2] == 1)//直通
                {
                    line1.Y2 = line1.Y1;
                }
                else
                {
                    if (search_list[i] % 2 == 0)
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
                if (search_list[i + 1] % 2 == 0)
                {
                    line2.Y2 = (double)rectangles_list[search_list[i + 1] / 2][i + 1].GetValue(Canvas.TopProperty) + 10;
                }
                else
                {
                    line2.Y2 = (double)rectangles_list[search_list[i + 1] / 2][i + 1].GetValue(Canvas.TopProperty) + 40;
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
            if (swtichs[floors - 1, search_list[floors - 1] / 2] == 1)
            {
                outline1.Y2 = outline1.Y1;
            }
            else
            {
                if (search_list[floors - 1] % 2 == 0)
                {
                    outline1.Y2 = outline1.Y1 + 30;
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

        //保存操作
        public void SaveAll(string filename)
        {
            try
            {
          
                StreamWriter sw = new StreamWriter(filename,true);
                sw.WriteLine(n);
                sw.WriteLine(";");
                foreach (var item in input_list)
                {
                    sw.WriteLine(item.Text);
                }
                sw.WriteLine(";");
                for (int i = 0; i < floors - 1; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        sw.WriteLine(rtag[i, j]);
                    }

                }
                sw.WriteLine(";");
                foreach (var item in output_list)
                {
                    sw.WriteLine(item.Text);
                }
                sw.WriteLine(";");
                for (int i = 0; i < floors; i++)
                {
                    for (int j = 0; j < n / 2; j++)
                    {
                        sw.WriteLine(swtichs[i, j]);
                    }

                }
                sw.WriteLine(";");
                sw.Close();


            }
            catch (Exception)
            {

                throw;
            }
        }

        //打开操作
        public void OpenProject(string filename)
        {
            try
            {
                
                FileStream openfile = new FileStream(filename, FileMode.Open);
                StreamReader sr = new StreamReader(openfile);
                string line = sr.ReadLine();
                int num = Convert.ToInt32(line);
                this.n = num;
                this.input_list = new List<TextBox>();
                this.output_list = new List<TextBox>();
                this.rectangles_list = new List<List<Rectangles>>();
                this.floors = Convert.ToInt32(2 * Math.Log((double)n, 2) - 1);
                this.rtag = new int[floors - 1, n];
                this.inports = new int[floors, n];
                this.outports = new int[floors, n];
                this.swtichs = new int[floors, n / 2];
               
                
                
                for (int i = 0; i < n; i++)
                {
                    input_list.Add(new TextBox());
                    output_list.Add(new TextBox());
                }
                for (int i = 0; i < n / 2; i++)
                {
                    List<Rectangles> rect_list = new List<Rectangles>();
                    for (int j = 0; j < floors; j++)
                    {
                        rect_list.Add(new Rectangles());
                    }
                    rectangles_list.Add(rect_list);
                }
               
                line = sr.ReadLine();
                for (int i = 0; i < n; i++)
                {
                    line = sr.ReadLine();
                    input_list[i].Text = line;
                }
                line = sr.ReadLine();
                for (int i = 0; i < floors - 1; i++)
                {
                    for (int j = 0; j < n ; j++)
                    {
                        line = sr.ReadLine();
                        rtag[i, j] = Convert.ToInt32(line);
                    }
                }
                line = sr.ReadLine();
                for (int i = 0; i < n; i++)
                {
                    line = sr.ReadLine();
                    output_list[i].Text = line;
                }
                line = sr.ReadLine();
                for (int i = 0; i < floors; i++)
                {
                    for (int j = 0; j < n / 2; j++)
                    {
                        line = sr.ReadLine();
                        swtichs[i, j] = Convert.ToInt32(line);
                    }
                }
                sr.Close();  
                this.benes = new Benes(); 
                ShowSave(n, rtag);
                this.view = new CreateView(n, input_list, output_list, rectangles_list);    
                

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ShowSave(int n,int[,] tag)
        {
            this.benes.SetTag(n, tag);
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {

                    this.input_list[i].Width = tboxwidth;
                    this.input_list[i].Height = tboxheight;
                    this.input_list[i].SetValue(Canvas.TopProperty, starttopspace + i * betweenspace);
                    this.input_list[i].SetValue(Canvas.LeftProperty, startleftspace);

                    MainWindow.mainwindow.canvas.Children.Add(this.input_list[i]);
                }
                else
                {

                    this.input_list[i].Width = tboxwidth;
                    this.input_list[i].Height = tboxheight;
                    this.input_list[i].SetValue(Canvas.TopProperty, starttopspace + 30 + 100 * (i - 1) / 2);
                    this.input_list[i].SetValue(Canvas.LeftProperty, startleftspace);

                    MainWindow.mainwindow.canvas.Children.Add(this.input_list[i]);
                }

            }

            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {

                    this.output_list[i].Width = tboxwidth;
                    this.output_list[i].Height = tboxheight;
                    this.output_list[i].SetValue(Canvas.TopProperty, starttopspace + i * betweenspace);
                    this.output_list[i].SetValue(Canvas.LeftProperty, startleftspace + (2 * floors + 2) * 50);


                    MainWindow.mainwindow.canvas.Children.Add(this.output_list[i]);
                }
                else
                {

                    this.output_list[i].Width = tboxwidth;
                    this.output_list[i].Height = tboxheight;
                    this.output_list[i].SetValue(Canvas.TopProperty, starttopspace + 30 + 100 * (i - 1) / 2);
                    this.output_list[i].SetValue(Canvas.LeftProperty, startleftspace + (2 * floors + 2) * 50);

                    MainWindow.mainwindow.canvas.Children.Add(this.output_list[i]);
                }

            }
            //显示每个rectangles
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < floors; j++)
                {
                    rectangles_list[i][j].SetValue(Canvas.TopProperty, starttopspace + 2 * betweenspace * i);
                    rectangles_list[i][j].SetValue(Canvas.LeftProperty, startleftspace + betweenspace * 2 + betweenspace * 2 * j);
                    rectangles_list[i][j].Set_Origin();
                    rectangles_list[i][j].SetValue(Rectangles.ToolTipProperty, j.ToString() + "行" + "," + i.ToString() + "列");
                    MainWindow.mainwindow.canvas.Children.Add(rectangles_list[i][j]);
                }
            }
            //显示初始连接
            for (int i = 0; i < floors - 1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Line line = new Line();
                    line.Stroke = new SolidColorBrush(Colors.Black);
                    if (j % 2 == 0)
                    {
                        line.X1 = (double)rectangles_list[j / 2][i].GetValue(Canvas.LeftProperty) + betweenspace;
                        line.Y1 = (double)rectangles_list[j / 2][i].GetValue(Canvas.TopProperty) + 10;
                        if (tag[i, j] % 2 == 0)
                        {
                            line.X2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.LeftProperty);
                            line.Y2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.TopProperty) + 10;
                        }
                        else
                        {
                            line.X2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.LeftProperty);
                            line.Y2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.TopProperty) + 40;
                        }

                    }
                    else
                    {
                        line.X1 = (double)rectangles_list[j / 2][i].GetValue(Canvas.LeftProperty) + betweenspace;
                        line.Y1 = (double)rectangles_list[j / 2][i].GetValue(Canvas.TopProperty) + 40;
                        if (tag[i, j] % 2 == 0)
                        {
                            line.X2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.LeftProperty);
                            line.Y2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.TopProperty) + 10;
                        }
                        else
                        {
                            line.X2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.LeftProperty);
                            line.Y2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.TopProperty) + 40;
                        }

                    }
                    MainWindow.mainwindow.canvas.Children.Add(line);
                }

            }

            Label input_label = new Label();
            input_label.Content = "输入端：";
            input_label.SetValue(Canvas.LeftProperty, (double)input_list[0].GetValue(Canvas.LeftProperty));
            input_label.SetValue(Canvas.TopProperty, 0.0);
            MainWindow.mainwindow.canvas.Children.Add(input_label);
            Label output_label = new Label();
            output_label.Content = "输出端：";
            output_label.SetValue(Canvas.TopProperty, 0.0);
            output_label.SetValue(Canvas.LeftProperty, (double)output_list[0].GetValue(Canvas.LeftProperty));
            MainWindow.mainwindow.canvas.Children.Add(output_label);


            //显示输入口和输出口的端口号
            for (int i = 0; i < n; i++)
            {
                Label label_in = new Label();
                label_in.Content = i.ToString();
                label_in.SetValue(Canvas.TopProperty, (double)input_list[i].GetValue(Canvas.TopProperty));
                label_in.SetValue(Canvas.LeftProperty, 10.0);
                MainWindow.mainwindow.canvas.Children.Add(label_in);

                Label label_out = new Label();
                label_out.Content = i.ToString();
                label_out.SetValue(Canvas.TopProperty, (double)output_list[i].GetValue(Canvas.TopProperty));
                label_out.SetValue(Canvas.LeftProperty, (double)output_list[i].GetValue(Canvas.LeftProperty) + tboxwidth + 10);
                MainWindow.mainwindow.canvas.Children.Add(label_out);

            }

            //输入口，输出口与Rectangle之间的连接
            for (int i = 0; i < n; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.Black);
                line.X1 = (double)input_list[i].GetValue(Canvas.LeftProperty) + tboxwidth;
                line.Y1 = (double)input_list[i].GetValue(Canvas.TopProperty) + tboxheight / 2;
                line.X2 = line.X1 + betweenspace;
                line.Y2 = line.Y1;
                MainWindow.mainwindow.canvas.Children.Add(line);
            }
            for (int i = 0; i < n; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.Black);
                line.X2 = (double)output_list[i].GetValue(Canvas.LeftProperty);
                line.Y2 = (double)output_list[i].GetValue(Canvas.TopProperty) + tboxheight / 2;
                line.X1 = line.X2 - betweenspace;
                line.Y1 = line.Y2;
                MainWindow.mainwindow.canvas.Children.Add(line);
            }



        }

       

        public void SetRandomInputs()
        {
            int[] ins=new int[n];
            int randomindex,swap;
            Random random=new Random();
            for (int i = 0; i < n; i++)
			{
			    ins[i]=i;
			}

            for (int i = 0; i < n; i++)
			{
			    randomindex=random.Next(0,n);
                swap=ins[i];
                ins[i]=ins[randomindex];
                ins[randomindex]=swap;
			}


            for (int i = 0; i < n; i++)
            {
                input_list[i].Text = ins[i].ToString();
            }
 
        }


        public int ReturnN()
        {
            return this.n;
        }


    }
}
