using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_play : MonoBehaviour
{
    public void ChangeMenusScene(string nameScene)
    {
        Application.LoadLevel(nameScene);
    }
    
}
