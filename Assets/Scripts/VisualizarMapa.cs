using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Vista
/// </summary>
public class VisualizarMapa
{

    public static void LimpiarMapa(Tilemap _mapaDeLosetas)
    {
        _mapaDeLosetas.ClearAllTiles();
    }

    //---------------------------------------------------//

    public static void MostrarMapa(int[,] _mapa, Tilemap _mapaDeLosetas, TileBase _loseta)
    {
        // GetUpperBound(0) - eje x; GetUpperBound(1) - eje y
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                if (_mapa[x, y] == 1) // 1 tiene suelo, 0 no tiene suelo
                {
                    _mapaDeLosetas.SetTile(new Vector3Int(x, y, 0), _loseta);
                }
            }
        }

    }
}
