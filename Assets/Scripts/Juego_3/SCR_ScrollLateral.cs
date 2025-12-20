using UnityEngine;
using System.Collections.Generic;

public class SCR_ScrollLateral : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    [Range(0f, 20f)]
    public float velocidadBase = 5f;

    [Range(0.1f, 3f)]
    public float multiplicadorVelocidad = 1f;

    [Header("Ralentización Externa")]
    [Range(0.01f, 1f)]
    public float factorRalentizacion = 1f;

    [Header("Capas del Escenario")]
    public CapaScroll[] capas;

    [System.Serializable]
    public class CapaScroll
    {
        public string nombre = "Capa";
        public List<Transform> piezas;

        [Range(0f, 1f)]
        public float factorParallax = 1f;

        public float anchoPieza = 20f;
        public bool seRepite = true;
    }

    private float velocidadActual = 0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        AjustarDificultadPorVictorias();
    }

    void Update()
    {
        ActualizarVelocidad();
        MoverCapas();
    }

    void ActualizarVelocidad()
    {
        velocidadActual = velocidadBase * multiplicadorVelocidad * factorRalentizacion;
    }

    void MoverCapas()
    {
        foreach (var capa in capas)
        {
            if (capa.piezas == null || capa.piezas.Count == 0) continue;

            float desplazamiento = velocidadActual * capa.factorParallax * Time.deltaTime;

            foreach (var pieza in capa.piezas)
            {
                if (pieza == null) continue;
                pieza.position += Vector3.left * desplazamiento;
            }

            if (capa.seRepite)
            {
                ReposicionarPiezas(capa);
            }
        }
    }

    void ReposicionarPiezas(CapaScroll capa)
    {
        if (mainCamera == null) return;

        float alturaCamera = mainCamera.orthographicSize;
        float anchoCamera = alturaCamera * mainCamera.aspect;
        float bordeIzquierdo = mainCamera.transform.position.x - anchoCamera - 10f;

        foreach (var pieza in capa.piezas)
        {
            if (pieza == null) continue;

            if (pieza.position.x < bordeIzquierdo)
            {
                float maxX = float.MinValue;
                foreach (var otraPieza in capa.piezas)
                {
                    if (otraPieza != null && otraPieza.position.x > maxX)
                    {
                        maxX = otraPieza.position.x;
                    }
                }

                Vector3 nuevaPos = pieza.position;
                nuevaPos.x = maxX + capa.anchoPieza;
                pieza.position = nuevaPos;

                //Resetear personaje al reposicionar
                ResetearPersonaje(pieza.gameObject);
            }
        }
    }

    void ResetearPersonaje(GameObject objeto)
    {
        // Solo procesar si es un personaje interceptable
        if (objeto.CompareTag("PersonajeInterceptable"))
        {
            // Reactivar collider
            Collider2D collider = objeto.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Restaurar opacidad completa
            SpriteRenderer sr = objeto.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 1f;
                sr.color = color;
            }

            //Desactivar nube de golpes
            DesactivarSpriteRobado(objeto);
        }
        foreach (Transform hijo in objeto.transform)
        {
            ResetearPersonaje(hijo.gameObject);
        }
    }

    void DesactivarSpriteRobado(GameObject personaje)
    {
        // Buscar hijo por nombre
        Transform hijo = personaje.transform.Find("Nube_Pelea");

        // Si no lo encuentra por nombre, buscar el primer hijo
        if (hijo == null && personaje.transform.childCount > 0)
        {
            hijo = personaje.transform.GetChild(0);
        }

        if (hijo != null)
        {
            hijo.gameObject.SetActive(false);
        }
    }

    public void AumentarVelocidad(float incremento)
    {
        velocidadBase += incremento;
    }

    public void CambiarMultiplicador(float nuevoMultiplicador)
    {
        multiplicadorVelocidad = nuevoMultiplicador;
    }

    public void DetenerScroll()
    {
        velocidadBase = 0f;
    }

    public void ReiniciarScroll(float velocidad)
    {
        velocidadBase = velocidad;
    }

    public float ObtenerVelocidadActual()
    {
        return velocidadActual;
    }

    void AjustarDificultadPorVictorias()
    {
        if (SCR_RachaTiempo.instance == null) return;

        int victorias = SCR_RachaTiempo.instance.juegosGanados;

        // Cada victoria aumenta velocidad
        float multiplicador = 1f + (victorias * 0.15f); // +15% por victoria
        multiplicador = Mathf.Min(multiplicador, 2.2f); // Máximo 2.2x

        velocidadBase *= multiplicador;

        Debug.Log($"[Nivel3-Scroll] Dificultad ajustada. Victorias: {victorias}, VelBase: {velocidadBase:F2}");
    }
}