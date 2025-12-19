using UnityEngine;

public class SCR_FondoParallax : MonoBehaviour
{
    [Header("Configuración de Escala")]
    public bool ajustarTamanoCamara = false;

    [Header("Configuración Parallax")]
    public bool parallaxActivado = false;
    [Range(0f, 2f)]
    public float factorParallax = 0.5f; // Intensidad del parallax
    [Range(1f, 10f)]
    public float suavidad = 3f;
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
        Vector3 posicionActualJugador = objetoASeguir.position;
        Vector3 movimientoJugador = posicionActualJugador - ultimaPosicionJugador;

        Vector3 desplazamientoFondo = new Vector3(
            movimientoJugador.x * factorParallax,
            0,
            0
        );

        posicionObjetivoFondo += desplazamientoFondo;

        // Límites para que no se vaya muy lejos
        float limiteX = 2f;
        posicionObjetivoFondo.x = Mathf.Clamp(posicionObjetivoFondo.x, -limiteX, limiteX);

        Vector3 nuevaPosicion = Vector3.Lerp(
            transform.position,
            posicionObjetivoFondo,
            Time.deltaTime * suavidad
        );

        nuevaPosicion.z = transform.position.z;
        transform.position = nuevaPosicion;

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