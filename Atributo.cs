using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    //Atributo Serializable para guardar y abrir archivos
    public class Atributo
    {
        private string sNombre;
        private long dirAtributo;
        private char cTipo;
        private int iLong;
        public int iTipoIndice;
        private long dirIndice;
        private long dirSigAtributo;

        //Constructor
        public Atributo(string n, char t, int l, int ti, long da)
        {
            sNombre = n;
            dirAtributo = da;
            cTipo = t;
            iLong = l;
            iTipoIndice = ti;
            dirIndice = -1;
            dirSigAtributo = -1;
        }

        public Atributo(string n, long dirAtr, char tipoAtr, int longatr, int tipoIart, long dirInd, long dirSigAtrib)
        {
            sNombre = n;
            dirAtributo = dirAtr;
            cTipo = tipoAtr;
            iLong = longatr;
            iTipoIndice = tipoIart;
            dirIndice = dirInd;
            dirSigAtributo = dirSigAtrib;
        }

        //Editar Atributo. Solo se puede modificar Nombre, tipo, long y tipo de indice siempre y cuando no existan registros de datos
        public bool Modificar(string n, char t, int l, int ti)
        {
            sNombre = n;
            cTipo = t;
            iLong = l;
            iTipoIndice = ti;

            return true;
        }

        #region METODOS DE ACCESO
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

        public char TipoDato
        {
            get
            {
                return cTipo;
            }
            set
            {
                cTipo = value;
            }
        }

        public long DirIndice
        {
            get
            {
                return dirIndice;
            }
            set
            {
                dirIndice = value;
            }
        }

        public int Longitud
        {
            get
            {
                return iLong;
            }
            set
            {
                iLong = value;
            }
        }

        public int TipoIndice
        {
            get
            {
                return iTipoIndice;
            }
        }

        public long Direccion
        {
            set
            {
                dirAtributo = value;
            }
            get
            {
                return dirAtributo;
            }
        }

        public long DirSigAtributo
        {
            set
            {
                dirSigAtributo = value;
            }
            get
            {
                return dirSigAtributo;
            }
        }
        #endregion
    }
}
