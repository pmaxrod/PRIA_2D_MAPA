using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generador : MonoBehaviour
{
    [Header("Caracteristicas del mapa")]
    [SerializeField] private Tilemap mapaDeLosetas;
    [SerializeField] private TileBase loseta;

    [Header("Dimensiones del mapa")]
    [SerializeField] private int ancho = 60;
    [SerializeField] private int alto = 40;




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

    public void GenerarMapa() {
        Debug.Log("Estoy en generar mapa");
    }

    public void LimpiarMapa()
    {
        Debug.Log("Estoy en LIMPIAR mapa");
    }
}
