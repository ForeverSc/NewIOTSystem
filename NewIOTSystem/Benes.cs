using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewIOTSystem
{
   public class Benes
    {
        //二进制循环右移，逆均匀洗牌
        private int BinaryCirculation(double n, int i)
        {
            if (i >= n)
            {
                i -= (int)n;
            }
            double z = Math.Log(n, 2);
            string istr = Convert.ToString(i, 2);
            while (istr.Length < z)
            {
                istr = "0" + istr;
            }
            istr = istr.Substring((int)z - 1, 1) + istr.Substring(0, (int)z - 1);
            return Convert.ToInt32(istr, 2);
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

        //一半网络初始状态设置
        private void Inverse(int[,] tag, int startnum, int n, int k)
        {
            if (n == 2)
            {
                return;
            }
            if (Pow2n(n) == 0)
            {
                for (int i = startnum; i < startnum + n; i++)
                {
                    tag[k, i] = BinaryCirculation(n, i - startnum) + startnum;
                }
            }
            Inverse(tag, startnum, n / 2, k + 1);
            Inverse(tag, startnum + n / 2, n / 2, k + 1);
        }
        //整个初始网络
        private void AllInverse(int[,] tag, int n)
        {
            int j = 0;
            double k = 2 * Math.Log(n, 2) - 2;
            for (int i = (int)k / 2; i < k; i++)
            {
                j = (int)k - i - 1;
                for (int z = 0; z < n; z++)
                {
                    tag[i, tag[j, z]] = z;//对称操作
                }
            }
        }

        public void SetTag(int n,int[,] rtag)
        {
            Inverse(rtag, 0, n, 0);
            AllInverse(rtag, n);   
        }

        public void SetBenes(int n,int floors,int[,] rtag, int[,] inports,int[,] outports,int[,] switchs)
        {
            for (int i = 0; i < n; i++)
            {
                outports[floors - 1, inports[0, i]] = i;
            }
            SetSwitch(rtag, inports, outports, switchs, 0, floors-1, n, 0);
        }

        
        private void Test()
        {


            int[,] rtag = new int[4, 8];
            Inverse(rtag, 0, 8, 0);
            AllInverse(rtag, 8);
            int[,] inports = new int[5, 8];
            int[,] outports = new int[5, 8];
            int[,] switchs = new int[5, 4];

            //设置输出端口的值
            for (int i = 0; i < 8; i++)
            {
                outports[4, inports[0, i]] = i;
            }//提到递归外


            SetSwitch(rtag, inports, outports, switchs, 0, 7, 16, 0);


            //for (int i = 0; i < 6; i++)
            //{
            //    for (int j = 0; j < 16; j++)
            //    {
            //        Console.Write(outports[i, j]);
            //        Console.Write(",");
            //    }
            //    Console.WriteLine();

            //}

            Console.WriteLine();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Console.Write(switchs[i, j]);
                    Console.Write(",");
                }
                Console.WriteLine();

            }



        }



        //链接算法
        private void Connect(int[,] inports, int[,] outports, int[,] switchs, int k, int z, int m, int start)
        {
            int i = m;
            int j = 0;
            while (true)
            {
                if (outports[z, i] % 2 == 0)//直通
                {//开关设置有误
                    if (switchs[k, outports[z, i] / 2 + start / 2] == 0)
                    {
                        switchs[k, outports[z, i] / 2 + start / 2] = 1;
                    }
                    else
                        return;

                }
                else
                {
                    if (switchs[k, outports[z, i] / 2 + start / 2] == 0)
                    {
                        switchs[k, outports[z, i] / 2 + start / 2] = 2;//交叉
                    }
                    else
                        return;

                }

                j = outports[z, i];
                if (j % 2 == 0)//偶数则它的相对为
                {
                    j += 1;
                }
                else
                {
                    j -= 1;
                }

                if (inports[k, j + start] % 2 == 0)//交叉
                {
                    if (switchs[z, inports[k, j + start] / 2 + start / 2] == 0)
                    {
                        switchs[z, inports[k, j + start] / 2 + start / 2] = 2;
                    }
                    else
                        return;


                }
                else
                {
                    if (switchs[z, inports[k, j + start] / 2 + start / 2] == 0)
                    {
                        switchs[z, inports[k, j + start] / 2 + start / 2] = 1;//直通
                    }
                    else
                        return;
                }

                i = inports[k, j + start];
                if (i % 2 == 0)//偶数则它的相对为
                {
                    i += 1;
                }
                else
                {
                    i -= 1;
                }
                i = i + start;

            }
        }





        //开关设置算法
        //k是inports开始的，z是从outports开始的
        private void SetSwitch(int[,] tag, int[,] inports, int[,] outports, int[,] switchs, int k, int z, int n, int m)
        {

            for (int i = m; i < n + m; i++)
            {
                Connect(inports, outports, switchs, k, z, i, m);
            }

            if (n == 2)
            {
                return;
            }

            //因为前面的开关状态已经变换，所以接口的位置会被调换
            //tag表示的仍是上一级直接相对的下一个，而不是本身接口的那一个
            //重新分配写错了
            //重新分配inports和outports


            //检查为何当n=4时，重置inport和outport出错
            int[] newinports = new int[n];
            int[] newoutports = new int[n];

            for (int i = 0; i < n; i++)
            {
                newinports[i] = inports[k, i + m];//i+m
            }
            for (int i = 0; i < n; i++)
            {
                newoutports[i] = outports[z, i + m];//i+m
            }
            int swap = 0;
            for (int i = m / 2; i < n / 2 + m / 2; i++)
            {
                if (switchs[k, i] == 2)
                {

                    swap = newinports[i * 2 - m];
                    newinports[i * 2 - m] = newinports[i * 2 + 1 - m];
                    newinports[i * 2 + 1 - m] = swap;
                }
                else
                    continue;
            }
            for (int i = m / 2; i < n / 2 + m / 2; i++)
            {
                if (switchs[z, i] == 2)
                {

                    swap = newoutports[i * 2 - m];
                    newoutports[i * 2 - m] = newoutports[i * 2 + 1 - m];
                    newoutports[i * 2 + 1 - m] = swap;
                }
                else
                    continue;
            }
            //重新设置
            //k=1时重新设置发生错误
            //下半部分没有设置
            for (int i = m; i < n + m; i++)
            {
                inports[k + 1, tag[k, i - m] + m] = newinports[i - m];
            }
            for (int i = m; i < n + m; i++)
            {
                outports[z - 1, i] = newoutports[tag[z - 1, i - m]];
            }

            //创建两个临时数组用来存储接口中的状态

            int[] inarray1 = new int[n / 2];
            int[] outarray1 = new int[n / 2];
            for (int i = m; i < n / 2 + m; i++)
            {
                inarray1[i - m] = inports[k + 1, i];
            }
            for (int i = m; i < n / 2 + m; i++)
            {
                outarray1[i - m] = outports[z - 1, i];
            }

            int[] inarray2 = new int[n / 2];
            int[] outarray2 = new int[n / 2];
            for (int i = m + n / 2; i < m + n; i++)
            {
                inarray2[i - n / 2 - m] = inports[k + 1, i];
            }
            for (int i = m + n / 2; i < m + n; i++)
            {
                outarray2[i - n / 2 - m] = outports[z - 1, i];
            }

            //重新将分部分设置为从0开

            //上半部分
            //将这段写成一段测试，重新计算
            for (int i = m; i < m + n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (outarray1[j] == outports[z, inarray1[i - m] + m])
                    {
                        inports[k + 1, i] = j;
                        break;
                    }
                }
            }
            for (int i = m; i < m + n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (inarray1[j] == inports[k, outarray1[i - m] + m])
                    {
                        outports[z - 1, i] = j;
                        break;
                    }
                }
            }
            //下半部分
            for (int i = m + n / 2; i < m + n; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (outarray2[j] == outports[z, inarray2[i - n / 2 - m] + m])
                    {
                        inports[k + 1, i] = j;
                        break;
                    }
                }
            }
            for (int i = m + n / 2; i < m + n; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (inarray2[j] == inports[k, outarray2[i - n / 2 - m] + m])
                    {
                        outports[z - 1, i] = j;
                        break;
                    }
                }
            }
            //然后分成两半递归
            SetSwitch(tag, inports, outports, switchs, k + 1, z - 1, n / 2, m);
            SetSwitch(tag, inports, outports, switchs, k + 1, z - 1, n / 2, m + n / 2);
        }



    }
}
