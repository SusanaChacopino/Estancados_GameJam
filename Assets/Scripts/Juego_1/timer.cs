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
