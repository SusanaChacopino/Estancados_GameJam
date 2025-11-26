using UnityEngine;

public class SCR_ColliderAnimations : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión " + other.gameObject.tag);
        if (other.gameObject.CompareTag("1p")|| other.gameObject.CompareTag("2p")|| other.gameObject.CompareTag("fruta")) 
        {
            Debug.Log("Abre Boca");
        }
    }
}
