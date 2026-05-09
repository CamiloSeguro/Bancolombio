using UnityEngine;
using Oculus.Interaction;

public class LeverSlotTrigger : MonoBehaviour
{
    public Grabbable grabbable;

    public Transform rotateTarget; // 👈 ESTE es el importante
    public SlotMachine slotMachine;

    private bool hasTriggered = false;

    void Update()
    {
        bool isGrabbed = grabbable.SelectingPointsCount > 0;

        float progress = GetProgress();

        if (isGrabbed && progress > 0.9f && !hasTriggered)
        {
            hasTriggered = true;
            slotMachine.StartSpin();
        }

        if (!isGrabbed)
        {
            hasTriggered = false;
        }
    }

    float GetProgress()
    {
        float angle = rotateTarget.localEulerAngles.x;

        if (angle > 180)
            angle -= 360;

        return Mathf.Clamp01(angle / 65f);
    }
}