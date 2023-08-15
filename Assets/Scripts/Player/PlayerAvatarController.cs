using System;
using UnityEngine;


public enum AvatarType{Dino,Bear,Cactus,Pi,Treasure}

public class PlayerAvatarController : MonoBehaviour
{
    [SerializeField] GameObject[] avatars;
    [SerializeField] AnimationRigController rigController;
    private AvatarType activeAvatar = AvatarType.Dino;
    private PlayerController playerController;

    public bool Grabbing => rigController.gameObject.activeSelf;

    void Start()
    {
        ActivateAvatarType(activeAvatar);
        rigController = avatars[(int)activeAvatar].GetComponent<AnimationRigController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void PretendToGrab(bool grab)
    {
        rigController.SetActive(grab);
    }
    public void ChangeActiveAvatar(AvatarType newType)
    {
        Debug.Log("Activating Avatar: "+newType);
        activeAvatar = newType;
        ActivateAvatarType(activeAvatar);
        GameObject avatar = avatars[(int)activeAvatar];
        playerController.SetNewCharacter(avatar);
        // Change RigController here
        rigController = avatar.GetComponent<AnimationRigController>();

    }

    private void ActivateAvatarType(AvatarType activeAvatar)
    {
        foreach (var avatar in avatars)
        {
            avatar.gameObject.SetActive(false);
        }
        avatars[(int)activeAvatar].gameObject.SetActive(true);
    }

    internal void Hold(bool v)
    {
        rigController.Hold(v);
    }
}
