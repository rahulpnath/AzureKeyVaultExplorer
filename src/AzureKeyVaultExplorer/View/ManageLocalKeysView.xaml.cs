namespace AzureKeyVaultExplorer.View
{
    using System.Windows;
    using System.Windows.Controls;

    using AzureKeyVaultExplorer.Model;

    /// <summary>
    /// Interaction logic for ManageLocalKeysView.xaml
    /// </summary>
    public partial class ManageLocalKeysView : UserControl
    {
        public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register("SelectedKey", typeof(Key), typeof(ManageLocalKeysView), new PropertyMetadata(default(Key)));

        public ManageLocalKeysView()
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
