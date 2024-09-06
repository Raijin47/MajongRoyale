using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;

    [SerializeField] private Wallet _wallet;

    public int Level { get; set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        _wallet.Init();
        Level = 2;
    }
}