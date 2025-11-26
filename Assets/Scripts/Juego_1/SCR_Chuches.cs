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
    public float chuche1Percent;
    public float frutaPercent;

    void Start()
    {
        InvokeRepeating("SpawnFruta", 0f, frecuencia);
    }

    // Update is called once per frame
    void Update(){
    }

    void SpawnFruta() 
    {
        float x = Random.Range(minHorizontal, maxHorizontal);
        Vector3 pos = new Vector3(x, spawnAltura, 0);

        float r = Random.value * 100f;

        GameObject frutaElegida;

        if (r < frutaPercent)
        {
            frutaElegida = fruta;
        }

        else if (frutaPercent < r && r < chuche1Percent)
        {
            frutaElegida = chuche1; 
        }

        else 
        {
            frutaElegida= chuche2;
        }

        Instantiate(frutaElegida,pos,Quaternion.identity);
    }
}
