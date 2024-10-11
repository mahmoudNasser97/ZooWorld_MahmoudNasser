using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    public static ScoreUIManager Instance;

    public TextMeshProUGUI frogCounterText;
    public TextMeshProUGUI snakeCounterText;
    public TextMeshProUGUI frogCounter;
    public TextMeshProUGUI snakeCounter;

    private int frogCount = 0;
    private int snakeCount = 0;
    private int frogsCount = 0;
    private int snakesCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementPreyCount()
    {
        frogCount++;
        UpdateUI();
    }

    public void IncrementPredatorCount()
    {
        snakeCount++;
        UpdateUI();
    }
    public void FrogsCounter()
    {
        frogsCount++;
        UpdateUI();
    }
    public void DeadFrogsCounter()
    {
        frogsCount--;
        UpdateUI();
    }
    public void SnakesCounter()
    {
        snakesCount++;
        UpdateUI();
    }
    public void DeadSnakesCounter()
    {
        snakesCount--;
        UpdateUI();
    }
    private void UpdateUI()
    {
        frogCounterText.text = "Prey: " + frogCount;
        snakeCounterText.text = "Predators: " + snakeCount;

        frogCounter.text = "FROGS:  " + frogsCount;
        snakeCounter.text = "SNAKES:  " + snakesCount;
    }
}
