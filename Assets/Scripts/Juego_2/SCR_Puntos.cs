using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SCR_Puntos : MonoBehaviour{
    private GameObject player;
    public float puntos;
    [SerializeField]
    private float puntosTotal;
    public TMP_Text puntosTXT;
    private float puntosMostrar;

    void Start(){
        puntos = 0f;
        puntosTotal = 20f;
        player = GameObject.Find("Player");
    }

    void Update(){
        if (player.GetComponent<SCR_PlayerLVL2>().enEspera) puntos = puntos;
        else puntos += Time.deltaTime;
        puntosMostrar = Mathf.FloorToInt(puntos);
        puntosTXT.text = ("Puntos: " + puntosMostrar);
        if (puntos >= puntosTotal){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
