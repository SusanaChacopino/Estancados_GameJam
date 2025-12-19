using UnityEngine;

public class SCR_PlayerLVL2 : MonoBehaviour
{
    private Animator animPlayer;
    private float limiteAnim;

    [Header("Balanceo")]
    public float fuerzaGiro = 20f;
    public float fuerzaImpulso = 40f; // Velocidad del impulso al acertar (más alto = vuelve más rápido)
    private int direccion;
    public int tecla;

    private float limite;
    private Quaternion posicionInicial;
    private float tiempoEspera = 1.65f;
    private float tiempoActual = 0f;
    public bool enEspera = false;

    [Header("Tecla Correcta")]
    public bool correcto = false;
    private float tiempoCorrecto;
    private bool volviendoACero = false; // Nuevo: está volviendo a 0°
    public GameObject jugador;

    [Header("Sonidos")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoCaerse;
    [SerializeField] private AudioClip sonidoTecla;

    void Start()
    {
        animPlayer = GetComponent<Animator>();
        Direccion();
        EleccionDeTecla();

        audioSource = GetComponent<AudioSource>();

        limite = 40f;
        limiteAnim = 5f;
        posicionInicial = transform.rotation;
    }

    void Update()
    {
        if (!enEspera)
        {
            if (!volviendoACero)
            {
                Balanceo();
                Limites();
            }
            else
            {
                VolverACero(); // Nuevo: volver a 0° con impulso
            }

            Acierto();
        }

        // Al caer
        if (enEspera)
        {
            if (direccion == 1) animPlayer.Play("Anim_CaidaIzq");
            if (direccion == -1) animPlayer.Play("Anim_CaidaDer");
            tiempoActual += Time.deltaTime;
            direccion = 0;

            if (tiempoActual >= tiempoEspera)
            {
                Direccion();
                EleccionDeTecla();
                enEspera = false;
                tiempoActual = 0f;
            }
        }
    }

    private void EleccionDeTecla()
    {
        tecla = Random.Range(0, 4);
    }

    private void Direccion()
    {
        int numAleatorio = Random.Range(1, 3);
        if (numAleatorio == 1) direccion = -1;
        if (numAleatorio == 2) direccion = 1;
    }

    private void Balanceo()
    {
        // Girar según la dirección actual
        float grados = fuerzaGiro * Time.deltaTime;
        transform.Rotate(0f, 0f, grados * direccion, Space.World);

        // Detección de teclas
        if (tecla == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                correcto = true;
                audioSource.PlayOneShot(sonidoTecla);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                Caida();
        }
        if (tecla == 1)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                correcto = true;
                audioSource.PlayOneShot(sonidoTecla);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                Caida();
        }
        if (tecla == 2)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                correcto = true;
                audioSource.PlayOneShot(sonidoTecla);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
                Caida();
        }
        if (tecla == 3)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                correcto = true;
                audioSource.PlayOneShot(sonidoTecla);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))
                Caida();
        }
    }

    private void Acierto()
    {
        if (correcto)
        {
            animPlayer.Play("Anim_Recto");

            // En lugar de teleport, activar modo "volver a cero"
            volviendoACero = true;
            direccion = 0;
        }
    }

    // NUEVO: Volver a 0° con impulso suave
    private void VolverACero()
    {
        float anguloActual = transform.eulerAngles.z;

        // Convertir ángulo a rango -180 a 180
        if (anguloActual > 180f)
            anguloActual -= 360f;

        // Si está cerca de 0° (±2°), ya llegó
        if (Mathf.Abs(anguloActual) < 2f)
        {
            transform.rotation = posicionInicial; // Ajustar a 0° exacto
            volviendoACero = false;
            correcto = false;

            // Esperar un momento antes de empezar nuevo balanceo
            tiempoCorrecto = Random.Range(0.5f, 2f);
            tiempoActual = 0f;

            // Elegir nueva dirección
            Direccion();
            EleccionDeTecla();
        }
        else
        {
            // Rotar hacia 0° (dirección opuesta al ángulo actual)
            int direccionVuelta = anguloActual > 0 ? -1 : 1;
            float velocidadVuelta = fuerzaImpulso * Time.deltaTime;
            transform.Rotate(0f, 0f, velocidadVuelta * direccionVuelta, Space.World);
        }
    }

    public void Caida()
    {
        if (enEspera) return;

        jugador.GetComponent<SCR_Puntos>().puntos -= 3;
        transform.rotation = posicionInicial; // Al caer sí puede teleportarse
        enEspera = true;

        if (audioSource != null && sonidoCaerse != null)
        {
            audioSource.PlayOneShot(sonidoCaerse);
        }
    }

    private void Limites()
    {
        if (!enEspera)
        {
            // Animaciones según ángulo
            if (direccion == 1 && transform.eulerAngles.z < limiteAnim ||
                direccion == -1 && transform.eulerAngles.z > 360 - limiteAnim)
            {
                animPlayer.Play("Anim_Recto");
            }
            if (direccion == 1 && transform.eulerAngles.z > limiteAnim)
            {
                animPlayer.Play("Anim_BalanceoIzq");
            }
            if (direccion == -1 && transform.eulerAngles.z < 360 - limiteAnim)
            {
                animPlayer.Play("Anim_BalanceoDer");
            }

            // Caída por límite
            if (direccion == 1 && transform.eulerAngles.z > limite ||
                direccion == -1 && transform.eulerAngles.z < 360 - limite)
            {
                Caida();
            }
        }
    }
}