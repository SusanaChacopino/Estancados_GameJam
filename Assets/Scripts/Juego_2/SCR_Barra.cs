using UnityEngine;

public class SCR_Barra : MonoBehaviour
{
    public SCR_Puntos puntos;
    public RectTransform spriteRana;
    public RectTransform mascara;

    public float puntosMax = 20f;
    public float spriteRanaPosXMax = 212f;
    public float mascaraWidthMax = 100f;

    float spriteRanaPosXInicio;

    void Start()
    {
        spriteRanaPosXInicio = spriteRana.anchoredPosition.x;
    }

    void Update()
    {
        //Convierte los puntos en un valor entre 0 y 1 dependiendo del maximo
        float ValorPuntos = Mathf.Clamp01(puntos.puntos / puntosMax);
        //Debug.Log(ValorPuntos);

        //mover el sprite de la Rana
        Vector2 pos = spriteRana.anchoredPosition;
        pos.x = Mathf.Lerp(spriteRanaPosXInicio, spriteRanaPosXMax, ValorPuntos);
        spriteRana.anchoredPosition = pos;

        //cambiar ancho de la mascara
        Vector2 width = mascara.sizeDelta;
        width.x = Mathf.Lerp(0, mascaraWidthMax, ValorPuntos);
        mascara.sizeDelta = width;
    }
}
