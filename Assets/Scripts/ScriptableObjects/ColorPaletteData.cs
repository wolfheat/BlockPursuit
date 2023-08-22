using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "New ColorPalette")]
public class ColorPaletteData : ScriptableObject
{  
    public Color MenuBackgroundBacks;
    public Color MenuBackgroundFronts;
    public Color SubMenuBackgrounds;
    public Color SubMenuBackgroundDarks;
    public Color MissionBackgrounds;
    public Color MissionPanelRewards;
    public Color RewardButtons;
    public Color LockedPanels;
    public Color LockedPanelRewards;
    public Color LockedButtons;
    public Color InventoryBar;
}
