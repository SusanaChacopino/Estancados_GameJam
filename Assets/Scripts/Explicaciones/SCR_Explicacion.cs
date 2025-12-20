using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class SCR_Explicacion : MonoBehaviour
{
    [Header("Activar/Desactivar Tutoriales")]
    public SCR_DesactivarTutorial TutorialBool;

    [Header("VideoTutoriales")]
    public VideoPlayer videoExplicacion;
    public TextMeshProUGUI textoExplicacion;

    public VideoClip videoChuches;
    public VideoClip videoEquilibrio;
    public VideoClip videoRobar;

    [Header("Imagenes Teclas")]
    public Image teclaW;
    public Image teclaA;
    public Image teclaS;
    public Image teclaD;
    public Image teclaFlechaArriba;
    public Image teclaFlechaAbajo;
    public Image teclaFlechaIzquierda;
    public Image teclaFlechaDerecha;
    public Image teclaEspacio;

    private bool yaEjecute = false;

    void Update()
    {
        // Ejecutar solo una vez en el primer frame 
        if (!yaEjecute)
        {
            yaEjecute = true;
            VerificarYMostrarTutorial();
        }
    }

    void VerificarYMostrarTutorial()
    {
        // Verificar modo Frenesí (sin tutoriales)
        bool modoFrenesi = PlayerPrefs.GetInt("ModoFrenesi", 0) == 1;

        if (modoFrenesi)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }

        // Verificacion si usuario desactivó tutoriales
        bool tutorialesActivos = PlayerPrefs.GetInt("TutorialesActivos", 1) == 1;

        if (!tutorialesActivos)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }

        // Mostrar tutorial
        gameObject.SetActive(true);
        Time.timeScale = 0;

        OcultarTodasLasTeclas();
        TextosExplicacionEscena();
    }

    private void OcultarTodasLasTeclas()
    {
        teclaW.gameObject.SetActive(false);
        teclaA.gameObject.SetActive(false);
        teclaS.gameObject.SetActive(false);
        teclaD.gameObject.SetActive(false);
        teclaFlechaArriba.gameObject.SetActive(false);
        teclaFlechaAbajo.gameObject.SetActive(false);
        teclaFlechaIzquierda.gameObject.SetActive(false);
        teclaFlechaDerecha.gameObject.SetActive(false);
        teclaEspacio.gameObject.SetActive(false);
    }

    private void TextosExplicacionEscena()
    {
        // ATENCION: Los nombres de las escenas deben coincidir exactamente
        string escena = SceneManager.GetActiveScene().name;

        switch (escena)
        {
            case "Juego chuches":
                textoExplicacion.text = "¡Engulle todas las chuches antes de que la rana termine de enrollar su enorme lengua! Pero OJO, si te tragas una fruta por accidente! ¡zas! ¡Tus puntos se escapan brincando!";
                videoExplicacion.clip = videoChuches;
                teclaA.gameObject.SetActive(true);
                teclaD.gameObject.SetActive(true);
                teclaFlechaIzquierda.gameObject.SetActive(true);
                teclaFlechaDerecha.gameObject.SetActive(true);
                break;

            case "Juego equilibrio":
                textoExplicacion.text = "Ey, rana mareada, ¡no te caigas! Mantén tu caminito tambaleante pulsando las teclas justo a tiempo, antes de que la otra rana termine de enrollar su larguísima lengua";
                videoExplicacion.clip = videoEquilibrio;
                teclaW.gameObject.SetActive(true);
                teclaA.gameObject.SetActive(true);
                teclaS.gameObject.SetActive(true);
                teclaD.gameObject.SetActive(true);
                break;

            case "Juego robar":
                textoExplicacion.text = "¡Rana sigilosa al ataque! Acierta el momento justo en la barra de precisión y roba los objetos de las otras ranas sin que te vean ¡y cuidado con la otra rana y su lengua que se enrolla sin parar!";
                videoExplicacion.clip = videoRobar;
                teclaEspacio.gameObject.SetActive(true);
                break;

            default:
                textoExplicacion.text = "no hay explicación";
                break;
        }

        videoExplicacion.Play();
    }

    public void Jugar()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}