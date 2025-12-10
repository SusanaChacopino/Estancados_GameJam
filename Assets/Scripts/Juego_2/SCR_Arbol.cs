using UnityEngine;

public class SCR_Arbol : MonoBehaviour
{
    void Update(){
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
}
