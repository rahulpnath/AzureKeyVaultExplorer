namespace AzureKeyVaultExplorer.View
{
    using System.Windows;
    using System.Windows.Controls;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    /// <summary>
    /// Interaction logic for KeyCryptographicOperationsView.xaml
    /// </summary>
    public partial class KeyCryptographicOperationsView : UserControl
    {
        public static readonly DependencyProperty CurrentKeyProperty = 
            DependencyProperty.Register(
            "CurrentKey", 
            typeof(Key),
            typeof(KeyCryptographicOperationsView),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCurrentKeyPropertyChanged));

        public KeyCryptographicOperationsView()
        {
            this.InitializeComponent();
        }

        public Key CurrentKey
        {
            get
            {
                return (Key)GetValue(CurrentKeyProperty);
            }

            set
            {
                this.SetValue(CurrentKeyProperty, value);
            }
        }

        private static void OnCurrentKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as KeyCryptographicOperationsView;
            if (current != null)
            {
                var dc = current.DataContext as KeyCryptographicOperationsViewModel;
                if (dc != null)
                {
                    dc.CurrentKey = current.CurrentKey;
                }
            }
        }
    }
}
