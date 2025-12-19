using UnityEngine;

public class SCR_ColisionesPuntos : MonoBehaviour
{
    [Header("puntos")]
    [SerializeField] private float puntaje;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoComer;
    [SerializeField] private AudioClip sonidoFrutaMala;

    [Header("Animacion")]
    public Animator animatorJugador;

    void Start()
    {
        puntaje = 0;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool comioAlgo = false;

        // Chuche de 1 punto
        if (other.gameObject.CompareTag("1p"))
        {
            puntaje += 1;
            Destroy(other.gameObject);
            comioAlgo = true;
            audioSource.PlayOneShot(sonidoComer);
        }

        // Chuche de 2 puntos
        if (other.gameObject.CompareTag("2p"))
        {
            puntaje += 2;
            Destroy(other.gameObject);
            comioAlgo = true;
            audioSource.PlayOneShot(sonidoComer);
        }

        // Fruta mala (resta puntos)
        if (other.gameObject.CompareTag("fruta"))
        {
            puntaje -= 1;
            Destroy(other.gameObject);
            comioAlgo = true;
            audioSource.PlayOneShot(sonidoFrutaMala);
        }

        // Activar animación de comer
        if (comioAlgo && animatorJugador != null)
        {
            animatorJugador.SetTrigger("Comer");
        }
    }

    public float ObtenerPuntaje()
    {
        return puntaje;
    }
}