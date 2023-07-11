using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastFixForButton : MonoBehaviour
{
    Image button; 
    // Update is called once per frame
    private void Start()
    {
        button = GetComponent<Image>();

        button.alphaHitTestMinimumThreshold = 0.02f;
    }
}
