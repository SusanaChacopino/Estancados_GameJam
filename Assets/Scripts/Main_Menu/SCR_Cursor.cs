using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SCR_Cursor : MonoBehaviour
{
    public Texture2D cursor_normal;
    public Vector2 normalCursorHotSpot;

    public Texture2D cursor_OnButton;
    public Vector2 onButtonCursorHotSpot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
