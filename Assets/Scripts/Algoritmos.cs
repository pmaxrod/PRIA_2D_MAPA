using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Modelo
/// </summary>
public class Algoritmos
{
    public static int[,] GenerarArray(int _ancho, int _alto, bool _vacio)
    {
        int[,] mapa = new int[_ancho, _alto];

        for (int x = 0; x < _ancho; x++)
        {
            for (int y = 0; y < _alto; y++)
            {
                mapa[x, y] = _vacio ? 0 : 1;
                /*
                 if (_vacio)
                    mapa[x, y] = 0;
                 else
                    mapa[x, y] = 1;
                */
            }
        }
        return mapa;
    }

    #region algoritmos

    public static int[,] PerlinNoise(int[,] _mapa, float _semilla)
    {
        // altura a la que va a ir el suelo;
        int alturaPunto;

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // conseguir la altura de la posicion X
            alturaPunto = Mathf.FloorToInt(Mathf.PerlinNoise(x, _semilla) * _mapa.GetUpperBound(1));

            for (int y = alturaPunto; y >= 0; y--)
            {
                _mapa[x, y] = 1;
            }
        }

        return _mapa;
    }
    public static int[,] PerlinNoise(int[,] _mapa, float _semilla, int _intervalo)
    {
        if (_intervalo > 1)
        {
            Vector2Int posActual, posAnterior;

            // Los puntos correspondientes para el suavizado
            // Una lista para EJE
            List<int> ruidoX = new List<int>();
            List<int> ruidoY = new List<int>();

            int alturaPunto, puntos;

            // Generar el ruido
            //
            for (int x = 0; x <= _mapa.GetUpperBound(0) + _intervalo; x += _intervalo)
            {
                alturaPunto = Mathf.FloorToInt(Mathf.PerlinNoise(x, _semilla) * _mapa.GetUpperBound(1));

                ruidoX.Add(x);
                ruidoY.Add(alturaPunto);
            }

            puntos = ruidoY.Count;

            // empezamos en la primera posicion para asi tener disponible una

            for (int i = 1; i < puntos; i++)
            {
                posActual = new Vector2Int(ruidoX[i], ruidoY[i]);
                posAnterior = new Vector2Int(ruidoX[i - 1], ruidoY[i - 1]);

                // calcular la recta que une los dos puntos
                Vector2 diferencia = posActual - posAnterior;

                // Cuanto hay que bajar en cada paso, en cada X
                float cambioAltura = diferencia.y / _intervalo;

                // guardar la altura actual;
                float alturaActual = posAnterior.y;

                for (int x = posAnterior.x; x < posActual.x && x <= _mapa.GetUpperBound(0); x++)
                {
                    for (int y = Mathf.FloorToInt(alturaActual); y >= 0; y--)
                    {
                        _mapa[x, y] = 1;
                    }
                    alturaActual += cambioAltura;
                }
            }

        }
        else
        {
            _mapa = PerlinNoise(_mapa, _semilla);
        }
        return _mapa;
    }

    //----------------------------------------------------------
    public static int[,] RandomWalk(int[,] _mapa, float _semilla)
    {
        // La semilla de nuestro random
        Random.InitState(_semilla.GetHashCode()); // obtiene el valor entero del valor real

        // Altura desde la cual vamos a dibujar el mapa
        int ultimaAltura = Random.Range(0, _mapa.GetUpperBound(1));

        // Recorremos el mapa a lo ancho - X
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // 0 Sube, 1 Baja, 2 igual
            int sigMovimiento = Random.Range(0, 3);

            // SUBIR
            if (sigMovimiento == 0 && ultimaAltura < _mapa.GetUpperBound(1))
            {
                ultimaAltura++;
            }
            // BAJAR
            else if (sigMovimiento == 1 && ultimaAltura > 0)
            {
                ultimaAltura--;
            }

            // no cambia la altura si sigMovimiento == 2

            for (int y = ultimaAltura; y >= 0; y--)
            {
                _mapa[x, y] = 1;
            }
        }
        return _mapa;
    }

    //----------------------------------------------------------------------
    public static int[,] RandomWalk(int[,] _mapa, float _semilla, int _minimoAnchoSeccion)
    {
        // La semilla de nuestro random
        Random.InitState(_semilla.GetHashCode()); // obtiene el valor entero del valor real

        // Altura desde la cual vamos a dibujar el mapa
        int ultimaAltura = Random.Range(0, _mapa.GetUpperBound(1));

        // llevar la centa del ancho de la seccion actual
        int anchoSeccion = 0;

        // Recorremos el mapa a lo ancho --> X
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            if (anchoSeccion >= _minimoAnchoSeccion)
            {
                // 0 Sube, 1 Baja, 2 igual
                int sigMovimiento = Random.Range(0, 3);

                // SUBIR
                if (sigMovimiento == 0 && ultimaAltura < _mapa.GetUpperBound(1))
                {
                    ultimaAltura++;
                }
                // BAJAR
                else if (sigMovimiento == 1 && ultimaAltura > 0)
                {
                    ultimaAltura--;
                }

                // empezamos una nueva seccion de suelo con la misma altura
                anchoSeccion = 0; // volvemo a poner a cero la seccion
            }
            //Henmos procesado otro bloque de la seccion actual
            anchoSeccion++;

            // rellenamos de suelo desde la ultimaAltura hasta abajo
            for (int y = ultimaAltura; y >= 0; y--)
            {
                _mapa[x, y] = 1;
            }
        }
        return _mapa;
    }
    #endregion

    #region cuevas

    //------------------------------------------------------------------
    public static int[,] PerlinNoise_Cueva(int[,] _mapa, float _modificador, bool _bordesSonMuros)
    {
        // almacena si hay que poner hueco o suelo

        int nuevoPunto;

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // comprobar si los bordes son muros o no

                if (_bordesSonMuros && (x == 0 || y == 0 ||
                    x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    _mapa[x, y] = 1;
                }
                else
                {
                    // se redonde el resultado de la funcion PerlinNoise
                    nuevoPunto = Mathf.RoundToInt(Mathf.PerlinNoise(x * _modificador, y * _modificador));
                    _mapa[x, y] = nuevoPunto;
                }
            }
        }
        return _mapa;
    }

    //-------------------------------------------------------------------------------------------
    // desplazar el mapa seg�n los valores de los offSet
    /// <summary>
    /// Crea una cueva usando el Perlin Noise para el proceso de generaci�n
    /// </summary>
    /// <param name="_mapa"> El mapa que se va a modificar</param>
    /// <param name="_modificador">El valor por el cual multiplicamos la posicion para obtener un valor del Perlin Noise. Mapa mas grande o mas pequeno</param>
    /// <param name="_bordesSonMuros">Si es verdadero, los bordes serian muros</param>
    /// <param name="_offSetX">Desplazamiento en X para el Perlin Noise</param>
    /// <param name="_offSetY">Desplazamiento en Y para el Perlin Noise</param>
    /// <param name="_semilla">Se usara para situarnos en un X,Y  (X = Y = semilla) en el Perlin Noise</param>
    /// <returns>El mapa con la cueva generada con el Perlin Noise</returns>
    public static int[,] PerlinNoise_Cueva(int[,] _mapa, float _modificador, bool _bordesSonMuros,
        float _offSetX = 0f, float _offSetY = 0f, float _semilla = 0f)
    {
        // almacena si hay que poner hueco o suelo

        int nuevoPunto;

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // comprobar si los bordes son muros o no

                if (_bordesSonMuros && (x == 0 || y == 0 || x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    _mapa[x, y] = 1;
                }
                else
                {
                    // se redonde el resultado de la funcion PerlinNoise
                    nuevoPunto = Mathf.RoundToInt(Mathf.PerlinNoise(x * _modificador + _offSetX + _semilla,
                        y * _modificador + _offSetY + _semilla));
                    _mapa[x, y] = nuevoPunto;
                }
            }
        }

        return _mapa;
    }

    //--------------------------------------------------------------------
    public static int[,] RandomWalk_Cueva(int[,] _mapa, float _semilla, float _porcentajeSueloEliminar,
        bool _bordesSonMuros = true, bool _movDiagonal = false)
    {
        // Las semilla de nuestro Random
        Random.InitState(_semilla.GetHashCode());


        // Definimos los limites
        int vMin = 0;
        int vMaxX = _mapa.GetUpperBound(0);
        int vMaxY = _mapa.GetUpperBound(1);
        int ancho = _mapa.GetUpperBound(0) + 1;
        int alto = _mapa.GetUpperBound(1) + 1;

        // mejora - bordes son muros
        // excluimos los bordes del radio de acci�n
        if (_bordesSonMuros)
        {
            vMin++;
            vMaxX--;
            vMaxY--;

            ancho -= 2;
            alto -= 2;
        }

        // Definir la posicion de inicio en X y en Y
        int posX = Random.Range(vMin, vMaxX);
        int posY = Random.Range(vMin, vMaxY);

        // redondeamos el valor
        int cantidadLosetasEliminar = Mathf.FloorToInt(ancho * alto * _porcentajeSueloEliminar);

        // para contar las losetas que llevamos eliminadas
        int losetasEliminadas = 0;

        while (losetasEliminadas < cantidadLosetasEliminar)
        {
            // si hay suelo
            if (_mapa[posX, posY] == 1)
            {
                _mapa[posX, posY] = 0; // eliminamos loseta
                losetasEliminadas++;
            }

            if (_movDiagonal)
            {
                // si nos podemos mover en diagonal
                int dirAleatoriaX = Random.Range(-1, 2); // devuelve -1, 0 o 1
                int dirAleatoriaY = Random.Range(-1, 2); // devuelve -1, 0 o 1

                posX += dirAleatoriaX;
                posY += dirAleatoriaY;

            }
            else
            {
                // direccion aleatoria a seguir
                int dirAleatoria = Random.Range(0, 4);
                switch (dirAleatoria)
                {
                    case 0:
                        // Arriba
                        posY++;
                        break;
                    case 1:
                        // Abajo
                        posY--;
                        break;
                    case 2:
                        // Izquierda
                        posX--;
                        break;
                    case 3:
                        // derecha
                        posX++;
                        break;
                }
            }
            // ver los limites del mapa
            // para asegurarnos que no salimos del area de trabajo
            posX = Mathf.Clamp(posX, vMin, vMaxX);
            posY = Mathf.Clamp(posY, vMin, vMaxY);

        } // fin del while

        return _mapa;
    }

    //-------------------------------------------------------------------
    public static int[,] TunelDireccional(int[,] _mapa, float _semilla, int _anchoMin,
        int _anchoMax, float _aspereza, int _desplazamientoMax, float _desplazamiento)
    {
        // este valor va desde su valor en negativo hasta el valor positivo
        // en este caso, con el valor 1, el cncho del tunel es 3 ( -1, 0, 1)
        int anchoTunel = 1;

        // posicion de comienzo del mapa
        //int x = _mapa.GetUpperBound(0) / 2; // la mitad del mapa
        int x = Random.Range(0, _mapa.GetUpperBound(0)); // 

        // la semilla de nuestro random
        Random.InitState(_semilla.GetHashCode());

        // recorremos la Y, ya que el tunel es vertical
        for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
        {
            // Generamos esta parte del tunel
            for (int i = -anchoTunel; i <= anchoTunel; i++)
            {
                _mapa[x + i, y] = 0; // quitar el suelo
            }

            // una _aspereza del 0, significa que no queremos que cambie el ancho del tunel
            // le damos la vuelta a _aspereza, para que cambie 1- 0, nunca cambia ya que 
            // el valor de Random.value nunca va a ser mayor que 1
            if (Random.value > 1 - _aspereza)
            {
                // cambio del ancho
                // Obtener aleatoriamente la cantidad que cambiaremos el ancho
                int cambioAncho = Random.Range(-_anchoMax, _anchoMax);
                anchoTunel += cambioAncho;

                // limitar el valor, para que este entre los limites establecidos
                anchoTunel = Mathf.Clamp(anchoTunel, _anchoMin, _anchoMax);
            }

            // Comprobar si cambiar la posicion central del tunel
            if (Random.value > 1 - _desplazamiento)
            {
                // valor aleatorio de desplazamiento de la posicion central
                int cambioDesplazamiento = Random.Range(-_desplazamientoMax, _desplazamientoMax + 1);

                x += cambioDesplazamiento;

                // la x no puede salir de los limites del mapa
                x = Mathf.Clamp(x, _anchoMax + 1, _mapa.GetUpperBound(0) - _anchoMax);
            }

        }
        return _mapa;
    }

    //-------------------------------------------------------------------
    public static int[,] TunelHorizontal(int[,] _mapa, float _semilla, int _anchoMin,
        int _anchoMax, float _aspereza, int _desplazamientoMax, float _desplazamiento)
    {
        // este valor va desde su valor en negativo hasta el valor positivo
        // en este caso, con el valor 1, el cncho del tunel es 3 ( -1, 0, 1)
        int anchoTunel = 1;

        // posicion de comienzo del mapa
        int y = _mapa.GetUpperBound(1) / 2; // la mitad del mapa

        //int x = Random.Range(0, _mapa.GetUpperBound(0)); // 

        // la semilla de nuestro random
        Random.InitState(_semilla.GetHashCode());

        // recorremos la Y, ya que el tunel es vertical
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // Generamos esta parte del tunel
            for (int i = -anchoTunel; i <= anchoTunel; i++)
            {
                _mapa[x, y + i] = 0; // quitar el suelo
            }

            // una _aspereza del 0, significa que no queremos que cambie el ancho del tunel
            // le damos la vuelta a _aspereza, para que cambie 1- 0, nunca cambia ya que 
            // el valor de Random.value nunca va a ser mayor que 1
            if (Random.value > 1 - _aspereza)
            {
                // cambio del ancho
                // Obtener aleatoriamente la cantidad que cambiaremos el ancho
                int cambioAncho = Random.Range(-_anchoMax, _anchoMax);
                anchoTunel += cambioAncho;

                // limitar el valor, para que este entre los limites establecidos
                anchoTunel = Mathf.Clamp(anchoTunel, _anchoMin, _anchoMax);
            }

            // Comprobar si cambiar la posicion central del tunel
            if (Random.value > 1 - _desplazamiento)
            {
                // valor aleatorio de desplazamiento de la posicion central
                int cambioDesplazamiento = Random.Range(-_desplazamientoMax, _desplazamientoMax + 1);

                y += cambioDesplazamiento;

                // la x no puede salir de los limites del mapa
                y = Mathf.Clamp(y, _anchoMax + 1, _mapa.GetUpperBound(1) - _anchoMax);
            }

        }
        return _mapa;
    }
    #endregion
    
    #region automatas_celulares
    //--------------------------------------------------------------------------------
    public static int[,] GenerarMapaAleatorio(int _ancho, int _alto, float _semilla,
        float _porcentajeRelleno, bool _bordesSonMuros)
    {
        // Establecer la semilla para los numeros aleatorios
        Random.InitState(_semilla.GetHashCode());

        int[,] mapa = new int[_ancho, _alto];

        // recorremos todas las posiciones del mapa
        for (int x = 0; x <= mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= mapa.GetUpperBound(1); y++)
            {
                if (_bordesSonMuros && (x == 0 || y == 0 || x == mapa.GetUpperBound(0)
                    || y == mapa.GetUpperBound(1)))
                {
                    mapa[x, y] = 1;
                }
                else
                {
                    // ponemos suelo si el resultado del random es inferior que el porcentaje
                    mapa[x, y] = (Random.value < _porcentajeRelleno) ? 1 : 0;
                }
            }
        }
        return mapa;
    }

    public static int LosetasVecinas(int[,] _mapa, int _x, int _y, bool _incluirDiagonales)
    {
        // contar las losetas vecinas
        int totalVecinas = 0;

        for (int vecinoX = _x - 1; vecinoX <= _x + 1; vecinoX++)
        {
            for (int vecinoY = _y - 1; vecinoX <= _y + 1; vecinoY++)
            {
                if (vecinoX >= 0 && vecinoX <= _mapa.GetUpperBound(0) && vecinoY >= 0 && vecinoY <= _mapa.GetUpperBound(1))
                {
                    // ignorar la posición de central (_x,_y)

                    // sin incluir las diagonales
                    //      N 
                    // N    T    N
                    //      N

                    // incluyendo las diagonales
                    // N    N    N 
                    // N    T    N
                    // N    N    N

                    if ((vecinoX != _x || vecinoY != _y) && (_incluirDiagonales || (vecinoX == _x || vecinoY == _y)))
                    {
                        // sumar las casillas que tienen 1, y así sabremos las casillas que tienen vecinas
                        totalVecinas += _mapa[vecinoX, vecinoY];
                    }
                }
            }
        }
        return totalVecinas;
    }

    public static int[,] AutomataCelularMoore(int[,] _mapa, int _totalDePasadas, bool _bordesSonMuros)
    {
        for (int i = 0; i < _totalDePasadas; i++)
        {
            for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
                {
                    // Incluye las diagonales
                    int totalVecinas = LosetasVecinas(_mapa, x, y, true);

                    if (_bordesSonMuros && (x == 0 || x == _mapa.GetUpperBound(0) || y == 0 || y == _mapa.GetUpperBound(1)))
                    {
                        _mapa[x, y] = 1;
                    }
                    // Si tenemos más de 4 vecinos, ponemos suelo - VIVE
                    else if (totalVecinas > 4)
                    {
                        _mapa[x, y] = 1;
                    }
                    else if (totalVecinas < 4)
                    {
                        _mapa[x, y] = 0;
                    }

                    // si tenemos exactamente 4 vecinos, no cambiamos nada
                }
            }
        }
        return _mapa;
    }

    public static int[,] AutomataCelularVonNeuman(int[,] _mapa, int _totalDePasadas, bool _bordesSonMuros)
    {
        for (int i = 0; i < _totalDePasadas; i++)
        {
            for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
                {
                    // Incluye las diagonales
                    int totalVecinas = LosetasVecinas(_mapa, x, y, false);

                    if (_bordesSonMuros && (x == 0 || x == _mapa.GetUpperBound(0) || y == 0 || y == _mapa.GetUpperBound(1)))
                    {
                        _mapa[x, y] = 1;
                    }
                    // Si tenemos más de 2 vecinos, ponemos suelo - VIVE
                    else if (totalVecinas > 2)
                    {
                        _mapa[x, y] = 1;
                    }
                    else if (totalVecinas < 2)
                    {
                        _mapa[x, y] = 0;
                    }

                    // si tenemos exactamente 2 vecinos, no cambiamos nada
                }
            }
        }
        return _mapa;
    }
    #endregion
}
