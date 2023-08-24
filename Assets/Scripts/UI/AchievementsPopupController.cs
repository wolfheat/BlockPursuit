using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class AchievementsPopupController : BasePanel
{
    [SerializeField] private AchievementsController achievementsController;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI desctriptionText;
    private Queue queue = new Queue();
    public bool HasUnviewedAchievements() => queue.Count > 0;
    public void AddAchivementToQueue(AchievementDefinition newItem) => queue.Enqueue(newItem);

    private void OnEnable()
    {
        PlayerGameData.AchievementUnlocked += index => AddAchivementToQueue(achievementsController.GetAchievementDefinition(index));
    }

    public void ShowPopup()
    {
        Debug.Log("Show Achievement Popup");
        ShowPanel();
        DisplayItem((AchievementDefinition)queue.Dequeue());
    }

    private void DisplayItem(AchievementDefinition newItem)
    {
        image.sprite = newItem.completedSprite;
        desctriptionText.text = newItem.description;
    }

    public void OKClicked()
    {
        // Go through queue or close
        if(queue.Count > 0)
        {
            DisplayItem((AchievementDefinition)queue.Dequeue());
            return;
        }
        HidePanel();
    }

}
