using UnityEngine;
using Oculus.Interaction;

public class LeverPositionLock : MonoBehaviour
{
    public float maxAngle = 65f;
    public Grabbable grabbable;

    private Vector3 lockedPosition;
    private float lockedEulerY;
    private float lockedEulerZ;

    void Start()
    {
        lockedPosition = transform.position;
        lockedEulerY = transform.localEulerAngles.y;
        lockedEulerZ = transform.localEulerAngles.z;
    }

    void LateUpdate()
    {
        transform.position = lockedPosition;

        if (grabbable == null || grabbable.SelectingPointsCount == 0)
            return;

        float x = transform.localEulerAngles.x;
        if (x > 180f) x -= 360f;
        x = Mathf.Clamp(x, 0f, maxAngle);

        transform.localEulerAngles = new Vector3(x, lockedEulerY, lockedEulerZ);
    }
}
