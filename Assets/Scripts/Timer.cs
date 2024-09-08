using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _image;

    private Coroutine _timerUpdateCoroutine;

    private const float RequiredTime = 120;
    private float _currentTime;
    private bool _isGameReady;

    private void Start()
    {
        GameController.OnPauseGame += PauseGame;
        GameController.OnStartGame += StartGame;
        GameController.OnFinishGame += FinishGame;
        GameController.OnWinGame += FinishGame;
    }

    private void StartGame()
    {
        _currentTime = RequiredTime;
        _isGameReady = true;

        StartTimerCoroutine();
    }

    private void FinishGame()
    {
        _isGameReady = false;
        StopTimerCoroutine();
    }

    private void StartTimerCoroutine()
    {
        StopTimerCoroutine();
        _timerUpdateCoroutine = StartCoroutine(TimerUpdateCoroutine());
    }

    private void StopTimerCoroutine()
    {
        if (_timerUpdateCoroutine != null)
        {
            StopCoroutine(_timerUpdateCoroutine);
            _timerUpdateCoroutine = null;
        }
    }

    private IEnumerator TimerUpdateCoroutine()
    {
        while(true)
        {
            if (_currentTime > 0f)
            {
                _currentTime -= Time.deltaTime;

                if (_currentTime <= 0f)
                {
                    _currentTime = 0;

                }

                UpdateTimeUI();
            }
            yield return null;
        }
    }

    private void UpdateTimeUI()
    {
        _timerText.text = GetTime();
        _image.fillAmount = _currentTime / RequiredTime;
    }

    public string GetTime()
    {
        int minutes = (int)(_currentTime / 60f);
        int seconds = (int)(_currentTime % 60f);

        return minutes.ToString("0") + ":" + seconds.ToString("00");
    }

    private void PauseGame(bool isPause)
    {
        if (!_isGameReady) return;

        if (isPause) StopTimerCoroutine();
        else StartTimerCoroutine();
    }
}