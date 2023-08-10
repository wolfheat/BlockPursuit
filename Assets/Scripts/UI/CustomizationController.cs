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
        // Make listener for loading complete
        SavingUtility.LoadingComplete += CreateToonButtonsFromDefinitions;
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

        // Read saved type
        AvatarType storedType = SavingUtility.playerGameData.Avatar;

        // Load from stored value here
        activeToonButton.SetToonDefinition(toonDefinitions[(int)storedType]);

        // Update player to loaded avatar
        ChangePlayerAvatar(storedType); 
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

        ChangePlayerAvatar(definition.type);

        SavingUtility.playerGameData.SetCharacter(definition.type);
    }

    private void ChangePlayerAvatar(AvatarType type)
    {
        // Change Player to this character
        FindObjectOfType<PlayerAvatarController>().ChangeActiveAvatar(type);

    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from Customization menu");
    }
}
