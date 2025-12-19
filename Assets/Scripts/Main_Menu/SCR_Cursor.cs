
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SCR_Cursor : MonoBehaviour, IPointerEnterHandler
{
    public Texture2D cursor_normal;
    public Vector2 normalCursorHotSpot;

    public Texture2D cursor_OnButton;
    public Vector2 onButtonCursorHotSpot;

    public static AudioSource audioGlobal;
    public AudioClip sonidoHover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Si no existe el AudioSource global, crearlo
        if (audioGlobal == null)
        {
            // Buscar o crear un GameObject para el audio
            GameObject audioObj = GameObject.Find("AudioBotones");
            if (audioObj == null)
            {
                audioObj = new GameObject("AudioBotones");
            }

            audioGlobal = audioObj.GetComponent<AudioSource>();
            if (audioGlobal == null)
            {
                audioGlobal = audioObj.AddComponent<AudioSource>();
            }

            audioGlobal.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonCursorEnter()
    {
        Cursor.SetCursor(cursor_OnButton, onButtonCursorHotSpot, CursorMode.Auto);
    }

    public void OnButtonCursorExit()
    {
        Cursor.SetCursor(cursor_normal, normalCursorHotSpot, CursorMode.Auto);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sonidoHover != null && audioGlobal != null)
        {
            audioGlobal.PlayOneShot(sonidoHover);
        }
    }
}
