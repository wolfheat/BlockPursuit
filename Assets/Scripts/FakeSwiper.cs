using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FakeSwiper : MonoBehaviour
{
    [SerializeField] private GameObject swipeObject;
    private int stage = 0;
    private Vector3 startPosition;
    private Vector2[] directions = { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
    private float timer;
    private const float MoveSpeed = 350f;
    private const float MoveTime = 0.6f;
    private const float IdleTime = 0.15f;

    void Start()
    {
        Inputs.Instance.Controls.Main.E.performed += AnimateNext;
        startPosition = swipeObject.transform.position;
    }

    private void AnimateNext(InputAction.CallbackContext context )
    {
        timer = 0;
        StartCoroutine(AnimateDirection(stage));                
        stage = (stage+1)%directions.Length;    
    }

    private IEnumerator AnimateDirection(int dir)
    {
        swipeObject.transform.position = startPosition;
        swipeObject.SetActive(true);
        while (timer < (MoveTime+2* IdleTime))
        {
            timer += Time.deltaTime;
            if(timer > IdleTime && timer < (MoveTime+ IdleTime))
                swipeObject.transform.position += (Vector3)directions[dir]*MoveSpeed*Time.deltaTime;
            yield return null;
        }
        swipeObject.SetActive(false);
    }
}
