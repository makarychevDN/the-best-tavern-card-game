using UnityEngine;

public class LastHoveredTargetContainer : MonoBehaviour
{
    [SerializeField] private MonoBehaviour lastHoveredTarget;

    public MonoBehaviour LastHoveredTarget => lastHoveredTarget;

    public void SetLastHoveredTarget(MonoBehaviour target)
    {
        lastHoveredTarget = target;
    }

    public void RemoveLastHoveredTarget() => SetLastHoveredTarget(null);
}
