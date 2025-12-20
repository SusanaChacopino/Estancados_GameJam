using UnityEngine;
using System.Collections;

public class SCR_Chuches : MonoBehaviour
{
    [Header("Frutas")]
    public GameObject chuche1;
    public GameObject chuche2;
    public GameObject fruta;

    [Header("Parametros")]
    public float frecuenciaInicial; // Segundos entre spawns al inicio
    public float frecuenciaMinima;  // Frecuencia más rápida posible
    public float tiempoParaAcelerar; // Cada cuánto acelera el spawn
    [Range(0.01f, 0.2f)]
    public float porcentajeReduccion; // Cuánto acelera cada vez

    public float margenX;
    public float offsetSpawnAltura;

    [Header("Probabilidades")]
    [Range(0f, 100f)]
    public float chuche1Percent;
    [Range(0f, 100f)]
    public float frutaPercent;

    [Header("Velocidad Caida")]
    public float velocidadCaidaInicial;
    public float velocidadCaidaMax;
    public float tiempoParaAceleracionCaida;
    [Range(0.01f, 0.2f)]
    public float porcentajeAumento;

    private Camera camaraPrincipal;
    private float minHorizontal;
    private float maxHorizontal;
    private float spawnAltura;
    private float frecuenciaActual;
    private float tiempoTranscurrido;
    private float siguienteAceleracion;
    private float siguienteAceleracionCaida;

    void Start()
    {
        camaraPrincipal = Camera.main;
        CalcularLimitesSpawn();

        AjustarDificultadPorVictorias();

        frecuenciaActual = frecuenciaInicial;
        siguienteAceleracion = tiempoParaAcelerar;
        siguienteAceleracionCaida = tiempoParaAceleracionCaida;

        SCR_FrutasCaida.velocidadGlobal = velocidadCaidaInicial;

        StartCoroutine(SpawnearFrutasCoroutine());
    }

    void Update()
    {
        TimerSpawn();
    }

    IEnumerator SpawnearFrutasCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(frecuenciaActual);
            SpawnFruta();
        }
    }

    void SpawnFruta()
    {
        float x = Random.Range(minHorizontal, maxHorizontal);
        Vector3 pos = new Vector3(x, spawnAltura, -0.5f);

        // Elegir fruta según probabilidades
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
            frutaElegida = chuche2;
        }

        Instantiate(frutaElegida, pos, Quaternion.identity);
    }

    void CalcularLimitesSpawn()
    {
        if (camaraPrincipal == null) return;

        float alturaCamara = camaraPrincipal.orthographicSize;
        float anchoCamara = alturaCamara * camaraPrincipal.aspect;

        minHorizontal = camaraPrincipal.transform.position.x - anchoCamara + margenX;
        maxHorizontal = camaraPrincipal.transform.position.x + anchoCamara - margenX;
        spawnAltura = camaraPrincipal.transform.position.y + alturaCamara + offsetSpawnAltura;
    }

    void TimerSpawn()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (tiempoTranscurrido >= siguienteAceleracion)
        {
            AcelerarSpawn();
            siguienteAceleracion += tiempoParaAcelerar;
        }

        if (tiempoTranscurrido >= siguienteAceleracionCaida)
        {
            AcelerarVelocidadCaida();
            siguienteAceleracionCaida += tiempoParaAceleracionCaida;
        }
    }

    void AcelerarSpawn()
    {
        float reduccion = frecuenciaActual * porcentajeReduccion;
        float nuevaFrecuencia = frecuenciaActual - reduccion;

        if (nuevaFrecuencia < frecuenciaMinima)
        {
            nuevaFrecuencia = frecuenciaMinima;
        }

        frecuenciaActual = nuevaFrecuencia;
    }

    void AcelerarVelocidadCaida()
    {
        float aumento = SCR_FrutasCaida.velocidadGlobal * porcentajeAumento;
        float nuevaVelocidad = SCR_FrutasCaida.velocidadGlobal + aumento;

        if (nuevaVelocidad > velocidadCaidaMax)
        {
            nuevaVelocidad = velocidadCaidaMax;
        }

        SCR_FrutasCaida.velocidadGlobal = nuevaVelocidad;
    }

    void AjustarDificultadPorVictorias()
    {
        if (SCR_RachaTiempo.instance == null) return;

        int victorias = SCR_RachaTiempo.instance.juegosGanados;

        // Velocidad y frecuencia
        float multiplicadorVelocidad = 1f + (victorias * 0.15f);
        float multiplicadorFrecuencia = 1f + (victorias * 0.20f);

        multiplicadorVelocidad = Mathf.Min(multiplicadorVelocidad, 2.5f);
        multiplicadorFrecuencia = Mathf.Min(multiplicadorFrecuencia, 2f);

        velocidadCaidaInicial *= multiplicadorVelocidad;
        velocidadCaidaMax *= multiplicadorVelocidad;
        frecuenciaInicial /= multiplicadorFrecuencia;
        frecuenciaMinima /= multiplicadorFrecuencia;
        frecuenciaActual = frecuenciaInicial;

        AjustarProbabilidades(victorias);

    }

    void AjustarProbabilidades(int victorias)
    {
        // Aumentar probabilidad de chuches con cada victoria    //Esto es porque al reducir el tiempo no da tiempo a pasar el nivel
        float aumentoPorVictoria = 5f; // +5% por victoria
        float aumentoMaximo = 25f; // Máximo +25%

        float aumento = Mathf.Min(victorias * aumentoPorVictoria, aumentoMaximo);

        // reducir fruta (buena)
        frutaPercent -= aumento;

        // Asegurar que no pase de 100%
        frutaPercent = Mathf.Min(frutaPercent, 80f); // Máximo 80% de frutas

    }


}