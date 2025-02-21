using UnityEngine;

public class LastHoveredTargetContainer : MonoBehaviour
{
    [SerializeField] private MonoBehaviour lastHoveredTarget;

    public void SetLastHoveredTarget(MonoBehaviour target)
    {
        lastHoveredTarget = target;
    }
}
