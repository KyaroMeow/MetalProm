using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace MetalProm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public void Command(string com)
        {
            using (var connection = new SqliteConnection("Data Source=Storage.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = com;
                command.ExecuteNonQuery();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] counts = { Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text) };
            string[] names = { label1.Text, label2.Text, label3.Text, label4.Text };
            for (int i = 0; i <= 3; i++)
            {
                counts[i] += CheckCount(names[i]);
                string query = $"UPDATE storage SET count = {counts[i]} WHERE material = '{names[i]}'";
                Command(query);
            }
            
        }
        public int CheckCount(string materialName)
        {
            using (var connection = new SqliteConnection("Data Source=Storage.db"))
            {
                string quaryCheck = $"SELECT count FROM storage WHERE material = '{materialName}'";
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = quaryCheck;
                object result = command.ExecuteScalar();
                int count = Convert.ToInt32(result);
                return count;
            }
        }
    }
}
