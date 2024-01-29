using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum Algoritmo
{
    PERLIN_NOISE, PERLIN_NOISE_SUAVIZADO
}
/// <summary>
/// Controlador
/// </summary>
public class Generador : MonoBehaviour
{
    [Header("Caracteristicas del mapa")]
    [SerializeField] private Tilemap mapaDeLosetas;
    [SerializeField] private TileBase loseta;

    [Header("Dimensiones del mapa")]
    [SerializeField] private int ancho = 60;
    [SerializeField] private int alto = 30;

    [Header("Semilla")]
    [SerializeField] private bool semillaAleatoria = true;
    [SerializeField] private float semilla = 0f;

    [Header("Perlin Noise suavizado")]
    [SerializeField] private int intervalo = 1;

    [Header("Elegir Algoritmo")]
    [SerializeField] private Algoritmo algoritmo = Algoritmo.PERLIN_NOISE;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerarMapa();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LimpiarMapa();
        }
    }

    public void GenerarMapa()
    {
        Debug.Log("Estoy en generar mapa");

        int[,] mapa = null;

        mapaDeLosetas.ClearAllTiles();

        if (semillaAleatoria)
        {
            semilla = Random.Range(0f, 1000f);
        }

        //mapa = Algoritmos.GenerarArray(ancho, alto, false);
        mapa = Algoritmos.GenerarArray(ancho, alto, true);

        switch (algoritmo)
        {
            case Algoritmo.PERLIN_NOISE:
                {
                    mapa = Algoritmos.PerlinNoise(mapa, semilla);
                    break;
                }
            case Algoritmo.PERLIN_NOISE_SUAVIZADO:
                {
                    mapa = Algoritmos.PerlinNoise(mapa, semilla, intervalo);
                    break;
                }

        }

        // dibujar el mapa
        VisualizarMapa.MostrarMapa(mapa, mapaDeLosetas, loseta);
    }

    public void LimpiarMapa()
    {
        Debug.Log("Estoy en LIMPIAR mapa");

        VisualizarMapa.LimpiarMapa(mapaDeLosetas);
    }
}
