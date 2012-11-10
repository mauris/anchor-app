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
    public class SettingsSaveEventArgs : EventArgs
    {
        public Settings Settings { get; set; }
    }

    /// <summary>
    /// Interaction logic for SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : UserControl
    {
        public event EventHandler Cancel;

        public event EventHandler<SettingsSaveEventArgs> Save;

        private bool isCancellable;

        public bool IsCancellable {
            get {
                return isCancellable;
            } 
            set {
                isCancellable = value;
                btnSettingsCancel.Visibility = isCancellable ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            } 
        }

        public SettingsForm()
        {
            InitializeComponent();
        }

        public void Load(Settings settings)
        {
            txtWampServerPath.Text = settings.WampServerPath;
        }

        private void btnSettingsCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel != null)
            {
                Cancel(this, new EventArgs());
            }
        }

        private void btnSettingsSave_Click(object sender, RoutedEventArgs e)
        {
            if (Save != null)
            {
                Save(this, new SettingsSaveEventArgs() {
                    Settings = new Settings() { 
                        WampServerPath = txtWampServerPath.Text 
                    }
                });
            }
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
    }
}
