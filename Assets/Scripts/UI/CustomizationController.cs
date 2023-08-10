using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CustomizationController : BasePanel
{
    [SerializeField] GameObject toonParent;
    [SerializeField] ToonButton toonButtonPrefab;
    [SerializeField] ToonButton activeToonButton;
    [SerializeField] ToonDefinition[] toonDefinitions;

    private ToonButton[] availableToonButtons;

    private void Start()
    {
        CreateToonButtonsFromDefinitions();
    }

    private void CreateToonButtonsFromDefinitions()
    {
        //Remove current buttons
        foreach (Transform child in toonParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var def in toonDefinitions)
        {
            ToonButton newButton = Instantiate(toonButtonPrefab, toonParent.transform);            
            newButton.SetToonDefinition(def);
        }

        // Load from stored value here
        activeToonButton.SetToonDefinition(toonDefinitions[0]);
    }

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.started += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.started -= RequestESC;
    }

    public void RequestChangeAvatar(ToonDefinition definition)
    {
        activeToonButton.SetToonDefinition(definition);
        // Change Player to this character
        FindObjectOfType<PlayerAvatarController>().ChangeActiveAvatar(definition.type);
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from Customization menu");
    }
}
