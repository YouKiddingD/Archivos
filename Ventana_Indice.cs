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

namespace Archivos
{
    public partial class Ventana_Indice : Form
    {
        public List<Atributo> listAttr = new List<Atributo>();
        public Dictionary<long, List<object>> dReg = new Dictionary<long, List<object>>();
        public Indice ArchivoInd;
        private string sDirectorio = "", sRuta = "", sEntidad = "";
        private List<Atributo> tipoInd = new List<Atributo>();
        public List<int> arbol = new List<int>();
        Dictionary<int, long> salva = new Dictionary<int, long>();

        public Ventana_Indice(List<Atributo> listAtr, Dictionary<long, List<object>> dicReg, string sruta, string ne, List<Atributo> esPrim, List<int> arb, Dictionary<int, long> sal)
        {
            InitializeComponent();
            listAttr = listAtr;
            dReg = dicReg;
            sDirectorio = sruta;
            arbol = arb;
            salva = sal;
            sEntidad = ne;
            tipoInd = esPrim;
            crearArchivo();
            ArchivoInd = new Indice(listAttr, sRuta, tipoInd);
            crearArchivoInd();
            inicializarGrids();
            crearPrimeraTabla();
        }

        public void inicializarGrids()
        {
            gridPrimerBloque.Columns.Add("Val", "Valor");
            gridPrimerBloque.Columns.Add("Direc", "Direccion");
            gridSegundoBloque.Columns.Add("Val", "Valor");
            gridSegundoBloque.Columns.Add("Direc", "Direccion");
            gridSecPrimBloq.Columns.Add("Val", "Valor");
            gridSecPrimBloq.Columns.Add("Direc", "Direccion");
            gridSecSegBloq.Columns.Add("Direc", "Direccion");
            gridArbol.Columns.Add("Direc", "Direccion");
            gridArbol.Columns.Add("Tip","Tipo");
            bool apun = true;
            for(int i=0;i<9;i++)
            {
                if(apun)
                {
                    gridArbol.Columns.Add("Apun", "Apuntador");
                    apun = false;
                }
                else
                {
                    gridArbol.Columns.Add("val", "Valor");
                    apun = true;
                }
            }
        }

        public void crearPrimeraTabla()
        {
            List<long> direcs = new List<long>();
            List<long> direcsSec = new List<long>();
            bool nofue = true;
            int inti = 0;
            foreach (long dirO in dReg.Keys)
            {
                foreach (object o in dReg[dirO])
                {
                    if (tipoInd[inti].TipoIndice == 2)
                    {
                        ArchivoInd.insertarPrimario(o, dirO);
                    }
                    else
                    {
                        if (tipoInd[inti].TipoIndice == 3)
                        {
                            ArchivoInd.insertarSecundario(tipoInd[inti], o, dirO);
                        }
                    }
                    inti++;
                }
            }
            foreach (Atributo a in listAttr)
            {
                if (a.TipoIndice == 4)
                {
                    ArchivoInd.crearArbol(a, arbol,salva);
                }
            }
            crearArchivoInd();
            ArchivoInd.guardarArchivo();
            foreach (string[] s in ArchivoInd.leerPrimerBloqueGeneral())
            {
                if (!s[0].Contains('\0'))
                {
                    gridPrimerBloque.Rows.Add(s);
                }
            }
            foreach (Atributo a in listAttr)
            {
                foreach (string[] s in ArchivoInd.leerArbol(a))
                {
                    gridArbol.Rows.Add(s);
                }
            }
            ArchivoInd.direccionesSecEnt.Clear();
            ArchivoInd.secundariosEnt.Clear();
            ArchivoInd.direccionesSeCad.Clear();
            ArchivoInd.secundariosCad.Clear();
            foreach (Atributo a in listAttr)
            {
                //gridSecSegBloq.Rows.Add("-----------------");
                if (a.TipoIndice == 3)
                {
                    foreach (string[] s in ArchivoInd.leerBloqSecGeneral(a))
                    {
                        gridSecPrimBloq.Rows.Add(s);
                    }
                }
            }
            foreach (Atributo a in listAttr)
            {
                //gridSecSegBloq.Rows.Add("-----------------");
                if (a.TipoIndice == 3)
                {
                    direcsSec = ArchivoInd.regresaDirecsSec(a);
                    int asd = 0;
                    foreach (string[] s in ArchivoInd.leerSegSecGeneral(a))
                    {
                        gridSecSegBloq.Rows.Add("Direccion: " + direcsSec[asd]);
                        foreach (string ese in s)
                        {
                            if (Int64.Parse(ese) < 10000 && Int64.Parse(ese) >= 0)
                            {
                                gridSecSegBloq.Rows.Add(ese);
                            }
                        }
                        asd++;
                    }
                }
            }
            direcs = ArchivoInd.regresarDirecs();
            int i = 0;
            foreach (List<string[]> l in ArchivoInd.leerSegundoBloqueGeneral())
            {
                if (direcs[i] != -1)
                {
                    gridSegundoBloque.Rows.Add(direcs[i].ToString());
                }
                foreach (string[] s in l)
                {
                    if (nofue)
                    {
                        nofue = false;
                        gridSegundoBloque.Rows.Add(s);
                    }
                    else
                    {
                        if (s[0] != "0" && !s[0].Contains('\0'))
                        {
                            gridSegundoBloque.Rows.Add(s);
                        }
                    }
                }
                i++;
            }
        }

        public void crearArchivoInd()
        {
            crearArchivo();
            try
            {
                using (var stream = new FileStream(sRuta, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var writer = new BinaryWriter(stream))
                {
                    foreach (Atributo a in listAttr)
                    {
                        if (a.TipoIndice == 2)
                        {
                            stream.Position = a.DirIndice;
                            if (ArchivoInd.primarioDigito)
                            {
                                foreach (int l in ArchivoInd.digitos.Keys)
                                {
                                    writer.Write(l);
                                    writer.Write(ArchivoInd.digitos[l]);
                                }
                            }
                            else
                            {
                                foreach (char l in ArchivoInd.abc.Keys)
                                {
                                    writer.Write(l);
                                    writer.Write(ArchivoInd.abc[l]);
                                }
                            }
                        }
                        else
                        {
                            if (a.TipoIndice == 3)
                            {
                                stream.Position = a.DirIndice;
                                if (a.TipoDato == 'E')
                                {
                                    foreach (int e in ArchivoInd.secundariosEnt[a].Keys)
                                    {
                                        writer.Write(e);
                                        writer.Write(ArchivoInd.direccionesSecEnt[a][e]);
                                    }
                                }
                                else
                                {
                                    foreach (string s in ArchivoInd.secundariosCad[a].Keys)
                                    {
                                        byte[] se = StringToBinary(s, s.Length);
                                        stream.Write(se, 0, s.Length);
                                        writer.Write(ArchivoInd.direccionesSeCad[a][s]);
                                    }
                                }
                            }
                        }
                    }
                    writer.Close();
                    stream.Close();
                }
            }
            catch (Exception excp)
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                gridArbol.Visible = true;
            }
            else
            {
                gridArbol.Visible = false;
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

        public void crearArchivo()
        {
            //Formato de Serializacion/Deserializacion
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            sRuta = sDirectorio + "\\" + sEntidad.Replace(" ", "") + ".idx";
            var fi = new System.IO.FileInfo(@sRuta);
            //Crea el archivo
            using (var binaryFile = fi.Create())
            {
                //Serializa el diccionario con el formato binario
                binaryFile.Flush();
                //currentDir = 0;
            }
        }


    }
}
