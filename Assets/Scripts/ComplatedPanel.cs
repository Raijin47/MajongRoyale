using TMPro;
using UnityEngine;

public class ComplatedPanel : MonoBehaviour
{
    [SerializeField] private GameObject _resultPanel, _gamePanel;
    [SerializeField] private Timer _timer;
    [SerializeField] private TextMeshProUGUI _resultTimerText, _resultScoreText, _resultText;

    private void Start()
    {
        GameController.OnWinGame += WinGame;
        GameController.OnTimeIsUp += TimeIsUp;
    }

    private void TimeIsUp()
    {
        UpdateUI();
        _resultText.text = "TIME IS UP!";
    }

    private void WinGame()
    {
        UpdateUI();
        _resultText.text = "LEVEL COMPLETE!";
    }

    private void UpdateUI()
    {
        _resultPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _resultTimerText.text = $"{TextUtility.GetColorText("Time left:", 2)} {_timer.GetTime()}";
        _resultScoreText.text = $"{TextUtility.GetColorText("Score:", 2)} {Data.Instance.Wallet.Score}";
    }
}