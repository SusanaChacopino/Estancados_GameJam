using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SCR_BarraRitmo : MonoBehaviour
{
    [Header("Configuración de la Barra")]
    public bool calcularAnchoAutomatico = true;
    [Range(1f, 20f)]
    public float anchoBarra = 10f;
    [Range(1f, 20f)]
    public float velocidadIndicador = 5f;

    [Header("Referencias Visuales")]
    public Transform indicador;
    public Transform puntoObjetivo;

    [Header("Zona de Acierto")]
    [Range(0.1f, 2f)]
    public float toleranciaAcierto = 0.8f;
    [Range(0.05f, 0.5f)]
    public float toleranciaPerfecto = 0.3f;

    [Header("Animación del Indicador")]
    public bool rotarIndicador = true;
    public float anguloIzquierdaADerecha = 0f;
    public float anguloDerechaAIzquierda = 180f;

    [Header("Feedback Visual")]
    [Range(0.1f, 2f)]
    public float duracionEfectoRojo = 0.3f;

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
    private Coroutine efectoRojoCoroutine = null;
    private Color colorOriginalObjetivo = Color.white;

    void Start()
    {
        CalcularAnchoAutomatico();
        CalcularLimites();
        InicializarIndicador();
        GenerarPosicionObjetivo();

        // Guardar color original
        if (puntoObjetivo != null)
        {
            SpriteRenderer sr = puntoObjetivo.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                colorOriginalObjetivo = sr.color;
            }
        }
    }

    void OnEnable()
    {
        // Detener efecto rojo si existe
        if (efectoRojoCoroutine != null)
        {
            StopCoroutine(efectoRojoCoroutine);
            efectoRojoCoroutine = null;
        }

        // Restaurar color
        RestaurarColorObjetivo();
    }

    void RestaurarColorObjetivo()
    {
        if (puntoObjetivo == null) return;

        SpriteRenderer sr = puntoObjetivo.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = colorOriginalObjetivo;
        }
    }

    void Update()
    {
        if (!juegoActivo) return;

        MoverIndicador();
        VerificarInput();
    }

    void CalcularAnchoAutomatico()
    {
        if (!calcularAnchoAutomatico) return;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            anchoBarra = spriteRenderer.bounds.size.x;
        }
    }

    void CalcularLimites()
    {
        limiteDerecho = anchoBarra / 2f;
        limiteIzquierdo = -anchoBarra / 2f;
    }

    void InicializarIndicador()
    {
        if (indicador == null) return;

        posicionActualIndicador = limiteIzquierdo;
        ActualizarPosicionVisualIndicador();
        moviendoHaciaDerecha = true;
    }

    void GenerarPosicionObjetivo()
    {
        if (puntoObjetivo == null) return;

        float margen = anchoBarra * 0.1f;
        posicionObjetivo = Random.Range(limiteIzquierdo + margen, limiteDerecho - margen);

        Vector3 posObjetivo = puntoObjetivo.localPosition;
        posObjetivo.x = posicionObjetivo;
        puntoObjetivo.localPosition = posObjetivo;
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

        if (distancia <= toleranciaPerfecto)
        {
            OnAciertoPerfecto?.Invoke();
            GenerarNuevoObjetivo();
        }
        else if (distancia <= toleranciaAcierto)
        {
            OnAcierto?.Invoke();
            GenerarNuevoObjetivo();
        }
        else
        {
            if (efectoRojoCoroutine != null)
            {
                StopCoroutine(efectoRojoCoroutine);
            }
            efectoRojoCoroutine = StartCoroutine(EfectoRojoFallo());

            OnFallo?.Invoke();
        }
    }

    IEnumerator EfectoRojoFallo()
    {
        if (puntoObjetivo == null) yield break;

        SpriteRenderer sr = puntoObjetivo.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        sr.color = Color.red;

        yield return new WaitForSeconds(duracionEfectoRojo);

        sr.color = colorOriginalObjetivo;

        GenerarNuevoObjetivo();

        efectoRojoCoroutine = null;
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
}