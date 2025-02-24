using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardItem cardItem;
    [SerializeField] private GameObject highlight;
    [SerializeField] private CardSlot cardSlot;
    [SerializeField] private float movementTime;
    private Level level;

    public CardItem CardItem => cardItem;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void Init(Level level, CardItem cardItem)
    {
        this.level = level; 
        this.cardItem = cardItem;
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
        if (cardSlot != null)
        {
            cardSlot.SetCard(null);
            cardSlot = null;
        }

        transform.DOMove(targetCardSlot.transform.position, movementTime).SetEase(Ease.InQuad);
        transform.DOScale(targetCardSlot.transform.localScale, movementTime).SetEase(Ease.InQuad);
        await Task.Delay((int)(movementTime * 1000));

        transform.position = targetCardSlot.transform.position;
        cardSlot = targetCardSlot;
        targetCardSlot.SetCard(this);
    }

    public async Task Move(Vector3 position)
    {
        if (cardSlot != null)
        {
            cardSlot.SetCard(null);
            cardSlot = null;
        }

        transform.DOMove(position, movementTime).SetEase(Ease.InQuad);
        await Task.Delay((int)(movementTime * 1000));

        transform.position = position;
    }
}
