using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    [Header("Configuración")]
    public RectTransform lenguaRect; // Arrastramos aquí el RectTransform de la lengua
    public float tiempoTotal = 10f;

    private float anchoInicial;
    private float tiempoRestante;

    void Start()
    {
        tiempoRestante = tiempoTotal;

       
        // sizeDelta.x es el ancho (width)
        anchoInicial = lenguaRect.sizeDelta.x;
    }

    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            // Calculamos el porcentaje de 0 a 1
            float porcentaje = tiempoRestante / tiempoTotal;

            // Calculamos el nuevo ancho basado en el porcentaje
            float nuevoAncho = anchoInicial * porcentaje;

            // Aplicamos el nuevo ancho manteniendo la altura original (.y)
            lenguaRect.sizeDelta = new Vector2(nuevoAncho, lenguaRect.sizeDelta.y);
        }
        else
        {
            // Al finalizar, ancho a 0
            lenguaRect.sizeDelta = new Vector2(0, lenguaRect.sizeDelta.y);
            // Fin del juego...
        }
    }
}