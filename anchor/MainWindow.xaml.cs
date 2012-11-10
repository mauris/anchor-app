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
using System.IO;
using System.Diagnostics;
using anchor.Drivers;
using System.Reflection;
using anchor.commons;

namespace anchor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Entry> entries = new ObservableCollection<Entry>();

        private Settings settings;

        HostEditor hostEditor;

        Driver driver;
        
        ConfigWriter writer;

        bool initialize = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        ~MainWindow()
        {
            if (entries.Count == 0)
            {
                if (File.Exists("entries.bin"))
                {
                    File.Delete("entries.bin");
                }
            }
            else
            {
                DataFile.Write("entries.bin", entries);
            }
            if (settings == null)
            {
                if (File.Exists("settings.bin"))
                {
                    File.Delete("settings.bin");
                }
            }
            else
            {
                DataFile.Write("settings.bin", settings);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            string hostFile = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32\\drivers\\etc\\hosts");
            hostEditor = new HostEditor(hostFile);

            // makes an original backup file that will always be pre-Anchor usage.
            if(!File.Exists(hostFile + ".orig"))
            {
                File.Copy(hostFile, hostFile + ".orig");
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;
            lblAbout.Text = "Anchor for Windows v" + version[0] + "." + version[2] + "." + version[4];

            lblLicenseAnchor.Text = anchor.Properties.Resources.license_Anchor;
            lblLicenseINIParser.Text = anchor.Properties.Resources.license_INIParser;
            lblLicenseF3Silk.Text = anchor.Properties.Resources.license_F3Silk;
            lblLicenseMedia.Text = anchor.Properties.Resources.license_Media;

            loadEntries();
            loadSettings();
            driver = new WampServer(settings.WampServerPath);
            writer = new ConfigWriter(System.IO.Path.Combine(driver.ApacheConfigPath, "anchor.conf"));
            initialize = true;
        }

        private void restartApache()
        {
            driver.restartServer();
            ProcessStartInfo dnsFlusher = new ProcessStartInfo("ipconfig", "/flushdns");
            dnsFlusher.WindowStyle = ProcessWindowStyle.Hidden;
            dnsFlusher.UseShellExecute = true;
            dnsFlusher.CreateNoWindow = true;
            Process.Start(dnsFlusher);
        }

        private void loadSettings()
        {
            try
            {
                settings = DataFile.Read<Settings>("settings.bin");
                if (settings == null)
                {
                    throw new ArgumentNullException();
                }
                tgbEnableDisable.IsChecked = settings.Enabled;
            }
            catch (Exception)
            {
                pnlSetup.Visibility = System.Windows.Visibility.Visible;
                tgbEnableDisable.IsChecked = false;
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
                closeSettingsPanel();
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
                if (settings.Enabled)
                {
                    hostEditor.add(hostName + ".dev");
                    hostEditor.update();
                    writer.write(driver.ServerRootPath, entries.ToList());
                    this.restartApache();
                }
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
                    if (settings.Enabled)
                    {
                        hostEditor.remove(entry.Name + ".dev");
                        hostEditor.update();
                        entries.Remove(entry);
                        writer.write(driver.ServerRootPath, entries.ToList());
                        this.restartApache();
                    }
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
            frmSettings.Load(settings);
            pnlAddHost.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void closeSettingsPanel()
        {
            btnSettings.Content = "";
            pnlContent.Visibility = System.Windows.Visibility.Visible;
            pnlSettings.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            TextBox path = (TextBox)(((FrameworkElement)sender).Parent as FrameworkElement).FindName((string)((FrameworkElement)sender).Tag);

            // Configure open file dialog box
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = "Select a folder";
            dlg.SelectedPath = path.Text;

            // Show open file dialog box
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Open document
                path.Text = dlg.SelectedPath;
                path.SelectAll();
                path.Focus();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void btnLicenseClose_Click(object sender, RoutedEventArgs e)
        {
            pnlLicense.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void License_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            pnlLicense.Visibility = System.Windows.Visibility.Visible;
            e.Handled = true;
        }

        private void tgbEnableDisable_Changed(object sender, EventArgs e)
        {
            settings.Enabled = tgbEnableDisable.IsChecked;
            if (tgbEnableDisable.IsChecked)
            {
                lblToggleState.Text = "ON";
                if (initialize)
                {
                    foreach (Entry entry in entries)
                    {
                        hostEditor.add(entry.Name + ".dev");
                    }
                    hostEditor.update();
                    writer.write(driver.ServerRootPath, entries.ToList());
                    this.restartApache();
                }
            }
            else
            {
                lblToggleState.Text = "OFF";
                if (initialize)
                {
                    hostEditor.clear();
                    hostEditor.update();
                    writer.write(driver.ServerRootPath, new List<Entry>());
                    this.restartApache();
                }
            }
        }

        private void SettingsSave(object sender, SettingsSaveEventArgs e)
        {
            settings.WampServerPath = e.Settings.WampServerPath;
            closeSettingsPanel();
        }

        private void SettingsCancel(object sender, EventArgs e)
        {
            closeSettingsPanel();
        }

        private void SetupSave(object sender, SettingsSaveEventArgs e)
        {
            settings = e.Settings;
            tgbEnableDisable.IsChecked = settings.Enabled;
            pnlSetup.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnResetAnchor_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset Anchor? There is no return.", "Reset Anchor App", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                entries.Clear();
                hostEditor.clear();
                hostEditor.update();
                writer.write(driver.ServerRootPath, new List<Entry>());
                this.restartApache();
                settings = null;

                string hostFile = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32\\drivers\\etc\\hosts");

                // reset hosts file back to pre-Anchor usage.
                if (File.Exists(hostFile + ".orig"))
                {
                    File.Copy(hostFile + ".orig", hostFile);
                }

                pnlSetup.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
