using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComplatedPanel : MonoBehaviour
{
    [SerializeField] private GameObject _resultPanel, _gamePanel;
    [SerializeField] private Timer _timer;
    [SerializeField] private Image _resultImage;
    [SerializeField] private TextMeshProUGUI _resultTimerText, _resultScoreText;
    [SerializeField] private Sprite[] _resultSprites;

    private void Start()
    {
        GameController.OnWinGame += WinGame;
        GameController.OnTimeIsUp += TimeIsUp;
    }

    private void TimeIsUp()
    {
        UpdateUI();
        _resultImage.sprite = _resultSprites[0];
        AudioController.Instance.Play(3);
    }

    private void WinGame()
    {
        UpdateUI();
        _resultImage.sprite = _resultSprites[1];
        AudioController.Instance.Play(4);
    }

    private void UpdateUI()
    {
        _resultPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _resultTimerText.text = $"{TextUtility.GetColorText("Time left:", 2)} {_timer.GetTime()}";
        _resultScoreText.text = $"{TextUtility.GetColorText("Score:", 2)} {Data.Instance.Wallet.Score}";
    }
}