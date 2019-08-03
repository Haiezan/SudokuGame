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
using System.Windows.Threading;

namespace SudokuGame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int m_iTik = 0;
        int m_iNum = 0;
        static int m_size = 9;
        //List <List<int> > m_data= new List<List<int>>();
        int[,] m_data0 = new int[m_size, m_size];
        int[,] m_data=new int[m_size, m_size];
        List<int[,]> m_allsolver = new List<int[,]>();
        public MainWindow()
        {
            InitializeComponent();
            InitData();
            InitSudoIntoGrid(grid_sudo,m_data);
        }
        private void InitData()
        {

            m_data0[0, 0] = 4;
            m_data0[3, 0] = 6;
            m_data0[6, 0] = 3;
            m_data0[8, 0] = 9;

            m_data0[4, 1] = 2;
            m_data0[8, 1] = 5;

            m_data0[5, 2] = 1;
            m_data0[7, 2] = 7;

            m_data0[5, 3] = 7;

            m_data0[1, 4] = 3;
            m_data0[3, 4] = 8;

            m_data0[0, 5] = 2;
            m_data0[4, 5] = 5;
            m_data0[5, 5] = 4;
            m_data0[8, 5] = 7;

            m_data0[2, 6] = 4;
            m_data0[7, 6] = 5;

            m_data0[0, 8] = 1;
            m_data0[1, 8] = 2;
            m_data0[7, 8] = 9;


        }
        private void InitSudoIntoGrid(Grid grid, int[,] data)
        {
            for (int i = 0; i < m_size; i++)
            {
                for (int j = 0; j < m_size; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Text = data[i,j].ToString();

                    textBox.Margin = new Thickness(0);
                    textBox.VerticalAlignment = VerticalAlignment.Center;
                    textBox.HorizontalAlignment = HorizontalAlignment.Center;
                    textBox.FontSize = 20;

                    if (data[i, j] != 0) textBox.Foreground = Brushes.Red;

                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    grid.Children.Add(textBox);
                    string name = "textBox" + i + "_" + j;
                    grid.RegisterName(name, textBox);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m_data = (int[,])m_data0.Clone();
            solve(m_data, 0);
            showdata(grid_sudo, m_data);
            return;
        }
        private void Button_Click_CalcAll(object sender, RoutedEventArgs e)
        {
            m_data = (int[,])m_data0.Clone();
            if (m_iTik == 0) 
                solveAll(m_data, 0);
            showdata(grid_sudo, m_allsolver[m_iNum]);
            m_iNum++;
            return;
        }
        private void showdata(Grid grid, int[,] data)
        {
            for (int i = 0; i < m_size; i++)
            {
                for (int j = 0; j < m_size; j++)
                {
                    string name = "textBox" + i + "_" + j;
                    TextBox textBox=grid.FindName(name) as TextBox;
                    textBox.Text = data[i, j].ToString();
                }
            }
        }
        private bool solve(int[,] data, int position)
        {
            if (position == 81)
                return true;

            int row = position / 9;
            int col = position % 9;
            if (data[row, col] == 0)
            {
                for (int i = 1; i <= m_size; i++)
                {
                    data[row, col] += i;
                    if (checkValid(data, position))
                    {
                        if (solve(data, position + 1))
                            return true;
                    }  
                    data[row, col] = 0;
                }
            }
            else
            {
                if (solve(data, position + 1))
                    return true;
            }
            return false;
        }
        private bool solveAll(int[,] data, int position)
        {
            if (position == 81)
                return true;

            int row = position / 9;
            int col = position % 9;
            if (data[row, col] == 0)
            {
                for (int i = 1; i <= m_size; i++)
                {
                    data[row, col] += i;
                    if (checkValid(data, position))
                    {
                        if (solveAll(data, position + 1))
                        {
                            saveData(data);
                            m_iTik++;
                            this.textBlock.Text = m_iTik.ToString();
                            this.DoEvents();
                        }
                        //return true;
                    }
                    data[row, col] = 0;
                }
            }
            else
            {
                if (solveAll(data, position + 1))
                    return true;
            }
            return false;
        }
        private bool checkValid(int[,] data,int position)
        {
            int row = position / 9;
            int col = position % 9;
            int target = data[row, col];

            //判断行列
            for (int i = 0; i < m_size; i++)
            {
                if (i != col)
                {
                    if (target == data[row, i])
                        return false;
                }
                if (i != row)
                {
                    if (target == data[i, col])
                        return false;
                }
            }

            //判断宫
            int beginx = row / 3 * 3;
            int beginy = col / 3 * 3;
            for (int i = beginx; i < beginx + 3; i++)
            {
                for (int j = beginy; j < beginy + 3; j++)
                {
                    if (i != row && j != col)
                    {
                        if (target == data[i, j])
                            return false;
                    }
                }
            }

            return true;
        }
        private void saveData(int[,] data)
        {
            int[,] temp_data = (int [,])data.Clone();
            //data.CopyTo(temp_data, 0);
            m_allsolver.Add(temp_data);
        }
        //实时更新窗口
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate (object f)
                {
                    ((DispatcherFrame)f).Continue = false;
                    return null;
                }
                    ), frame);
            Dispatcher.PushFrame(frame);
        }

        
    }
}
