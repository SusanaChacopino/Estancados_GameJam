using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_TextosNivel1 : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuaje;
    public TextMeshProUGUI textoPuntosParaGanar;

    public SCR_ColisionesPuntos colisiones;
    public SCR_GestorNiveles gestionarNiv;
    public float puntuaje;
    void Start()
    {
        int meta = gestionarNiv.puntosParaGanar;
        textoPuntosParaGanar.text = meta.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        puntuaje = colisiones.ObtenerPuntaje();
        textoPuntuaje.text = puntuaje.ToString();
    }
}
