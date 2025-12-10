using UnityEngine;

public class SCR_Camino : MonoBehaviour{
    public float velocidad = -0.2f;
    void Start(){
        
    }

    void Update(){
        transform.Rotate(velocidad, 0f, 0f, Space.World);
    }
}
