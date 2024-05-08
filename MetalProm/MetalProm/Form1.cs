using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using SQLitePCL;
using static MetalProm.Form1;
using System.Data;

namespace MetalProm
{


    public partial class Form1 : Form
    {
        DataTable dataTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
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
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
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
            }
            return material;
        }
    }
}
