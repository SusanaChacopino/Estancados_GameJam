using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MenuController : MonoBehaviour
{
    int NextLvl, PastLvl;
    bool Paused = false, SkipHints = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRandomScene() 
    {
        NextLvl = Random.Range(1, 3);
        if (NextLvl == PastLvl)
        {
            NextLvl = Random.Range(1, 3);
        }
        else
        {
            PastLvl = NextLvl;
            LoadScene(NextLvl);
        }
    }
    public void LoadScene(int Level)
    {
        if (Level == 0)
        {
            //Inicio
            SceneManager.LoadScene(0);
        }
        if (Level == 1)
        {
            //Chuches
            SceneManager.LoadScene(1);
            LoadExplicacion(1);
        }
        if (Level == 2)
        {
            //Equilibrio
            SceneManager.LoadScene(2);
            LoadExplicacion(2);
        }
        if (Level == 3)
        {
            //Robar
            SceneManager.LoadScene(3);
            LoadExplicacion(3);
        }
        if(Level == 4) 
        {
            //Menú final
            SceneManager.LoadScene(4);
        }


    }

    public void LoadExplicacion(int Explicacion)
    {
        if (SkipHints == false) 
        {
            if (Explicacion == 1)
                Debug.Log("EXP_1");
            if (Explicacion == 2)
                Debug.Log("EXP_2");
            if (Explicacion == 3)
                Debug.Log("EXP_3");
        }




    }

    public void Load(int ButtonPressed) 
    {
        if (ButtonPressed == 0)
        {
            //Menú inicio (Restart)
            LoadScene(0);
        }
        if (ButtonPressed == 1)
        {
            //Historia
            LoadScene(1);
        }
        if (ButtonPressed == 2)
        {
            //Frenesis
            LoadRandomScene();
        }
        if (ButtonPressed == 3)
        {
            //Ajustes
            SceneManager.LoadScene(5);
        }
        if (ButtonPressed == 4)
        {
            //Exit
            Debug.Log("Exit");
        }
        if (ButtonPressed == 5)
        {
            Paused = true;
        }
        if (ButtonPressed == 6)
        {
            Paused = false;
        }
    }
    
 }
