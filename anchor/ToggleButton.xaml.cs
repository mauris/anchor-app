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
    /// Interaction logic for ToggleButton.xaml
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        public ToggleButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UncheckedImageProperty =
            DependencyProperty.Register(
            "UncheckedImage",
            typeof(ImageSource),
            typeof(ToggleButton),
            new PropertyMetadata(onUncheckedImageChangedCallback));

        public ImageSource UncheckedImage
        {
            get { return (ImageSource)GetValue(UncheckedImageProperty); }
            set { SetValue(UncheckedImageProperty, value); }
        }

        static void onUncheckedImageChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }

        public static readonly DependencyProperty DisabledImageProperty =
            DependencyProperty.Register(
            "DisabledImage",
            typeof(ImageSource),
            typeof(ToggleButton),
            new PropertyMetadata(onDisabledImageChangedCallback));

        public ImageSource DisabledImage
        {
            get { return (ImageSource)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        static void onDisabledImageChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }


        public static readonly DependencyProperty CheckedImageProperty =
            DependencyProperty.Register(
            "CheckedImage",
            typeof(ImageSource),
            typeof(ToggleButton),
            new PropertyMetadata(onCheckedImageChangedCallback));

        public ImageSource CheckedImage
        {
            get { return (ImageSource)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }

        static void onCheckedImageChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }


        public static readonly DependencyProperty CheckedHoverImageProperty =
            DependencyProperty.Register(
            "CheckedHoverImage",
            typeof(ImageSource),
            typeof(ToggleButton),
            new PropertyMetadata(onCheckedHoverImageChangedCallback));

        public ImageSource CheckedHoverImage
        {
            get { return (ImageSource)GetValue(CheckedHoverImageProperty); }
            set { SetValue(CheckedHoverImageProperty, value); }
        }

        static void onCheckedHoverImageChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }

        public static readonly DependencyProperty UncheckedHoverImageProperty =
            DependencyProperty.Register(
            "UncheckedHoverImage",
            typeof(ImageSource),
            typeof(ToggleButton),
            new PropertyMetadata(onUncheckedHoverImageChangedCallback));

        public ImageSource UncheckedHoverImage
        {
            get { return (ImageSource)GetValue(UncheckedHoverImageProperty); }
            set { SetValue(UncheckedHoverImageProperty, value); }
        }

        static void onUncheckedHoverImageChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }


        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
            "IsChecked",
            typeof(Boolean),
            typeof(ToggleButton),
            new PropertyMetadata(onCheckedChangedCallback));

        public Boolean IsChecked
        {
            get { return (Boolean)GetValue(IsCheckedProperty); }
            set { if (value != IsChecked) SetValue(IsCheckedProperty, value); }
        }

        static void onCheckedChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something, if needed
        }


    }
}
