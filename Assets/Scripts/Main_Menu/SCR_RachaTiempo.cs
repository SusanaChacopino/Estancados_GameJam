using UnityEngine;

public class SCR_RachaTiempo : MonoBehaviour
{
    public static SCR_RachaTiempo instance;

    public int juegosGanados = 0;
    private float tiempoJugado = 0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        tiempoJugado += Time.deltaTime;
    }

    public void SumarVictoria()
    {
        juegosGanados++;
        Debug.Log("juegos ganados " + juegosGanados);
    }

    public string ObtenerTiempo()
    {
        int min = (int)(tiempoJugado / 60);
        int seg = (int)(tiempoJugado % 60);
        return $"{min:00}:{seg:00}";
    }

    public void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}
