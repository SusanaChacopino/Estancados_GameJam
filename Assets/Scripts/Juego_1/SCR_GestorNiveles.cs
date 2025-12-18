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
    [Tooltip("Script que maneja los puntos (Nivel 1 y 2)")]
    public SCR_ColisionesPuntos scriptPuntos;

    [Tooltip("Script de la rana interceptora (Nivel 3)")]
    public SCR_RanaInterceptora ranaInterceptora; // ← NUEVO

    [Header("Información")]
    [SerializeField]
    [Tooltip("Solo para visualizar en el Inspector")]
    private bool nivelCompletado = false;

    [SerializeField]
    private int aciertosActuales = 0; // ← NUEVO

    void Start()
    {
        // Buscar script de puntos (Niveles 1 y 2)
        if (scriptPuntos == null)
        {
            scriptPuntos = FindFirstObjectByType<SCR_ColisionesPuntos>();
        }

        // Buscar rana interceptora (Nivel 3)
        if (ranaInterceptora == null)
        {
            ranaInterceptora = FindFirstObjectByType<SCR_RanaInterceptora>();
        }

        // Suscribirse a eventos de la barra de ritmo si existe
        if (ranaInterceptora != null && ranaInterceptora.scriptBarraRitmo != null)
        {
            ranaInterceptora.scriptBarraRitmo.OnAcierto.AddListener(ContarAcierto);
            ranaInterceptora.scriptBarraRitmo.OnAciertoPerfecto.AddListener(ContarAcierto);
            Debug.Log("[GestorNiveles] Modo Nivel 3 (Robos) activado");
        }
        else if (scriptPuntos != null)
        {
            Debug.Log("[GestorNiveles] Modo Niveles 1-2 (Puntos) activado");
        }
        else
        {
            Debug.LogWarning("[GestorNiveles] No se encontró sistema de puntos ni rana interceptora");
        }

        nivelCompletado = false;
        aciertosActuales = 0;
    }

    void Update()
    {
        VerificarVictoria();
    }

    void VerificarVictoria()
    {
        if (nivelCompletado) return;

        // Sistema de puntos (Niveles 1 y 2)
        if (scriptPuntos != null)
        {
            float puntosActuales = scriptPuntos.ObtenerPuntaje();
            if (puntosActuales >= puntosParaGanar)
            {
                NivelCompletado();
            }
        }
        // Sistema de aciertos (Nivel 3)
        else if (ranaInterceptora != null)
        {
            if (aciertosActuales >= puntosParaGanar)
            {
                NivelCompletado();
            }
        }
    }

    void ContarAcierto()
    {
        aciertosActuales++;
        Debug.Log($"[GestorNiveles] Aciertos: {aciertosActuales}/{puntosParaGanar}");
    }

    void NivelCompletado()
    {
        nivelCompletado = true;
        Debug.Log($"[Nivel] ¡Completado! Pasando a {nombreSiguienteNivel}");
        CargarSiguienteNivel();
    }

    void CargarSiguienteNivel()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreSiguienteNivel))
        {
            SceneManager.LoadScene(nombreSiguienteNivel);
            SCR_RachaTiempo.instance.SumarVictoria();
        }
        else
        {
            Debug.LogError($"[GestorNiveles] La escena '{nombreSiguienteNivel}' no existe o no está añadida al Build Settings");
        }
    }

    public void CargarNivelManual(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        // Desuscribirse de eventos
        if (ranaInterceptora != null && ranaInterceptora.scriptBarraRitmo != null)
        {
            ranaInterceptora.scriptBarraRitmo.OnAcierto.RemoveListener(ContarAcierto);
            ranaInterceptora.scriptBarraRitmo.OnAciertoPerfecto.RemoveListener(ContarAcierto);
        }
    }
}