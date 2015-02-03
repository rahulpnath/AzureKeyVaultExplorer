using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureKeyVaultExplorer.View
{
    /// <summary>
    /// Interaction logic for KeyCryptographicOperationsView.xaml
    /// </summary>
    public partial class KeyCryptographicOperationsView : UserControl
    {
        public KeyCryptographicOperationsView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty CurrentKeyProperty = DependencyProperty.Register("CurrentKey", typeof(Key), typeof(KeyCryptographicOperationsView), new PropertyMetadata(default(Key)));

        public Key CurrentKey
        {
            get
            {
                return (Key)GetValue(CurrentKeyProperty);
            }

            set
            {
                SetValue(CurrentKeyProperty, value);
            }
        }
    }
}
