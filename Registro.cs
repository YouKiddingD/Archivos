using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    public class Registro
    {
        public long Direccion;
        public long DirSigReg;
        public int clavebusqent=-1;
        public string clavebusstring = "";

        public Registro(long dir, string clav, long dirSig)
        {
            Direccion = dir;
            clavebusstring = clav;
            DirSigReg = dirSig;
        }

        public Registro()
        {
        }

        public Registro(long dir,int clav, long dirSig)
        {
            Direccion = dir;
            clavebusqent = clav;
            DirSigReg = dirSig;
        }
    }
}
