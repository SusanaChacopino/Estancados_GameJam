using UnityEngine;

public class SCR_PlayerLVL2 : MonoBehaviour{
    public float fuerzaGiro;

    //límites
    public float limiteIzq, limiteDer;
    public bool izq;

    //caída
    private Quaternion posicionInicial;

    //balanceo
    private float tiempoEspera;
    private float tiempoActual;
    public bool correcto;

    void Start(){
        fuerzaGiro = 50f;
        posicionInicial = transform.rotation;
        limiteIzq = 45f;
        limiteDer = 315f;
        tiempoEspera = 0.5f;
        tiempoActual = Time.time;
        correcto = false;
    }

    void Update(){

        Balanceo();

        //controlador
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            transform.Rotate(0f, 0f, fuerzaGiro, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            transform.Rotate(0f, 0f, -fuerzaGiro, Space.World);
        }
        Limites();
    }

    private void Limites(){
        //límite
        if (izq && transform.eulerAngles.z > limiteIzq || !izq && transform.eulerAngles.z < limiteDer){
            Debug.Log("Límite atravesado");
            transform.rotation = posicionInicial;
            //cambiar anim a caída
        }
    }

    private void Balanceo(){
        //si está en la derecha o en la izquierda
        if (transform.eulerAngles.z < 180) izq = true;
        else izq = false;

        //Debug.Log("Balanceo accedido");

        if (tiempoEspera <= tiempoActual){
            Debug.Log("Balanceo accedido");
            transform.Rotate(0f, 0f, fuerzaGiro, Space.World);
            if (correcto){
                correcto = false;
                tiempoEspera = Random.Range(0.5f,4f);
                tiempoActual = Time.time;
            }
        }
    }
}
