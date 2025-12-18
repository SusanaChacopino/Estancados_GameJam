using UnityEngine;

public class SCR_Camino : MonoBehaviour {
    private GameObject player;
    public float velocidad = -20f;
    private float grados;

    void Start(){
        player = GameObject.Find("Player");
    }

    void Update(){
        if (player.GetComponent<SCR_PlayerLVL2>().enEspera) grados = 0;
        else grados = velocidad * Time.deltaTime;

        transform.Rotate(grados, 0f, 0f, Space.World);
    }
}