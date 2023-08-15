using System.Text;
using TMPro;
using UnityEngine;

public class LoadingTileRotator : MonoBehaviour
{
    private const float RotateSpeed = 60f;
    [SerializeField] GameObject objectToRotate;
    [SerializeField] TextMeshProUGUI text;
    private int dots = 0;
    private const float TimePerDot = 0.8f;
    private float timer = 0;
    void Update()
    {
        objectToRotate.transform.Rotate(Vector3.back * Time.deltaTime * RotateSpeed);
        timer += Time.deltaTime;
        if(timer >= TimePerDot)
        {
            timer -= TimePerDot;
            dots = (dots + 1) % 4;
            UpdateLoadingText();
        }
    }

    private void UpdateLoadingText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Loading");
        sb.Append('.', dots);
        text.text = sb.ToString();
    }
}
