using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static Action OnStartGame;
    public static Action OnWinGame;
    public static Action AddScore;
    public static Action OnFinishGame;
    public static Action OnTimeIsUp;
    public static Action<bool> OnPauseGame;

    [SerializeField] private Field _field;

    private void Start()
    {
        Data.Instance.Init();
        _field.Init();
    }

    public void StartGame() => OnStartGame?.Invoke();
    public void FinishGame() => OnFinishGame?.Invoke();
    public void PauseGame(bool value) => OnPauseGame?.Invoke(value);
}