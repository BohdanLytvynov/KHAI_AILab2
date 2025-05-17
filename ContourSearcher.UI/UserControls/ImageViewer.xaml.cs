using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace UserControls
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        #region Dependency Property

        public Style MainContainerStyle
        {
            get { return (Style)GetValue(MainContainerStyleProperty); }
            set { SetValue(MainContainerStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainContainerStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainContainerStyleProperty;



        public Style ImageBorderStyle
        {
            get { return (Style)GetValue(ImageBorderStyleProperty); }
            set { SetValue(ImageBorderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageBorderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageBorderStyleProperty;



        public Style OpenButtonStyle
        {
            get { return (Style)GetValue(OpenButtonStyleProperty); }
            set { SetValue(OpenButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenButtonStyleProperty;



        public Style PathTextBlockStyle
        {
            get { return (Style)GetValue(PathTextBlockStyleProperty); }
            set { SetValue(PathTextBlockStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathTextBlockStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathTextBlockStyleProperty;



        #endregion

        public ImageViewer()
        {
            InitializeComponent();            
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty;

        static ImageViewer()
        {
            PathProperty =
            DependencyProperty.Register("Path", typeof(string),
            typeof(ImageViewer),
            new PropertyMetadata(string.Empty));

            MainContainerStyleProperty =
            DependencyProperty.Register("MainContainerStyle", typeof(Style),
               typeof(ImageViewer), 
               new PropertyMetadata(new Style(), OnMainContainerStyleChanged));

            ImageBorderStyleProperty =
            DependencyProperty.Register("ImageBorderStyle", typeof(Style), 
            typeof(ImageViewer), 
            new PropertyMetadata(new Style(), OnImageBorderStyleChanged));

            OpenButtonStyleProperty =
            DependencyProperty.Register("OpenButtonStyle", typeof(Style),
            typeof(ImageViewer), 
            new PropertyMetadata(new Style(), OnOpenButtonStyleChanged));

            PathTextBlockStyleProperty =
            DependencyProperty.Register("PathTextBlockStyle", typeof(Style), 
            typeof(ImageViewer), 
            new PropertyMetadata(new Style(), OnPathTextBlockStyleChanged));
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() ?? false)
            {
                string selectedFileName = openFileDialog.FileName;
                this.PathTextBox.Text = selectedFileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedFileName, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                this.Img.Source = bitmapImage;
                Path = selectedFileName;
            }
        }

        #region On dependency property changed

        private static void OnMainContainerStyleChanged(DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        { 
            var This = obj as ImageViewer;

            This.MainContainerStyle = (Style)e.NewValue;
            This.Container.Style = This.MainContainerStyle;
        }

        private static void OnImageBorderStyleChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        { 
            var This = obj as ImageViewer;

            This.ImageBorderStyle = (Style)e.NewValue;
            This.ImageBorder.Style = This.ImageBorderStyle;
        }


        private static void OnOpenButtonStyleChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        { 
            var This = obj as ImageViewer;

            This.OpenButtonStyle = (Style)e.NewValue;
            This.OpenButton.Style = This.OpenButtonStyle;
        }

        private static void OnPathTextBlockStyleChanged(DependencyObject obj,
           DependencyPropertyChangedEventArgs e)
        {
            var This = obj as ImageViewer;

            This.PathTextBlockStyle = (Style)e.NewValue;
            This.PathTextBox.Style = This.PathTextBox.Style;
        }
        #endregion
    }
}
