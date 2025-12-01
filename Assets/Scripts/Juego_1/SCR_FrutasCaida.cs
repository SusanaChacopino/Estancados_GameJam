using UnityEngine;
using UnityEngine.Rendering;

public class SCR_FrutasCaida : MonoBehaviour
{
    [Header("Parametros")]
    [Tooltip("Altura Y en la que el objeto se destruye automáticamente")]
    public float alturaDestruccion = -12f;

    // Variable estática compartida por TODAS las instancias
    public static float velocidadGlobal = 3f;

    void Update()
    {
        // Mover hacia abajo usando la velocidad global compartida
        transform.Translate(Vector3.down * velocidadGlobal * Time.deltaTime);

        // Destruir si sale de la pantalla por abajo
        if (transform.position.y < alturaDestruccion)
        {
            Destroy(gameObject);
        }
    }
}

