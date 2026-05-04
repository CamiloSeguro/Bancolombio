using UnityEngine;

public class LeverSlotTrigger : MonoBehaviour
{
    public Transform lever; // la palanca
    public float triggerAngle = 30f; // ángulo para activar
    public SlotMachine slotMachine;

    private bool hasTriggered = false;

    void Update()
    {
        float angle = lever.localEulerAngles.x;

        // Normalizar ángulo (porque Unity usa 0–360)
        if (angle > 180) angle -= 360;

        if (angle < -triggerAngle && !hasTriggered)
        {
            hasTriggered = true;
            slotMachine.StartSpin();
        }

        // Reset cuando la palanca vuelve arriba
        if (angle > -5f)
        {
            hasTriggered = false;
        }
    }
}