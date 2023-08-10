using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToonButton : MonoBehaviour
{
    public ToonDefinition ToonDefinition { get; private set; }

    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Image image;
    
    internal void SetToonDefinition(ToonDefinition def)
    {
        ToonDefinition = def;
        UpdateVisuals();
    }

    public void OnClick()
    {
        Debug.Log("Clicking Button: "+ToonDefinition.toonName);
        FindObjectOfType<CustomizationController>().RequestChangeAvatar(ToonDefinition);
    }
    private void UpdateVisuals()
    {
        image.sprite = ToonDefinition.sprite;
        textField.text = ToonDefinition.toonName;
    }

}
