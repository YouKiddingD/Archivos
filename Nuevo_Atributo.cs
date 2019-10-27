using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archivos
{
    public partial class Nuevo_Atributo : Form
    {
        Archivo Archivo;
        public Atributo atributo;
        public Entidad destino;
        public bool modif = false;

        public Nuevo_Atributo(Archivo a,bool modify)
        {
            modif = modify;
            Archivo = a;
            InitializeComponent();
            LlenarCbo();
        }

        public void LlenarCbo()
        {
            foreach (Entidad ent in Archivo.DiccDatos.Keys)
            {
                cboEntidades.Items.Add(ent.Nombre);
            }
            if(modif)
            {
                cboEntidades.Enabled = false;
                txtAtributo.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!modif)
            {
                atributo = new Atributo(txtAtributo.Text, cboTipo.SelectedItem.ToString()[0], Int32.Parse(txtLong.Text), Int32.Parse(cboTipoIindice.SelectedItem.ToString()), 0);
                destino = Archivo.BuscarEntidad(cboEntidades.SelectedItem.ToString());
            }
            else
            {
                atributo = new Atributo(txtAtributo.Text, cboTipo.SelectedItem.ToString()[0], Int32.Parse(txtLong.Text), Int32.Parse(cboTipoIindice.SelectedItem.ToString()), 0);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void txtAtributo_TextChanged(object sender, EventArgs e)
        {
            if (txtAtributo.Text != "")
            {
                cboTipo.Enabled = true;
            }
        }

        private void cboTipo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboTipo.SelectedItem.ToString() == "E")
            {
                txtLong.Text = "4";
            }
            else
            {
                txtLong.Enabled = true;
            }
        }

        private void txtLong_TextChanged(object sender, EventArgs e)
        {
            if (txtLong.Text != "")
            {
                cboTipoIindice.Enabled = true;
            }
        }

        private void cboEntidades_SelectedValueChanged(object sender, EventArgs e)
        {
                txtAtributo.Enabled = true;
        }

        private void cboTipoIindice_SelectedValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
