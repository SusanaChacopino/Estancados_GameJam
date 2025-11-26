using UnityEngine;

public class SCR_ColisionesPuntos : MonoBehaviour
{
    [Header("puntos")]
    [SerializeField] private float puntaje;
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
        if (other.gameObject.CompareTag("1p"))
        {
            puntaje += 1;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
        }

        if (other.gameObject.CompareTag("2p"))
        {
            puntaje += 2;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
        }

        if (other.gameObject.CompareTag("fruta"))
        {
            puntaje -= 1;
            Destroy(other.gameObject);
            Debug.Log(puntaje);
        }

    }
}
