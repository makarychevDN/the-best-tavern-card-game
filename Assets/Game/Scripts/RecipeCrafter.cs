using UnityEngine;

public class RecipeCrafter : MonoBehaviour
{
    private BattleField battleField;

    public void Init(BattleField battleField)
    {
        this.battleField = battleField;
        foreach(var cardSlot in battleField.CardSlots)
        {
            cardSlot.OnFilled.AddListener(TryToExecuteRecepies);
        }
    }

    private void TryToExecuteRecepies()
    {
        print("I've tried to execute recepies!");
    }
}
