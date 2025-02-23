using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject highlight;
    private Level level;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void OnPointerEnter(PointerEventData eventData) => Hover();

    public void OnPointerExit(PointerEventData eventData) => Unhover();

    private void EnableHighlight(bool value) => highlight.SetActive(value);

    public void Hover()
    {
        EnableHighlight(true);
        level.LastHoveredTargetContainer.SetLastHoveredTarget(this);
    }

    public void Unhover()
    {
        EnableHighlight(false);
        level.LastHoveredTargetContainer.RemoveLastHoveredTarget();
    }

    public void Select()
    {
        transform.localScale = Vector3.one * 2f;
    }

    public void Unselect()
    {
        transform.localScale = Vector3.one;
    }

    public async Task Move(CardSlot targetCardSlot)
    {
        transform.DOMove(targetCardSlot.transform.position, 0.15f).SetEase(Ease.InQuad);
        transform.DOScale(targetCardSlot.transform.localScale, 0.15f).SetEase(Ease.InQuad);
        await Task.Delay(150);
        transform.position = targetCardSlot.transform.position;
        targetCardSlot.SetCard(this);
    }
}
