using UnityEngine;

public class SCR_Puntos : MonoBehaviour{
    private GameObject player;
    public float puntos;
    [SerializeField]

    void Start(){
        puntos = 0f;
        player = GameObject.Find("Player");
    }

    void Update(){
        if (player.GetComponent<SCR_PlayerLVL2>().enEspera) puntos = puntos;
        else puntos += Time.deltaTime;
        //Debug.Log(puntos);
    }

    public float ObtenerPuntaje(){
        return puntos;
    }
}
