using UnityEngine;
using UnityEngine.Events;

public class SCR_RanaEquilibrio : MonoBehaviour
{
    [Header("Configuracion Equlibrio")]
    [Range(1f, 50f)]
    public float velocidadInclinacionBase;
    [Range(5f, 100f)]
    public float fuerzaCorreccion;
    [Range(30f, 90f)]
    public float angulolimite;

    [Header("Sistema velocidad")]
    [Range(1f, 120f)]
    public float velocidadActual;
    [Range(50f,200f)]
    public float velocidadObjetivo;
    [Range(1f, 20f)]
    public float gananciaPorAcierto;
    [Range(5f, 50f)]
    public float perdidaPorCaida;
    [Range(0f, 20f)]
    public float velocidadMinima;

    [Header("Estado")]
    [SerializeField] private float anguloActual = 0;
    [SerializeField] private float velocidadAngular = 0f;
    [SerializeField] private bool juegoActivo = true;
    [SerializeField] private bool enRecuperacion = false;

    [Header("Eventos")]
    public UnityEvent OnCaida;
    public UnityEvent OnVictoria;
    public UnityEvent<float> OnVelocidadCambiada;

    private float direccionInclinacion = 1f;
    private float tiempoRecuperacion = 0.5f;
    private float tiempoRecuperacionActual = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anguloActual = 0f;
        velocidadAngular = 0f;
        velocidadActual = 0f;
        juegoActivo = true;

        direccionInclinacion = Random.value > 0.5 ? 1f : -1f;
    }


    // Update is called once per frame
    void Update()
    {

        if (!juegoActivo) 
        {
            return;
        }

        if (enRecuperacion)
        {
            ActualizarRecuperacion();
        }
        
    }

    void ActualizarRecuperacion()
    {
        tiempoRecuperacionActual += Time.deltaTime;

        //Vuelve suavemente a 0grados
        anguloActual = Mathf.Lerp(anguloActual, 0f, tiempoRecuperacionActual / tiempoRecuperacion);
        transform.rotation = Quaternion.Euler(0, 0, -anguloActual);

        if (tiempoRecuperacionActual >= tiempoRecuperacion)
        {
            //Recuperacion Completa
            anguloActual = 0f;
            velocidadAngular = 0f;
            enRecuperacion = false;
            tiempoRecuperacionActual = 0f;

            //Nueva direccion aleatoria
            direccionInclinacion = Random.value > 0.5f ? 1f : -1f;
        }
    }
}
