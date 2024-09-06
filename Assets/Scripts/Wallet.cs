using System;
using TMPro;
using UnityEngine;

[Serializable]
public class Wallet
{
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _totalText;

    private int _record;
    private int _score;
    private int _total;

    public void Init()
    {
        GameController.AddScore += AddScore;
        GameController.OnStartGame += StartGame;
    }

    private void StartGame() => SetGameScore(0);
    private void AddScore(int value) => SetGameScore(_score + value);
    private void SetGameScore(int value)
    {
        _score = value;
        _scoreText.text = $"{TextUtility.GetColorText("Score", 0)} {_score}";
    }

    private void SetRecord()
    {
        if(_score > _record)
        {
            _record = _score;

        }
        _recordText.text = $"{TextUtility.GetColorText("Record: ", 3)}\n{_record}";
    }
}