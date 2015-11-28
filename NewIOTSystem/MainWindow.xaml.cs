using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewIOTSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainwindow;

        private List<List<Rectangle>> rectangles_list;
        private List<TextBox> input_list;
        private List<TextBox> output_list;
        int n = 16;
        int floors;
        int[,] rtag;
        int[,] inports;
        int[,] outports;
        int[,] swtichs;
        Benes benes;
        CreateView view;

        public MainWindow()
        {
            InitializeComponent();
            mainwindow = this;
            Show_All();
        }
        
        private void Show_All()
        {

            input_list = new List<TextBox>();
            output_list = new List<TextBox>();
            rectangles_list = new List<List<Rectangle>>();
            floors = Convert.ToInt32(2 * Math.Log((double)n,2) - 1);
            rtag = new int[floors - 1, n];
            inports = new int[floors, n];
            outports = new int[floors, n];
            swtichs = new int[floors, n];
            benes = new Benes();
            view = new CreateView(n,input_list,output_list,rectangles_list);
            view.Show_All_InputBoxs();
            view.Show_All_Rectangles();
            benes.SetTag(n,rtag);
            view.SetOriginalContacts(rtag);
            view.Show_All_OutputBoxs();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < n; i++)
            {
                inports[0,i] = Convert.ToInt32(input_list[i].Text);
            }
            benes.SetBenes(n, floors, rtag, inports, outports, swtichs);


            for (int i = 0; i < n/2; i++)
            {
                for (int j = 0; j < floors; j++)
                {

                    view.SetRectanglesStatus(swtichs[j, i], rectangles_list[i][j]);

                }
                
            }
        }



    }
}
