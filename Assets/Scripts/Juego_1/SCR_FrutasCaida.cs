using UnityEngine;

public class SCR_FrutasCaida : MonoBehaviour
{
    [Header("Parametros")]
    public float alturaDestruccion = -12f; // Altura Y donde se destruye

    // Variable estática compartida por TODAS las frutas
    public static float velocidadGlobal = 3f;

    void Update()
    {
        // Mover hacia abajo
        transform.Translate(Vector3.down * velocidadGlobal * Time.deltaTime);

        // Destruir si sale de pantalla
        if (transform.position.y < alturaDestruccion)
        {
            Destroy(gameObject);
        }
    }
}