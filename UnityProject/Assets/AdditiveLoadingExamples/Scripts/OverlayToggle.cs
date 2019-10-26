using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class OverlayToggle : MonoBehaviour
{
    private string watchedScenePath;
    public Object sceneToWatch;
    public GameObject[] overlayGameObjects;

    private void Awake()
    {
        if (sceneToWatch!=null)
        {
            watchedScenePath = AssetDatabase.GetAssetPath(sceneToWatch);
        }
        else
        {
            Debug.LogError("Scenes to Watch Scene Array is empty");
        }
    }

    public void Update()
    {
        for (int i = 0; i < overlayGameObjects.Length; i++)
        {
            if (!SceneManager.GetSceneByPath(watchedScenePath).isLoaded && overlayGameObjects[i] != null)
            {
                overlayGameObjects[i].SetActive(true);
            }
            else
            {
                overlayGameObjects[i].SetActive(false);
            }
        }
    }
}
