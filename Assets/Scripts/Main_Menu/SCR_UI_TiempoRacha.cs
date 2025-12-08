using UnityEngine;
using TMPro;

public class SCR_UI_TiempoRacha : MonoBehaviour
{
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoTiempo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SCR_RachaTiempo.instance != null)
        {
            ActualizarPuntos();
            ActualizarTiempo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarPuntos()
    {
        textoPuntos.text = SCR_RachaTiempo.instance.juegosGanados.ToString();;
    }

    // ---- FUNCIÓN PARA ACTUALIZAR EL TEXTO DE TIEMPO ----
    public void ActualizarTiempo()
    {
        textoTiempo.text = SCR_RachaTiempo.instance.ObtenerTiempo();
    }

     public void EliminarRachaTiempo()
    {
            SCR_RachaTiempo.instance.DestruirObjeto();
            Debug.Log("SCR_RachaTiempo destruido.");
        }
}
