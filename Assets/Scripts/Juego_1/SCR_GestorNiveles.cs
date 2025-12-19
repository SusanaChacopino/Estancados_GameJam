using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_GestorNiveles : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public int puntosParaGanar = 20; // Meta para completar nivel

    [Tooltip("Nombre de la escena del siguiente nivel")]
    public string nombreSiguienteNivel = "Nivel2";

    [Header("Referencias")]
    public SCR_ColisionesPuntos scriptPuntos; // Para niveles 1 y 2
    public SCR_RanaInterceptora ranaInterceptora; // Para nivel 3

    [Header("Información")]
    [SerializeField] private bool nivelCompletado = false;
    [SerializeField] private int aciertosActuales = 0;

    void Start()
    {
        // Buscar sistema de puntos (si no está asignado)
        if (scriptPuntos == null)
        {
            scriptPuntos = FindFirstObjectByType<SCR_ColisionesPuntos>();
        }

        // Buscar rana interceptora para nivel 3
        if (ranaInterceptora == null)
        {
            ranaInterceptora = FindFirstObjectByType<SCR_RanaInterceptora>();
        }

        // Suscribirse a eventos de la barra de ritmo (nivel 3)
        if (ranaInterceptora != null && ranaInterceptora.scriptBarraRitmo != null)
        {
            ranaInterceptora.scriptBarraRitmo.OnAcierto.AddListener(ContarAcierto);
            ranaInterceptora.scriptBarraRitmo.OnAciertoPerfecto.AddListener(ContarAcierto);
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

        // Sistema de puntos (niveles 1 y 2)
        if (scriptPuntos != null)
        {
            float puntosActuales = scriptPuntos.ObtenerPuntaje();
            if (puntosActuales >= puntosParaGanar)
            {
                NivelCompletado();
            }
        }
        // Sistema de aciertos (nivel 3)
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
    }

    void NivelCompletado()
    {
        nivelCompletado = true;
        CargarSiguienteNivel();
    }

    void CargarSiguienteNivel()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreSiguienteNivel))
        {
            SceneManager.LoadScene(nombreSiguienteNivel);
            SCR_RachaTiempo.instance.SumarVictoria();
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