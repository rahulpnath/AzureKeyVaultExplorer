namespace AzureKeyVaultExplorer.View
{
    using System.Windows;
    using System.Windows.Controls;

    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    /// <summary>
    /// Interaction logic for ManageVaultKeysView.xaml
    /// </summary>
    public partial class ManageVaultKeysView : UserControl
    {
        public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register(
         "SelectedKey",
         typeof(Key),
         typeof(ManageVaultKeysView),
    new FrameworkPropertyMetadata(
        null,
        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        OnSelectedKeyChanged));

        public ManageVaultKeysView()
        {
            this.InitializeComponent();
        }

        public Key SelectedKey
        {
            get
            {
                return (Key)GetValue(SelectedKeyProperty);
            }

            set
            {
                this.SetValue(SelectedKeyProperty, value);
            }
        }

        private static void OnSelectedKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ManageVaultKeysView;
            if (current != null)
            {
                var dataContext = current.DataContext as ManageVaultKeysViewModel;
                if (dataContext != null)
                {
                    dataContext.SetSelectedKey(current.SelectedKey);
                }
            }
        }
    }
}
