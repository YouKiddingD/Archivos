using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    public class Indice
    {
        public List<Atributo> listaAttr = new List<Atributo>();
        public Dictionary<int, long> digitos = new Dictionary<int, long>();
        public Dictionary<char, long> abc = new Dictionary<char, long>();
        public Dictionary<Atributo, SortedDictionary<string, List<long>>> secundariosCad = new Dictionary<Atributo, SortedDictionary<string, List<long>>>();
        public Dictionary<Atributo, SortedDictionary<int, List<long>>> secundariosEnt = new Dictionary<Atributo, SortedDictionary<int, List<long>>>();
        public Dictionary<long, Dictionary<object, long>> segundoBloque = new Dictionary<long, Dictionary<object, long>>();
        public Dictionary<Atributo, Dictionary<int, long>> direccionesSecEnt = new Dictionary<Atributo, Dictionary<int, long>>();
        public Dictionary<Atributo, Dictionary<string, long>> direccionesSeCad = new Dictionary<Atributo, Dictionary<string, long>>();
        public List<nodoArbol> ArbolB = new List<nodoArbol>();
        public bool primarioDigito = true;
        public long currentDir = 0;
        public string sRuta = "";
        public Atributo modelo;
        private List<Atributo> tipoInd = new List<Atributo>();

        public Indice(List<Atributo> lAtributos, string ruta, List<Atributo> tipoInda)
        {
            listaAttr = lAtributos;
            checarTipo();
            sRuta = ruta;
            tipoInd = tipoInda;
        }

        public void insertarPrimario(object o, long dirO)
        {
            if (o is string)
            {
                string cad = (string)o;
                if (abc[cad[0]] == -1)
                {
                    abc[cad[0]] = crearBloque();
                }
                if (!segundoBloque[abc[cad[0]]].ContainsKey(cad))
                {
                    segundoBloque[abc[cad[0]]].Add(cad, dirO);
                }
            }
            else
            {
                int ent = (int)o;
                int primerDigit = Int32.Parse(ent.ToString()[0].ToString());
                if (digitos[primerDigit] == -1)
                {
                    digitos[primerDigit] = crearBloque();
                }
                segundoBloque[digitos[primerDigit]].Add(ent, dirO);
            }
        }

        public void crearBloqueAtributo(Atributo a)
        {
            if (a.TipoDato == 'E')
            {
                if (!secundariosEnt.ContainsKey(a))
                {
                    secundariosEnt.Add(a, new SortedDictionary<int, List<long>>());
                    a.DirIndice = currentDir;
                }
            }
            else
            {
                if (!secundariosCad.ContainsKey(a))
                {
                    secundariosCad.Add(a, new SortedDictionary<string, List<long>>());
                    a.DirIndice = currentDir;
                }
            }
            currentDir += ((a.Longitud + 8) * 200);
        }

        public void insertarSecundario(Atributo a, object o, long dirO)
        {
            if (a.TipoDato == 'C')
            {
                string cad = (string)o;
                if (!secundariosCad.ContainsKey(a))
                {
                    crearBloqueAtributo(a);
                    direccionesSeCad.Add(a, new Dictionary<string, long>());
                }
                if (!secundariosCad[a].ContainsKey(cad))
                {
                    secundariosCad[a].Add(cad, new List<long>());
                    direccionesSeCad[a].Add(cad, currentDir);
                    currentDir += 400;
                }
                secundariosCad[a][cad].Add(dirO);
            }
            else
            {
                int ent = (int)o;
                if (!secundariosEnt.ContainsKey(a))
                {
                    crearBloqueAtributo(a);
                    direccionesSecEnt.Add(a, new Dictionary<int, long>());
                }
                if (!secundariosEnt[a].ContainsKey(ent))
                {
                    secundariosEnt[a].Add(ent, new List<long>());
                    direccionesSecEnt[a].Add(ent, currentDir);
                    currentDir += 400;
                }
                secundariosEnt[a][ent].Add(dirO);
            }
        }

        public List<long> regresaDirecsSec(Atributo a)
        {
            List<long> direcs = new List<long>();
            if (a != null)
            {
                if (a.TipoDato == 'E')
                {
                    foreach (long l in direccionesSecEnt[a].Values)
                    {
                        direcs.Add(l);
                    }
                }
                else
                {
                    foreach (long l in direccionesSeCad[a].Values)
                    {
                        direcs.Add(l);
                    }
                }
            }
            return direcs;
        }

        public List<long> regresarDirecs()
        {
            List<long> direcs = new List<long>();
            if (modelo != null)
            {
                if (modelo.TipoDato == 'E')
                {
                    foreach (long l in digitos.Values)
                    {
                        direcs.Add(l);
                    }
                }
                else
                {
                    foreach (long l in abc.Values)
                    {
                        direcs.Add(l);
                    }
                }
            }
            return direcs;
        }

        public List<string[]> leerArbol(Atributo a)
        {
            List<string[]> values = new List<string[]>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                reader.BaseStream.Position = 0;
                int i = 0;
                while (reader.BaseStream.Position < /*(a.DirIndice + (3 * 65))*/reader.BaseStream.Length)
                {
                    byte[] atrBytes = new byte[63];
                    reader.Read(atrBytes, 0, 8);
                    long i1 = BitConverter.ToInt64(atrBytes, 0);
                    reader.Read(atrBytes, 0, 1);
                    string i2 = BinaryToString(atrBytes)[0].ToString();
                    reader.Read(atrBytes, 0, 8);
                    long i3 = BitConverter.ToInt64(atrBytes, 0);
                    reader.Read(atrBytes, 0, 4);
                    int i4 = BitConverter.ToInt32(atrBytes, 0);
                    reader.Read(atrBytes, 0, 8);
                    long i5 = BitConverter.ToInt64(atrBytes, 0);
                    reader.Read(atrBytes, 0, 4);
                    int i6 = BitConverter.ToInt32(atrBytes, 0);
                    reader.Read(atrBytes, 0, 8);
                    long i7 = BitConverter.ToInt64(atrBytes, 0);
                    reader.Read(atrBytes, 0, 4);
                    int i8 = BitConverter.ToInt32(atrBytes, 0);
                    reader.Read(atrBytes, 0, 8);
                    long i9 = BitConverter.ToInt64(atrBytes, 0);
                    reader.Read(atrBytes, 0, 4);
                    int i10 = BitConverter.ToInt32(atrBytes, 0);
                    reader.Read(atrBytes, 0, 8);
                    long i11 = BitConverter.ToInt64(atrBytes, 0);
                    if (!((i4 == i6) && i6 == i8 && i8 == i10 && i10 == -1))
                    {
                        values.Add(new string[11]);
                        values[i][0] = i1.ToString();
                        values[i][1] = i2;
                        values[i][2] = i3.ToString();
                        values[i][3] = i4.ToString();
                        values[i][4] = i5.ToString();
                        values[i][5] = i6.ToString();
                        values[i][6] = i7.ToString();
                        values[i][7] = i8.ToString();
                        values[i][8] = i9.ToString();
                        values[i][9] = i10.ToString();
                        values[i][10] = i11.ToString();
                        i++;
                    }
                }
                reader.Close();
            }
            return values;
        }

        public void crearArbol(Atributo a, List<int> llego, Dictionary<int, long> sal)
        {
            ArbolB.Clear();
            a.DirIndice = currentDir;
            int nodoAct = 0;
            ArbolB.Add(new nodoArbol(a.DirIndice));
            ArbolB[0].Tipo = 'E';
            currentDir += 65;
            List<int> enteros = new List<int>();
            foreach (int i in llego)
            {
                enteros.Add(i);
            }
            foreach (int ent in llego)
            {
                if (ArbolB[0].valores.Count < 4)
                {
                    ArbolB[0].insertarDato(ent);
                    enteros.Remove(ent);
                }
                else
                {
                    break;
                }
            }
            foreach (int ent in enteros)
            {
                while (true)
                {
                    if (ArbolB[nodoAct].valores.Count < 4 && ArbolB[nodoAct].Tipo == 'H')
                    {
                        ArbolB[nodoAct].insertarDato(ent);
                        nodoAct = buscarRaiz();
                        break;
                    }
                    else
                    {
                        long nuevaDirec = ArbolB[nodoAct].buscarPosicion(ent);
                        //Caso donde el nodo a dividir es la raiz
                        if (nuevaDirec == -1)
                        {
                            if (ArbolB[nodoAct].Tipo == 'E')
                            {
                                casoEspecial(nodoAct, ent);
                                nodoAct = buscarRaiz();
                                break;
                            }
                            if (ArbolB[nodoAct].Tipo == 'H')
                            {
                                casoHoja(nodoAct, ent);
                                nodoAct = buscarRaiz();
                                break;
                            }
                        }
                        else
                        {
                            nodoAct = buscarIndiceNodo(nuevaDirec);
                        }
                    }
                }
            }

        }

        public int buscarRaiz()
        {
            int i = -1;
            foreach (nodoArbol nA in ArbolB)
            {
                i++;
                if (nA.Tipo == 'R')
                {
                    return i;
                }
            }
            return -1;
        }

        public void casoEspecial(int nodoAct, int ent)
        {
            ArbolB.Add(new nodoArbol(currentDir));
            int raiz = buscarIndiceNodo(currentDir);
            currentDir += 65;
            ArbolB[raiz].Tipo = 'R';
            ArbolB.Add(new nodoArbol(currentDir));
            int hermano = buscarIndiceNodo(currentDir);
            currentDir += 65;
            ArbolB[hermano].Tipo = 'H';
            ArbolB[nodoAct].Tipo = 'H';
            int subir = regresarSubir(ArbolB[nodoAct].valores, ent);
            dividirNodoHoja(nodoAct, hermano);
            conQuien(nodoAct, hermano, ent, subir);
            ArbolB[hermano].insertarDato(subir);
            ArbolB[raiz].insertarDato(subir);
            int aux = ArbolB[raiz].valores.IndexOf(subir);
            ArbolB[raiz].apuntadores[aux] = ArbolB[nodoAct].direccion;
            ArbolB[raiz].apuntadores[aux + 1] = ArbolB[hermano].direccion;
        }

        public void conQuien(int original, int nuevo, int ent, int subir)
        {
            if (ent < subir)
            {
                ArbolB[original].insertarDato(ent);
            }
            else
            {
                if (ent > subir)
                {
                    ArbolB[nuevo].insertarDato(ent);
                }
            }
        }

        public void casoRaiz(int nodoAct, int ent, int nuevo)
        {
            List<long> com = new List<long>();
            if (ArbolB[nodoAct].valores.Count >= 4)
            {
                com = buscarPosicion(nodoAct, ArbolB[nuevo].valores[0], ArbolB[nuevo].direccion);
                ArbolB.Add(new nodoArbol(currentDir));
                int raiz = buscarIndiceNodo(currentDir);
                currentDir += 65;
                ArbolB[raiz].Tipo = 'R';
                ArbolB.Add(new nodoArbol(currentDir));
                int hermano = buscarIndiceNodo(currentDir);
                currentDir += 65;
                ArbolB[nodoAct].Tipo = 'I';
                ArbolB[hermano].Tipo = 'I';
                int subir = regresarSubir(ArbolB[nodoAct].valores, ent);
                dividirNodoRaiz(nodoAct, hermano,com);
                conQuien(nodoAct, hermano, ent, subir);
                if (ArbolB[nodoAct].valores.Contains(subir))
                {
                    ArbolB[nodoAct].valores.Remove(subir);
                }
                else
                {
                    if (ArbolB[hermano].valores.Contains(subir))
                    {
                        ArbolB[hermano].valores.Remove(subir);
                    }
                }
                ArbolB[raiz].insertarDato(subir);
                int aux = ArbolB[raiz].valores.IndexOf(subir);
                ArbolB[raiz].apuntadores[aux] = ArbolB[nodoAct].direccion;
                ArbolB[raiz].apuntadores[aux + 1] = ArbolB[hermano].direccion;
            }
            else
            {
                ArbolB[nodoAct].insertarDato(ent);
                int aux = ArbolB[nodoAct].valores.IndexOf(ent)+1;
                ArbolB[nodoAct].recorrerApuntadores(aux);
                ArbolB[nodoAct].apuntadores[aux] = ArbolB[nuevo].direccion;
            }
        }

        public List<long> buscarPosicion(int nodoAct,int dato, long insertar)
        {
            List<long> com = new List<long>();
            int index = 0;
            foreach (long l in ArbolB[nodoAct].apuntadores)
            {
                com.Add(l);
            }
            if (dato>ArbolB[nodoAct].valores.Last<int>())
            {
                com.Add(insertar);
                return com;
            }
            else
            {
                for(int i=0;i<4;i++)
                {
                    if(dato<ArbolB[nodoAct].valores[i])
                    {
                        index = i;
                        break;
                    }
                }
            }
            com.Insert(index, insertar);
            return com;
        }

        public void casoIntermedio(int nodoAct, int ent, int nuevo)
        {
            List<long> com = new List<long>();
            if (ArbolB[nodoAct].valores.Count >= 4)
            {
                int padre = buscarPadre(ArbolB[nodoAct].direccion);
                int subir = regresarSubir(ArbolB[nodoAct].valores, ent);
                com = buscarPosicion(nodoAct, ArbolB[nuevo].valores[0],ArbolB[nuevo].direccion);
                ArbolB.Add(new nodoArbol(currentDir));
                int hermano = buscarIndiceNodo(currentDir);
                currentDir += 65;
                ArbolB[hermano].Tipo = 'I';
                dividirNodoIntermedio(nodoAct, hermano,com);
                conQuien(nodoAct, hermano, ent, subir);
                if(ArbolB[nodoAct].valores.Contains(subir))
                {
                    ArbolB[nodoAct].valores.Remove(subir);
                }
                else
                {
                    if (ArbolB[hermano].valores.Remove(subir));
                }
                if (ArbolB[padre].Tipo == 'R')
                {
                    casoRaiz(padre, subir,hermano);
                }
                else
                {
                    casoIntermedio(padre,subir,hermano);
                }
            }
            else
            {
                ArbolB[nodoAct].insertarDato(ent);
                int aux= ArbolB[nodoAct].valores.IndexOf(ent) +1;
                ArbolB[nodoAct].recorrerApuntadores(aux);
                ArbolB[nodoAct].apuntadores[aux] = ArbolB[nuevo].direccion;
            }
        }

        public void casoHoja(int nodoAct, int ent)
        {
            if (ArbolB[nodoAct].valores.Count >= 4)
            {
                int padre = buscarPadre(ArbolB[nodoAct].direccion);
                ArbolB.Add(new nodoArbol(currentDir));
                int nuevo = buscarIndiceNodo(currentDir);
                currentDir += 65;
                ArbolB[nuevo].Tipo = 'H';
                int subir = regresarSubir(ArbolB[nodoAct].valores, ent);
                dividirNodoHoja(nodoAct, nuevo);
                conQuien(nodoAct, nuevo, ent, subir);
                if (ArbolB[nuevo].valores.Count == 2)
                {
                    ArbolB[nuevo].insertarDato(subir);
                }
                if (ArbolB[nodoAct].valores.Contains(subir))
                {
                    ArbolB[nodoAct].valores.Remove(subir);
                }
                if (ArbolB[padre].Tipo == 'R')
                {
                    casoRaiz(padre, subir,nuevo);
                }
                else
                {
                    casoIntermedio(padre,subir,nuevo);
                }
            }
            else
            {
                ArbolB[nodoAct].insertarDato(ent);
            }
        }

        public int buscarPadre(long direc)
        {
            int i = -1;
            foreach (nodoArbol nA in ArbolB)
            {
                i++;
                foreach (long l in nA.apuntadores)
                {
                    if (l == direc)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int regresarSubir(List<int> vals, int ent)
        {
            List<int> aux = new List<int>();
            foreach (int i in vals)
            {
                aux.Add(i);
            }
            aux.Add(ent);
            aux.Sort();
            return aux[2];
        }

        public void dividirNodoIntermedio(int original, int nuevo, List<long> direcciones)
        {
            for(int i = 2; i<4; i++)
            {
                ArbolB[nuevo].insertarDato(ArbolB[original].valores[i]);
            }
            for(int i=3;i>1;i--)
            {
                ArbolB[original].valores.Remove(ArbolB[original].valores[i]);
            }
            ArbolB[original].inicializarApuntadores();
            ArbolB[original].apuntadores[0] = direcciones[0];
            ArbolB[original].apuntadores[1] = direcciones[1];
            ArbolB[original].apuntadores[2] = direcciones[2];
            ArbolB[nuevo].apuntadores[0] = direcciones[3];
            ArbolB[nuevo].apuntadores[1] = direcciones[4];
            ArbolB[nuevo].apuntadores[2] = direcciones[5];
        }

        public void dividirNodoHoja(int original, int nuevo)
        {
            for (int i = 2; i < 4; i++)
            {
                ArbolB[nuevo].insertarDato(ArbolB[original].valores[i]);
            }
            for (int i = 3; i > 1; i--)
            {
                ArbolB[original].valores.Remove(ArbolB[original].valores[i]);
            }
        }

        public void dividirNodoRaiz(int original, int nuevo, List<long> direcciones)
        {
            for (int i = 2; i < 4; i++)
            {
                ArbolB[nuevo].insertarDato(ArbolB[original].valores[i]);
            }
            for (int i = 3; i > 1; i--)
            {
                ArbolB[original].valores.Remove(ArbolB[original].valores[i]);
            }
            ArbolB[original].inicializarApuntadores();
            ArbolB[original].apuntadores[0] = direcciones[0];
            ArbolB[original].apuntadores[1] = direcciones[1];
            ArbolB[original].apuntadores[2] = direcciones[2];
            ArbolB[nuevo].apuntadores[0] = direcciones[3];
            ArbolB[nuevo].apuntadores[1] = direcciones[4];
            ArbolB[nuevo].apuntadores[2] = direcciones[5];
        }

        public int buscarIndiceNodo(long direc)
        {
            int index = -1;
            foreach (nodoArbol nA in ArbolB)
            {
                index++;
                if (nA.direccion == direc)
                {
                    break;
                }
            }
            return index;
        }

        public List<string[]> leerSegBloqSecEnt(Atributo a)
        {
            List<string[]> values = new List<string[]>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                int i = 0;
                foreach (int ent in direccionesSecEnt[a].Keys)
                {
                    if (direccionesSecEnt[a][ent] != -1)
                    {
                        int j = 0;
                        reader.BaseStream.Position = direccionesSecEnt[a][ent];
                        values.Add(new string[50]);
                        while (reader.BaseStream.Position < direccionesSecEnt[a][ent] + 400)
                        {
                            byte[] dirBytes = new byte[8];
                            reader.Read(dirBytes, 0, 8);
                            values[i][j] = BitConverter.ToInt64(dirBytes, 0).ToString();
                            j++;
                        }
                    }
                    i++;
                }
                reader.Close();
            }
            return values;
        }

        public List<string[]> leerSegBloqSecCad(Atributo a)
        {
            List<string[]> values = new List<string[]>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                int i = 0;
                foreach (string cad in direccionesSeCad[a].Keys)
                {
                    if (direccionesSeCad[a][cad] != -1)
                    {
                        int j = 0;
                        reader.BaseStream.Position = direccionesSeCad[a][cad];
                        values.Add(new string[50]);
                        while (reader.BaseStream.Position < direccionesSeCad[a][cad] + 400)
                        {
                            byte[] dirBytes = new byte[8];
                            reader.Read(dirBytes, 0, 8);
                            values[i][j] = BitConverter.ToInt64(dirBytes, 0).ToString();
                            j++;
                        }
                    }
                    i++;
                }
                reader.Close();
            }
            return values;
        }

        public List<string[]> leerBloqSecEnt(Atributo a)
        {
            List<string[]> values = new List<string[]>();
            if (!direccionesSecEnt.ContainsKey(a))
            {
                direccionesSecEnt.Add(a, new Dictionary<int, long>());
            }
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                reader.BaseStream.Position = a.DirIndice;
                int i = 0;
                while (reader.BaseStream.Position < a.DirIndice + ((a.Longitud + 8) * 50))
                {
                    values.Add(new string[2]);
                    byte[] datoBytes = new byte[4];
                    reader.Read(datoBytes, 0, 4);
                    values[i][0] = BitConverter.ToInt32(datoBytes, 0).ToString();
                    byte[] dirBytes = new byte[8];
                    reader.Read(dirBytes, 0, 8);
                    values[i][1] = BitConverter.ToInt64(dirBytes, 0).ToString();
                    int agregar = Int32.Parse(values[i][0]);
                    if (!direccionesSecEnt[a].ContainsKey(agregar) && agregar != 0)
                    {
                        direccionesSecEnt[a].Add(Int32.Parse(values[i][0]), Int64.Parse(values[i][1]));
                    }
                    i++;
                }
                reader.Close();
            }
            return values;
        }

        public List<string[]> leerBloqSecCad(Atributo a)
        {
            List<string[]> values = new List<string[]>();
            if (!direccionesSeCad.ContainsKey(a))
            {
                direccionesSeCad.Add(a, new Dictionary<string, long>());
            }
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                reader.BaseStream.Position = a.DirIndice;
                int i = 0;
                while (reader.BaseStream.Position < a.DirIndice + ((a.Longitud + 8) * 200))
                {
                    values.Add(new string[2]);
                    byte[] datoBytes = new byte[a.Longitud];
                    reader.Read(datoBytes, 0, a.Longitud);
                    values[i][0] = BinaryToString(datoBytes);
                    byte[] dirBytes = new byte[8];
                    reader.Read(dirBytes, 0, 8);
                    values[i][1] = BitConverter.ToInt64(dirBytes, 0).ToString();
                    if (!direccionesSeCad[a].ContainsKey(values[i][0]))
                    {
                        direccionesSeCad[a].Add(values[i][0], Int64.Parse(values[i][1]));
                    }
                    i++;
                }
                reader.Close();
            }
            return values;
        }

        public List<string[]> leerBloqSecGeneral(Atributo a)
        {
            if (a != null)
            {
                if (a.TipoDato == 'E')
                {
                    return leerBloqSecEnt(a);
                }
                else
                {
                    return leerBloqSecCad(a);
                }
            }
            return new List<string[]>();
        }

        public List<string[]> leerSegSecGeneral(Atributo a)
        {
            if (a != null)
            {
                if (a.TipoDato == 'E')
                {
                    return leerSegBloqSecEnt(a);
                }
                else
                {
                    return leerSegBloqSecCad(a);
                }
            }
            return new List<string[]>();
        }

        public List<string[]> leerPrimerBloqueGeneral()
        {
            if (modelo != null)
            {
                if (modelo.TipoDato == 'E')
                {
                    return leerPrimerBloqueEntero();
                }
                else
                {
                    return leerPrimerBloqueCadena();
                }
            }
            return new List<string[]>();
        }

        public List<List<string[]>> leerSegundoBloqueGeneral()
        {
            if (modelo != null)
            {
                if (modelo.TipoDato == 'E')
                {
                    return leerSegundoBloqueEntero();
                }
                else
                {
                    return leerSegundoBloqueCadena();
                }
            }
            return new List<List<string[]>>();
        }

        public List<List<string[]>> leerSegundoBloqueEntero()
        {
            List<List<string[]>> values = new List<List<string[]>>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                for (int i = 1; i < 10; i++)
                {
                    values.Add(new List<string[]>());
                    if (digitos[i] != -1)
                    {
                        int aux = 0, j = 0;
                        reader.BaseStream.Position = digitos[i];
                        while (aux < 1200)
                        {
                            values[i - 1].Add(new string[2]);
                            byte[] datoBytes = new byte[4];
                            reader.Read(datoBytes, 0, 4);
                            values[i - 1][j][0] = BitConverter.ToInt32(datoBytes, 0).ToString();
                            byte[] dirBytes = new byte[8];
                            reader.Read(dirBytes, 0, 8);
                            values[i - 1][j][1] = BitConverter.ToInt64(dirBytes, 0).ToString();
                            aux += 12;
                            j++;
                        }
                    }
                }
                reader.Close();
            }
            return values;
        }

        public List<List<string[]>> leerSegundoBloqueCadena()
        {
            List<List<string[]>> values = new List<List<string[]>>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                int i = 1;
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    values.Add(new List<string[]>());
                    if (abc[c] != -1)
                    {
                        int aux = 0, j = 0;
                        reader.BaseStream.Position = abc[c];
                        while (aux < (modelo.Longitud + 8) * 100)
                        {
                            values[i - 1].Add(new string[2]);
                            byte[] datoBytes = new byte[modelo.Longitud];
                            reader.Read(datoBytes, 0, modelo.Longitud);
                            values[i - 1][j][0] = BinaryToString(datoBytes);
                            byte[] dirBytes = new byte[8];
                            reader.Read(dirBytes, 0, 8);
                            values[i - 1][j][1] = BitConverter.ToInt64(dirBytes, 0).ToString();
                            aux += (modelo.Longitud + 8);
                            j++;
                        }
                    }
                    i++;
                }
                reader.Close();
            }
            return values;
        }

        public List<string[]> leerPrimerBloqueEntero()
        {
            digitos.Clear();
            List<string[]> values = new List<string[]>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                reader.BaseStream.Position = modelo.DirIndice;
                while (reader.BaseStream.Position < 108)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        values.Add(new string[2]);
                        byte[] datoBytes = new byte[4];
                        reader.Read(datoBytes, 0, 4);
                        values[i][0] = BitConverter.ToInt32(datoBytes, 0).ToString();
                        byte[] dirBytes = new byte[8];
                        reader.Read(dirBytes, 0, 8);
                        values[i][1] = BitConverter.ToInt64(dirBytes, 0).ToString();
                        digitos.Add(Int32.Parse(values[i][0]), Int64.Parse(values[i][1]));
                    }
                }
                reader.Close();
            }
            return values;
        }

        public List<string[]> leerPrimerBloqueCadena()
        {
            abc.Clear();
            List<string[]> values = new List<string[]>();
            using (BinaryReader reader = new BinaryReader(new FileStream(sRuta, FileMode.Open)))
            {
                reader.BaseStream.Position = modelo.DirIndice;
                while (reader.BaseStream.Position < 234)
                {
                    for (int i = 0; i < 26; i++)
                    {
                        values.Add(new string[2]);
                        byte[] datoBytes = new byte[1];
                        reader.Read(datoBytes, 0, 1);
                        values[i][0] = BinaryToString(datoBytes);
                        byte[] dirBytes = new byte[8];
                        reader.Read(dirBytes, 0, 8);
                        values[i][1] = BitConverter.ToInt64(dirBytes, 0).ToString();
                        abc.Add(values[i][0][0], Int64.Parse(values[i][1]));
                    }
                }
                reader.Close();
            }
            return values;
        }

        public string BinaryToString(byte[] entBytes)
        {
            return Encoding.ASCII.GetString(entBytes);
        }

        public void llenarBlancos(long start, long final)
        {
            using (Stream stream = File.Open(sRuta, FileMode.Open))
            {
                stream.Position = start;
                while (stream.Position < final)
                {
                    stream.Write(Encoding.ASCII.GetBytes('\0'.ToString()), 0, 1);
                }
                stream.Close();
            }
        }

        public void guardarArchivo()
        {
            //try
            //{
            foreach (Atributo a in listaAttr)
            {
                if (a.TipoIndice == 2)
                {
                    foreach (long l in segundoBloque.Keys)
                    {
                        using (Stream stream = File.Open(sRuta, FileMode.Open))
                        {
                            stream.Position = l;
                            foreach (object o in segundoBloque[l].Keys)
                            {
                                if (o is int)
                                {
                                    int obj = (int)o;
                                    stream.Write(BitConverter.GetBytes(obj), 0, 4);
                                    stream.Write(BitConverter.GetBytes(segundoBloque[l][o]), 0, 8);
                                }
                                else
                                {
                                    string obj = (string)o;
                                    byte[] s = StringToBinary(obj, obj.Length);
                                    stream.Write(s, 0, obj.Length);
                                    stream.Write(BitConverter.GetBytes(segundoBloque[l][o]), 0, 8);
                                }
                            }
                            stream.Position = currentDir - 1;
                            stream.Write(Encoding.ASCII.GetBytes(' '.ToString()), 0, 1);
                            stream.Close();
                        }
                    }
                }
                else
                {
                    if (a.TipoIndice == 3)
                    {
                        if (a.TipoDato == 'E')
                        {
                            using (Stream stream = File.Open(sRuta, FileMode.Open))
                            {
                                long aux = a.DirIndice;
                                foreach (int e in secundariosEnt[a].Keys)
                                {
                                    stream.Position = aux;
                                    stream.Write(BitConverter.GetBytes(e), 0, 4);
                                    stream.Write(BitConverter.GetBytes(direccionesSecEnt[a][e]), 0, 8);
                                    aux = stream.Position;
                                    stream.Position = direccionesSecEnt[a][e];
                                    foreach (long l in secundariosEnt[a][e])
                                    {
                                        stream.Write(BitConverter.GetBytes(l), 0, 8);
                                    }
                                }
                                stream.Position = currentDir - 1;
                                stream.Write(Encoding.ASCII.GetBytes(' '.ToString()), 0, 1);
                                //llenarBlancos(stream.Position, currentDir);
                                stream.Close();
                            }
                        }
                        else
                        {
                            using (Stream stream = File.Open(sRuta, FileMode.Open))
                            {
                                long aux = a.DirIndice;
                                foreach (string s in secundariosCad[a].Keys)
                                {
                                    stream.Position = aux;
                                    byte[] se = StringToBinary(s, s.Length);
                                    stream.Write(se, 0, s.Length);
                                    stream.Write(BitConverter.GetBytes(direccionesSeCad[a][s]), 0, 8);
                                    aux = stream.Position;
                                    stream.Position = direccionesSeCad[a][s];
                                    foreach (long l in secundariosCad[a][s])
                                    {
                                        stream.Write(BitConverter.GetBytes(l), 0, 8);
                                    }
                                }
                                stream.Position = currentDir - 1;
                                stream.Write(Encoding.ASCII.GetBytes(' '.ToString()), 0, 1);
                                //llenarBlancos(stream.Position, currentDir);
                                stream.Close();
                            }
                        }
                    }
                    else
                    {
                        if (a.TipoIndice == 4)
                        {
                            using (Stream stream = File.Open(sRuta, FileMode.Open))
                            {
                                foreach (nodoArbol nA in ArbolB)
                                {
                                    stream.Position = nA.direccion;
                                    stream.Write(BitConverter.GetBytes(nA.direccion), 0, 8);
                                    stream.Write(Encoding.ASCII.GetBytes(nA.Tipo.ToString()), 0, 1);
                                    for (int i = 0; i < 4; i++)
                                    {
                                        stream.Write(BitConverter.GetBytes(nA.apuntadores[i]), 0, 8);
                                        if (nA.valores.Count > i)
                                        {
                                            stream.Write(BitConverter.GetBytes(nA.valores[i]), 0, 4);
                                        }
                                        else
                                        {
                                            stream.Write(BitConverter.GetBytes(-1), 0, 4);
                                        }
                                    }
                                    stream.Write(BitConverter.GetBytes(nA.apuntadores[4]), 0, 8);
                                }
                            }
                        }
                    }
                }
            }
            //}
            //catch (Exception excp)
            //{

            //}
        }

        public long crearBloque()
        {
            long aux = currentDir;
            segundoBloque.Add(currentDir, new Dictionary<object, long>() { });
            currentDir += ((modelo.Longitud + 8) * 100);
            return aux;
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

        public void checarTipo()
        {
            foreach (Atributo a in listaAttr)
            {
                if (a.TipoIndice == 2)
                {
                    modelo = a;
                    modelo.DirIndice = currentDir;
                    if (a.TipoDato == 'E')
                    {
                        primarioDigito = true;
                        for (int i = 1; i < 10; i++)
                        {
                            digitos.Add(i, -1);
                        }
                        currentDir = 108;
                    }
                    if (a.TipoDato == 'C')
                    {
                        primarioDigito = false;
                        for (char c = 'A'; c <= 'Z'; c++)
                        {
                            abc.Add(c, -1);
                        }
                        currentDir = 234;
                    }
                }
            }
        }
    }
}
