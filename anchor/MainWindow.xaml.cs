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
using anchor.Drivers;

namespace anchor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Entry> entries = new ObservableCollection<Entry>();

        private Settings settings = new Settings();

        HostEditor hostEditor = new HostEditor(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32\\drivers\\etc\\hosts"));

        Driver driver;

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
            settings.WampServerPath = @"C:\wamp";
            driver = new WampServer(settings.WampServerPath);
        }

        private void restartApache()
        {
            driver.restartServer();
            Process.Start(new ProcessStartInfo("ipconfig", "/flushdns"));
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
                if (entries.Count > 0)
                {

                    lblNoSites.Visibility = System.Windows.Visibility.Collapsed;
                    lstSites.Visibility = System.Windows.Visibility.Visible;
                }
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
                txtAddHostName.Focus();
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

            if (hostName == "")
            {
                MessageBox.Show("You must enter a name for the host", "Add Host Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (path == "")
            {
                MessageBox.Show("You must enter a folder path to be the root folder of the host", "Add Host Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                entries.Add(new Entry { Name = hostName, Path = path });
                hostEditor.add(hostName + ".dev");
                hostEditor.update();
                ConfigWriter writer = new ConfigWriter(System.IO.Path.Combine(driver.ApacheConfigPath, "anchor.conf"));
                writer.write(driver.ServerRootPath, entries.ToList());
                this.restartApache();
                lstSites.DataContext = entries;

                lblNoSites.Visibility = System.Windows.Visibility.Collapsed;
                lstSites.Visibility = System.Windows.Visibility.Visible;

                txtAddHostName.Text = "";
                txtAddHostPath.Text = "";
                pnlAddHost.Visibility = System.Windows.Visibility.Collapsed;
            }
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
            Entry entry = (Entry)lstSites.SelectedItem;
            if (entry != null)
            {
                Process.Start(entry.Path);
            }
        }

        private void btnHostDelete_Click(object sender, RoutedEventArgs e)
        {
            Entry entry = (Entry)lstSites.SelectedItem;
            if (entry != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete \"" + entry.Name + ".dev\"?", "Delete Host", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    hostEditor.remove(entry.Name + ".dev");
                    hostEditor.update();
                    entries.Remove(entry);
                    ConfigWriter writer = new ConfigWriter(System.IO.Path.Combine(driver.ApacheConfigPath, "anchor.conf"));
                    writer.write(driver.ServerRootPath, entries.ToList());
                    this.restartApache();
                    lstSites.DataContext = entries;
                    if (entries.Count == 0)
                    {

                        lblNoSites.Visibility = System.Windows.Visibility.Visible;
                        lstSites.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }
        }

        private void btnAddHostCancel_Click(object sender, RoutedEventArgs e)
        {
            pnlAddHost.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            btnSettings.Content = "set";
            pnlContent.Visibility = System.Windows.Visibility.Collapsed;
            pnlSettings.Visibility = System.Windows.Visibility.Visible;
            txtWampServerPath.Text = settings.WampServerPath;
        }

        private void closeSettingsPanel()
        {
            btnSettings.Content = "";
            pnlContent.Visibility = System.Windows.Visibility.Visible;
            pnlSettings.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnSettingsSave_Click(object sender, RoutedEventArgs e)
        {
            settings.WampServerPath = txtWampServerPath.Text;
            closeSettingsPanel();
        }

        private void btnSettingsCancel_Click(object sender, RoutedEventArgs e)
        {
            closeSettingsPanel();
        }
    }
}
