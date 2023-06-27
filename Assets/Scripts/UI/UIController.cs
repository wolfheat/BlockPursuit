using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] StartMenu startMenu;
    [SerializeField] LevelComplete levelComplete;

    private void Start()
    {
        Debug.Log("toggle on Start Menu");
        startMenu.ShowPanel();
    }
    
    public void ShowLevelComplete()
    {
        Debug.Log("Show Level Complete Panel");
        levelComplete.ShowPanel();
    }

    internal void ShowMainLevel()
    {
        startMenu.ShowPanel();
    }
}
