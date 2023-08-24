using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class AchievementGraphics : MonoBehaviour
{
    [SerializeField] GameObject completed;
    public TextMeshProUGUI descriptionText;
    public Image completedImage;
    [SerializeField] GameObject incomplete;

    internal void SetCompleted(bool v)
    {
        completed.SetActive(v);
        incomplete.SetActive(!v);
    }
}