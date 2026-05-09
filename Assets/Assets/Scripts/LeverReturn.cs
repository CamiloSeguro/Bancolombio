using System.Collections;
using UnityEngine;
using Oculus.Interaction;

public class LeverReturn : MonoBehaviour
{
    public Grabbable grabbable;

    public float returnSpeed = 6f;

    private Quaternion initialRotation;
    private bool wasGrabbed;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        bool isGrabbed = grabbable.SelectingPointsCount > 0;

        // cuando suelta
        if (!isGrabbed && wasGrabbed)
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToStart());
        }

        wasGrabbed = isGrabbed;
    }

    IEnumerator ReturnToStart()
    {
        while (Quaternion.Angle(transform.localRotation, initialRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                initialRotation,
                Time.deltaTime * returnSpeed
            );

            yield return null;
        }

        transform.localRotation = initialRotation;
    }
}