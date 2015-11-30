using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
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
            swtichs = new int[floors, n];
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




    }
}
