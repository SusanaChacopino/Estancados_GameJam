using UnityEngine;

public class SCR_PlayerLVL2 : MonoBehaviour{
    public float fuerzaGiro;
    private Quaternion posicionInicial;
    public float limiteIzq, limiteDer;
    public bool izq;

    void Start(){
        fuerzaGiro = 0.2f;
        posicionInicial = transform.rotation;
        limiteIzq = 35f;
        limiteDer = 105f;
    }

    void Update(){
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
        //ESTO ESTÁ POR REVISAR, falla el límite derecho

        //esto está porque eulerangles no cuenta num negativos
        if (transform.eulerAngles.z < 180){
            izq = true;
        }
        if (transform.eulerAngles.z >= 180){
            izq = false;
        }

        //límite
        if (izq && transform.eulerAngles.z > limiteIzq){
            Debug.Log("Límite atravesado izq");
            //transform.rotation = posicionInicial;
            //cambiar anim a caída
        }
        if (!izq && transform.eulerAngles.z < limiteDer){
            Debug.Log("Límite atravesado der");
        }
    }
}
