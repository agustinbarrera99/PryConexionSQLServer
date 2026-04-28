using System;
using System.Data;
using System.Windows.Forms;

namespace PryConexionSQLServer
{
    public partial class frmPrincipal : Form
    {
        private ClassConexionDB db;

        private const string CONNECTION_STRING =
            "Server=PC142;Database=NEPTUNO;User Id=sa;Password=sa;";

        private readonly string[] tablas = new string[]
        {
            "Categorías", "Clientes", "Compañías de envíos",
            "Detalles de pedidos", "Empleados", "Pedidos",
            "Productos", "Proveedores"
        };

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            db = new ClassConexionDB(CONNECTION_STRING);
            IntentarConectar();

            cmbTablas.Items.Clear();
            cmbTablas.Items.AddRange(tablas);
            cmbTablas.SelectedIndex = 0;
        }

        private void IntentarConectar()
        {
            bool conectado = db.ConectarDB();
            ActualizarEstadoConexion(conectado);

            // Habilitar/deshabilitar controles según el estado
            cmbTablas.Enabled = conectado;
            btnCargar.Enabled = conectado;
            dgvDatos.Enabled = conectado;
        }

        private void ActualizarEstadoConexion(bool conectado)
        {
            if (conectado)
            {
                toolStripLabelEstado.Text = "● Conectado a la base de datos";
                toolStripLabelEstado.ForeColor = System.Drawing.Color.LimeGreen;
                toolStripStatusLabel1.Text = "Estado: OK";
            }
            else
            {
                toolStripLabelEstado.Text = "● Sin conexión a la base de datos";
                toolStripLabelEstado.ForeColor = System.Drawing.Color.Red;
                toolStripStatusLabel1.Text = "Estado: Error";
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (cmbTablas.SelectedItem == null) return;

            string tabla = cmbTablas.SelectedItem.ToString();

            try
            {
                // Llama a ObtenerDatos con la consulta SQL correspondiente
                DataTable datos = db.ObtenerDatos($"SELECT * FROM [{tabla}]");

                dgvDatos.DataSource = datos;

                toolStripStatusLabel1.Text =
                    $"Estado: OK  |  Tabla: {tabla}  |  Filas: {datos.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReconectar_Click(object sender, EventArgs e)
        {
            db = new ClassConexionDB(CONNECTION_STRING);
            IntentarConectar();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            db?.CerrarConexion();
            base.OnFormClosing(e);
        }
    }
}