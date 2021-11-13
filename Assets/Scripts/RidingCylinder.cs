using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingCylinder : MonoBehaviour
{
    float currentValue;
    bool filled;

    public void IncrementCylinderVolumeR(float value)
    {
        currentValue += value;
        if (currentValue > 1 )
        {
            float leftValue = currentValue - 1;
            int cylinderCount = PlayerController.current.listCylinders.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cylinderCount - 1) - 0.25f, transform.localPosition.z);
            transform.localScale = new Vector3(0.5f, transform.localScale.y, 0.5f);
            PlayerController.current.CreateCylinder(leftValue);
        }
        else if (currentValue < 0)
        {
            PlayerController.current.DestroyCylinder(this);
        }
        else
        {
            int cylinderCount = PlayerController.current.listCylinders.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cylinderCount - 1) - 0.25f * currentValue, transform.localPosition.z);
            transform.localScale = new Vector3(0.5f * currentValue, transform.localScale.y, 0.5f * currentValue);
        }
    }
}
