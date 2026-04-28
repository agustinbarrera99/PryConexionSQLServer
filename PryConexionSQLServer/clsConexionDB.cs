using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PryConexionSQLServer
{
    internal class ClassConexionDB
    {
        private SqlConnection conn;
        private readonly string connectionString;

        public ClassConexionDB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool ConectarDB()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                return true;
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al conectar: " + error.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable ObtenerDatos(string consulta)
        {
            DataTable tabla = new DataTable();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);
            adaptador.Fill(tabla);
            return tabla;
        }

        public bool ValidarUsuario(string mail, string contrasena)
        {
            try
            {
                // Usamos parametrización para evitar SQL Injection
                string query = "SELECT nombre FROM jugador WHERE mail = @mail AND contrasena = @contrasena";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mail", mail);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    string nombre = reader.GetString(0);
                    reader.Close();
                    MessageBox.Show("Bienvenido " + nombre, "Acceso correcto",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }

                reader.Close();
                return false;
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void CerrarConexion()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }
    }
}