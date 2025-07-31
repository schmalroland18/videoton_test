using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GetID; // DLL namespace

namespace videoton_test
{
    public partial class Form1 : Form
    {
        private GetID.GetID dllObject; // Pontos osztálynév

        public Form1()
        {
            InitializeComponent();
            InitializeDll();
        }

        private void InitializeDll()
        {
            dllObject = new GetID.GetID();
            dllObject.ValueChanged += OnValueChanged;
            dllObject.ErrorChanged += OnErrorChanged;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            dllObject.Go();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            dllObject.Stop();
        }

        private void OnValueChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                string value = dllObject.Value;
                string status = IsValidValue(value) ? "MEGFELELÕ" : "NEM MEGFELELÕ";
                textBoxOutput.AppendText($"{value} - {status}{Environment.NewLine}");
            }));
        }

        private void OnErrorChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                string error = dllObject.ErrorMessage;
                MessageBox.Show($"Hiba: {error}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private bool IsValidValue(string value)
        {
            return Regex.IsMatch(value, @"^[A-Z][0-9]{4}$");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveDialog.FileName, textBoxOutput.Text);
                        MessageBox.Show("Mentés sikeres.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Mentési hiba: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
