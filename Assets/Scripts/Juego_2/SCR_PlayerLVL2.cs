using UnityEngine;

public class SCR_PlayerLVL2 : MonoBehaviour{
    //balanceo
    public float fuerzaGiro = 0.08f;
    private int direccion;

    //caída
    private float limite;
    private Quaternion posicionInicial;
    private float tiempoEspera = 3f;
    private float tiempoActual = 0f;
    public bool enEspera = false;
   /* private float esperaPorCaida;

    //balanceo
    public bool correcto;
    public bool fallo;
    private int tecla;
*/
    void Start(){

        Direccion();
        EleccionDeTecla();

        //caída
        limite = 45f;
        posicionInicial = transform.rotation;
        /*tecla = 0;
        esperaPorCaida = 3f;
        tiempoEspera = 0.5f;
        tiempoActual = Time.time;
        correcto = true;
        tecla = Random.Range(0, 4);

        //--------------//
        numAleatorio = Random.Range(1,3);
        if(numAleatorio == 1) direccion = -1;
        if(numAleatorio == 2) direccion = 1;*/
    }

    void Update(){

        Balanceo();
        Limites();

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
        /*
        //Teclas
        if(tecla==0){
            if (Input.GetKey(KeyCode.W)) correcto = true;
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) fallo = true;
        }
        if(tecla==1){
            if (Input.GetKey(KeyCode.A)) correcto = true;
            else if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) fallo = true;
        }
        if(tecla==2){
            if (Input.GetKey(KeyCode.S)) correcto = true;
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) fallo = true;
        }
        if(tecla==3){
            if (Input.GetKey(KeyCode.D)) correcto = true;
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)) fallo = true;
        }*/
    }

    private void EleccionDeTecla(){
        int tecla = Random.Range(0,4);
        Debug.Log(tecla);
    }

    private void Direccion(){
        int numAleatorio = Random.Range(1,3);
        if(numAleatorio == 1) direccion = -1;
        if(numAleatorio == 2) direccion = 1;
    }
    
    private void Balanceo(){
        transform.Rotate(0f, 0f, fuerzaGiro * direccion, Space.World);
    }

    /*    if (correcto){//si acierta correcto es activado
            Debug.Log("¡Correcto!");
            transform.Rotate(0f, 0f, fuerzaGiro * direccion, Space.World);
            correcto = false;
            tiempoEspera = Random.Range(0.5f, 4f);
            tiempoActual = Time.time;
            tecla = Random.Range(0, 4);
            DireccionYTecla();
        } else {
            if (fallo){//si se equivoca falo es activado
                Debug.Log("Fallo");
                tiempoActual = Time.time;
                transform.Rotate(0f, 0f, fuerzaGiro, Space.World);
                if (esperaPorCaida <= tiempoActual){
                    fallo = false;
                }
            }
        }
        //si está en la derecha o en la izquierda
        /*if (transform.eulerAngles.z < 180) izq = true;
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
    */
    private void Limites(){
        //límite
        if (direccion==1 && transform.eulerAngles.z > limite || direccion==-1 && transform.eulerAngles.z < 360-limite){
            Debug.Log("Límite atravesado");
            Direccion();
            transform.rotation = posicionInicial;
            enEspera = true;
        }
    }
}
