using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color hoveringColor;
    private Color col;

    private void Start()
    {
        col = text.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoveringColor; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = col;
    }
}