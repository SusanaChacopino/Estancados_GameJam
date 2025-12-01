using UnityEngine;

public class SCR_Chuches : MonoBehaviour
{
    [Header("Frutas")]
    public GameObject chuche1;
    public GameObject chuche2;
    public GameObject fruta;

    [Header("Parametros")]
    public float frecuenciaInicial;
    public float frecuenciaMinima;
    public float tiempoParaAcelerar;
    [Range(0.01f, 0.2f)]
    public float porcentajeReduccion;

    [Tooltip("Margen desde el borde de la pantalla")]
    public float margenX;
    [Tooltip("Altura de spawn relativa a la cámara (0 = centro, valores positivos = arriba)")]
    public float offsetSpawnAltura;
    [Range(0f, 100f)]
    public float chuche1Percent;
    [Range(0f, 100f)]
    public float frutaPercent;

    [Header("Velocidad Caida")]
    public float velocidadCaidaInicial;
    public float velocidadCaidaMax;
    public float tiempoParaAceleracionCaida;
    [Range(0.01f,0.2f)]
    public float porcentajeAumento;

    //----privado----//
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

        InvokeRepeating("SpawnFruta", 0f, frecuenciaActual);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tiempoTranscurrido);
        TimerSpawn();
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

    void CalcularLimitesSpawn()
    {
        if (camaraPrincipal == null)
        {
            return;
        }

        //obtener los limites de la camara
        float alturaCamara = camaraPrincipal.orthographicSize;
        float anchoCamara = alturaCamara * camaraPrincipal.aspect;

        //Calculo de las margenes en X
        minHorizontal = camaraPrincipal.transform.position.x - anchoCamara + margenX;
        maxHorizontal = camaraPrincipal.transform.position.x + anchoCamara - margenX;

        //Calculo de la altura del Spawn (justo arriba del margen Y)
        spawnAltura = camaraPrincipal.transform.position.y + alturaCamara + offsetSpawnAltura;

    }

    void TimerSpawn()
    {
        //tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;

        //verificador de acelerecion
        if (tiempoTranscurrido>=siguienteAceleracion)
        {
            AcelerarSpawn();
        }

        if (tiempoTranscurrido>=siguienteAceleracionCaida)
        {
            AcelerarVelocidadCaida();
            siguienteAceleracionCaida += tiempoParaAceleracionCaida;
        }
    }
    
    void AcelerarSpawn()
    {
        float reduccion = frecuenciaActual * porcentajeReduccion;

        float nuevaFrecuencia =  frecuenciaActual-reduccion;

        //reduccion de frecuencia hasta el minimo
        if (nuevaFrecuencia < frecuenciaMinima)
        {
            nuevaFrecuencia = frecuenciaMinima;
        }

        if (nuevaFrecuencia != frecuenciaActual)
        {
            float objetosPorMinutoAntes = 60f / frecuenciaActual;
            float objetosPorMinutoDespues = 60f / nuevaFrecuencia;

            frecuenciaActual = nuevaFrecuencia;

            CancelInvoke("SpawnFruta");
            InvokeRepeating("SpawnFruta", 0f, frecuenciaActual);
        }


    }

    void AcelerarVelocidadCaida()
    {
        float aumento = SCR_FrutasCaida.velocidadGlobal * porcentajeAumento;

        // Aumentar velocidad (pero no más que el máximo)
        float nuevaVelocidad = SCR_FrutasCaida.velocidadGlobal + aumento;

        if (nuevaVelocidad > velocidadCaidaMax)
        {
            nuevaVelocidad = velocidadCaidaMax;
        }

        // Solo actualizar si cambió
        if (nuevaVelocidad != SCR_FrutasCaida.velocidadGlobal)
        {
            float velocidadAnterior = SCR_FrutasCaida.velocidadGlobal;
            SCR_FrutasCaida.velocidadGlobal = nuevaVelocidad;

        }
    }
}
