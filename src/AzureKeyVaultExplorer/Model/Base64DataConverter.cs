namespace AzureKeyVaultExplorer.Model
{
    using System;

    using AzureKeyVaultExplorer.Interface;

    public class Base64DataConverter : IDataConverter
    {
        public string DisplayMessage
        {
            get
            {
                return "Enter a vaild base64 formatted string";
            }
        }

        public string ConvertToString(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public byte[] ConvertToByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}