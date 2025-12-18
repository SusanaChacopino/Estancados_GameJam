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
    [Tooltip("Distancia máxima entre frames antes de considerar teletransporte")]
    [Range(1f, 20f)]
    public float distanciaMaximaFrame = 5f;

    private Vector3 ultimaPosicionPersonaje;

    [Header("Estado")]
    [SerializeField] private bool persiguiendoObjetivo = false;
    [SerializeField] private bool esperandoMinijuego = false;
    [SerializeField] private bool siguiendoPersonaje = false; // ← NUEVO
    [SerializeField] private bool ralentizacionActiva = false;
    [SerializeField] private bool enCooldown = false;
    [SerializeField] private int intentosRestantes = 0;

    private Transform objetivoActual = null;
    private Vector3 posicionInicial;
    private Collider2D personajeObjetivo = null;
    private Transform personajeRobado = null;


    [Header("Sonidos")]
    [Tooltip("Sonido al acertar en la barra")]
    public AudioClip sonidoAcierto;

    [Tooltip("Sonido al fallar en la barra")]
    public AudioClip sonidoFallo;

    private AudioSource audioSource;

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
        // Si está siguiendo a un personaje robado
        if (siguiendoPersonaje)
        {
            SeguirPersonajeRobado();
            return; // No hacer nada más
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
            // Calcular distancia movida desde el último frame
            float distanciaMovida = Vector3.Distance(personajeRobado.position, ultimaPosicionPersonaje);

            // Detectar teletransporte
            if (distanciaMovida > distanciaMaximaFrame)
            {
                // Personaje fue reposicionado = volver a inicio
                siguiendoPersonaje = false;
                personajeRobado = null;
                StartCoroutine(VolverAPosicionInicialSuave());
                return;
            }

            // Actualizar posición anterior
            ultimaPosicionPersonaje = personajeRobado.position;

            // Seguir suavemente
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

        // Después de volver, iniciar cooldown
        StartCoroutine(CooldownCoroutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (esperandoMinijuego) return;

        if (other.CompareTag("PersonajeInterceptable"))
        {
            personajeObjetivo = other;
            esperandoMinijuego = true;
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
        // Si está esperando el minijuego y sale del trigger = automáticamente falla
        if (esperandoMinijuego && other.CompareTag("PersonajeInterceptable"))
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
            personajeObjetivo.enabled = false; // Ahora esto no activará OnTriggerExit como fallo
            personajeRobado = personajeObjetivo.transform;

            // Activar sprite hijo
            ActivarSpriteRobado(personajeRobado.gameObject, true);

            ultimaPosicionPersonaje = personajeRobado.position;
            siguiendoPersonaje = true;
        }

        TerminarMinijuego(true);
    }

    void ActivarSpriteRobado(GameObject personaje, bool activar)
    {
        // Buscar hijo por nombre (puedes ajustar el nombre)
        Transform hijo = personaje.transform.Find("SpriteRobado");

        // Si no lo encuentra por nombre, buscar el primer hijo
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
        // Desactivar barra
        if (barraRitmo != null)
        {
            barraRitmo.SetActive(false);
        }

        // Terminar ralentización
        if (scrollLateral != null)
        {
            scrollLateral.factorRalentizacion = 1f;
        }

        // Manejar personaje
        if (personajeObjetivo != null)
        {
            personajeObjetivo.enabled = false;

            if (!exito)
            {
                // Hacer semi-transparente si falló
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
            // FALLO: Volver suavemente
            StartCoroutine(VolverAPosicionInicialSuave());
        }
        // Si ACERTÓ, siguiendoPersonaje ya está en true
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

        // Indicador de siguiendo personaje
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