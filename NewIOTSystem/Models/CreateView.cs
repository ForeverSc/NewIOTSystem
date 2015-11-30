using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;




namespace NewIOTSystem
{
    public class CreateView
    {
        private int n = 0;
        private int floors = 0;
        //betweenspace=rectanglewidth=rectangleheight
        private double betweenspace = 50;
        private double startleftspace = 20;
        private double starttopspace = 20;
        private double tboxwidth = 50;
        private double tboxheight = 20;

        private List<List<Rectangles>> rectangles_list;
        private List<TextBox> input_list;
        private List<TextBox> output_list;
        public CreateView(int n,List<TextBox> inputlist,List<TextBox> outputlist,List<List<Rectangles>> rectlist )
        {
            this.n = n;
            this.floors = Convert.ToInt32(2 * Math.Log((double)n, 2) - 1);
            this.input_list = inputlist;
            this.output_list = outputlist;
            this.rectangles_list = rectlist;
        }

        public void Show_Inputs()
        {
            for (int i = 0; i < n; i++)
            {
                if (i%2==0)
                {
                    TextBox tbox = new TextBox();
                    tbox.Width = tboxwidth;
                    tbox.Height = tboxheight;
                    tbox.SetValue(Canvas.TopProperty, starttopspace + i * betweenspace);
                    tbox.SetValue(Canvas.LeftProperty, startleftspace);
                    input_list.Add(tbox);
                    MainWindow.mainwindow.canvas.Children.Add(tbox);
                }
                else
                {
                    TextBox tbox = new TextBox();
                    tbox.Width = tboxwidth;
                    tbox.Height = tboxheight;
                    tbox.SetValue(Canvas.TopProperty, starttopspace + 30+100*(i-1)/2);
                    tbox.SetValue(Canvas.LeftProperty, startleftspace);
                    input_list.Add(tbox);
                    MainWindow.mainwindow.canvas.Children.Add(tbox);
                }
                
            }
        }
        public void Show_outputs()
        {
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {
                    TextBox tbox = new TextBox();
                    tbox.Width = tboxwidth;
                    tbox.Height = tboxheight;
                    tbox.SetValue(Canvas.TopProperty, starttopspace + i * betweenspace);
                    tbox.SetValue(Canvas.LeftProperty, startleftspace+(2*floors+2)*50);
                    output_list.Add(tbox);
                    MainWindow.mainwindow.canvas.Children.Add(tbox);
                }
                else
                {
                    TextBox tbox = new TextBox();
                    tbox.Width = tboxwidth;
                    tbox.Height = tboxheight;
                    tbox.SetValue(Canvas.TopProperty, starttopspace + 30 + 100 * (i - 1) / 2);
                    tbox.SetValue(Canvas.LeftProperty, startleftspace + ( 2 * floors + 2) * 50);
                    output_list.Add(tbox);
                    MainWindow.mainwindow.canvas.Children.Add(tbox);
                }

            }
 
        }

        public void Show_All_RectanglesAndConnections(int[,] tag)
        {            
            for (int i = 0; i < n / 2; i++)
            {
                List<Rectangles> rect_list = new List<Rectangles>();
                for (int j = 0; j < floors; j++)
                {
                    rect_list.Add(new Rectangles());
                }
                rectangles_list.Add(rect_list);
            }
            //显示每个rectangles
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < floors; j++)
                {
                    rectangles_list[i][j].SetValue(Canvas.TopProperty, starttopspace +2*betweenspace * i);
                    rectangles_list[i][j].SetValue(Canvas.LeftProperty, startleftspace + betweenspace * 2 + betweenspace * 2 * j);
                    rectangles_list[i][j].Set_Origin();
                   MainWindow.mainwindow.canvas.Children.Add(rectangles_list[i][j]);
                } 
            }
            //显示初始连接
            for (int i = 0; i < floors-1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Line line=new Line();
                    line.Stroke = new SolidColorBrush(Colors.Black);
                    if (j%2==0)
                    {
                        line.X1=(double)rectangles_list[j/2][i].GetValue(Canvas.LeftProperty)+betweenspace;
                        line.Y1 = (double)rectangles_list[j / 2][i].GetValue(Canvas.TopProperty)+10;
                        if (tag[i,j]%2==0)
                        {
                            line.X2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.LeftProperty);
                            line.Y2 = (double)rectangles_list[tag[i, j] / 2][i+1].GetValue(Canvas.TopProperty) + 10;
                        }
                        else
                        {
                            line.X2 = (double)rectangles_list[tag[i, j] / 2][i + 1].GetValue(Canvas.LeftProperty);
                            line.Y2 = (double)rectangles_list[tag[i, j] / 2][i+1].GetValue(Canvas.TopProperty) + 40;
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
            
            
        }
        public void Set_Status(int flag,Rectangles rectangle)
        {        
            if (flag==1)//直通
            {
                rectangle.Set_Straight();
            }
            else//交叉
            {
                rectangle.Set_Cross();
            }
           
        }


    }
}
