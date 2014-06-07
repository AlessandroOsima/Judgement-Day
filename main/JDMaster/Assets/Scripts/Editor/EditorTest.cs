using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class EditorTest  : EditorWindow
{
    public int test = 10;
    public float anothertest = 20.0f;

    [MenuItem("Window/Create Window")]
    public static void CreateWizard() 
    {
        EditorWindow.GetWindow<EditorTest>();
    }

    void OnGUI()
    {
        GUILayout.Label("Test Window", EditorStyles.boldLabel);

        GUILayout.BeginVertical("box");
        GUILayout.Label("a value", "box");
        GUILayout.Label("another value", "box");
        GUILayout.EndVertical();
    }
}
