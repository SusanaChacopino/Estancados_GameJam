using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_GestorNiveles : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Puntos necesarios para pasar al siguiente nivel")]
    public int puntosParaGanar = 20;

    [Tooltip("Nombre de la escena del siguiente nivel")]
    public string nombreSiguienteNivel = "Nivel2";

    [Header("Referencias")]
    [Tooltip("Script que maneja los puntos")]
    public SCR_ColisionesPuntos scriptPuntos;

    [Header("Información")]
    [SerializeField]
    [Tooltip("Solo para visualizar en el Inspector")]
    private bool nivelCompletado = false;

    void Start()
    {
        // Buscar automáticamente el script de puntos si no está asignado
        if (scriptPuntos == null)
        {
            scriptPuntos = FindFirstObjectByType<SCR_ColisionesPuntos>();
        }

        if (scriptPuntos == null)
        {
            Debug.LogError("[GestorNiveles] No se encontró SCR_ColisionesPuntos en la escena");
        }

        nivelCompletado = false;
    }

    void Update()
    {
        VerificarVictoria();
    }

    void VerificarVictoria()
    {
        // Si ya completamos el nivel, no verificar más
        if (nivelCompletado) return;

        // Si no hay script de puntos, no podemos verificar
        if (scriptPuntos == null) return;

        // Obtener puntos actuales
        float puntosActuales = scriptPuntos.ObtenerPuntaje();

        // ¿Llegó al límite?
        if (puntosActuales >= puntosParaGanar)
        {
            NivelCompletado();
        }
    }

    void NivelCompletado()
    {
        nivelCompletado = true;

        Debug.Log($"[Nivel] ¡Completado! Pasando a {nombreSiguienteNivel}");

        // Cargar siguiente nivel
        CargarSiguienteNivel();
    }

    void CargarSiguienteNivel()
    {
        // Verificar que la escena existe
        if (Application.CanStreamedLevelBeLoaded(nombreSiguienteNivel))
        {
            SceneManager.LoadScene(nombreSiguienteNivel);
        }
        else
        {
            Debug.LogError($"[GestorNiveles] La escena '{nombreSiguienteNivel}' no existe o no está añadida al Build Settings");
            Debug.Log("[GestorNiveles] Ve a File → Build Settings y añade la escena");
        }
    }

    // Método público para cargar nivel manualmente (útil para botones)
    public void CargarNivelManual(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    // Método para reiniciar el nivel actual
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}