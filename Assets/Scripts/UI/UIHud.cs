using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemiesRemainingText;
    [SerializeField] TextMeshProUGUI pointsRemainingText;
    [SerializeField] TextMeshProUGUI starsText;

    [SerializeField] Slider healthBar;
    
    public static UIHud Instance;

    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        UpdateHealthBar(1);
    }

    public void ShowStarAmount(int amt)
    {
        starsText.text = amt.ToString();
    }
    public void SetEnemiesRemaining(int points,int enemies)
    {
        pointsRemainingText.text = "Stored Points: " + points;
        enemiesRemainingText.text = "Active Enemies: " + enemies;
    }

	public void UpdateHealthBar(float v)
    {
        healthBar.value = v;
    }
}
