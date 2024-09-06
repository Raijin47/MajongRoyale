using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static Action OnStartGame;
    public static Action OnWinGame;
    public static Action<int> AddScore;
    public static Action OnFinishGame;

    [SerializeField] private Field _field;

    private void Start()
    {
        _field.Init();
    }

    public void StartGame() => OnStartGame?.Invoke();
    public void FinishGame() => OnFinishGame?.Invoke();
}