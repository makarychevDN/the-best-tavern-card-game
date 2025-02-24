using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardItem cardItem;
    [SerializeField] private CardSlot cardSlot;
    [SerializeField] private float movementTime;
    [Header("UI Refs")]
    [SerializeField] private GameObject highlight;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private TMP_Text chargesText;
    [Header("Item Properties")]
    [SerializeField] private int power;
    [SerializeField] private int charges;
    private Level level;

    public CardItem CardItem => cardItem;

    public void Init(Level level)
    {
        this.level = level;
        InitCardItem(cardItem);
    }

    public void Init(Level level, CardItem cardItem)
    {
        this.level = level;
        InitCardItem(cardItem);
    }

    private void InitCardItem(CardItem cardItem)
    {
        this.cardItem = cardItem;
        itemImage.sprite = cardItem.Image;
        itemNameText.text = cardItem.ItemName;
        powerText.text = cardItem.Power.ToString();
        chargesText.text = cardItem.Charges.ToString();

        power = cardItem.Power;
        charges = cardItem.Charges;
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
        await transform.DOMove(position, movementTime).SetEase(Ease.InQuad).AsyncWaitForCompletion();
    }

    public async Task Attack(Card targetCard)
    {
        await Move(targetCard.transform.position);

        charges--;
        targetCard.power -= power;

        await Move(cardSlot.transform.position);
    }
}
