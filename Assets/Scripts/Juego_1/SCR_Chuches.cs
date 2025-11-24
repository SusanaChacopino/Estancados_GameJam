using UnityEngine;

public class SCR_Chuches : MonoBehaviour
{
    [Header("Frutas")]
    public GameObject chuche1;
    public GameObject chuche2;
    public GameObject fruta;

    [Header("Parametros")]
    public float frecuencia;
    public float minHorizontal;
    public float maxHorizontal;
    public float spawnAltura;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnFruta() 
    {
        float x = Random.Range(minHorizontal, maxHorizontal);
    }
}
