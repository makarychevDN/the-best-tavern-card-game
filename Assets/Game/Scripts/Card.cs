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

    public void EnqueueMove(CardSlot targetCardSlot)
    {
        level.Executor.EnqueueAnimation(() => PerformMove(targetCardSlot));
    }

    public async Task PerformMove(CardSlot targetCardSlot)
    {
        if (cardSlot != null)
        {
            cardSlot.SetCard(null);
            cardSlot = null;
        }

        await MovementAnimation(targetCardSlot.transform.position);

        cardSlot = targetCardSlot;
        targetCardSlot.SetCard(this);
    }

    public async Task EnqueueInteract(Card targetCard)
    {
        await level.Executor.EnqueueAnimation(() => Interact(targetCard));
    }

    public async Task Interact(Card targetCard)
    {
        await InteractAnimation(targetCard.transform.position);

        charges--;
        targetCard.power -= power;

        if (charges <= 0)
        {
            Destroy(gameObject);
            return;
        }

        await MovementAnimation(cardSlot.transform.position);
    }

    public async Task InteractAnimation(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.DORotate(new Vector3(0, 0, angle - 90), movementTime * 0.5f).SetEase(Ease.InQuad);
        transform.DOScale(Vector3.one * 0.25f, movementTime).SetEase(Ease.InQuad);
        await transform.DOMove(position, movementTime).SetEase(Ease.InQuad).AsyncWaitForCompletion();
    }

    public async Task MovementAnimation(Vector3 position)
    {
        transform.DORotate(new Vector3(0, 0, 0), movementTime);
        transform.DOScale(Vector3.one, movementTime).SetEase(Ease.InQuad);
        await transform.DOMove(position, movementTime).SetEase(Ease.InQuad).AsyncWaitForCompletion();
    }

    public async Task EnqueueExecuteRecipe(Vector3 position)
    {
        var tcs = new TaskCompletionSource<bool>();

        await level.Executor.EnqueueAnimation(async () =>
        {
            await InteractAnimation(position);
            Destroy(gameObject);
            tcs.SetResult(true);
        });

        await tcs.Task;
    }
}
