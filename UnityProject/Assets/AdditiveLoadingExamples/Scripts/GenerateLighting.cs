using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GenerateLighting : MonoBehaviour
{
    // Button labels
    private string generateButton;
    const string cancelBakeLabel = "Cancel Bake";
    const string generateLightingLabel = "Generate Lighting";

    // Console messages
    const string bakeStartedMessage = "Bake started";
    const string bakeCancelledMessage = "Bake cancelled";

    void OnEnable()
    {
        generateButton = generateLightingLabel;

        // Set lightmapping delegates to update button UI on completion
        ResetGenerateButton();
        Lightmapping.completed += ResetGenerateButton;
    }
    // Reset the label for the generate button when lightmapping is complete
    void ResetGenerateButton()
    {
        generateButton = generateLightingLabel;
    }

    public string GetGenerateButtonLabel()
    {
        return generateButton;
    }

    // Cleanup lightmapping delegates when not inspector active
    void OnDisable()
    {
        Lightmapping.completed -= ResetGenerateButton;
    }

    // Starts an asynchronous bake when called
    public void GenerateLightingToggle()
    {
        if (!Lightmapping.isRunning) // If lightmapper is not already running, start the bake
        {
            Debug.Log(bakeStartedMessage);
            generateButton = cancelBakeLabel;
            Lightmapping.BakeAsync();
        }
        else
        {
            Debug.Log(bakeCancelledMessage); // Otherwise, if the lightmapper is already running, cancel the curent job 
            ResetGenerateButton();
            Lightmapping.Cancel();
        }
    }
}
