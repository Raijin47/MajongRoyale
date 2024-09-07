using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DressButton : MonoBehaviour
{
    [SerializeField] private GameObject _cloth, _help;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText, _priceText;

    [SerializeField] private int _id;
    [SerializeField] private int _price;

    [SerializeField] private string _name;

    private bool _isPurchased;

    private void Start()
    {
        _button.onClick.AddListener(BuyDress);
        _isPurchased = PlayerPrefs.GetInt($"isPurchased{_id}", 0) == 1;
        Active();
    }

    private void BuyDress()
    {
        if(Data.Instance.Wallet.Total > _price)
        {
            Data.Instance.Wallet.Total -= _price;
            _isPurchased = true;
            PlayerPrefs.SetInt($"isPurchased{_id}", 1);
            Active();
        }
    }

    private void Active()
    {
        _button.interactable = !_isPurchased;
        _cloth.SetActive(_isPurchased);
        _help.SetActive(!_isPurchased);
        _priceText.text = _isPurchased ? "!" : ConvertPrice(_price);
        _nameText.text = _isPurchased ? TextUtility.GetColorText("Purchased", 1) : TextUtility.GetColorText(_name, 0);
    }

    private string ConvertPrice(int value)
    {
        return value == 1000 ? "1k" : $"{value}";
    }
}