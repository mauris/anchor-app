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

namespace anchor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Entry> entries;

        private Settings settings;

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
            settings = DataFile.Read<Settings>("settings.bin");
            if (settings == null)
            {
                settings = new Settings();
            }
        }

        private void loadEntries()
        {
            entries = DataFile.Read<List<Entry>>("entries.bin");
            if (entries == null)
            {
                entries = new List<Entry>();
            }
            lstSites.DataContext = entries;
        }

        private void btnAddHostToggle_Click(object sender, RoutedEventArgs e)
        {
            if (pnlAddHost.Visibility == System.Windows.Visibility.Collapsed)
            {
                pnlAddHost.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnlAddHost.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
