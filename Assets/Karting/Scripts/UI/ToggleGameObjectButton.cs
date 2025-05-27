using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleGameObjectButton : MonoBehaviour
{
    public GameObject objectToToggle;
    public GameObject mainScenekart;
    public bool resetSelectionAfterClick;

    public GameObject defaultButtonToSelect; // For when menu opens
    public GameObject fallbackButtonToSelect; // ⬅ For when menu closes

    void Update()
    {
        if (objectToToggle.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel))
        {
            SetGameObjectActive(false);
        }
    }

    public void SetGameObjectActive(bool active)
    {
        objectToToggle.SetActive(active);

        if (mainScenekart != null)
            mainScenekart.SetActive(!active);

        if (active)
        {
            if (defaultButtonToSelect != null)
                EventSystem.current.SetSelectedGameObject(defaultButtonToSelect);
        }
        else
        {
            if (fallbackButtonToSelect != null)
                EventSystem.current.SetSelectedGameObject(fallbackButtonToSelect);
            else if (resetSelectionAfterClick)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
