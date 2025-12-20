using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Configuración")]
    public RectTransform lenguaRect;
    public float tiempoTotal = 100f;

    private float anchoInicial;
    private float tiempoRestante;
    private bool tiempoTerminado = false;

    [Header("Referencias")]
    public SCR_ColisionesPuntos sistemaPuntos;
    public SCR_MenuController menuController;

    void Start()
    {
        // Verificar que lenguaRect existe
        if (lenguaRect == null)
        {
            //Debug.LogError("[Timer] lenguaRect no está asignado en el Inspector");
            enabled = false; // Desactivar el script
            return;
        }

        // Reducir tiempo según rachas ganadas
        if (SCR_RachaTiempo.instance != null)
        {
            int juegos = SCR_RachaTiempo.instance.juegosGanados;
            int reducciones = juegos / 3;
            tiempoTotal = tiempoTotal / Mathf.Pow(2, reducciones);
        }

        tiempoRestante = tiempoTotal;
        anchoInicial = lenguaRect.sizeDelta.x;
    }

    void Update()
    {
        if (lenguaRect == null)
        {
            enabled = false; // Desactiva script si se destruye
            return;
        }

        if (tiempoTerminado) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            // Actualizar ancho de la barra
            float porcentaje = tiempoRestante / tiempoTotal;
            float nuevoAncho = anchoInicial * porcentaje;
            lenguaRect.sizeDelta = new Vector2(nuevoAncho, lenguaRect.sizeDelta.y);
        }
        else
        {
            // Tiempo agotado
            tiempoTerminado = true;
            lenguaRect.sizeDelta = new Vector2(0, lenguaRect.sizeDelta.y);

            if (menuController != null)
            {
                menuController.LoadScene(4);
            }
            else
            {
                //Debug.LogError("[Timer] menuController no está asignado");
            }
        }
    }
}