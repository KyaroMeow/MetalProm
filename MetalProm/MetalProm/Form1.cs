using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using SQLitePCL;


namespace MetalProm
{


    public partial class Form1 : Form
    {
        DataTable dataTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Архивный шкаф");
            comboBox1.Items.Add("Верстак");
        }
        public void UpdateData()
        {
            dataGridView1.DataSource = null;
            dataTable.Rows.Clear();

            List<Material> materialList = ListMaterial();
            foreach (var material in materialList)
            {
                dataTable.Rows.Add(
                    material.ID,
                    material.Name,
                    material.Count

                );
            }
            dataGridView1.DataSource = dataTable;
        }

        private void Craft_click(object sender, EventArgs e)
        {
            int countProduct=Convert.ToInt32(textBox1.Text);
            int[] countsMaterials = { 20, 2, 0, 5 };
            if (comboBox1.Text == "Верстак")
            {
                countsMaterials = new int[] { 10, 0, 2, 2 };

            }
            for(int i = 0; i <= 3; i++)
            {
                countsMaterials[i] *= countProduct;
            }
            
            string[] names = { "Саморезы", "Большая дверь", "Дверь тумбочки", "Полка" };
            int[] counts = new int[4];
            for (int i = 0; i <= 3; i++)
            {
                counts[i] = CheckCount(names[i])-countsMaterials[i];
                string query = $"UPDATE storage SET count = {counts[i]} WHERE material = '{names[i]}'";
                Command(query);
            }
            UpdateData();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("material", typeof(string));
            dataTable.Columns.Add("count", typeof(int));
            UpdateData();
        }

        private void AddMaterial_click(object sender, EventArgs e)
        {
            Form form2 = new Form2();
            form2.Show();
        }

        public class Material
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
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
        public List<Material> ListMaterial()
        {
            List<Material> material = new List<Material>();
            string sqlExpression = "SELECT * FROM storage";
            using (var connection = new SqliteConnection("Data Source=Storage.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue(0);
                        var name = reader.GetValue(1);
                        var count = reader.GetValue(2);

                        material.Add(new Material
                        {
                            ID = Convert.ToInt32(id),
                            Name = name.ToString(),
                            Count = Convert.ToInt32(count)
                        });
                    }
                }
            }
            return material;
        }
    }
}
