using UnityEngine;
using UnityEngine.UI;


public class RadioButtonNavigation : MonoBehaviour
{
    public int activeButton;
    public Toggle[] radioButtons;
    public GameObject[] targetObjects;

    private void Awake()
    {
        Debug.Assert(activeButton < radioButtons.Length, "Radio Button Navigation: Active button greater than the number of radio buttons");
        Debug.Assert(radioButtons.Length > 0, "Radio Button Navigation: Radio Buttons array is empty");
        Debug.Assert(targetObjects.Length > 0, "Radio Button Navigation: Navigation Item array is empty");
        Debug.Assert(radioButtons.Length == targetObjects.Length, "Radio Button Navigation: Your radio buttons and navigation arrays are not the same size. Do you need to add more buttons?");
    }

    public void Next()  // Increment the index of the active button and then re-update button states
    {
        if (activeButton < radioButtons.Length)
        {
            activeButton++;
            UpdateToggles(activeButton);
        }
    }

    private bool ValidateArrays()
    {
        return (radioButtons.Length > 0 && targetObjects.Length <= radioButtons.Length && activeButton < radioButtons.Length ? true : false);
    }


    public void Previous() // Increment the index of the active button down and then re-update button states
    {
        if (activeButton > 0)
        {
            activeButton--;
            UpdateToggles(activeButton);
        }
    }

    public void UpdateTargetObjects(int activeItem)  // Iterate through array of GameObjects and enable that whose index matches the corresponding toggle
    {
        for (int i = 0; i < radioButtons.Length; i++)
        {
            targetObjects[i].SetActive(ValidateArrays() && i == activeItem ? true : false);
        }
    }

    public void UpdateToggles(int newToggle) // Iterate through toggle array. Set the toggle with index matching new value to be "on" and update navigation.
    {
        if (ValidateArrays())
        {
            for (int i = 0; i < radioButtons.Length; i++)
            {
                radioButtons[i].isOn = (newToggle == i);
                radioButtons[i].graphic.CrossFadeAlpha(radioButtons[i].isOn ? 1f : 0f, 0.1f, false);
                UpdateTargetObjects(activeButton);
            }
        }
    }

    public void ToggleSelected() // Update active toggle and then use this to update navigation items
    {
        for (int i = 0; i < radioButtons.Length; i++)
        {
            if (ValidateArrays() && radioButtons[i].isOn && i != activeButton)
            {
                activeButton = i;
                UpdateTargetObjects(activeButton);
            }
        }
    }
}