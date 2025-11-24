using UnityEngine;

public class SCR_PlayerLVL1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movimiento")]
    public float velocidad;

    public Rigidbody2D rb;

    private Vector2 moverDireccion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
    }

    void Movimiento() 
    {
        float movX = Input.GetAxisRaw("Horizontal");
        moverDireccion= new Vector2(movX, 0).normalized;

        rb.linearVelocity = new Vector2 (moverDireccion.x*velocidad,0);
    }

}
