using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;
using System.Diagnostics;

namespace anchor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Entry> entries = new ObservableCollection<Entry>();

        private Settings settings = new Settings();

        public MainWindow()
        {
            InitializeComponent();
        }

        ~MainWindow()
        {
            DataFile.Write("entries.bin", entries);
            DataFile.Write("settings.bin", settings);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadEntries();
            loadSettings();
        }

        private void restartApache()
        {

        }

        private void loadSettings()
        {
            try
            {
                settings = DataFile.Read<Settings>("settings.bin");
                if (settings == null)
                {
                    settings = new Settings();
                }
            }
            catch (IOException)
            {

            }
        }

        private void loadEntries()
        {
            try
            {
                entries = DataFile.Read<ObservableCollection<Entry>>("entries.bin");
                if (entries == null)
                {
                    entries = new ObservableCollection<Entry>();
                }
                lstSites.DataContext = entries;
            }
            catch (IOException)
            {

            }
        }

        private void btnAddHostToggle_Click(object sender, RoutedEventArgs e)
        {
            lstSites.SelectedIndex = -1;
            if (pnlAddHost.Visibility == System.Windows.Visibility.Collapsed)
            {
                pnlAddHost.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnlAddHost.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btnAddHost_Click(object sender, RoutedEventArgs e)
        {
            string hostName = txtAddHostName.Text.Trim();
            string path = txtAddHostPath.Text.Trim();

            entries.Add(new Entry { Name = hostName, Path = path });
            lstSites.DataContext = entries;

            txtAddHostName.Text = "";
            txtAddHostPath.Text = "";
            pnlAddHost.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnHostOpen_Click(object sender, RoutedEventArgs e)
        {
            Entry entry = (Entry)lstSites.SelectedItem;
            if (entry != null)
            {
                Process.Start("http://" + entry.Name + ".dev/");
            }
        }

        private void btnHostBrowse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHostDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
