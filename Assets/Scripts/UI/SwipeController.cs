using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouch;
    private Vector2 endTouch;
    private const float MoveDistaneLimit = 30f;
    private const float XpositionInputLimit = 120f;

    [SerializeField] private TouchPanelController touchPanelController;

    private void ExecuteTouch()
    {
        if (startTouch == null || endTouch == null) return;
        if (startTouch.x < XpositionInputLimit) return;
        
        //Distance
        float distance = Vector2.Distance(startTouch, endTouch);
        

        if(distance < MoveDistaneLimit)
        {
            Debug.Log("Lift!");
            touchPanelController.ActionTouch();
        }
        else
        {
            float Xdist = endTouch.x - startTouch.x;
            float Ydist = endTouch.y - startTouch.y;
            //Decide direction
            if (Mathf.Abs(Xdist) > Mathf.Abs(Ydist))
            {
                // X motion
                if (Xdist > 0) touchPanelController.DirectionTouch(0);
                else touchPanelController.DirectionTouch(2);
            }
            else
            {
                if (Ydist > 0) touchPanelController.DirectionTouch(1);
                else touchPanelController.DirectionTouch(3);
            }
        }
    }



    private void OnEnable()
    {
        Inputs.Instance.Controls.Touch.TouchPress.started += StartTouch;
        Inputs.Instance.Controls.Touch.TouchPress.canceled += EndTouch;
    }
    private void OnDisable()
    {
        Inputs.Instance.Controls.Touch.TouchPress.started -= StartTouch;
        Inputs.Instance.Controls.Touch.TouchPress.canceled -= EndTouch;
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        endTouch = Inputs.Instance.Controls.Touch.TouchPosition.ReadValue<Vector2>();
        Debug.Log("End Touch at: " + endTouch);
        ExecuteTouch();
    }
    private void StartTouch(InputAction.CallbackContext context)
    {
        startTouch = Inputs.Instance.Controls.Touch.TouchPosition.ReadValue<Vector2>();
        Debug.Log("Start Touch at: " + startTouch);
    }
}
