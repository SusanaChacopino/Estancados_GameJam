using UnityEngine;
using TMPro;

public class SCR_UI_TiempoRacha : MonoBehaviour
{
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoTiempo;

    public SCR_RachaTiempo rachaTiempo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActualizarPuntos();
        ActualizarTiempo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarPuntos()
    {
        textoPuntos.text = rachaTiempo.juegosGanados.ToString();
    }

    // ---- FUNCIÓN PARA ACTUALIZAR EL TEXTO DE TIEMPO ----
    public void ActualizarTiempo()
    {
        textoTiempo.text = rachaTiempo.ObtenerTiempo();
    }
}
