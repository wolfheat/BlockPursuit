using UnityEngine;


public enum AvatarType{Dino,Bear,Cactus,Pi,Treasure}

public class PlayerAvatarController : MonoBehaviour
{
    [SerializeField] GameObject[] avatars;
    private AvatarType activeAvatar = AvatarType.Dino;

    void Start()
    {
        ActivateAvatarType(activeAvatar);
    }

    public void ChangeActiveAvatar(AvatarType newType)
    {
        Debug.Log("Activating Avatar: "+newType);
        activeAvatar = newType;
        ActivateAvatarType(activeAvatar);
        FindObjectOfType<PlayerController>().SetNewCharacter(avatars[(int)activeAvatar]);
    }

    private void ActivateAvatarType(AvatarType activeAvatar)
    {
        foreach (var avatar in avatars)
        {
            avatar.gameObject.SetActive(false);
        }
        avatars[(int)activeAvatar].gameObject.SetActive(true);
    }
}
