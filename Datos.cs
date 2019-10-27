using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Archivos
{
    public partial class Datos : Form
    {
        private List<Atributo> lAtributos;
        private List<Registro> listDat = new List<Registro>();
        private List<Registro> listDatOriginal = new List<Registro>();
        private bool clavebusent = true;
        private List<long> direcs = new List<long>();
        private string sDirectorio;
        private string sEntidad;
        private string sRuta;
        public long currentDir;
        public int longitudTot = 15;
        public long Encabezado = -1;
        public long dirInicio;
        private Dictionary<long, List<object>> secuencial = new Dictionary<long, List<object>>();
        private List<Atributo> tipoInd = new List<Atributo>();
        private List<int> arbolito = new List<int>();
        private Dictionary<int, long> salva = new Dictionary<int, long>();

        public Datos(List<Atributo> atributos, string direc, string ne, long dirDatos)
        {
            InitializeComponent();
            lAtributos = atributos;
            sDirectorio = direc;
            sEntidad = ne;
            sRuta = "";
            currentDir = 0;
            AñadirColumns();
            dirInicio = dirDatos;
            LeerDatos(dirDatos);
            calcularLong();
        }

        private void AñadirColumns()
        {
            gridDatos.Columns.Add("DirDat", "Direccion");
            foreach (Atributo a in lAtributos)
            {
                gridDatos.Columns.Add(a.Nombre, a.Nombre);
            }
            gridDatos.Columns.Add("DirSigDat", "Direccion sig. dato");
        }

        private void calcularLong()
        {
            foreach (Atributo a in lAtributos)
            {
                longitudTot += a.Longitud;
            }
        }

        private void actualizarGridDatos()
        {
            string[] values = new string[lAtributos.Count + 2];
            int i = 1;
            bool listo = false;
            if (currentDir > 0)
            {
                guardarSiguiente();
            }
            guardarDato(currentDir);
            currentDir += 8;
            foreach (Atributo a in lAtributos)
            {
                values[i] = Interaction.InputBox("Value: ", a.Nombre, "");
                listo = false;
                if (a.TipoDato == 'C')
                {
                    string v = values[i].PadRight(a.Longitud);
                    v = v.Substring(0, a.Longitud);
                    guardarDato(v);
                    if (a.TipoIndice == 1)
                    {
                        listDat.Add(new Registro(currentDir, v, (long)-1));
                        clavebusent = false;
                    }
                }
                else if (a.TipoDato == 'E')
                {
                    while (!listo)
                    {
                        try
                        {
                            int v = Int32.Parse(values[i]);
                            if (a.TipoIndice == 1)
                            {
                                listDat.Add(new Registro(currentDir, v, (long)-1));
                                clavebusent = true;
                            }
                            guardarDato(v);
                            listo = true;
                        }
                        catch (FormatException exp)
                        {
                            values[i] = Interaction.InputBox("El dato debe de ser entero: ", a.Nombre, "");
                        }
                    }
                }
                currentDir += a.Longitud;
                i++;
            }
            guardarDato((long)-1);
            currentDir += 8;
            //ordenaClaveBusq();
            //calcularDirecciones();
            LeerDatos(dirInicio);
        }

        public void ordenaClaveBusq()
        {
            if (clavebusent)
            {
                direcs.Clear();
                List<Registro> auxiliar = new List<Registro>();
                foreach (Registro r in listDat)
                {
                    auxiliar.Add(r);
                }
                while (auxiliar.Count > 0)
                {
                    Registro menor = auxiliar[0];
                    foreach (Registro r in auxiliar)
                    {
                        if (r.clavebusqent < menor.clavebusqent)
                        {
                            menor = r;
                        }
                    }
                    direcs.Add(menor.Direccion);
                    auxiliar.Remove(menor);
                }
                label1.Text = direcs[0].ToString();
                Encabezado = direcs[0];
                for (int i = 0; i < listDat.Count; i++)
                {
                    foreach (Registro r in listDat)
                    {
                        if (r.Direccion == direcs[i])
                        {
                            if (i + 1 < listDat.Count)
                            {
                                r.DirSigReg = direcs[i + 1];
                            }
                            else
                            {
                                r.DirSigReg = -1;
                            }
                        }
                    }
                }
            }
            else
            {
                compararCadena();
            }
        }

        private void compararCadena()
        {
            direcs.Clear();
            List<Registro> auxiliar = new List<Registro>();
            foreach (Registro r in listDat)
            {
                auxiliar.Add(r);
            }
            while (auxiliar.Count > 0)
            {
                Registro menor = auxiliar[0];
                foreach (Registro r in auxiliar)
                {
                    if (r.clavebusstring != "" && string.Compare(r.clavebusstring, menor.clavebusstring) == -1)
                    {
                        menor = r;
                    }
                }
                direcs.Add(menor.Direccion);
                auxiliar.Remove(menor);
            }
            label1.Text = direcs[0].ToString();
            Encabezado = direcs[0];
            for (int i = 0; i < listDat.Count; i++)
            {
                foreach (Registro r in listDat)
                {
                    if (r.Direccion == direcs[i])
                    {
                        if (i + 1 < listDat.Count)
                        {
                            r.DirSigReg = direcs[i + 1];
                        }
                        else
                        {
                            r.DirSigReg = -1;
                        }
                    }
                }
            }
        }

        private void guardarDato(int value)
        {
            try
            {
                using (Stream stream = File.Open(sRuta, FileMode.Open))
                {
                    stream.Position = currentDir;
                    stream.Write(BitConverter.GetBytes(value), 0, 4);
                    stream.Close();
                }
            }
            catch (Exception excp)
            {
                crearArchivo();
                guardarDato(value);
            }
        }

        private void guardarDato(string value)
        {
            try
            {
                using (Stream stream = File.Open(sRuta, FileMode.Open))
                {
                    stream.Position = currentDir;
                    byte[] s = StringToBinary(value, value.Length);
                    stream.Write(s, 0, value.Length);
                    stream.Close();
                }
            }
            catch (Exception excp)
            {
                crearArchivo();
                guardarDato(value);
            }
        }

        private void guardarDato(long value)
        {
            try
            {
                using (Stream stream = File.Open(sRuta, FileMode.Open))
                {
                    stream.Position = currentDir;
                    stream.Write(BitConverter.GetBytes(value), 0, 8);
                    stream.Close();
                }
            }
            catch (Exception excp)
            {
                crearArchivo();
                guardarDato(value);
            }
        }

        private void guardarSiguiente()
        {
            using (Stream stream = File.Open(sRuta, FileMode.Open))
            {
                stream.Position = currentDir - 7;
                stream.Write(BitConverter.GetBytes(currentDir), 0, 8);
                currentDir++;
                listDat.Last<Registro>().DirSigReg = currentDir;
                stream.Close();
            }
        }

        public void calcularDirecciones()
        {
            ordenaClaveBusq();
            int i = 0;
            foreach (Registro r in listDat)
            {
                gridDatos.Rows[i].Cells[lAtributos.Count + 1].Value = r.DirSigReg.ToString();
                i++;
            }
        }

        public void LeerDatos(long dir)
        {
            try
            {
                sRuta = sDirectorio + "\\" + sEntidad.Replace(" ", "") + ".dat";
                listDat.Clear();
                long sigDato = dir, auxDato = 0;
                secuencial.Clear();
                tipoInd.Clear();
                arbolito.Clear();
                salva.Clear();
                listDatOriginal.Clear();
                currentDir = new System.IO.FileInfo(sRuta).Length - 1;
                gridDatos.Rows.Clear();
                using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
                {
                    reader.BaseStream.Seek(dir, SeekOrigin.Begin);
                    byte[] dirBytes = new byte[8];
                    reader.Read(dirBytes, 0, 8);
                    auxDato = BitConverter.ToInt64(dirBytes, 0);
                    string[] values = new string[lAtributos.Count + 2];
                    int i = 1;
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        if (auxDato != -1)
                        {
                            dir = sigDato;
                            reader.BaseStream.Position = dir;
                            Registro aux = new Registro();
                            Registro aux2 = new Registro();
                            dirBytes = new byte[8];
                            reader.Read(dirBytes, 0, 8);
                            values[0] = BitConverter.ToInt64(dirBytes, 0).ToString();
                            aux.Direccion = BitConverter.ToInt64(dirBytes, 0);
                            aux2.Direccion = BitConverter.ToInt64(dirBytes, 0);
                            i = 1;
                            dir += 8;
                            foreach (Atributo a in lAtributos)
                            {
                                dir += a.Longitud;
                                byte[] atrBytes = new byte[a.Longitud];
                                reader.Read(atrBytes, 0, a.Longitud);
                                string value = "";
                                if (a.TipoDato == 'E')
                                {
                                    value = BitConverter.ToInt32(atrBytes, 0).ToString();
                                    if (a.TipoIndice == 1)
                                    {
                                        aux.clavebusqent = BitConverter.ToInt32(atrBytes, 0);
                                        aux2.clavebusqent = BitConverter.ToInt32(atrBytes, 0);
                                        clavebusent = true;
                                    }
                                    if (a.TipoIndice == 2)
                                    {
                                        long agregar = Convert.ToInt64(values[0]);
                                        if (!secuencial.ContainsKey(agregar))
                                        {
                                            secuencial.Add(agregar, new List<object>());
                                        }
                                        secuencial[agregar].Add(Int32.Parse(value));
                                        tipoInd.Add(a);
                                    }
                                    if (a.TipoIndice == 3)
                                    {
                                        long agregar = Convert.ToInt64(values[0]);
                                        if (!secuencial.ContainsKey(agregar))
                                        {
                                            secuencial.Add(agregar, new List<object>());
                                        }
                                        secuencial[agregar].Add(Int32.Parse(value));
                                        tipoInd.Add(a);
                                    }
                                    if (a.TipoIndice == 4)
                                    {
                                        arbolito.Add(Int32.Parse(value));
                                        salva.Add(Int32.Parse(value), Int64.Parse(values[0]));
                                    }
                                }
                                else
                                {
                                    value = BinaryToString(atrBytes);
                                    if (a.TipoIndice == 1)
                                    {
                                        aux.clavebusstring = value;
                                        aux2.clavebusstring = value;
                                        clavebusent = false;
                                    }
                                    if (a.TipoIndice == 2)
                                    {
                                        long agregar = Convert.ToInt64(values[0]);
                                        if (!secuencial.ContainsKey(agregar))
                                        {
                                            secuencial.Add(agregar, new List<object>());
                                        }
                                        secuencial[agregar].Add(value);
                                        tipoInd.Add(a);
                                    }
                                    if (a.TipoIndice == 3)
                                    {
                                        long agregar = Convert.ToInt64(values[0]);
                                        if (!secuencial.ContainsKey(agregar))
                                        {
                                            secuencial.Add(agregar, new List<object>());
                                        }
                                        secuencial[agregar].Add(value);
                                        tipoInd.Add(a);
                                    }
                                }
                                values[i] = value;
                                i++;
                            }
                            byte[] arrBytes = new byte[8];
                            reader.Read(arrBytes, 0, 8);
                            dir = BitConverter.ToInt64(arrBytes, 0);
                            sigDato = reader.BaseStream.Position;
                            values[i] = dir.ToString();
                            aux.DirSigReg = dir;
                            aux2.DirSigReg = dir;
                            listDat.Add(aux);
                            listDatOriginal.Add(aux2);
                            gridDatos.Rows.Add(values);
                            reader.Read(arrBytes, 0, 8);
                            auxDato = BitConverter.ToInt64(arrBytes, 0);
                        }
                        else
                        {
                            reader.BaseStream.Position = sigDato + 8;
                            foreach (Atributo a in lAtributos)
                            {
                                reader.BaseStream.Position += a.Longitud;
                            }
                            reader.BaseStream.Position += 8;
                            sigDato = reader.BaseStream.Position;
                            byte[] arrBytes = new byte[8];
                            reader.Read(arrBytes, 0, 8);
                            auxDato = BitConverter.ToInt64(arrBytes, 0);
                        }
                    }
                    calcularDirecciones();
                    reader.Close();
                }
            }
            catch (Exception excep)
            {

            }
        }

        public string BinaryToString(byte[] entBytes)
        {
            return Encoding.ASCII.GetString(entBytes);
        }

        public void crearArchivo()
        {
            //Formato de Serializacion/Deserializacion
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            sRuta = sDirectorio + "\\" + sEntidad.Replace(" ", "") + ".dat";
            var fi = new System.IO.FileInfo(@sRuta);
            //Crea el archivo
            using (var binaryFile = fi.Create())
            {
                //Serializa el diccionario con el formato binario
                binaryFile.Flush();
                currentDir = 0;
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            actualizarGridDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Registro eliminar = null;
                long dirElim = Convert.ToInt64(gridDatos.SelectedCells[0].Value);
                foreach (Registro r in listDat)
                {
                    if (dirElim == r.Direccion)
                    {
                        eliminar = r;
                        break;
                    }
                }
                if (eliminar.Direccion == listDatOriginal[0].Direccion)
                {
                    dirInicio = listDatOriginal[0].DirSigReg + 1;
                }
                else
                {
                    foreach (Registro r in listDatOriginal)
                    {
                        if (r.DirSigReg + 1 == eliminar.Direccion)
                        {
                            modificarSig(r.Direccion, eliminar.Direccion);
                            break;
                        }
                    }
                }
                modificarDirec(eliminar.Direccion);
                LeerDatos(dirInicio);
            }
            catch (Exception ex)
            {

            }
        }

        private void modificarDirec(long direc)
        {
            using (Stream stream = File.Open(sRuta, FileMode.Open))
            {
                stream.Position = direc;
                stream.Write(BitConverter.GetBytes((long)-1), 0, 8);
                stream.Close();
            }
        }

        private void modificarSig(long dir, long sigDir)
        {
            long direc = -1;
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                byte[] dirBytes = new byte[8];
                reader.BaseStream.Seek(sigDir + longitudTot - 7, SeekOrigin.Begin);
                reader.Read(dirBytes, 0, 8);
                direc = BitConverter.ToInt64(dirBytes, 0);
            }

            using (Stream stream = File.Open(sRuta, FileMode.Open))
            {
                stream.Position = dir + longitudTot - 7;
                stream.Write(BitConverter.GetBytes((long)direc), 0, 8);
                stream.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ventana_Indice visIndice = new Ventana_Indice(lAtributos, secuencial, sDirectorio, sEntidad, tipoInd, arbolito, salva);
            visIndice.Show();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            long dirEdit = Convert.ToInt64(gridDatos.SelectedCells[0].Value) + 8;
            foreach (Atributo a in lAtributos)
            {
                if (a.TipoIndice != 0)
                {
                    if (a.TipoDato == 'C')
                    {
                        string dato = Interaction.InputBox("Value: ", a.Nombre, "").PadRight(a.Longitud);
                        dato = dato.Substring(0, a.Longitud);
                        sobreEscribir(dirEdit, dato);
                    }
                    else
                    {
                        int dato = Int32.Parse(Interaction.InputBox("Value: ", a.Nombre, ""));
                        sobreEscribir(dirEdit, dato);
                    }
                    dirEdit += a.Longitud;
                }
                else
                {
                    dirEdit += a.Longitud;
                }
            }
            LeerDatos(dirInicio);
        }

        private void sobreEscribir(long direcDato, int dato)
        {
            using (Stream stream = File.Open(sRuta, FileMode.Open))
            {
                stream.Position = direcDato;
                stream.Write(BitConverter.GetBytes(dato), 0, 4);
                stream.Close();
            }
        }

        private void sobreEscribir(long direcDato, string dato)
        {
            using (Stream stream = File.Open(sRuta, FileMode.Open))
            {
                stream.Position = direcDato;
                byte[] s = StringToBinary(dato, dato.Length);
                stream.Write(s, 0, s.Length);
                stream.Close();
            }
        }

        public byte[] StringToBinary(string texto, int longi)
        {
            byte[] lb = new byte[longi];

            for (int i = 0; i < longi; i++)
            {
                lb[i] = Convert.ToByte(texto[i]);
            }
            return lb;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            importarArchivo();
        }

        private void importarArchivo()
        {
            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Comma Separated Values File (*.csv)|*.csv";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            string ruta = "";
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        ruta = Path.GetFullPath(openFileDialog.FileName);
                        myStream.Close();
                    }
                }
                string[] archivo = File.ReadAllLines(ruta);
                var primeraLinea = archivo[0].Split(',');
                bool primero = false;
                if (int.TryParse(primeraLinea[0], out int n))
                {
                    primero = true;
                }
                foreach (var l in archivo)
                {
                    if (currentDir > 0)
                    {
                        guardarSiguiente();
                    }
                    guardarDato(currentDir);
                    currentDir += 8;
                    var values = l.Split(',');
                    foreach (Atributo a in lAtributos)
                    {
                        if (primero)
                        {
                            if (a.TipoDato == 'E')
                            {
                                int v = Int32.Parse(values[0]);
                                if (a.TipoIndice == 1)
                                {
                                    listDat.Add(new Registro(currentDir, v, (long)-1));
                                    clavebusent = true;
                                }
                                guardarDato(v);
                            }
                            else
                            {
                                string v = values[1].PadRight(a.Longitud);
                                v = v.Substring(0, a.Longitud);
                                guardarDato(v);
                                if (a.TipoIndice == 1)
                                {
                                    listDat.Add(new Registro(currentDir, v, (long)-1));
                                    clavebusent = false;
                                }
                            }
                        }
                        else
                        {
                            if (a.TipoDato == 'E')
                            {
                                int v = Int32.Parse(values[1]);
                                if (a.TipoIndice == 1)
                                {
                                    listDat.Add(new Registro(currentDir, v, (long)-1));
                                    clavebusent = true;
                                }
                                guardarDato(v);
                            }
                            else
                            {
                                string v = values[0].PadRight(a.Longitud);
                                v = v.Substring(0, a.Longitud);
                                guardarDato(v);
                                if (a.TipoIndice == 1)
                                {
                                    listDat.Add(new Registro(currentDir, v, (long)-1));
                                    clavebusent = false;
                                }
                            }
                        }
                        currentDir += a.Longitud;
                    }
                    guardarDato((long)-1);
                    currentDir += 8;
                    LeerDatos(dirInicio);
                }
            }
            catch (Exception ecx)
            {
                MessageBox.Show("El archivo no existe.");
            }
        }
    }

}
