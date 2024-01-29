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




}
