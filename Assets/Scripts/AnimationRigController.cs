using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigController : MonoBehaviour
{
    [SerializeField] RigBuilder rigBuilder;

    [SerializeField] GameObject[] targets;
    private float[] targetsStartHeight;
    private const float TargetHeightOffset = -0.5f;

    private void Start()
    {
        targetsStartHeight = new float[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targetsStartHeight[i] = targets[i].transform.position.z;
        }
    }

    public void SetActive(bool set)
    {
        rigBuilder.enabled = set;        
    }
    
    public void Hold(bool set)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].transform.position = new Vector3(targets[i].transform.position.x, targets[i].transform.position.y, targetsStartHeight[i] + (set?TargetHeightOffset:0));
        }
    }

}
