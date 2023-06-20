using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnStart : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);    
    }
}
