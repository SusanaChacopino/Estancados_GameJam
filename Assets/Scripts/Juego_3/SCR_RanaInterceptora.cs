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

    [Header("Barra de Ritmo")]
    public GameObject barraRitmo;
    public SCR_BarraRitmo scriptBarraRitmo;

    [Range(1, 5)]
    public int intentosMaximos = 3;

    [Header("Detección de Teletransporte")]
    [Range(1f, 20f)]
    public float distanciaMaximaFrame = 5f;

    [Header("Sonidos")]
    public AudioClip sonidoAcierto;
    public AudioClip sonidoFallo;

    [Header("Estado")]
    [SerializeField] private bool persiguiendoObjetivo = false;
    [SerializeField] private bool esperandoMinijuego = false;
    [SerializeField] private bool siguiendoPersonaje = false;
    [SerializeField] private bool ralentizacionActiva = false;
    [SerializeField] private bool enCooldown = false;
    [SerializeField] private int intentosRestantes = 0;

    private Transform objetivoActual = null;
    private Vector3 posicionInicial;
    private Collider2D personajeObjetivo = null;
    private Transform personajeRobado = null;
    private Vector3 ultimaPosicionPersonaje;
    private AudioSource audioSource;

    // Para evitar múltiples llamadas
    private bool minijuegoTerminado = false; // ← NUEVO

    void Start()
    {
        posicionInicial = transform.position;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (barraRitmo != null)
        {
            barraRitmo.SetActive(false);
        }

        if (scriptBarraRitmo != null)
        {
            scriptBarraRitmo.OnAcierto.AddListener(AlAcertarBarra);
            scriptBarraRitmo.OnAciertoPerfecto.AddListener(AlAcertarBarra);
            scriptBarraRitmo.OnFallo.AddListener(AlFallarBarra);
        }
    }

    void Update()
    {
        if (siguiendoPersonaje)
        {
            SeguirPersonajeRobado();
            return;
        }

        if (esperandoMinijuego || enCooldown) return;

        if (!persiguiendoObjetivo || objetivoActual == null)
        {
            BuscarObjetivoCercano();
        }

        if (persiguiendoObjetivo && objetivoActual != null && !esperandoMinijuego)
        {
            MoverseHaciaObjetivo();
        }
        else if (!esperandoMinijuego)
        {
            VolverAPosicionInicial();
        }
    }

    void SeguirPersonajeRobado()
    {
        if (personajeRobado != null)
        {
            float distanciaMovida = Vector3.Distance(personajeRobado.position, ultimaPosicionPersonaje);

            if (distanciaMovida > distanciaMaximaFrame)
            {
                siguiendoPersonaje = false;
                personajeRobado = null;
                StartCoroutine(VolverAPosicionInicialSuave());
                return;
            }

            ultimaPosicionPersonaje = personajeRobado.position;

            transform.position = Vector3.Lerp(
                transform.position,
                personajeRobado.position,
                Time.deltaTime * velocidadMovimiento
            );
        }
        else
        {
            siguiendoPersonaje = false;
            StartCoroutine(VolverAPosicionInicialSuave());
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

            Collider2D collider = personaje.GetComponent<Collider2D>();
            if (collider == null || !collider.enabled)
            {
                continue;
            }

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

    IEnumerator VolverAPosicionInicialSuave()
    {
        float duracion = 0.5f;
        float tiempoTranscurrido = 0f;
        Vector3 posicionInicio = transform.position;

        while (tiempoTranscurrido < duracion)
        {
            transform.position = Vector3.Lerp(
                posicionInicio,
                posicionInicial,
                tiempoTranscurrido / duracion
            );

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        transform.position = posicionInicial;

        // Cooldown
        yield return StartCoroutine(CooldownCoroutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (esperandoMinijuego) return;

        if (other.CompareTag("PersonajeInterceptable"))
        {
            personajeObjetivo = other;
            esperandoMinijuego = true;
            minijuegoTerminado = false;
            persiguiendoObjetivo = false;
            objetivoActual = null;
            intentosRestantes = intentosMaximos;

            if (barraRitmo != null)
            {
                barraRitmo.SetActive(true);
            }

            IniciarRalentizacion();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Solo ejecutar si está en minijuego Y no ha terminado
        if (esperandoMinijuego && !minijuegoTerminado && other.CompareTag("PersonajeInterceptable"))
        {
            TerminarMinijuego(false);
        }
    }

    void AlAcertarBarra()
    {
        if (sonidoAcierto != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoAcierto);
        }

        esperandoMinijuego = false;

        if (personajeObjetivo != null)
        {
            personajeObjetivo.enabled = false;
            personajeRobado = personajeObjetivo.transform;

            ActivarSpriteRobado(personajeRobado.gameObject, true);

            ultimaPosicionPersonaje = personajeRobado.position;
            siguiendoPersonaje = true;
        }

        TerminarMinijuego(true);
    }

    void ActivarSpriteRobado(GameObject personaje, bool activar)
    {
        Transform hijo = personaje.transform.Find("SpriteRobado");

        if (hijo == null && personaje.transform.childCount > 0)
        {
            hijo = personaje.transform.GetChild(0);
        }

        if (hijo != null)
        {
            hijo.gameObject.SetActive(activar);
        }
    }

    void AlFallarBarra()
    {
        if (sonidoFallo != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoFallo);
        }

        intentosRestantes--;

        if (intentosRestantes <= 0)
        {
            TerminarMinijuego(false);
        }
    }

    void TerminarMinijuego(bool exito)
    {
        // Evitar ejecutar múltiples veces
        if (minijuegoTerminado) return;
        minijuegoTerminado = true; 

        if (barraRitmo != null)
        {
            barraRitmo.SetActive(false);
        }

        if (scrollLateral != null)
        {
            scrollLateral.factorRalentizacion = 1f;
        }

        if (personajeObjetivo != null)
        {
            personajeObjetivo.enabled = false;

            if (!exito)
            {
                SpriteRenderer sr = personajeObjetivo.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color color = sr.color;
                    color.a = 0.5f;
                    sr.color = color;
                }
            }
        }

        ralentizacionActiva = false;
        esperandoMinijuego = false;
        personajeObjetivo = null;

        if (!exito)
        {
            StartCoroutine(VolverAPosicionInicialSuave());
        }
    }

    void IniciarRalentizacion()
    {
        if (ralentizacionActiva) return;
        if (scrollLateral == null) return;

        ralentizacionActiva = true;
        scrollLateral.factorRalentizacion = factorRalentizacion;
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

        if (Application.isPlaying && esperandoMinijuego)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.8f);
        }

        if (Application.isPlaying && siguiendoPersonaje)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 1.2f);
        }
    }

    void OnDestroy()
    {
        if (scriptBarraRitmo != null)
        {
            scriptBarraRitmo.OnAcierto.RemoveListener(AlAcertarBarra);
            scriptBarraRitmo.OnAciertoPerfecto.RemoveListener(AlAcertarBarra);
            scriptBarraRitmo.OnFallo.RemoveListener(AlFallarBarra);
        }
    }
}