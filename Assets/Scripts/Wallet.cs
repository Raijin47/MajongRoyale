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

    public int Total 
    { 
        get => _total;
        set 
        {
            _total = value;
            _totalText.text = $"{TextUtility.GetColorText("Total:", 2)} {_total}";
            PlayerPrefs.SetInt("SaveTotal", value);
        } 
    }
    public int Score => _score;

    public void Init()
    {
        GameController.AddScore += AddScore;
        GameController.OnFinishGame += FinishGame;

        Total = PlayerPrefs.GetInt("SaveTotal", 0);
        _record = PlayerPrefs.GetInt("SaveRecord", 0);
        SetRecord();
    }

    private void AddScore(int value)
    {
        _score += value;
        _scoreText.text = $"{TextUtility.GetColorText("Score", 0)} {_score}";
    }
        
    private void SetRecord()
    {
        if(_score > _record)
        {
            _record = _score;
            PlayerPrefs.SetInt("SaveRecord", _record);
        }

        _recordText.text = $"{TextUtility.GetColorText("Record: ", 3)}\n{_record}";
    }

    private void FinishGame()
    {
        Total += _score;
        SetRecord();
        _score = 0;
    }
}