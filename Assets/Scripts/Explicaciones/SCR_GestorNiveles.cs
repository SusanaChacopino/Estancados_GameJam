using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SCR_GestorNiveles : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public int puntosParaGanar = 20;
    public float puntosParaGanarLVL2 = 20f;

    [Tooltip("Nombre de la escena del siguiente nivel")]
    public string nombreSiguienteNivel = "Nivel2";

    [Header("Referencias")]
    public SCR_ColisionesPuntos scriptPuntos;
    public SCR_Puntos scriptPuntosLVL2;
    public SCR_RanaInterceptora ranaInterceptora;

    [Header("Información")]
    [SerializeField] private bool nivelCompletado = false;
    [SerializeField] private int aciertosActuales = 0;

    void Start()
    {
        if (scriptPuntos == null)
        {
            scriptPuntos = FindFirstObjectByType<SCR_ColisionesPuntos>();
        }

        if (scriptPuntosLVL2 == null)
        {
            scriptPuntosLVL2 = FindFirstObjectByType<SCR_Puntos>();
        }

        if (ranaInterceptora == null)
        {
            ranaInterceptora = FindFirstObjectByType<SCR_RanaInterceptora>();
        }

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

        if (scriptPuntos != null)
        {
            float puntosActuales = scriptPuntos.ObtenerPuntaje();
            if (puntosActuales >= puntosParaGanar)
            {
                NivelCompletado();
            }
        }
        else if (scriptPuntosLVL2 != null)
        {
            float puntosActuales = scriptPuntosLVL2.ObtenerPuntaje();
            if (puntosActuales >= puntosParaGanarLVL2)
            {
                NivelCompletado();
            }
        }
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
        // Suma victoria
        if (SCR_RachaTiempo.instance != null)
        {
            SCR_RachaTiempo.instance.SumarVictoria();
        }

        // Verificar modo ACTUAL
        bool modoFrenesi = PlayerPrefs.GetInt("ModoFrenesi", 0) == 1;

        //Si se completan 3 niveles en Historia, activa Frenesi
        if (!modoFrenesi && SCR_RachaTiempo.instance != null && SCR_RachaTiempo.instance.juegosGanados >= 3)
        {
            Debug.Log("[GestorNiveles] ¡Completaste los 3 niveles! Activando Modo Frenesi");
            PlayerPrefs.SetInt("ModoFrenesi", 1);
            PlayerPrefs.Save();
            modoFrenesi = true; // Actualizar para esta carga
        }

        if (modoFrenesi)
        {
            // Modo Frenesi: Aleatorio sin tutoriales
            CargarNivelAleatorio();
        }
        else
        {
            // Modo Historia: Siguiente configurado
            if (Application.CanStreamedLevelBeLoaded(nombreSiguienteNivel))
            {
                SceneManager.LoadScene(nombreSiguienteNivel);
            }
            else
            {
                //Debug.LogError($"[GestorNiveles] La escena '{nombreSiguienteNivel}' no existe");
            }
        }
    }

    void CargarNivelAleatorio()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        string[] niveles = { "Juego chuches", "Juego equilibrio", "Juego robar" };

        // Filtrar el nivel actual para que no se repita
        List<string> nivelesDisponibles = new List<string>();

        foreach (string nivel in niveles)
        {
            if (nivel != escenaActual)
            {
                nivelesDisponibles.Add(nivel);
            }
        }

        // Elegir uno aleatorio
        if (nivelesDisponibles.Count > 0)
        {
            int indiceAleatorio = Random.Range(0, nivelesDisponibles.Count);
            string nivelElegido = nivelesDisponibles[indiceAleatorio];

            Debug.Log($"[GestorNiveles] Frenesi: {escenaActual} → {nivelElegido} (Victorias: {SCR_RachaTiempo.instance?.juegosGanados})");
            SceneManager.LoadScene(nivelElegido);
        }
        else
        {
            Debug.LogError("[GestorNiveles] No hay niveles disponibles para Modo Frenesi");
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
        if (ranaInterceptora != null && ranaInterceptora.scriptBarraRitmo != null)
        {
            ranaInterceptora.scriptBarraRitmo.OnAcierto.RemoveListener(ContarAcierto);
            ranaInterceptora.scriptBarraRitmo.OnAciertoPerfecto.RemoveListener(ContarAcierto);
        }
    }
}