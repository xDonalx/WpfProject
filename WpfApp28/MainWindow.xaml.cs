using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp28
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadProcesses();
            PlaceholderText.Visibility = Visibility.Visible;
        }

        private void LoadProcesses()
        {
            ProcessListBox.Items.Clear(); 
            string input = SearchTextBox.Text.Trim().ToLower();

            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                try
                {
                    if (string.IsNullOrEmpty(input) || process.ProcessName.ToLower().Contains(input))
                    {
                        string displayName = $"{process.ProcessName} (ID: {process.Id})";
                        ProcessListBox.Items.Add(displayName);
                    }
                }
                catch
                {
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string processName = SearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(processName))
            {
                try
                {
                    Process.Start(processName);
                    LoadProcesses();
                }
                catch
                {
                    MessageBox.Show("Ошибка запуска процесса.");
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessListBox.SelectedItem != null)
            {
                var selectedText = ProcessListBox.SelectedItem.ToString();
                int processId = int.Parse(selectedText.Split(new[] { " (ID: " }, StringSplitOptions.None)[1].TrimEnd(')'));

                try
                {
                    var processToKill = Process.GetProcessById(processId);
                    processToKill.Kill(); 
                    LoadProcesses();
                }
                catch
                {
                    MessageBox.Show("Ошибка остановки процесса.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите процесс, чтобы его остановить.");
            }
        }

        private void ProcessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                PlaceholderText.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProcesses();
        }
    }
}