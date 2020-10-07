
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Symplex
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            autoChoise.Content = "    Досчитать\n автоматически";

            InitializeNewTask(new Task(@"C:\Users\Odman\Documents\Visual Studio 2019\Symplex\Symplex\Test Examples\Input.txt"));
        }

        public List<SimplexTable> Tables;
        Task task;
        SimplexTableView? View = null;

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewTask_Click(object sender, RoutedEventArgs e)
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
            newTaskWindow.Owner = this;
            newTaskWindow.ShowDialog();
        }

        private void OpenTask_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            openDlg.InitialDirectory = dir.Parent.Parent.FullName + "\\Test Examples";
            openDlg.ShowDialog();

            if (openDlg.FileName != "")
            {
                DeleteContent();

                InitializeNewTask(new Task(openDlg.FileName));
            }
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();

            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            saveDlg.InitialDirectory = dir.Parent.Parent.FullName + "\\Test Examples";
            saveDlg.ShowDialog();
            if (saveDlg.FileName != "") 
            {
                string[] str = new string[10 + task.m];
                str[0] = "Функция:";
                for (int i = 0; i < task.n; i++)
                {
                    str[1] += task.func[i];
                    if (i != task.n - 1)
                        str[1] += " ";
                }
                str[2] = "";
                str[3] = "Размеры:";
                str[4] = task.m + " " + task.n;
                str[5] = "";
                str[6] = "Базис:";
                if (task.IBazis == true)
                {
                    str[7] = "-1";
                }
                else 
                {
                    for (int i = 0; i < task.bazis.Count; i++)
                    {
                        str[7] += task.bazis[i] + 1;
                        if (i != task.bazis.Count - 1)
                            str[7] += " ";
                    }
                }
                    
                str[8] = "";
                str[9] = "Матрица:";
                for (int i = 0; i < task.m; i++)
                    for (int j = 0; j < task.n; j++)
                    {
                        str[10 + i] += task.matr[i, j] ;
                        if (j != task.n - 1)
                            str[10 + i] += " ";
                    }

                File.WriteAllText(saveDlg.FileName, "");
                for(int i = 0; i < str.Length; i++)
                    File.AppendAllText(saveDlg.FileName, str[i] + "\n");

            }
        }

        public void InitializeNewTask(Task task)
        {
            this.task = task;

            TaskGrid.Height = (task.m + 1) * 35;
            SolutionGrid.Margin = new Thickness(10, TaskGrid.Margin.Top + TaskGrid.Height + 20, 0, 0);
            
            function.Content = Task.prettyFunc(task.func);
            taskMatrix.Content = Task.prettyMatr(task.matr);

            AnswerGrid.Margin = new Thickness(function.Content.ToString().Length *10, 50, 0, 0);

            Tables = new List<SimplexTable>();
            SimplexTable simplexTable = new SimplexTable(task);

            if (simplexTable.table == null) return;

            View = new SimplexTableView();
            View.CreateNewSimplexTable(simplexTable);
            Tables.Add(simplexTable);
            Tables[Tables.Count - 1].PrintTable();
            View.ColourHint(null, View.SolutionGrids[0]);

            if (simplexTable.IsSolution() == -1) //нашли решение
            {
                //View.BlockTable(View.selectedTable);
                answer.Content = simplexTable.GetSolution(task);
            }
            else if (simplexTable.IsSolution() >= 0) //ребро на бесконеч
            {
                //View.BlockTable(View.selectedTable);
                answer.Content = String.Format("Ребро уходит на бесконечность");
            }
        }

        private void autoChoise_Click(object sender, RoutedEventArgs e)
        {
            int[] a; int b;
            SimplexTable simplexTable = Tables[Tables.Count - 1];

            while (simplexTable.IsSolution() == -2)
            {
                a = simplexTable.ChoiceOfOporniyEl();
                if (a[0] == -1) { 
                    MessageBox.Show("Такого не может быть (autoChoise_Click fail)");
                    return;
                }

                View.ColourHint(a[0],a[1],View.SolutionGrids[View.selectedTable]); 
                simplexTable = NextStep(simplexTable.ChoiceOfOporniyEl(), simplexTable);
            }
            
        }

        public SimplexTable NextStep(int[] oporniy_element, SimplexTable simplexTable)
        {
            if (simplexTable.IsSolution() == -1 || simplexTable.IsSolution() >= 0) { }
            else
            {
                View.BlockTable(View.selectedTable);
                simplexTable = simplexTable.Transformation(oporniy_element[0], oporniy_element[1]);

                View.CreateNewSimplexTable(simplexTable);
                Tables.Add(simplexTable);

                View.ColourHint(null, View.SolutionGrids[View.selectedTable]);

                if (simplexTable.IsSolution() == -1) //нашли решение
                {
                    //View.BlockTable(View.selectedTable);
                    answer.Content = simplexTable.GetSolution(task);
                }
                else if (simplexTable.IsSolution() >= 0) //ребро на бесконеч
                {
                    //View.BlockTable(View.selectedTable);
                    answer.Content = String.Format("Ребро уходит на бесконечность");
                }
            }

            return simplexTable;
        }

        public void NextStepClick(object sender, RoutedEventArgs e)
        {
            int[] a = View.GetSelected();
            if (a != null && Tables[View.selectedTable].CheckOporniy(a[0],a[1])) NextStep(a, Tables[Tables.Count - 1]);
        }

        private void PrevStepClick(object sender, RoutedEventArgs e)
        {
            if (View.selectedTable > 0)
            {
                answer.Content = "";
                View.DeleteLastTable();
                Tables.RemoveAt(Tables.Count - 1);
                View.UnBlockTable(View.selectedTable);
            }
        }

        public void DeleteContent()
        {
            answer.Content = "";
            if (View != null)
                foreach (Grid grid in View.SolutionGrids)
                    SolutionGrid.Children.Remove(grid);
        }

        
    }
}
