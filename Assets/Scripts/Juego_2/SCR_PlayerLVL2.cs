using UnityEngine;

public class SCR_PlayerLVL2 : MonoBehaviour{
    //Animaciones (en Limites())
    private Animator animPlayer;
    private float limiteAnim;

    //balanceo
    public float fuerzaGiro = 20f;
    private int direccion;
    public int tecla;
    
    //caída
    private float limite;
    private Quaternion posicionInicial;
    private float tiempoEspera = 1.65f;
    private float tiempoActual = 0f;
    public bool enEspera = false;

    //Tecla correcta
    public bool correcto = false;
    private float tiempoCorrecto;
    public GameObject jugador;

    void Start(){
        animPlayer = GetComponent<Animator>();
        Direccion();
        EleccionDeTecla();

        //caída
        limite = 40f;
        limiteAnim = 15f;
        posicionInicial = transform.rotation;
    }

    void Update(){
        if (!enEspera){
            Balanceo();
            Limites();
            Acierto();
        }

        //Al caer
        if (enEspera){
            if (direccion==1) animPlayer.Play("Anim_CaidaIzq");
            if (direccion==-1) animPlayer.Play("Anim_CaidaDer");
            tiempoActual += Time.deltaTime;
            direccion = 0;
            if (tiempoActual >= tiempoEspera){
                Direccion();
                EleccionDeTecla();
                enEspera = false;
                tiempoActual = 0f;
            }
        }
    }

    private void EleccionDeTecla(){
        tecla = Random.Range(0,4);
    }

    private void Direccion(){
        int numAleatorio = Random.Range(1,3);
        if(numAleatorio == 1) direccion = -1;
        if(numAleatorio == 2) direccion = 1;
    }

    private void Balanceo(){
        float grados = fuerzaGiro * Time.deltaTime;
        transform.Rotate(0f, 0f, grados * direccion, Space.World);
        //Teclas
        if (tecla == 0){
            if (Input.GetKey(KeyCode.W)) correcto = true;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) Caida();
        }
        if (tecla == 1){
            if (Input.GetKey(KeyCode.A)) correcto = true;
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) Caida();
        }
        if (tecla == 2){
            if (Input.GetKey(KeyCode.S)) correcto = true;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) Caida();
        }
        if (tecla == 3){
            if (Input.GetKey(KeyCode.D)) correcto = true;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)) Caida();
        }
    }

    private void Acierto(){
        if (correcto){
            animPlayer.Play("Anim_Recto");
            transform.rotation = posicionInicial;

            //espera
            tiempoCorrecto = Random.Range(0.5f, 2f);
            tiempoActual += Time.deltaTime;
            direccion = 0;
            if (tiempoActual >= tiempoCorrecto){
                Direccion();
                EleccionDeTecla();
                correcto = false;
                tiempoActual = 0f;
            }
        }
    }

    public void Caida(){
        jugador.GetComponent<SCR_Puntos>().puntos -= 3;
        transform.rotation = posicionInicial;
        enEspera = true;
    }
    private void Limites(){
        //límite
        if (!enEspera);
        if (direccion==1 && transform.eulerAngles.z < limiteAnim || direccion==-1 && transform.eulerAngles.z > 360-limiteAnim){
            animPlayer.Play("Anim_Recto");
        }
        if (direccion==1 && transform.eulerAngles.z > limiteAnim){
            animPlayer.Play("Anim_BalanceoIzq");
        }
        if (direccion==-1 && transform.eulerAngles.z < 360-limiteAnim){
            animPlayer.Play("Anim_BalanceoDer");
        }
        if (direccion==1 && transform.eulerAngles.z > limite || direccion==-1 && transform.eulerAngles.z < 360-limite){
            Caida();
        }
    }
}