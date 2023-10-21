using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private string _scoreFormat = "Current score: {0}";
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private UnityEvent _onScoreChanged;
    private float _currentScore;

    private event Action ScoreChanged;

    public static ScoreManager Instance; 

    public float CurrentScore
    {
        get => _currentScore;
        set
        {
            if (_currentScore!=value)
            {
                _currentScore = value;
                OnScoreChanged();
            }
            
        }
    }

    private void OnEnable()
    {
        var otherScoreManager = FindObjectOfType<ScoreManager>();
        if (otherScoreManager != null && otherScoreManager != this)
        {
            Destroy(otherScoreManager.gameObject);
        }
        Instance = this;
        
        DontDestroyOnLoad(this);

        CurrentScore = 0;
    }

    public void EnemyDeregistered(Enemy enemy)
    {
        CurrentScore+= enemy.PointValue;
    }

    private void OnScoreChanged()
    {
        _scoreText.text = string.Format(_scoreFormat, _currentScore);
        _onScoreChanged.Invoke();
        ScoreChanged?.Invoke();
    }
}
