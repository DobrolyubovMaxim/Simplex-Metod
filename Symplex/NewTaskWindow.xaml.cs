using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RationalLibrary;
using UnicodeDataLib;

namespace Symplex
{
    /// <summary>
    /// Логика взаимодействия для NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        public int n;
        public int m;

        TextBox[,] matrix;
        Label[,] labelsX;
        Label[,] labelsPlus;
        Label[] labelsRavno;

        Label[] labelsFunc;
        TextBox[] function;
        Label[] funcPlus;
        ToggleButton[] bazis;

        public NewTaskWindow()
        {
            InitializeComponent();

            n = 5;
            m = 1;
            matrix = new TextBox[16, 16];
            labelsX = new Label[16, 16];
            labelsPlus = new Label[16, 16];
            labelsRavno = new Label[16];
            labelsFunc = new Label[16];
            function = new TextBox[16];
            funcPlus = new Label[16];
            bazis = new ToggleButton[16];
            var rand = new Random();

            //инициализация лэйблов и текстбоксов матрицы
            for (int i = 0; i < 16; i++) 
            {
                labelsRavno[i] = new Label();
                labelsRavno[i].HorizontalAlignment = HorizontalAlignment.Left;
                labelsRavno[i].VerticalAlignment = VerticalAlignment.Top;
                labelsRavno[i].Height = 25;
                labelsRavno[i].Width = 30;
                labelsRavno[i].Content = "=";
                labelsRavno[i].Margin = new Thickness(n * 50-15, i * 40 - 4, 0, 0);
                matrixGrid.Children.Add(labelsRavno[i]);
                for (int j = 0; j < 16; j++)
                {
                    labelsX[i, j] = new Label();
                    labelsX[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    labelsX[i, j].VerticalAlignment = VerticalAlignment.Top;
                    labelsX[i, j].Height = 25;
                    labelsX[i, j].Width = 30;
                    labelsX[i, j].Content = String.Format("x{0}", UnicodeData.DownIndexDigits[j + 1]);
                    labelsX[i, j].Margin = new Thickness(j * 50 + 26, i * 40 - 4, 0, 0);
                    matrixGrid.Children.Add(labelsX[i, j]);

                    labelsPlus[i, j] = new Label();
                    labelsPlus[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    labelsPlus[i, j].VerticalAlignment = VerticalAlignment.Top;
                    labelsPlus[i, j].Height = 25;
                    labelsPlus[i, j].Width = 30;
                    labelsPlus[i, j].Content = "+";
                    labelsPlus[i, j].Margin = new Thickness(j * 50 + 35, i * 40 - 4, 0, 0);
                    matrixGrid.Children.Add(labelsPlus[i, j]);

                    matrix[i, j] = new TextBox();
                    matrix[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    matrix[i, j].VerticalAlignment = VerticalAlignment.Top;
                    matrix[i, j].Height = 20;
                    matrix[i, j].Width = 30;
                    matrix[i, j].Focusable = true;
                    matrix[i, j].MouseEnter += textBoxEnter;
                    matrix[i, j].DragEnter += textBoxEnter;
                    matrix[i, j].TouchEnter += textBoxEnter;
                    matrix[i, j].PreviewDragEnter += textBoxEnter;

                    matrix[i, j].Text = String.Format("{0:N0}", rand.Next(-9, 10));
                    matrix[i, j].Margin = new Thickness(j * 50, i * 40, 0, 0);
                    matrixGrid.Children.Add(matrix[i, j]);
                }
            }

            //инициализация лэйблов и текстбоксов функции
            for (int i = 0; i < 16; i++)
            {
                labelsFunc[i] = new Label();
                labelsFunc[i].HorizontalAlignment = HorizontalAlignment.Left;
                labelsFunc[i].VerticalAlignment = VerticalAlignment.Top;
                labelsFunc[i].Height = 25;
                labelsFunc[i].Width = 30;
                labelsFunc[i].Content = String.Format("x{0}", UnicodeData.DownIndexDigits[i + 1]);
                labelsFunc[i].Margin = new Thickness(i * 50 + 46, 16, 0, 0);//(i <= 9) ? new Thickness(i * 50 + 38, 16, 0, 0) : new Thickness(450 + 38 + (i-9)*55, 16, 0, 0);
                MainGrid.Children.Add(labelsFunc[i]);

                function[i] = new TextBox();
                function[i].HorizontalAlignment = HorizontalAlignment.Left;
                function[i].VerticalAlignment = VerticalAlignment.Top;
                function[i].Height = 20;
                function[i].Width = 30;
                function[i].Text = String.Format("{0:N0}", rand.Next(-9,10));
                function[i].Margin = new Thickness(i * 50 + 20, 20, 0, 0);//(i <= 9) ? new Thickness(i * 50 + 20, 20, 0, 0): new Thickness(450 + 20 + (i-9) * 55, 20, 0, 0);
                MainGrid.Children.Add(function[i]);

                funcPlus[i] = new Label();
                funcPlus[i].HorizontalAlignment = HorizontalAlignment.Left;
                funcPlus[i].VerticalAlignment = VerticalAlignment.Top;
                funcPlus[i].Height = 25;
                funcPlus[i].Width = 30;
                funcPlus[i].Content = "+";
                funcPlus[i].Margin = new Thickness(i * 50 + 55, 16, 0, 0);//(i < 9) ? new Thickness(i * 50 + 51, 16, 0, 0) : new Thickness(450 + 60 + (i-9) * 55, 16, 0, 0);
                MainGrid.Children.Add(funcPlus[i]);

                bazis[i] = new ToggleButton();
                bazis[i].HorizontalAlignment = HorizontalAlignment.Left;
                bazis[i].VerticalAlignment = VerticalAlignment.Top;
                bazis[i].Height = 20;
                bazis[i].Width = 50;
                bazis[i].Margin = new Thickness(i * 50 + 20, -10, 0, 0);
                bazis[i].Content = String.Format("x{0}", UnicodeData.DownIndexDigits[i+1]);
                bazis[i].IsChecked = false;
                MainGrid.Children.Add(bazis[i]);
            }

            setAreaVisible(m, n);
            buttonMinus2.IsEnabled = false;
            Grid.SetZIndex(nSlider, 1); //на передний план
            Grid.SetZIndex(mSlider, 1);
            Grid.SetZIndex(buttonOK, 1);
            Grid.SetZIndex(buttonOK, 1);
            //MainGrid.Height = 

        }
        private void textBoxEnter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void buttonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (n >= 14) buttonPlus.IsEnabled = false;
            if (n <= 3) buttonMinus.IsEnabled = true;
            setAreaVisible(m, ++n);
            int i = (n == 10) ? 54 : 50;
            nSlider.Margin = new Thickness(nSlider.Margin.Left + i, nSlider.Margin.Top, 0, 0);
            OK_Cancel_area.Margin = new Thickness(OK_Cancel_area.Margin.Left + i, OK_Cancel_area.Margin.Top, 0, 0);
            FuncRect.Width += i;
        }

        private void buttonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (n >= 14) buttonPlus.IsEnabled = true;
            if (n <= 3) buttonMinus.IsEnabled = false;
            setAreaVisible(m, --n);
            int i = (n == 9) ? 54 : 50;
            nSlider.Margin = new Thickness(nSlider.Margin.Left - i, nSlider.Margin.Top, 0, 0);
            OK_Cancel_area.Margin = new Thickness(OK_Cancel_area.Margin.Left - i, OK_Cancel_area.Margin.Top, 0, 0);
            FuncRect.Width -= i;
        }

        private void buttonPlus2_Click(object sender, RoutedEventArgs e)
        {
            if (m >= 15) buttonPlus2.IsEnabled = false;
            if (m <= 2) buttonMinus2.IsEnabled = true;
            setAreaVisible(++m, n);
            mSlider.Margin = new Thickness(mSlider.Margin.Left, mSlider.Margin.Top + 40, 0, 0);
            OK_Cancel_area.Margin = new Thickness(OK_Cancel_area.Margin.Left, OK_Cancel_area.Margin.Top + 40, 0, 0);
        }

        private void buttonMinus2_Click(object sender, RoutedEventArgs e)
        {
            if (m >= 14) buttonPlus2.IsEnabled = true;
            if (m <= 2) buttonMinus2.IsEnabled = false;
            setAreaVisible(--m, n);
            mSlider.Margin = new Thickness(mSlider.Margin.Left, mSlider.Margin.Top - 40, 0, 0);
            OK_Cancel_area.Margin = new Thickness(OK_Cancel_area.Margin.Left, OK_Cancel_area.Margin.Top - 40, 0, 0);
        }

        private void setAreaVisible(int m, int n)
        {
            for (int i = 0; i < 16; i++)
            {
                if (i < m)
                {
                    labelsRavno[i].Margin = new Thickness(n*50-15, labelsRavno[i].Margin.Top,0,0);
                    labelsRavno[i].Visibility = Visibility.Visible;
                }
                else 
                {
                    labelsRavno[i].Visibility = Visibility.Collapsed; 
                }
                for (int j = 0; j < 16; j++)
                {
                    if (i < m && j < n)
                        labelsX[i, j].Visibility = Visibility.Visible;
                    else labelsX[i, j].Visibility = Visibility.Collapsed;

                    if (i<m && j<n+1) 
                        matrix[i, j].Visibility = Visibility.Visible;
                    else matrix[i, j].Visibility = Visibility.Collapsed;

                    if (i < m && j < n - 1)
                        labelsPlus[i, j].Visibility = Visibility.Visible;
                    else labelsPlus[i, j].Visibility = Visibility.Collapsed;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (i < n)
                {
                    function[i].Visibility = Visibility.Visible;
                    funcPlus[i].Visibility = Visibility.Visible;
                    labelsFunc[i].Visibility = Visibility.Visible;
                    bazis[i].Visibility = Visibility.Visible;
                    
                }
                else
                {
                    function[i].Visibility = Visibility.Collapsed;
                    funcPlus[i].Visibility = Visibility.Collapsed;
                    labelsFunc[i].Visibility = Visibility.Collapsed;
                    bazis[i].Visibility = Visibility.Collapsed;
                    bazis[i].IsChecked = false;
                }

            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Rational[] rationalFunc = new Rational[n + 1] ;

            List<int> intBazis = new List<int>();

            for (int j = 0; j < n; j++)
            {
                if (Rational.TryParse(function[j].Text, out rationalFunc[j]));
                else
                {
                    MessageBox.Show(String.Format("Проверьте правильность {0} коофициента функции", j+1));
                    return;
                }
                if (minMax.SelectedIndex == 1) rationalFunc[j] = -rationalFunc[j];
                if ((bool)bazis[j].IsChecked)
                    intBazis.Add(j);
            }

            if (intBazis.Count != m)
            {
                MessageBox.Show(m<=4?String.Format("Пожалуйста, выберите {0} базисные переменные", m): String.Format("Пожалуйста, выберите {0} базисных переменных", m));
                return;
            }

            Rational[,] rationalMatrix = new Rational[m, n + 1];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Rational.TryParse(matrix[i, j].Text, out rationalMatrix[i, j])) ;
                    else
                    {
                        MessageBox.Show(String.Format("Проверьте правильность  коофициента матрицы[{0}][{1}]", i+1,j+1));
                        return;
                    }
                }
                if (Rational.TryParse(matrix[i, n].Text, out rationalMatrix[i, n]))
                    if(rationalMatrix[i, n]<0)
                        for (int j = 0; j < n+1; j++)
                            rationalMatrix[i, j] *= -1;

            }

            rationalFunc[n] = 0;
            Task newTask = new Task(rationalMatrix, rationalFunc, intBazis);

            ((MainWindow)Application.Current.MainWindow).DeleteContent();
            Close();
            ((MainWindow)Application.Current.MainWindow).InitializeNewTask(newTask);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
