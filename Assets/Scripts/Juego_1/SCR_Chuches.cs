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

    [Tooltip("Margen desde el borde de la pantalla")]
    public float margenX;
    [Tooltip("Altura de spawn relativa a la cámara")]
    public float offsetSpawnAltura;

    // Probabilidades (deben sumar <= 100)
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
}