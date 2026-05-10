using UnityEngine;

public class BetButton : MonoBehaviour
{
    public MoneyTracker moneyTracker;
    public bool isAddButton = true;
    public OVRSkeleton leftHandSkeleton;
    public OVRSkeleton rightHandSkeleton;
    public Transform pressPoint;
    public float pressDistance = 0.08f;
    public float pressCooldown = 0.5f;

    private float lastPressTime = -999f;

    Vector3 DetectionCenter => pressPoint != null ? pressPoint.position : transform.position;

    void Update()
    {
        if (moneyTracker == null) return;
        if (Time.time - lastPressTime < pressCooldown) return;

        if (GetIndexDistance(leftHandSkeleton) < pressDistance ||
            GetIndexDistance(rightHandSkeleton) < pressDistance)
        {
            lastPressTime = Time.time;
            if (isAddButton) moneyTracker.AddBet();
            else moneyTracker.SubtractBet();
        }
    }

    float GetIndexDistance(OVRSkeleton skeleton)
    {
        if (skeleton == null || !skeleton.IsInitialized) return float.MaxValue;

        foreach (var bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                return Vector3.Distance(DetectionCenter, bone.Transform.position);
        }

        return float.MaxValue;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(DetectionCenter, pressDistance);
    }
}

