using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    [SerializeField] private Button[] _buttonsSkill;
    [SerializeField] private TextMeshProUGUI[] _countText;
    [SerializeField] private Field _field;
    [SerializeField] private Timer _timer;

    private readonly int[] Count = new int[3];

    private void Start()
    {
        _buttonsSkill[0].onClick.AddListener(()=> UseSkill(0));
        _buttonsSkill[1].onClick.AddListener(() => UseSkill(1));
        _buttonsSkill[2].onClick.AddListener(() => UseSkill(2));

        GameController.OnStartGame += RestartSkillCount;
    }

    private void RestartSkillCount()
    {
        Count[0] = 1;
        Count[1] = 2;
        Count[2] = 3;

        for (int i = 0; i < _buttonsSkill.Length; i++) UpdateUI(i);
    }

    private void UseSkill(int id)
    {
        if (Count[id] <= 0) return;

        switch (id)
        {
            case 0: _field.FindAndPrintCollapsibleCells(); break;
            case 1: _timer.AddTime(30); break;
            case 2: _field.Shuffle(); break;
        }

        Count[id]--;
        UpdateUI(id);
    }

    private void UpdateUI(int id)
    {
        _countText[id].text = Count[id].ToString();
    }
}