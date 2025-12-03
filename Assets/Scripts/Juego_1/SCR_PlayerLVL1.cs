using UnityEngine;

public class SCR_PlayerLVL1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movimiento")]
    public float velocidad;

    [Header("Referencias")]
    public Rigidbody2D rb;

    private float margenPantalla=0.5f;    
    private Vector2 moverDireccion;
    private Camera camaraPrincipal;
    private float limiteIzq;
    private float limiteDer;
    void Start()
    {
        camaraPrincipal = Camera.main;

        CalcularLimitesPantalla();
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
    }

    void CalcularLimitesPantalla()
    {
        if (camaraPrincipal == null) 
        {
            return;
        }

        float alturaCamara = camaraPrincipal.orthographicSize;

        float anchorCamara = alturaCamara * camaraPrincipal.aspect;

        Vector3 posicionCamara = camaraPrincipal.transform.position;
        
        limiteIzq = posicionCamara.x-anchorCamara+margenPantalla;
        limiteIzq = posicionCamara.x + anchorCamara - margenPantalla;
    }

    void Movimiento() 
    {
        float movX = Input.GetAxisRaw("Horizontal");
        moverDireccion= new Vector2(movX, 0).normalized;

        rb.linearVelocity = new Vector2 (moverDireccion.x*velocidad,0);
    }

    
    
}
