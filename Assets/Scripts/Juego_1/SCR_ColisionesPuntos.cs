using UnityEngine;

public class SCR_ColisionesPuntos : MonoBehaviour
{
    [Header("puntos")]
    [SerializeField] private float puntaje;

    [Header("Animacion")]
    public Animator animatorJugador;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puntaje = 0;

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
        }

        if (other.gameObject.CompareTag("2p"))
        {
            puntaje += 2;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
            comioAlgo = true;
        }

        if (other.gameObject.CompareTag("fruta"))
        {
            puntaje -= 1;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
            comioAlgo=true;
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
