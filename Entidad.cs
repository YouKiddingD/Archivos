using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    //Atributo Serializable para guardar y abrir archivos
    public class Entidad
    {
        private string sNombre;
        private long dirEntidad;
        private long dirAtributo;
        private long dirRegDat;
        private long dirSigEnt;
        public long dirRealDat;

        //Constructor
        public Entidad(string n, long de)
        {
            sNombre = n;
            dirEntidad = de;
            dirAtributo = -1;
            dirRegDat = -1;
            dirSigEnt = -1;
            dirRealDat = 0;
        }

        public Entidad(string n, long de,long dirAtr,long dirDatos,long sigEnt)
        {
            sNombre = n;
            dirEntidad = de;
            dirAtributo = dirAtr;
            dirRegDat = dirDatos;
            dirSigEnt = sigEnt;
        }

        #region METODOS DE ACCESO
        public void Modificar(string n)
        {
            this.sNombre = n;
        }

        public void GuardarEnt(string directorio)
        {
            //Formato de Serializacion/Deserializacion
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var fi = new System.IO.FileInfo(@directorio + sNombre + ".dat");
            //Crea el archivo
            using (var binaryFile = fi.Create())
            {
                //Serializa el diccionario con el formato binario
                binaryFormatter.Serialize(binaryFile, 1);
                binaryFile.Flush();
            }
        }

        public string Nombre
        {
            get
            {
                return sNombre;
            }
            set
            {
                sNombre = value;
            }
        }

        public long Direccion
        {
            get
            {
                return dirEntidad;
            }
        }

        public long DireccionAtr
        {
            get
            {
                return dirAtributo;
            }
            set
            {
                dirAtributo = value;
            }
        }

        public long DireccionDatos
        {
            get
            {
                return dirRegDat;
            }
            set
            {
                dirRegDat = value;
            }
        }

        public long DirSigEntidad
        {
            set
            {
                dirSigEnt = value;
            }
            get
            {
                return dirSigEnt;
            }
        }
        #endregion
    }
}
