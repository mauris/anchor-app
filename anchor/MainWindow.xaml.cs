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
        private List<Entry> entries = new List<Entry>();

        public MainWindow()
        {
            InitializeComponent();

            pnlToolbar.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/toolbar-background.png")));
            pnlContent.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/toolbar-shadow.png"))) { Stretch = Stretch.None, AlignmentY = AlignmentY.Top, AlignmentX = AlignmentX.Left, Viewport = new Rect(0, 0, 20, 1000000), ViewportUnits = BrushMappingMode.Absolute, TileMode = TileMode.FlipXY };
        }

        ~MainWindow()
        {
            DataFile.Write("entries.bin", entries);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadEntries();
        }

        private void loadEntries()
        {
            entries = DataFile.Read<List<Entry>>("entries.bin");
            lstSites.DataContext = entries;
        }
    }
}
