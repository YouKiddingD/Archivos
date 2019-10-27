using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    public class nodoArbol
    {
        public long direccion;
        public char Tipo = ' ';
        public long[] apuntadores = new long[5];
        public List<int> valores = new List<int>();

        public nodoArbol(long direc)
        {
            direccion = direc;
            inicializarApuntadores();
        }

        public void insertarDato(int dato)
        {
            try
            {
                valores.Add(dato);
                valores.Sort();
            }
            catch (Exception e)
            {

            }
        }

        public long buscarPosicion(int dato)
        {
            int i = 0;
            int aux = -1;
            foreach (int ent in valores)
            {
                if (dato < ent)
                {
                    return apuntadores[i];
                }
                i++;
            }
            if (dato > valores.Last<int>())
            {
                aux = valores.IndexOf(valores.Last<int>());
                return apuntadores[aux + 1];
            }
            return -1;
        }

        public void recorrerApuntadores(int index)
        {
            long[] auxis = new long[5];
            for(int i=index;i<4;i++)
            {
                auxis[i+1] = apuntadores[i];
            }
            for(int i=index;i<5;i++)
            {
                apuntadores[i] = auxis[i];
            }
        }

        public void inicializarApuntadores()
        {
            for (int i = 0; i < 5; i++)
            {
                apuntadores[i] = -1;
            }
        }
    }
}
