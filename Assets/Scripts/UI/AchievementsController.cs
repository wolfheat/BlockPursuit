using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AchievementsController : BasePanel
{

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.performed += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestESC;
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("Achievements ESC");
        CloseAchievements();
    }
    [SerializeField] GameObject mainSelected;
    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelected.gameObject);
    }
    public void CloseAchievements()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideAchievements);
    }


}
