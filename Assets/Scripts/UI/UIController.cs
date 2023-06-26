using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] StartMenu startMenu;


    private void Start()
    {
        Debug.Log("toggle on Start Menu");
        startMenu.ShowMenu();
    }
    
}
