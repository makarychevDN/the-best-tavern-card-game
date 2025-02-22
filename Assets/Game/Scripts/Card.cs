using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private Level level;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHighlight(true);
        level.LastHoveredTargetContainer.SetLastHoveredTarget(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EnableHighlight(false);
        level.LastHoveredTargetContainer.RemoveLastHoveredTarget();
    }

    private void EnableHighlight(bool value)
    {
        print("Highlighted is " + value);
    }

    public void Select()
    {
        print("Selected");
        level.PlayerInput.SetSelectedCard(this);
        transform.localScale = Vector3.one * 2f;
    }

    public void Unselect()
    {
        level.PlayerInput.RemoveSelectedCard();
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
