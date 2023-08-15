using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigController : MonoBehaviour
{
    [SerializeField] RigBuilder rigBuilder;


    public void SetActive(bool set)
    {
        rigBuilder.enabled = set;
    }

}
