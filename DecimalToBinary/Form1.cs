using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecimalToBinary
{
    public partial class Form1 : Form
    {

        double[] numbers = new double[17];
        List<Button> buttons = new List<Button>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (double i = 0; i < numbers.Length; i++)
            {
                numbers[(int)i] = Math.Pow(2, i);
            }
            generate();
            timer1.Start();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            btnConvert.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void convert(int i, int i2, Color color, string txtNum)
        {
            buttons.Where(x => x.Name.Equals($"button{i}")).FirstOrDefault().BackColor = color;
            var button = buttons.Where(x => x.Name.Equals($"button{i2}")).FirstOrDefault();
            button.Invoke((MethodInvoker)delegate
            {
                button.Text = txtNum;
            });
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var item in this.Controls.OfType<Button>())
            {
                buttons.Add(item);
                item.BackColor = SystemColors.Control;
            }
            for (int i = (numbers.Length - 1); i >= 0; i--)
            {
                var button = buttons.Where(x => x.Name.Equals($"button{i}")).FirstOrDefault();
                button.Invoke((MethodInvoker)delegate
                {
                    button.Text = "0";
                });
            }
            int num = Convert.ToInt32(txtNumber.Text);
            int soma = 0;
            int i2 = (numbers.Length - 1);
            string binaryText = string.Empty;
            for (int i = ((numbers.Length * 2) - 1); i >= numbers.Length; i--)
            {
                if ((soma + numbers[i2]) > num)
                {
                    convert(i, i2, Color.LightCoral, "0");
                    binaryText += "0";
                }
                else
                {
                    soma += (int)numbers[i2];
                    convert(i, i2, Color.LightGreen, "1");
                    binaryText += "1";
                }
                i2--;
                Thread.Sleep(250);
            }
            textBox1.Invoke((MethodInvoker)delegate
            {
                textBox1.AppendText($"{txtNumber.Text} ::: {binaryText}");
                textBox1.AppendText(Environment.NewLine);
            });
            buttons.Clear();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnConvert.Enabled = true;
        }

        private void generate()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 131072);
            txtNumber.Text = $"";
            txtNumber.Text = $"{randomNumber}";
            btnConvert_Click(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            generate();
        }
    }
}
