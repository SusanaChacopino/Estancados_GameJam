using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MenuController : MonoBehaviour
{
    int NextLvl, PastLvl;

    public GameObject PauseMenu, GameUI, Ajustes;

    void Start()
    {
        // NO hacer nada aquí para no interferir
    }

    void Update()
    {

    }

    public void LoadRandomScene()
    {
        NextLvl = Random.Range(1, 4);

        if (NextLvl == PastLvl)
        {
            NextLvl = Random.Range(1, 4);
        }

        PastLvl = NextLvl;
        LoadScene(NextLvl);
    }

    public void LoadScene(int Level)
    {
        if (Level == 0)
        {
            //Inicio - Resetear al volver al menú
            PlayerPrefs.SetInt("ModoFrenesi", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
        if (Level == 1)
        {
            //Chuches
            SceneManager.LoadScene(1);
        }
        if (Level == 2)
        {
            //Equilibrio
            SceneManager.LoadScene(2);
        }
        if (Level == 3)
        {
            //Robar
            SceneManager.LoadScene(3);
        }
        if (Level == 4)
        {
            //Menú final
            PlayerPrefs.SetInt("ModoFrenesi", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(4);
        }
    }

    public void Load(int ButtonPressed)
    {
        if (ButtonPressed == 0)
        {
            //Menú inicio (Restart)
            PlayerPrefs.SetInt("ModoFrenesi", 0);
            PlayerPrefs.Save();
            LoadScene(0);
        }
        if (ButtonPressed == 1)
        {
            //Historia - CON tutoriales
            Debug.Log("[MENU] Historia: Guardando ModoFrenesi = 0");
            PlayerPrefs.SetInt("ModoFrenesi", 0);
            PlayerPrefs.Save();
            Debug.Log("[MENU] Verificando: " + PlayerPrefs.GetInt("ModoFrenesi"));
            LoadScene(1);
        }
        if (ButtonPressed == 2)
        {
            //Frenesí - SIN tutoriales
            Debug.Log("[MENU] Frenesi: Guardando ModoFrenesi = 1");
            PlayerPrefs.SetInt("ModoFrenesi", 1);
            PlayerPrefs.Save();
            Debug.Log("[MENU] Verificando: " + PlayerPrefs.GetInt("ModoFrenesi"));
            LoadRandomScene();
        }
        if (ButtonPressed == 3)
        {
            //Ajustes
            Settings();
        }
        if (ButtonPressed == 4)
        {
            //Exit
            Application.Quit();
            Debug.Log("Exit");
        }
        if (ButtonPressed == 5)
        {
           
            Pause();
        }
        if (ButtonPressed == 6)
        {
     
            Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Instantiate(PauseMenu);
    }

    public void Resume()
    {
        GameObject PauseMenuToDestroy = GameObject.Find("Pause menu(Clone)");
        Destroy(PauseMenuToDestroy);
        Time.timeScale = 1;
    }

    public void Settings()
    {
        Instantiate(Ajustes);
    }

    public void ExitSettings()
    {
        GameObject AjustesToDestroy = GameObject.Find("Ajustes(Clone)");
        Destroy(AjustesToDestroy);
    }
}