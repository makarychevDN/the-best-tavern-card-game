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
    [SerializeField] private int actionCost;
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
    public int ActionCost => actionCost;

    public void SetSlot(CardSlot slot) => cardSlot = slot;

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
        actionCost = cardItem.ActionCost;
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
        transform.parent = level.SelectedCardParent;
    }

    public void Unselect()
    {
        transform.localScale = Vector3.one;
        transform.parent = level.CardsParent;
    }

    public async Task EnqueueAction(MonoBehaviour target)
    {
        if (target is CardSlot cardSlot)
        {
            await EnqueueMove(cardSlot);
        }
        else if (target is Card card)
        {
            await EnqueueInteract(card);
        }
    }

    public async Task EnqueueMove(CardSlot targetCardSlot)
    {
        await level.Executor.EnqueueAnimation(() => PerformMove(targetCardSlot));
    }

    public async Task EnqueueInteract(Card targetCard)
    {
        await level.Executor.EnqueueAnimation(() => PerformInteract(targetCard));
    }

    public async Task PerformMove(CardSlot targetCardSlot)
    {
        if (cardSlot != null)
        {
            cardSlot.SetCard(null);
            cardSlot = null;
        }

        await AnimateMovement(targetCardSlot.transform.position);

        cardSlot = targetCardSlot;
        targetCardSlot.SetCard(this);
    }

    public async Task PerformInteract(Card targetCard)
    {
        await AnimateInteraction(targetCard.transform.position);

        charges--;
        targetCard.power -= power;

        if (charges <= 0)
        {
            Destroy(gameObject);
            return;
        }

        await AnimateMovement(cardSlot.transform.position);
    }

    public async Task AnimateInteraction(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        await (AnimateMovement(position, Vector3.one * 0.25f, angle, 0.5f));
    }

    public async Task AnimateMovement(Vector3 position)
    {
        await AnimateMovement(position, Vector3.one);
    }

    public async Task AnimateMovement(Vector3 position, Vector3 scale, float angle = 0, float rotationSpeedModificator = 1)
    {
        transform.DORotate(new Vector3(0, 0, angle), movementTime * rotationSpeedModificator).SetEase(Ease.InQuad);
        transform.DOScale(scale, movementTime).SetEase(Ease.InQuad);
        await transform.DOMove(position, movementTime).SetEase(Ease.InQuad).AsyncWaitForCompletion();
        transform.parent = level.CardsParent;
    }

    public async Task EnqueueExecuteRecipe(Vector3 position)
    {
        var tcs = new TaskCompletionSource<bool>();

        await level.Executor.EnqueueAnimation(async () =>
        {
            await AnimateInteraction(position);
            Destroy(gameObject);
            tcs.SetResult(true);
        });

        await tcs.Task;
    }
}
