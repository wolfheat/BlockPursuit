using UnityEngine;

internal class AchievementGraphics : MonoBehaviour
{
    [SerializeField] GameObject completed;
    [SerializeField] GameObject incomplete;

    internal void SetCompleted(bool v)
    {
        completed.SetActive(v);
        incomplete.SetActive(!v);
    }
}