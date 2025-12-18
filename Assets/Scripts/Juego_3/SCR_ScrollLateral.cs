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
            }
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
}