using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Esta clase es el Editor del componente o script Generador
// Paquetes: 2D Sprite, 2D Tilemap Editor y 2D Tilemap Extras

[CustomEditor(typeof(Generador))]
public class GeneradorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Dibuja los componentes de los atributos públicos de la clase Generador
        base.OnInspectorGUI();

        Generador generador = (Generador)target;

        if (GUILayout.Button("Generar Mapa"))
        {
            generador.GenerarMapa();
        }

        if (GUILayout.Button("Limpiar Mapa"))
        {
            generador.LimpiarMapa();
        }
    }
}
