using UnityEngine;
using UnityEngine.UI;

public class SCR_Teclas : MonoBehaviour{
    public Sprite[] teclasImg; //ordenados por w0,a1,s2,d3
    private GameObject player;
    private Image sr;

    void Start(){
        player = GameObject.Find("Player");
        sr = GetComponent<Image>();
    }

    void Update(){
        if (player.GetComponent<SCR_PlayerLVL2>().tecla == 0) sr.sprite = teclasImg[0];
        if (player.GetComponent<SCR_PlayerLVL2>().tecla == 1) sr.sprite = teclasImg[1];
        if (player.GetComponent<SCR_PlayerLVL2>().tecla == 2) sr.sprite = teclasImg[2];
        if (player.GetComponent<SCR_PlayerLVL2>().tecla == 3) sr.sprite = teclasImg[3];

        if (player.GetComponent<SCR_PlayerLVL2>().enEspera || player.GetComponent<SCR_PlayerLVL2>().correcto) sr.enabled = false;
        else sr.enabled = true;
    }
}
