using System;
using System.Windows.Forms;

namespace SharpBTC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            tbOutput.Text = String.Empty;
            try {
                if (BTCValidator.ValidateAddress(tbAddress.Text, Prefix.Pubkey_hash))
                {
                    tbOutput.Text += "This is a valid Bitcoin Address. \r\n";
                }
            }
            catch (Exception excp) {
                tbOutput.Text += "This is NOT a valid Bitcoin Address. \r\n";
            }

            try {
                if (BTCValidator.ValidateAddress(tbAddress.Text, Prefix.Testnet_pubkey_hash))
                {
                    tbOutput.Text += "This is a valid Testnet Address. \r\n";
                }
            }             
            catch (Exception excp) {
                tbOutput.Text += "This is NOT a valid Testnet Address. \r\n";
            }

            try {
                if (BTCValidator.ValidateAddress(tbAddress.Text, Prefix.Script_hash))
                {
                    tbOutput.Text += "This is a valid Bitcoin Multisig Address. \r\n";
                }
            } catch (Exception excp) {
                tbOutput.Text += "This is NOT a valid Bitcoin Multisig Address. \r\n";
            }

            try {
                if (BTCValidator.ValidateAddress(tbAddress.Text, Prefix.Testnet_script_hash))
                {
                    tbOutput.Text += "This is a valid Testnet Multisig Address. \r\n";
                }
            }
            catch (Exception excp) {
                tbOutput.Text += "This is NOT a valid Testnet Multisig Address. \r\n";
            }
           
        }
    }
}
