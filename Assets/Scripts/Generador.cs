using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum Algoritmo
{
    PERLIN_NOISE, PERLIN_NOISE_SUAVIZADO, RANDOMWALK,
    RANDOMWALK_SUAVISADO,
    PERLIN_NOISE_CUEVA,
    PERLIN_NOISE_CUEVA_MODIFICADO,
    RANDOMWALK_CUEVA,
    TUNELVERTICAL,
    TUNELHORIZONTAL,
    MAPA_ALEATORIO,
    AUTOMATA_CELULARMOORE,
    AUTOMATA_CELULAR_VONNEUMAN

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

    [Header("Algoritmo - RandomWalk suavizado")]
    [SerializeField] private int minimoAnchoSeccion = 2;

    [Header("Cuevas------------------------------")]
    [SerializeField] private bool bordesSonMuros = true;

    [Header("PerlinNoise Cuevas")]
    [SerializeField] private float modificador = 0.1f;
    [SerializeField] private float offSetX = 0f;
    [SerializeField] private float offSetY = 0f;


    [Header("RandomWalk Cueva")]
    [Range(0, 1)] // para asegurarnos que esta entre 0 y 1
    [SerializeField] private float porcentajeEliminar = 0.25f; //  25 %
    [SerializeField] private bool movimientoDiagonal = false;

    [Header("Tunel Direccional")]
    [SerializeField] private int anchoMaximo = 4;
    [SerializeField] private int anchoMinimo = 1; // se quitar� 3 bloques

    [Range(0, 1)] // para asegurarnos que esta entre 0 y 1
    [SerializeField] private float aspereza = 0.75f;

    [SerializeField] private int desplazamientoMaximo = 1;

    [Range(0, 1)] // para asegurarnos que esta entre 0 y 1
    [SerializeField] private float desplazamiento = 0.75f;


    [Header("Automata celular")]
    [Range(0, 1)]
    [SerializeField] private float porcentajeRelleno = 0.45f;

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

        switch (algoritmo)
        {
            case Algoritmo.PERLIN_NOISE:
                {
                    mapa = Algoritmos.GenerarArray(ancho, alto, true);
                    mapa = Algoritmos.PerlinNoise(mapa, semilla);
                    break;
                }
            case Algoritmo.PERLIN_NOISE_SUAVIZADO:
                {
                    mapa = Algoritmos.GenerarArray(ancho, alto, true);
                    mapa = Algoritmos.PerlinNoise(mapa, semilla, intervalo);
                    break;
                }
            case Algoritmo.RANDOMWALK:
                mapa = Algoritmos.GenerarArray(ancho, alto, true);
                mapa = Algoritmos.RandomWalk(mapa, semilla);
                break;

            case Algoritmo.RANDOMWALK_SUAVISADO:
                mapa = Algoritmos.GenerarArray(ancho, alto, true);
                mapa = Algoritmos.RandomWalk(mapa, semilla, minimoAnchoSeccion);
                break;

            case Algoritmo.PERLIN_NOISE_CUEVA:
                mapa = Algoritmos.GenerarArray(ancho, alto, false);
                mapa = Algoritmos.PerlinNoise_Cueva(mapa, modificador, bordesSonMuros);
                break;

            case Algoritmo.PERLIN_NOISE_CUEVA_MODIFICADO:
                mapa = Algoritmos.GenerarArray(ancho, alto, false);
                mapa = Algoritmos.PerlinNoise_Cueva(mapa, modificador, bordesSonMuros, offSetX, offSetY, semilla);
                break;

            case Algoritmo.RANDOMWALK_CUEVA:
                mapa = Algoritmos.GenerarArray(ancho, alto, false);
                mapa = Algoritmos.RandomWalk_Cueva(mapa, semilla, porcentajeEliminar, bordesSonMuros, movimientoDiagonal);
                break;

            case Algoritmo.TUNELVERTICAL:
                mapa = Algoritmos.GenerarArray(ancho, alto, false);
                mapa = Algoritmos.TunelDireccional(mapa, semilla, anchoMinimo, anchoMaximo, aspereza, desplazamientoMaximo, desplazamiento);
                break;

            case Algoritmo.TUNELHORIZONTAL:
                mapa = Algoritmos.GenerarArray(ancho, alto, false);
                mapa = Algoritmos.TunelHorizontal(mapa, semilla, anchoMinimo, anchoMaximo, aspereza, desplazamientoMaximo, desplazamiento);
                break;

            case Algoritmo.MAPA_ALEATORIO:
                mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, porcentajeRelleno,
                    bordesSonMuros);
                break;

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
