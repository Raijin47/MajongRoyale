using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Button _tutorialButton;

    public bool IsTutorial => PlayerPrefs.GetInt("SaveTutorial", 0) == 0;

    private void Start()
    {
        _tutorialPanel.SetActive(IsTutorial);
        _tutorialButton.onClick.AddListener(()=> {
            Destroy(_tutorialPanel);
            PlayerPrefs.SetInt("SaveTutorial", 1);
            GameController.OnStartGame?.Invoke();
        });    
    }
}