using UnityEngine;
using TMPro;

public class SCR_PlayerLVL2 : MonoBehaviour{
    //balanceo
    public TMP_Text teclasTXT;
    public float fuerzaGiro = 0.08f;
    private int direccion;
    private int tecla;
    
    //caída
    private float limite;
    private Quaternion posicionInicial;
    private float tiempoEspera = 3f;
    private float tiempoActual = 0f;
    public bool enEspera = false;

    //Tecla correcta
    public bool correcto = false;
    private float tiempoCorrecto;

    void Start(){
        Direccion();
        EleccionDeTecla();

        //caída
        limite = 45f;
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
        //teclasTXT.text = ("Pulsa " + tecla);
    }

    private void Direccion(){
        int numAleatorio = Random.Range(1,3);
        if(numAleatorio == 1) direccion = -1;
        if(numAleatorio == 2) direccion = 1;
    }

    private void Balanceo(){
        transform.Rotate(0f, 0f, fuerzaGiro * direccion, Space.World);
        //Teclas
        if (tecla == 0){
            teclasTXT.text = ("Pulsa W");
            if (Input.GetKey(KeyCode.W)) correcto = true;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) Caida();
        }
        if (tecla == 1){
            teclasTXT.text = ("Pulsa A");
            if (Input.GetKey(KeyCode.A)) correcto = true;
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) Caida();
        }
        if (tecla == 2){
            teclasTXT.text = ("Pulsa S");
            if (Input.GetKey(KeyCode.S)) correcto = true;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) Caida();
        }
        if (tecla == 3){
            teclasTXT.text = ("Pulsa D");
            if (Input.GetKey(KeyCode.D)) correcto = true;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)) Caida();
        }
    }

    private void Acierto(){
        if (correcto){
            teclasTXT.text = ("¡Has acertado!");
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

    private void Caida(){
        teclasTXT.text = ("Has caído");
        transform.rotation = posicionInicial;
        enEspera = true;
    }
    private void Limites(){
        //límite
        if (direccion==1 && transform.eulerAngles.z > limite || direccion==-1 && transform.eulerAngles.z < 360-limite){
            Caida();
        }
    }
}
