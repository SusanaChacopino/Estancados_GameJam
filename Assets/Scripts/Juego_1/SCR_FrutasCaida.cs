using UnityEngine;
using UnityEngine.Rendering;

public class SCR_FrutasCaida : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadMin;
    public float velocidadMax;
    [SerializeField]
    float velocidad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocidad=Random.Range(velocidadMin, velocidadMax);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down*velocidad*Time.deltaTime);

        if (transform.position.y<-12f) 
        {
            Destroy(gameObject);
        }
    }
}
