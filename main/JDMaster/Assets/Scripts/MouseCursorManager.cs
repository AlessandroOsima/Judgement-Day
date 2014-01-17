using UnityEngine;
using System.Collections;

public class MouseCursorManager : MonoBehaviour
{
    public Texture2D originalCursor;
    public int cursorSizeX; // set to width of your cursor texture
    public int cursorSizeY; // set to height of your cursor texture

    private bool _showCursor = true;
    private static MouseCursorManager _mouseManager;

    public MouseCursorManager()
    {

    }

    void Start()
    {
        //Screen.showCursor = false;      //hide standard cursor
        //Screen.lockCursor = true;
    }

    void Awake()
    {
        if (mouseManager == null) //this way you cannot create more than one Player_Score instance
            _mouseManager = this;
    }


    void OnGUI()
    {

        if (_showCursor == true)
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x - cursorSizeX / 2 + 1, (Screen.height - Input.mousePosition.y) - cursorSizeY / 2 + 1, cursorSizeX, cursorSizeY), originalCursor);
        }

    }

    #region Properties
    public bool ShowCursor
    {
        get
        {
            return _showCursor;
        }
        set
        {
            _showCursor = value;
        }
    }

    public static MouseCursorManager mouseManager
    {
        get
        {
            return _mouseManager;
        }
    }
    #endregion
}
