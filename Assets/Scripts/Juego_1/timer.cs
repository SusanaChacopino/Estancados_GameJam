using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    [Header("Configuración")]
    public RectTransform lenguaRect;
    public float tiempoTotal = 100f;

    private float anchoInicial;
    private float tiempoRestante;
    private bool tiempoTerminado = false;

    public SCR_ColisionesPuntos sistemaPuntos;
    public SCR_MenuController menuController;

    void Start()
    {
        if (SCR_RachaTiempo.instance != null)
        {
        int juegos = SCR_RachaTiempo.instance.juegosGanados;

        //Divide los juegosGanados entre 3 y guarda solo el numero entero ejemp. 1/3 = 0
        int reducciones = juegos / 3;

        // Reduce el tiempo a la mitad multiplicando el numero de rachas siempre elevado por 2
        tiempoTotal = tiempoTotal /= Mathf.Pow(2, reducciones);

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

            float porcentaje = tiempoRestante / tiempoTotal;
            float nuevoAncho = anchoInicial * porcentaje;

            lenguaRect.sizeDelta = new Vector2(nuevoAncho, lenguaRect.sizeDelta.y);
        }
        else
        {
            tiempoTerminado = true;

            Debug.Log("Se acabó el tiempo");
            lenguaRect.sizeDelta = new Vector2(0, lenguaRect.sizeDelta.y);

            menuController.LoadScene(4);
        }
    }
}
