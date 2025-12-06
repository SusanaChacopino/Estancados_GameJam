using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class SCR_Explicacion : MonoBehaviour
{
    public TextMeshProUGUI textoExplicación;
    public VideoPlayer videoExplicacion;

    public VideoClip videoChuches;
    //public VideoClip videoEquilibrio;
    //public VideoClip videoRobar;

    public Image teclaW;
    public Image teclaA;
    public Image teclaS;
    public Image teclaD;
    public Image teclaFlechaArriba;
    public Image teclaFlechaAbajo;
    public Image teclaFlechaIzquierda;
    public Image teclaFlechaDerecha;
    public Image teclaEspacio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0;

        teclaW.gameObject.SetActive(false);
        teclaA.gameObject.SetActive(false);
        teclaS.gameObject.SetActive(false);
        teclaD.gameObject.SetActive(false);
        teclaFlechaArriba.gameObject.SetActive(false);
        teclaFlechaAbajo.gameObject.SetActive(false);
        teclaFlechaIzquierda.gameObject.SetActive(false);
        teclaFlechaDerecha.gameObject.SetActive(false);
        teclaEspacio.gameObject.SetActive(false);

        TextosExplicacionEscena();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TextosExplicacionEscena()
    {
        string escena = SceneManager.GetActiveScene().name;

         switch (escena)
        {
            case "Juego chuches":
                textoExplicación.text = "¡Engulle todas las chuches antes de que la rana termine de enrollar su enorme lengua! Pero OJO, si te tragas una fruta por accidente… ¡zas! ¡Tus puntos se escapan brincando!";
                videoExplicacion.clip = videoChuches;
                teclaA.gameObject.SetActive(true);
                teclaD.gameObject.SetActive(true);
                teclaFlechaIzquierda.gameObject.SetActive(true);
                teclaFlechaDerecha.gameObject.SetActive(true);
                break;

            case "Juego equilibrio":
                textoExplicación.text = "Ey, rana mareada, ¡no te caigas! Mantén tu caminito tambaleante pulsando las teclas justo a tiempo… antes de que la otra rana termine de enrollar su larguísima lengua";
                videoExplicacion.clip = videoChuches;
                break;

            case "Juego robar":
                textoExplicación.text = "¡Rana sigilosa al ataque! Acierta el momento justo en la barra de precisión y roba los objetos de las otras ranas sin que te vean… ¡y cuidado con la otra rana y su lengua que se enrolla sin parar!";
                videoExplicacion.clip = videoChuches;
                teclaEspacio.gameObject.SetActive(true);
                break;

            default:
                textoExplicación.text = "no hay explicación";
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
