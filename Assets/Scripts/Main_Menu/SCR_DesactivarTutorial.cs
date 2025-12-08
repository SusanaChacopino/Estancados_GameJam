using UnityEngine;

public class SCR_DesactivarTutorial : MonoBehaviour
{
    public GameObject tickSprite;
    private bool tutorialesActivos = true;

    void Start()
    {
        tutorialesActivos = PlayerPrefs.GetInt("TutorialesActivos", 1) == 1;
        tickSprite.SetActive(tutorialesActivos);
    }

    public void DesactivarTutoriales()
    {

        tutorialesActivos = !tutorialesActivos;
        tickSprite.SetActive(tutorialesActivos);

        PlayerPrefs.SetInt("TutorialesActivos", tutorialesActivos ? 1 : 0);
        PlayerPrefs.Save();

        GameObject[] tutoriales = GameObject.FindGameObjectsWithTag("Tutorial");

        foreach (GameObject t in tutoriales)
        {
            t.SetActive(tutorialesActivos);
        }

        Debug.Log("Tutoriales activos: " + tutorialesActivos);
    }
}
