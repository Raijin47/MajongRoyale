using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;

    [SerializeField] private Wallet _wallet;
    [SerializeField] private Level _level;

    public Wallet Wallet => _wallet;
    public Level Level => _level;



    private void Awake() => Instance = this;

    public void Init()
    {
        _wallet.Init();
        _level.Init();
    }


}