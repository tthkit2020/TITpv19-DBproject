using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TITpv19CatsAndDogs
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["TITpv19CatsAndDogs.Properties.Settings.PetsConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulatePetTypeTable();
        }

        private void PopulatePetTypeTable()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PetType", connection))
            {
                DataTable petTypeTable = new DataTable();
                adapter.Fill(petTypeTable);

                types.DisplayMember = "PetTypeName";
                types.ValueMember = "Id";
                types.DataSource = petTypeTable;
        
            }
        }

        private void types_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePetNames();
        }

        private void PopulatePetNames()
        {
            string query = "SELECT Pet.Name, PetType.PetTypeName FROM PET INNER JOIN PetType ON Pet.TypeId = PetType.Id WHERE PetType.Id = @TypeId";
            using(connection = new SqlConnection(connectionString))
            using(SqlCommand command = new SqlCommand(query, connection))
            using(SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@TypeId", types.SelectedValue);
                DataTable petNameTable = new DataTable();
                adapter.Fill(petNameTable);

                names.DisplayMember = "Name";
                names.ValueMember = "Id";
                names.DataSource = petNameTable;

            }
        
        }

    }
}
