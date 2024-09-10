using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HelpSearch : MonoBehaviour
{
    public static IEnumerator AnimateCell(Cell cell1, Cell cell2)
    {
        Image first = cell1.transform.GetChild(0).GetComponent<Image>();
        Image second = cell2.transform.GetChild(0).GetComponent<Image>();

        Color color = Color.white;
        float value = 1;

        for(int i = 0; i < 10; i++)
        {
            while (value > 0)
            {
                value -= Time.deltaTime;
                color.b = value;
                color.g = value;

                first.color = color;
                second.color = color;
                yield return null;
            }
            while (value < 1)
            {
                value += Time.deltaTime;
                color.b = value;
                color.g = value;

                first.color = color;
                second.color = color;
                yield return null;
            }
        }
    }
}