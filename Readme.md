# AzureKeyVaultExplorer
Azure Key Vault Explorer can be used to manage different Azure Key vault accounts, for managing keys and also to encrypt/decrypt content using the keys in the vault.
Follow the below steps to get started
 
1. To add an account you need to provide the vault url, and the AD appication ID and secret.

2. Once the account is added, you will see the options to manage the keys for the vault. You could either manage all the keys from the vault(if you have the necessary get permissions from the AD application), using '*Manage Key Vault Keys*'. If not you can add the key details that you have got offline channel, using '*Manage Local Keys*'


3. With the key information, you can encrypt/decrypt the data that accordingly.

The beta version can be installed from below location.
[https://github.com/rahulpnath/AzureKeyVaultExplorer/raw/master/Installer/Beta/setup.exe](https://github.com/rahulpnath/AzureKeyVaultExplorer/raw/master/Installer/Beta/setup.exe)  

PS : 
Current only beta version is available and only managing the local keys are available. Also only adding of the accounts/keys are available. Features will be added soon. For any issues and feature requests please feel free to raise them [here](https://github.com/rahulpnath/AzureKeyVaultExplorer/issues)