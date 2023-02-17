using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Open.Numeric.Primes;

namespace DiffieHellman
{
    public partial class Form1 : Form
    {
        private readonly int minValue = 10000, maxValue = 500000;

        public Form1()
        {
            InitializeComponent();
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
        }

        private async void GenerateButton(object sender, EventArgs e)
        {
            Int32 n, g;
            Int32 x, y;
            try
            {
                // Public values
                n = Int32.Parse(primeTextBox1.Text);
                g = Int32.Parse(primeTextBox2.Text);

                // Private values
                x = Int32.Parse(secretTextBox1.Text);
                y = Int32.Parse(secretTextBox2.Text);
            }
            catch 
            { 
                MessageBox.Show("Invalid values. Expected Int32.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Stopwatch st = new Stopwatch();
            button1.Text = "Computing...";

            BigInteger a, b;

            // Calculation
            a = await Task.Run(() => ComputeKey(g, x, n, label7, "a"));
            b = await Task.Run(() => ComputeKey(g, y, n, label8, "b"));

            BigInteger k1, k2;

            // Secret keys
            k1 = await Task.Run(() => ComputeKey(b, x, n, label9, "k1"));
            k2 = await Task.Run(() => ComputeKey(a, y, n, label10, "k2"));

            if (k1.CompareTo(k2) == 0)
            {
                keyTextBox.Text = k1.ToString();
                button1.Text = "Finished";
                MessageBox.Show("Key generated.", "Key exchange", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                button1.Text = "Error";
                MessageBox.Show("Key mismatch.", "Key error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button1.Text = "Compute";
        }

        private BigInteger ComputeKey(BigInteger x, Int32 y, Int32 mod, Label label, String str)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            BigInteger t = BigInteger.Pow(x, y) % mod;
            st.Stop();

            if (label.InvokeRequired)
            {
                label.Invoke(new MethodInvoker(delegate { label.Text = $"{str}: {st.ElapsedMilliseconds} ms"; }));
            }
            else
            {
                label.Text = $"{str}: {st.ElapsedMilliseconds} ms";
            }
            return t;
        }

        private void GenerateSecretButton(object sender, EventArgs e)
        {
            // Secret values, x and y
            Random random = new Random();
            long[] p = new long[2];

            for (int i = 0; i < p.Length; i++)
            {
                p[i] = random.Next(minValue, maxValue);
            }

            secretTextBox1.Text = p[0].ToString();
            secretTextBox2.Text = p[1].ToString();

        }

        private void ClearFieldsButton(object sender, EventArgs e)
        {
            primeTextBox1.Text = "";
            primeTextBox2.Text = "";
            secretTextBox1.Text = "";
            secretTextBox2.Text = "";
            keyTextBox.Text = "";
        }

        private void GeneratePrimesButton(object sender, EventArgs e)
        {
            // Public numbers, n and g
            Random random = new Random();
            long[] p = new long[2];

            // int minValue = 10000, maxValue = 1000000;

            for (int i = 0; i < p.Length; i++)
            {
                p[i] = Prime.Numbers.StartingAt(random.Next(minValue, maxValue)).ElementAt(random.Next(minValue));
            }

            primeTextBox1.Text = p[0].ToString();
            primeTextBox2.Text = p[1].ToString();
        }
    }
}
