using UnityEngine;
using System.Collections;

public class SCR_RanaInterceptora : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Range(1f, 20f)]
    public float velocidadMovimiento = 8f;

    [Range(0.1f, 2f)]
    public float distanciaMinima = 0.5f;

    [Header("Detección de Objetivos")]
    [Range(1f, 15f)]
    public float rangoDeteccion = 8f;

    [Range(0.5f, 5f)]
    public float cooldownBusqueda = 1f;

    [Header("Ralentización")]
    public SCR_ScrollLateral scrollLateral;

    [Range(0.01f, 1f)]
    public float factorRalentizacion = 0.2f;

    [Range(0.5f, 10f)]
    public float duracionRalentizacion = 2f;

    [Header("Barra de Ritmo")]
    [Tooltip("GameObject de la barra que se activa al interceptar")]
    public GameObject barraRitmo; // ← NUEVO

    [Header("Estado")]
    [SerializeField] private bool persiguiendoObjetivo = false;
    [SerializeField] private bool ralentizacionActiva = false;
    [SerializeField] private bool enCooldown = false;

    private Transform objetivoActual = null;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;

        // Asegurarse de que la barra esté desactivada al inicio
        if (barraRitmo != null)
        {
            barraRitmo.SetActive(false);
        }
    }

    void Update()
    {
        if (enCooldown) return;

        if (!persiguiendoObjetivo || objetivoActual == null)
        {
            BuscarObjetivoCercano();
        }

        if (persiguiendoObjetivo && objetivoActual != null)
        {
            MoverseHaciaObjetivo();
        }
        else
        {
            VolverAPosicionInicial();
        }
    }

    void BuscarObjetivoCercano()
    {
        GameObject[] personajes = GameObject.FindGameObjectsWithTag("PersonajeInterceptable");

        if (personajes.Length == 0)
        {
            objetivoActual = null;
            persiguiendoObjetivo = false;
            return;
        }

        Transform objetivoMasCercano = null;
        float distanciaMasCercana = rangoDeteccion;

        foreach (GameObject personaje in personajes)
        {
            if (personaje == null) continue;

            float distancia = Vector3.Distance(transform.position, personaje.transform.position);

            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                objetivoMasCercano = personaje.transform;
            }
        }

        if (objetivoMasCercano != null && objetivoMasCercano != objetivoActual)
        {
            objetivoActual = objetivoMasCercano;
            persiguiendoObjetivo = true;
        }
        else if (objetivoMasCercano == null)
        {
            objetivoActual = null;
            persiguiendoObjetivo = false;
        }
    }

    void MoverseHaciaObjetivo()
    {
        if (objetivoActual == null)
        {
            persiguiendoObjetivo = false;
            return;
        }

        Vector3 direccion = (objetivoActual.position - transform.position).normalized;
        transform.position += direccion * velocidadMovimiento * Time.deltaTime;
    }

    void VolverAPosicionInicial()
    {
        float distanciaInicial = Vector3.Distance(transform.position, posicionInicial);

        if (distanciaInicial > 0.1f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                posicionInicial,
                Time.deltaTime * velocidadMovimiento * 0.5f
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PersonajeInterceptable"))
        {
            // Desactivar collider
            other.enabled = false;

            // Resetear objetivo
            objetivoActual = null;
            persiguiendoObjetivo = false;

            // ACTIVAR BARRA ← NUEVO
            if (barraRitmo != null)
            {
                barraRitmo.SetActive(true);
            }

            // Ralentizar
            IniciarRalentizacion();

            // Cooldown
            StartCoroutine(CooldownCoroutine());
        }
    }

    void IniciarRalentizacion()
    {
        if (ralentizacionActiva) return;
        if (scrollLateral == null) return;

        StartCoroutine(RalentizacionCoroutine());
    }

    IEnumerator RalentizacionCoroutine()
    {
        ralentizacionActiva = true;

        if (scrollLateral != null)
        {
            scrollLateral.factorRalentizacion = factorRalentizacion;
        }

        yield return new WaitForSeconds(duracionRalentizacion);

        if (scrollLateral != null)
        {
            scrollLateral.factorRalentizacion = 1f;
        }

        // DESACTIVAR BARRA ← NUEVO
        if (barraRitmo != null)
        {
            barraRitmo.SetActive(false);
        }

        ralentizacionActiva = false;
    }

    IEnumerator CooldownCoroutine()
    {
        enCooldown = true;
        yield return new WaitForSeconds(cooldownBusqueda);
        enCooldown = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        if (persiguiendoObjetivo && objetivoActual != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, objetivoActual.position);
        }

        if (Application.isPlaying && enCooldown)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one * 0.5f);
        }
    }
}