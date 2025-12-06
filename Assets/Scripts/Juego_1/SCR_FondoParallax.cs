using UnityEngine;

public class SCR_FondoParallax : MonoBehaviour
{
    [Header("Configuración de Escala")]
    [Tooltip("Si está activado, ajustará automáticamente el fondo al tamaño de la cámara")]
    public bool ajustarTamanoCamara = false;

    [Header("Configuración Parallax")]
    [Tooltip("Activa o desactiva el efecto parallax")]
    public bool parallaxActivado = false;

    [Tooltip("Factor de parallax (valores más altos = más movimiento visible)")]
    [Range(0f, 2f)]
    public float factorParallax = 0.5f;

    [Tooltip("Suavidad del movimiento (más alto = más suave)")]
    [Range(1f, 10f)]
    public float suavidad = 3f;

    [Tooltip("Objeto que el fondo seguirá (normalmente el jugador)")]
    public Transform objetoASeguir;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector3 posicionObjetivoFondo;
    private Vector3 ultimaPosicionJugador;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        if (ajustarTamanoCamara && spriteRenderer != null)
        {
            AjustarAlTamanoCamara();
        }

        // Inicializar posiciones
        posicionObjetivoFondo = transform.position;

        if (objetoASeguir != null)
        {
            ultimaPosicionJugador = objetoASeguir.position;
        }
    }

    void Update()
    {
        if (parallaxActivado && objetoASeguir != null)
        {
            AplicarParallaxSutil();
        }
    }

    void AplicarParallaxSutil()
    {
        // Calcular el movimiento del jugador en este frame
        Vector3 posicionActualJugador = objetoASeguir.position;
        Vector3 movimientoJugador = posicionActualJugador - ultimaPosicionJugador;

        // Calcular desplazamiento del fondo basado en el movimiento del jugador
        Vector3 desplazamientoFondo = new Vector3(
            movimientoJugador.x * factorParallax,
            0, // No mover verticalmente
            0
        );

        // Actualizar posición objetivo del fondo
        posicionObjetivoFondo += desplazamientoFondo;

        // Aplicar límites para que no se vaya demasiado lejos
        float limiteX = 2f; // Ajusta este valor según necesites
        posicionObjetivoFondo.x = Mathf.Clamp(posicionObjetivoFondo.x, -limiteX, limiteX);

        // Mover suavemente hacia la posición objetivo
        Vector3 nuevaPosicion = Vector3.Lerp(
            transform.position,
            posicionObjetivoFondo,
            Time.deltaTime * suavidad
        );

        // Mantener la Z original
        nuevaPosicion.z = transform.position.z;
        transform.position = nuevaPosicion;

        // Actualizar última posición del jugador
        ultimaPosicionJugador = posicionActualJugador;
    }

    void AjustarAlTamanoCamara()
    {
        if (mainCamera == null) return;

        float alturaCamera = mainCamera.orthographicSize * 2f;
        float anchoCamera = alturaCamera * mainCamera.aspect;
        Vector2 tamanoSprite = spriteRenderer.sprite.bounds.size;

        float escalaX = anchoCamera / tamanoSprite.x;
        float escalaY = alturaCamera / tamanoSprite.y;
        float escalaFinal = Mathf.Max(escalaX, escalaY);

        transform.localScale = new Vector3(escalaFinal, escalaFinal, 1);
    }
}