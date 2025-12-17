using Unity.VisualScripting;
using UnityEngine;

public class SCR_MusicManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static SCR_MusicManager Instancia;

    private void Awake()
    {
        // Si no hay instancia, yo soy la instancia
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // No lo matamos al cargar otra escena
        }
        // Si ya existe una instancia, yo soy un duplicado
        else
        {
            Destroy(gameObject); // Me destruyo a mí mismo para no causar eco
        }
    }
}