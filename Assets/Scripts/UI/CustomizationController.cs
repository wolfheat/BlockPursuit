using UnityEngine;

public class CustomizationController : EscapableBasePanel
{
    [SerializeField] GameObject toonParent;
    [SerializeField] ToonButton toonButtonPrefab;
    [SerializeField] ToonButton activeToonButton;
    [SerializeField] ToonDefinition[] toonDefinitions;

    private ToonButton[] availableToonButtons;

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

    public void CloseMenu()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideCustomize);
    }

}
