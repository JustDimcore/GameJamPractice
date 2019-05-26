using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [MenuItem("[ MENU ]/Restart", false, 100)]
    public static void Restart()
    {
        if (EditorApplication.isPlaying)
            GameController.Instance.Restart();
    }

    [MenuItem("[ MENU ]/AddMeat", false, 100)]
    public static void AddMeat()
    {
        if (EditorApplication.isPlaying)
            GameController.Instance.AddMeat();
    }
}
