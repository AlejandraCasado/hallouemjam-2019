using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


[ExecuteInEditMode]
public class SceneSwapper : MonoBehaviour
{
    // Setup array for Scene objects and a list to store their paths
    public Object sceneA;
    public Object sceneB;
    private string sceneAPath;
    private string sceneBPath;

    private bool activeScene = true;

    void OnEnable()
    {
        sceneAPath = AssetDatabase.GetAssetPath(sceneA);
        sceneBPath = AssetDatabase.GetAssetPath(sceneB);
    }

    private bool CheckScenes()
    {
        return (sceneA != null && sceneB != null ? true : false);
    }

    // Receives a Scene list index which is then loaded additvely
    public void SceneSwap()
    {
        if (Application.isPlaying && CheckScenes())
        {
            if (activeScene)
            {
                Debug.Log("Unloading: " + sceneAPath);
                SceneManager.UnloadSceneAsync(sceneAPath);
                SceneManager.LoadScene(sceneBPath, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.UnloadSceneAsync(sceneBPath);
                SceneManager.LoadScene(sceneAPath, LoadSceneMode.Additive);
            }
            activeScene = !activeScene;
        }
    }
} 