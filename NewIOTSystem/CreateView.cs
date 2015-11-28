using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;



namespace NewIOTSystem
{
    public class CreateView
    {
        private int n = 0;
        private int rectHeight = 60;
        private int rectWidth = 60;
        private int rectsLeftSpace = 80;
        private int rectsTopSpace = 80;
        private List<List<Rectangle>> rectangles_list;
        private List<TextBox> input_list;
        private List<TextBox> output_list;
        public CreateView(int n,List<TextBox> inputlist,List<TextBox> outputlist,List<List<Rectangle>> rectlist )
        {
            this.n = n;
            this.input_list = inputlist;
            this.output_list = outputlist;
            this.rectangles_list = rectlist;
        }

        public void Show_All_InputBoxs()
        {
            for (int i = 0; i < n; i++)
            {
                TextBox textbox = new TextBox();
                textbox.Width = rectWidth;
                textbox.Height = rectHeight / 3;
                textbox.SetValue(Canvas.LeftProperty, (double)10);
                textbox.SetValue(Canvas.TopProperty, (double)((i + 2) * (rectsTopSpace / 2)));
                input_list.Add(textbox);
                MainWindow.mainwindow.canvas.Children.Add(textbox);

            }
        }

        public void Show_All_OutputBoxs()
        {
            double outBoxLeftSpace = (double)(170 + (n / 2 + 1) * (rectWidth));
            for (int i = 0; i < n; i++)
            {
                TextBox outbox = new TextBox();
                outbox.Width = rectWidth;
                outbox.Height = rectHeight / 3;
                outbox.SetValue(Canvas.LeftProperty, outBoxLeftSpace);
                outbox.SetValue(Canvas.TopProperty, (double)((i + 2) * (rectsTopSpace / 2)));
                output_list.Add(outbox);
                MainWindow.mainwindow.canvas.Children.Add(outbox);

            }
        }


        public void Show_All_Rectangles()
        {
            
            int floors = Convert.ToInt32(2 * Math.Log((double)n,2) - 1);
            for (int i = 0; i < n/2; i++)
            {
                List<Rectangle> rect_list = new List<Rectangle>();
                for (int j = 0; j < floors; j++)
                {
                    rect_list.Add(new Rectangle());
                }
                rectangles_list.Add(rect_list);
            }
            for (int i = 0; i < n/2; i++)
            {
                for (int j = 0; j < floors; j++)
                {
                    ShowRectanglesInCanvas(rectangles_list[i][j], i, j);
                }
            }



        }
        //在画布上显示一个矩形
        private void ShowRectanglesInCanvas(Rectangle rect, int i, int j)
        {
            rect.Width = rectWidth;
            rect.Height = rectHeight;
            rect.Fill = new SolidColorBrush(Colors.LightBlue);
            rect.SetValue(Canvas.TopProperty, (double)((i + 1) * rectsTopSpace));//j
            rect.SetValue(Canvas.LeftProperty, (double)((j + 1) * rectsLeftSpace));//i
            MainWindow.mainwindow.canvas.Children.Add(rect);
        }


        //设置初始Benes网络的连接
        public void SetOriginalContacts(int[,] tag)
        {
            int[] taglist = new int[n];
            int floors = Convert.ToInt32(2 * Math.Log((double)n, 2) - 1);
            for (int i = 0; i <floors-1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    taglist[j] = tag[i, j];
                }
                CreateContactsBetweenTwoFloors(taglist,n, i);
            }

        }
        //每一层级间的端口线段连接
        public void CreateContactsBetweenTwoFloors(int[] tag,int n, int floors)
        {

            for (int j = 0; j < n; j++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.Black);
                line.SetValue(Canvas.TopProperty, (double)rectangles_list[j / 2][floors].GetValue(Canvas.TopProperty));
                line.SetValue(Canvas.LeftProperty, (double)rectangles_list[j / 2][floors].GetValue(Canvas.LeftProperty));
                line.X1 = rectangles_list[j / 2][floors].Width;
                line.Y1 = ((j % 2 == 0) ? (rectangles_list[j / 2][floors].Height / 6) : (5 * rectangles_list[j / 2][floors].Height / 6));

                //下一层 
                line.X2 = (double)rectangles_list[tag[j] / 2][floors + 1].GetValue(Canvas.LeftProperty)
                    - (double)rectangles_list[j / 2][floors].GetValue(Canvas.LeftProperty);

                line.Y2 = (double)rectangles_list[tag[j] / 2][floors + 1].GetValue(Canvas.TopProperty)
                    - (double)rectangles_list[j / 2][floors].GetValue(Canvas.TopProperty)
                    + ((tag[j] % 2 == 0) ? (rectangles_list[tag[j] / 2][floors + 1].Height / 6) : (5 * rectangles_list[tag[j] / 2][floors + 1].Height / 6));

                MainWindow.mainwindow.canvas.Children.Add(line);
            }
        }

        //设置直通和交换的rectangle中的不同显示
        public void SetRectanglesStatus(int flag, Rectangle rect)
        {

            Line line1 = new Line();
            line1.Stroke = new SolidColorBrush(Colors.Black);
            line1.SetValue(Canvas.TopProperty, (double)rect.GetValue(Canvas.TopProperty));
            line1.SetValue(Canvas.LeftProperty, (double)rect.GetValue(Canvas.LeftProperty));

            Line line2 = new Line();
            line2.Stroke = new SolidColorBrush(Colors.Black);
            line2.SetValue(Canvas.TopProperty, (double)rect.GetValue(Canvas.TopProperty));
            line2.SetValue(Canvas.LeftProperty, (double)rect.GetValue(Canvas.LeftProperty));

            //交换
            if (flag == 2)
            {
                line1.X1 = 0;
                line1.Y1 = rect.Height / 6;
                line1.X2 = rect.Width;
                line1.Y2 = (rect.Height * 5) / 6;

                line2.X1 = 0;
                line2.Y1 = (rect.Height * 5) / 6;
                line2.X2 = rect.Width;
                line2.Y2 = rect.Height / 6;

                MainWindow.mainwindow.canvas.Children.Add(line1);
                MainWindow.mainwindow.canvas.Children.Add(line2);


            }
            //直通
            else if (flag == 1)
            {


                line1.X1 = 0;
                line1.Y1 = rect.Height / 6;
                line1.X2 = rect.Width;
                line1.Y2 = rect.Height / 6;

                line2.X1 = 0;
                line2.Y1 = (rect.Height * 5) / 6;
                line2.X2 = rect.Width;
                line2.Y2 = (rect.Height * 5) / 6;
                MainWindow.mainwindow.canvas.Children.Add(line1);
                MainWindow.mainwindow.canvas.Children.Add(line2);

            }
        }









    }
}
