using UnityEngine;
using System.Collections.Generic;

public class SCR_ScrollLateral : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    [Tooltip("Velocidad base de scroll (unidades por segundo)")]
    [Range(0f, 20f)]
    public float velocidadBase = 5f;

    [Tooltip("Multiplicador de velocidad")]
    [Range(0.1f, 3f)]
    public float multiplicadorVelocidad = 1f;

    [Header("Capas del Escenario")]
    [Tooltip("Capas que se moverán con parallax")]
    public CapaScroll[] capas;

    [System.Serializable]
    public class CapaScroll
    {
        [Tooltip("Nombre de la capa (solo para organización)")]
        public string nombre = "Capa";

        [Tooltip("Lista de piezas de esta capa")]
        public List<Transform> piezas;

        [Tooltip("Factor de parallax (0 = no se mueve, 1 = velocidad completa)")]
        [Range(0f, 1f)]
        public float factorParallax = 1f;

        [Tooltip("Ancho de cada pieza (para saber cuándo reposicionar)")]
        public float anchoPieza = 20f;

        [Tooltip("¿Esta capa se repite infinitamente?")]
        public bool seRepite = true;
    }

    private float velocidadActual = 0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        velocidadActual = velocidadBase;
    }

    void Update()
    {
        ActualizarVelocidad();
        MoverCapas();
    }

    void ActualizarVelocidad()
    {
        velocidadActual = velocidadBase * multiplicadorVelocidad;
    }

    void MoverCapas()
    {
        foreach (var capa in capas)
        {
            if (capa.piezas == null || capa.piezas.Count == 0) continue;

            // Calcular desplazamiento según parallax
            float desplazamiento = velocidadActual * capa.factorParallax * Time.deltaTime;

            // Mover todas las piezas hacia la IZQUIERDA
            foreach (var pieza in capa.piezas)
            {
                if (pieza == null) continue;
                pieza.position += Vector3.left * desplazamiento;
            }

            // Si se repite, verificar reposicionamiento
            if (capa.seRepite)
            {
                ReposicionarPiezas(capa);
            }
        }
    }

    void ReposicionarPiezas(CapaScroll capa)
    {
        if (mainCamera == null) return;

        // Calcular borde izquierdo de la cámara
        float alturaCamera = mainCamera.orthographicSize;
        float anchoCamera = alturaCamera * mainCamera.aspect;
        float bordeIzquierdo = mainCamera.transform.position.x - anchoCamera - 2f;

        foreach (var pieza in capa.piezas)
        {
            if (pieza == null) continue;

            // Si la pieza salió completamente por la izquierda
            if (pieza.position.x < bordeIzquierdo)
            {
                // Encontrar la pieza más a la derecha
                float maxX = float.MinValue;
                foreach (var otraPieza in capa.piezas)
                {
                    if (otraPieza != null && otraPieza.position.x > maxX)
                    {
                        maxX = otraPieza.position.x;
                    }
                }

                // Reposicionar justo después de la última pieza
                Vector3 nuevaPos = pieza.position;
                nuevaPos.x = maxX + capa.anchoPieza;
                pieza.position = nuevaPos;

                Debug.Log($"[Scroll] Reposicionada {pieza.name} a X={nuevaPos.x:F2}");
            }
        }
    }

    // Métodos públicos para control externo
    public void AumentarVelocidad(float incremento)
    {
        velocidadBase += incremento;
        Debug.Log($"[Scroll] Velocidad aumentada a {velocidadBase:F2}");
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
}
