using UnityEngine;
using UnityEditor;

public class GridEditor : EditorWindow
{
    [MenuItem("Grid / Open")]
    static void Init()
    {
        GridEditor window = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
        window.position = new Rect(Screen.width/2,Screen.height/2-150,600,700);
        window.Show();
    }
}
