using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Level
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private Field _field;
    [SerializeField] private int[] _width;
    [SerializeField] private int[] _height;

    private int _current;
   
    public int Height => _height[_current > _height.Length ? _height.Length : _current];
    public int Width => _width[_current > _width.Length ? _width.Length : _current];

    public int Current 
    {
        get => _current;
        set 
        {
            _current = value;
            _levelText.text = $"{TextUtility.GetColorText("Level", 0)} {value + 1}";
            PlayerPrefs.SetInt("SaveLevel", value);

            _grid.constraintCount = Width;
            _field.InitializeField(Height, Width);
        }  
    }

    public void Init()
    {
        Current = PlayerPrefs.GetInt("SaveLevel", 0);
        GameController.OnWinGame += ()=> { Current++; };
    }
}