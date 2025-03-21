using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;

    public Field Field { get; set; }
    public Sprite Sprite { set => _image.sprite = value; }
    public RectTransform Rect => _rect;
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (ID == 0) return;

        if(!Field.IsSelected)
        {
            Field.FirstCell = new Vector2Int(X, Y);
        }
        else
        {
            if(Field.FirstCell != new Vector2Int(X, Y))
                Field.SecondCell = new Vector2Int(X, Y);
        }
    }
}