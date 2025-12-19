using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Configuración")]
    public RectTransform lenguaRect;
    public float tiempoTotal = 100f; // Tiempo inicial del temporizador

    private float anchoInicial;
    private float tiempoRestante;
    private bool tiempoTerminado = false;

    public SCR_ColisionesPuntos sistemaPuntos;
    public SCR_MenuController menuController;

    void Start()
    {
        // Reducir tiempo según rachas ganadas (cada 3 juegos ganados = mitad del tiempo)
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
            menuController.LoadScene(4);
        }
    }
}