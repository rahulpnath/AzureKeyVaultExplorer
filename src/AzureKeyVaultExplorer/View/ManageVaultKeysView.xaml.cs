namespace AzureKeyVaultExplorer.View
{
    using System.Windows;
    using System.Windows.Controls;

    using AzureKeyVaultExplorer.Model;

    /// <summary>
    /// Interaction logic for ManageVaultKeysView.xaml
    /// </summary>
    public partial class ManageVaultKeysView : UserControl
    {
        public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register("SelectedKey", typeof(Key), typeof(ManageVaultKeysView), new PropertyMetadata(default(Key)));

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
    }
}
