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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puntaje = 0;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool comioAlgo=false;

        if (other.gameObject.CompareTag("1p"))
        {
            puntaje += 1;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
            comioAlgo = true;
           // sonidoComer.Play();
            audioSource.PlayOneShot(sonidoComer);
        }

        if (other.gameObject.CompareTag("2p"))
        {
            puntaje += 2;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
            comioAlgo = true;
            // sonidoComer.Play();
            audioSource.PlayOneShot(sonidoComer);
        }

        if (other.gameObject.CompareTag("fruta"))
        {
            puntaje -= 1;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
            comioAlgo=true;
            //sonidoFrutaMala.Play();
            audioSource.PlayOneShot(sonidoFrutaMala);
        }

        if (comioAlgo&&animatorJugador!=null) 
        {
            animatorJugador.SetTrigger("Comer");
        }

    }

    public float ObtenerPuntaje()
    {
        return puntaje;
    }
}
