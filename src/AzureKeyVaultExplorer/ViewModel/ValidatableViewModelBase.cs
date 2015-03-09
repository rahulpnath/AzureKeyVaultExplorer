namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight;
    using MvvmValidation;

    public class ValidatableViewModelBase : ViewModelBase, IDataErrorInfo
    {
        public ValidatableViewModelBase()
        {
            this.Validator = new ValidationHelper();
        }

        public string Error
        {
            get { return this.Validator.GetResult().ToString(); }
        }

        protected ValidationHelper Validator { get; set; }

        public string this[string columnName]
        {
            get { return this.Validator.GetResult(columnName).ToString(); }
        }
    }
}
