using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestiondeVehiculos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbTipoVehiculo.Items.Add("Automovil");
            cbTipoVehiculo.Items.Add("Motocicleta");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string tipo = cbTipoVehiculo.SelectedItem?.ToString();
                string modelo = txtModelo.Text.Trim();
                string marca = txtMarca.Text.Trim();
                double precioBase;

                if (string.IsNullOrEmpty(tipo))
                    throw new Exception("Debe seleccionar un tipo de vehiculo");
                if(string.IsNullOrEmpty(modelo) || string.IsNullOrEmpty(marca))
                    throw new Exception("Debe ingresar un modelo y marca, son obligatorios");
                if (!double.TryParse(txtPrecioBase.Text, out precioBase) || precioBase <= 0)
                    throw new Exception("Debe ingresar un numero mayor a cero");

                Vehiculos vehiculos = VehiculosFactory.CrearVehiculo(tipo, modelo, marca, precioBase);
                GestionVehiculos.ObtenerInstancia().AgregarVehiculo(vehiculos);

                MessageBox.Show("Vehiculo agregado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MostrarVehiculos();
                txtMarca.Clear();
                txtModelo.Clear();
                txtPrecioBase.Clear();
                cbTipoVehiculo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarVehiculos()
        {
            lvVehiculos.Items.Clear();
            foreach (var vehiculo in GestionVehiculos.ObtenerInstancia().Vehiculos)
            {
                var item = new ListViewItem(new[]
                {
                    vehiculo.CalcularPrecioFinal().ToString("C"),
                    vehiculo.GetType().Name,
                    vehiculo.Modelo,
                    vehiculo.Marca,
                    vehiculo.PrecioBase.ToString("C"),
                    vehiculo.CalcularImpuesto().ToString("C"),
                    vehiculo.CalcularCargoExtra().ToString("C")
                }
                );
                lvVehiculos.Items.Add(item);
            }
        }
    }
}
