using UnityEngine;
using UnityEngine.UI;

enum TierName{Bronze,Silver,Gold}

public class TierButton : MonoBehaviour
{
    [SerializeField] TierName tier;
    [SerializeField] Image image;
    [SerializeField] Color HighLightColor;
    [SerializeField] Color DiffuseColor;

    public int ID = 0;

    private void Awake()
    {
        HighLight(ID==0?true:false);            
    }

    public void HighLight(bool set = true)
    {
        image.color = set?HighLightColor:DiffuseColor;
    }

}
