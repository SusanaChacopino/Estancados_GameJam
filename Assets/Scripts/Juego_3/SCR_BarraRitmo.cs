using UnityEngine;
using UnityEngine.Events;
public class SCR_BarraRitmo : MonoBehaviour
{
    [Header("Configuración de la Barra")]
    [Tooltip("Si está activo, calcula el ancho desde el sprite")]
    public bool calcularAnchoAutomatico = true;

    [Tooltip("Ancho de la barra (manual o automático)")]
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
        CalcularAnchoAutomatico();
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

    void CalcularAnchoAutomatico()
    {
        if (!calcularAnchoAutomatico)
        {
            Debug.Log($"[BarraRitmo] Usando ancho manual: {anchoBarra}");
            return;
        }

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            anchoBarra = spriteRenderer.bounds.size.x;
            Debug.Log($"[BarraRitmo] Ancho automático: {anchoBarra:F2} unidades");
        }
        else
        {
            Debug.LogWarning($"[BarraRitmo] No hay SpriteRenderer. Usando manual: {anchoBarra}");
        }
    }

    void CalcularLimites()
    {
        // Límites RELATIVOS (coordenadas locales)
        limiteDerecho = anchoBarra / 2f;
        limiteIzquierdo = -anchoBarra / 2f;

        Debug.Log($"[BarraRitmo] Límites: Izq={limiteIzquierdo:F2}, Der={limiteDerecho:F2}");
    }

    void InicializarIndicador()
    {
        if (indicador == null)
        {
            Debug.LogError("[BarraRitmo] No hay indicador asignado");
            return;
        }

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

        float margen = anchoBarra * 0.1f;
        posicionObjetivo = Random.Range(limiteIzquierdo + margen, limiteDerecho - margen);

        // Usar localPosition (relativo al padre)
        Vector3 posObjetivo = puntoObjetivo.localPosition;
        posObjetivo.x = posicionObjetivo;
        puntoObjetivo.localPosition = posObjetivo;

        Debug.Log($"[BarraRitmo] Objetivo en X local={posicionObjetivo:F2}");
    }

    void MoverIndicador()
    {
        float desplazamiento = velocidadIndicador * Time.deltaTime;

        if (moviendoHaciaDerecha)
        {
            posicionActualIndicador += desplazamiento;

            if (posicionActualIndicador >= limiteDerecho)
            {
                posicionActualIndicador = limiteDerecho;
                moviendoHaciaDerecha = false;
            }
        }
        else
        {
            posicionActualIndicador -= desplazamiento;

            if (posicionActualIndicador <= limiteIzquierdo)
            {
                posicionActualIndicador = limiteIzquierdo;
                moviendoHaciaDerecha = true;
            }
        }

        ActualizarPosicionVisualIndicador();
    }

    void ActualizarPosicionVisualIndicador()
    {
        if (indicador == null) return;

        // Usar localPosition (relativo al padre)
        Vector3 posIndicador = indicador.localPosition;
        posIndicador.x = posicionActualIndicador;
        indicador.localPosition = posIndicador;

        if (rotarIndicador)
        {
            float anguloObjetivo = moviendoHaciaDerecha ? anguloIzquierdaADerecha : anguloDerechaAIzquierda;
            Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, anguloObjetivo);
            indicador.rotation = Quaternion.Lerp(indicador.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        }
    }

    void VerificarInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EvaluarAcierto();
        }
    }

    void EvaluarAcierto()
    {
        float distancia = Mathf.Abs(posicionActualIndicador - posicionObjetivo);

        Debug.Log($"[BarraRitmo] Distancia: {distancia:F2}");

        if (distancia <= toleranciaPerfecto)
        {
            Debug.Log("[BarraRitmo] ¡PERFECTO!");
            OnAciertoPerfecto?.Invoke();
            GenerarNuevoObjetivo();
        }
        else if (distancia <= toleranciaAcierto)
        {
            Debug.Log("[BarraRitmo] ¡Acierto!");
            OnAcierto?.Invoke();
            GenerarNuevoObjetivo();
        }
        else
        {
            Debug.Log("[BarraRitmo] Fallaste");
            OnFallo?.Invoke();
            GenerarNuevoObjetivo();
        }
    }

    void GenerarNuevoObjetivo()
    {
        GenerarPosicionObjetivo();
    }

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

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Convertir límites locales a coordenadas del mundo para dibujar
        Vector3 posicionMundo = transform.position;

        Gizmos.color = Color.red;
        float offsetY = 0.5f;

        // Límite derecho
        Vector3 limiteDer = new Vector3(posicionMundo.x + limiteDerecho, posicionMundo.y, 0);
        Gizmos.DrawLine(limiteDer + Vector3.down * offsetY, limiteDer + Vector3.up * offsetY);

        // Límite izquierdo
        Vector3 limiteIzq = new Vector3(posicionMundo.x + limiteIzquierdo, posicionMundo.y, 0);
        Gizmos.DrawLine(limiteIzq + Vector3.down * offsetY, limiteIzq + Vector3.up * offsetY);

        // Zona de acierto
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            new Vector3(posicionMundo.x + posicionObjetivo, posicionMundo.y, 0),
            toleranciaAcierto
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            new Vector3(posicionMundo.x + posicionObjetivo, posicionMundo.y, 0),
            toleranciaPerfecto
        );
    }
}