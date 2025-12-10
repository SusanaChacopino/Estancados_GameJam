using UnityEngine;
using UnityEngine.Events;

public class SCR_BarraRitmo : MonoBehaviour
{
    [Header("Configuración de la Barra")]
    [Tooltip("Ancho total de la barra en unidades del mundo")]
    [Range(1f, 20f)]
    public float anchoBarra = 10f;

    [Tooltip("Velocidad de movimiento del indicador")]
    [Range(1f, 20f)]
    public float velocidadIndicador = 5f;

    [Header("Referencias Visuales")]
    [Tooltip("Transform del indicador que se mueve (cuchillo)")]
    public Transform indicador;

    [Tooltip("Transform del punto objetivo (billetes)")]
    public Transform puntoObjetivo;

    [Header("Zona de Acierto")]
    [Tooltip("Tolerancia para considerar un acierto (en unidades)")]
    [Range(0.1f, 2f)]
    public float toleranciaAcierto = 0.8f;

    [Tooltip("Tolerancia para acierto perfecto")]
    [Range(0.05f, 0.5f)]
    public float toleranciaPerfecto = 0.3f;

    [Header("Animación del Indicador")]
    [Tooltip("¿Rotar el indicador según dirección?")]
    public bool rotarIndicador = true;

    [Tooltip("Ángulo cuando va a la derecha")]
    public float anguloIzquierdaADerecha = 0f;

    [Tooltip("Ángulo cuando va a la izquierda")]
    public float anguloDerechaAIzquierda = 180f;

    [Header("Estado")]
    [SerializeField] private float posicionActualIndicador = 0f;
    [SerializeField] private bool moviendoHaciaDerecha = true;
    [SerializeField] private bool juegoActivo = true;

    [Header("Eventos")]
    public UnityEvent OnAcierto;
    public UnityEvent OnAciertoPerfecto;
    public UnityEvent OnFallo;

    private float limiteDerecho;
    private float limiteIzquierdo;
    private float posicionObjetivo;

    void Start()
    {
        CalcularLimites();
        InicializarIndicador();
        GenerarPosicionObjetivo();
    }

    void Update()
    {
        if (!juegoActivo) return;

        MoverIndicador();
        VerificarInput();
    }

    void CalcularLimites()
    {
        // Calcular límites HORIZONTALES basados en la posición de esta barra
        limiteDerecho = transform.position.x + (anchoBarra / 2f);
        limiteIzquierdo = transform.position.x - (anchoBarra / 2f);

        Debug.Log($"[BarraRitmo] Límites: Derecho={limiteDerecho:F2}, Izquierdo={limiteIzquierdo:F2}");
    }

    void InicializarIndicador()
    {
        if (indicador == null)
        {
            Debug.LogError("[BarraRitmo] No hay indicador asignado");
            return;
        }

        // Empezar en el límite izquierdo
        posicionActualIndicador = limiteIzquierdo;
        ActualizarPosicionVisualIndicador();
        moviendoHaciaDerecha = true;
    }

    void GenerarPosicionObjetivo()
    {
        if (puntoObjetivo == null)
        {
            Debug.LogError("[BarraRitmo] No hay punto objetivo asignado");
            return;
        }

        // Generar posición aleatoria dentro de la barra (evitando extremos)
        float margen = anchoBarra * 0.1f; // 10% de margen en los lados
        posicionObjetivo = Random.Range(limiteIzquierdo + margen, limiteDerecho - margen);

        // Actualizar posición visual del objetivo
        Vector3 posObjetivo = puntoObjetivo.position;
        posObjetivo.x = posicionObjetivo;
        puntoObjetivo.position = posObjetivo;

        Debug.Log($"[BarraRitmo] Objetivo en X={posicionObjetivo:F2}");
    }

    void MoverIndicador()
    {
        // Calcular desplazamiento
        float desplazamiento = velocidadIndicador * Time.deltaTime;

        if (moviendoHaciaDerecha)
        {
            posicionActualIndicador += desplazamiento;

            // ¿Llegó al límite derecho?
            if (posicionActualIndicador >= limiteDerecho)
            {
                posicionActualIndicador = limiteDerecho;
                moviendoHaciaDerecha = false; // Cambiar dirección
            }
        }
        else
        {
            posicionActualIndicador -= desplazamiento;

            // ¿Llegó al límite izquierdo?
            if (posicionActualIndicador <= limiteIzquierdo)
            {
                posicionActualIndicador = limiteIzquierdo;
                moviendoHaciaDerecha = true; // Cambiar dirección
            }
        }

        ActualizarPosicionVisualIndicador();
    }

    void ActualizarPosicionVisualIndicador()
    {
        if (indicador == null) return;

        // Actualizar posición X (horizontal)
        Vector3 posIndicador = indicador.position;
        posIndicador.x = posicionActualIndicador;
        indicador.position = posIndicador;

        // Rotar según dirección
        if (rotarIndicador)
        {
            float anguloObjetivo = moviendoHaciaDerecha ? anguloIzquierdaADerecha : anguloDerechaAIzquierda;

            // Rotar suavemente
            Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, anguloObjetivo);
            indicador.rotation = Quaternion.Lerp(indicador.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        }
    }

    void VerificarInput()
    {
        // Detectar tecla de acción (espacio)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EvaluarAcierto();
        }
    }

    void EvaluarAcierto()
    {
        // Calcular distancia entre indicador y objetivo (HORIZONTAL)
        float distancia = Mathf.Abs(posicionActualIndicador - posicionObjetivo);

        Debug.Log($"[BarraRitmo] Distancia: {distancia:F2}");

        // Evaluar según tolerancia
        if (distancia <= toleranciaPerfecto)
        {
            // ¡PERFECTO!
            Debug.Log("[BarraRitmo] ¡PERFECTO!");
            OnAciertoPerfecto?.Invoke();
            GenerarNuevoObjetivo();
        }
        else if (distancia <= toleranciaAcierto)
        {
            // Acierto normal
            Debug.Log("[BarraRitmo] ¡Acierto!");
            OnAcierto?.Invoke();
            GenerarNuevoObjetivo();
        }
        else
        {
            // Fallo - también genera nuevo objetivo
            Debug.Log("[BarraRitmo] Fallaste");
            OnFallo?.Invoke();
            GenerarNuevoObjetivo(); // ← AÑADIDO: Cambia de posición al fallar
        }
    }

    void GenerarNuevoObjetivo()
    {
        GenerarPosicionObjetivo();
    }

    // Métodos públicos
    public float ObtenerPosicionIndicador()
    {
        return posicionActualIndicador;
    }

    public float ObtenerPosicionObjetivo()
    {
        return posicionObjetivo;
    }

    public float ObtenerDistanciaAlObjetivo()
    {
        return Mathf.Abs(posicionActualIndicador - posicionObjetivo);
    }

    public void AumentarVelocidad(float incremento)
    {
        velocidadIndicador += incremento;
        Debug.Log($"[BarraRitmo] Velocidad aumentada a {velocidadIndicador:F2}");
    }

    public void ReiniciarBarra()
    {
        InicializarIndicador();
        GenerarPosicionObjetivo();
        juegoActivo = true;
    }

    public void DetenerBarra()
    {
        juegoActivo = false;
    }

}