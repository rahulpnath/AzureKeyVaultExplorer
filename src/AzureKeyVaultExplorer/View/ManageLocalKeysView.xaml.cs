namespace AzureKeyVaultExplorer.View
{
    using System.Windows;
    using System.Windows.Controls;

    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    /// <summary>
    /// Interaction logic for ManageLocalKeysView.xaml
    /// </summary>
    public partial class ManageLocalKeysView : UserControl
    {
        public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register(
            "SelectedKey",
             typeof(Key),
            typeof(ManageLocalKeysView),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedKeyChanged));

        public ManageLocalKeysView()
        {
            this.InitializeComponent();
        }

        public Key SelectedKey
        {
            get { return (Key)GetValue(SelectedKeyProperty); }
            set { this.SetValue(SelectedKeyProperty, value); }
        }

        private static void OnSelectedKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ManageLocalKeysView;
            if (current != null)
            {
                var dataContext = current.DataContext as ManageLocalKeysViewModel;
                if (dataContext != null)
                {
                    dataContext.SetSelectedKey(current.SelectedKey);
                }
            }
        }
    }
}
