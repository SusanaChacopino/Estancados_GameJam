using UnityEngine;
using TMPro;

public class SCR_Puntos : MonoBehaviour{
    public float puntos;
    [SerializeField]
    private float puntosTotal;
    public TMP_Text puntosTXT;
    private float puntosMostrar;

    void Start(){
        puntos = 0f;
        puntosTotal = 25f;
    }

    void Update(){
        puntos += Time.deltaTime;
        puntosMostrar = Mathf.FloorToInt(puntos);
        puntosTXT.text = ("Puntos: " + puntosMostrar);
        if (puntos >= puntosTotal){
            puntosTXT.text = ("Cambio de nivel");
        }
    }
}
