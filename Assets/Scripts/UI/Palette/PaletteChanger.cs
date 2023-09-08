using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Tags{ MenuBackgroundBacks , MenuBackgroundFronts , SubMenuBackgrounds, SubMenuBackgroundDarks, MissionBackgrounds, LockedPanels, LockedPanelRewards, LockedButtons, RewardButtons }

public class PaletteChanger : MonoBehaviour
{
    [SerializeField] ColorPaletteData[] palettes;
    [SerializeField] UIController uIController;
    private int activePalette = 0;

    private void OnEnable()
    {
        Inputs.Instance.Controls.UI.C.started += ChangePalette;
        Inputs.Instance.Controls.UI.V.started += UpdatePalette;
    }
    public void UpdatePalette(InputAction.CallbackContext contect)
    {
        Debug.Log(" - Change Color palette - ");
        ChangeColors();
    }
    
    public void ChangePalette(InputAction.CallbackContext contect)
    {
        Debug.Log(" - Change Color palette - ");
        activePalette = (activePalette + 1) % palettes.Length;
        ChangeColors();

    }

    private void ChangeColors()
    {
        SetColorRecursivelyByTag(uIController.transform, "MenuBackgroundBacks", palettes[activePalette].MenuBackgroundBacks);
        SetColorRecursivelyByTag(uIController.transform, "MenuBackgroundFronts", palettes[activePalette].MenuBackgroundFronts);
        SetColorRecursivelyByTag(uIController.transform, "SubMenuBackgrounds", palettes[activePalette].SubMenuBackgrounds);
        SetColorRecursivelyByTag(uIController.transform, "SubMenuBackgroundDarks", palettes[activePalette].SubMenuBackgroundDarks);
        SetColorRecursivelyByTag(uIController.transform, "MissionBackgrounds", palettes[activePalette].MissionBackgrounds);
        SetColorRecursivelyByTag(uIController.transform, "LockedPanels", palettes[activePalette].LockedPanels);
        SetColorRecursivelyByTag(uIController.transform, "LockedPanelRewards", palettes[activePalette].LockedPanelRewards);
        SetColorRecursivelyByTag(uIController.transform, "LockedButtons", palettes[activePalette].LockedButtons);
        SetColorRecursivelyByTag(uIController.transform, "RewardButtons", palettes[activePalette].RewardButtons);
        SetColorRecursivelyByTag(uIController.transform, "MissionPanelRewards", palettes[activePalette].MissionPanelRewards);
        SetColorRecursivelyByTag(uIController.transform, "InventoryBar", palettes[activePalette].InventoryBar);
    }

    private void SetColorRecursivelyByTag(Transform parent, string tag, Color c)
    {
        // Check if the parent's tag matches the desired tag
        if (parent.CompareTag(tag))
        {
            // Get the Image component and set its color
            Image imageComponent = parent.gameObject.GetComponent<Image>();
            if (imageComponent != null)
            {
                //Debug.Log("Setting color to: "+c);
                imageComponent.color = c;
            }
        }

        // Recursively process children
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            SetColorRecursivelyByTag(child, tag, c);
        }
}
}
