using UnityEngine;

public class CustomizationController : EscapableBasePanel
{
    [SerializeField] GameObject toonParent;
    [SerializeField] ToonButton toonButtonPrefab;
    [SerializeField] ToonButton activeToonButton;
    [SerializeField] ToonDefinition[] toonDefinitions;

    private ToonButton[] availableToonButtons;
    private PlayerAvatarController controller;
    private bool didChange = false;
    private AvatarType oldType;
    private AvatarType newType;
    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from Customization menu");
        CloseMenu();
    }

    private void Start()
    {
        // Make listener for loading complete
        SavingUtility.LoadingComplete += CreateToonButtonsFromDefinitions;
        controller = FindObjectOfType<PlayerAvatarController>();
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
        oldType = SavingUtility.playerGameData.Avatar;

        // Load from stored value here
        activeToonButton.SetToonDefinition(toonDefinitions[(int)oldType]);

        // Update player to loaded avatar
        ChangePlayerAvatar(oldType); 
    }

    public void RequestChangeAvatar(ToonDefinition definition)
    {
        activeToonButton.SetToonDefinition(definition);

        if (definition.type == oldType)
        {
            Debug.Log("Changing to already active avatar, dont save");
            didChange = false;
            return;
        }

        didChange = true;
        newType = definition.type;
    }

    private void ChangePlayerAvatar(AvatarType type)
    {
        if (controller == null)
        {
            Debug.LogWarning("Could Not find PlayerAvatarController");
            controller = FindObjectOfType<PlayerAvatarController>();
        }
        // Change Player to this character
        controller.ChangeActiveAvatar(type);

    }

    public void CloseMenu()
    {
        if (didChange)
        {
            ChangePlayerAvatar(newType);
            Debug.Log("SAVE INVOKE - AVATAR CHANGED");
            SavingUtility.playerGameData.SetCharacter(newType);
            oldType = newType;
        }

        didChange = false;

        TransitionScreen.Instance.StartTransition(GameAction.HideCustomize);
    }

}
