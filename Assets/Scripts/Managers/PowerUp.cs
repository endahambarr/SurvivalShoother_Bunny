using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    public PowerUpType powerType;
    public float amount;
    public float disappearTime;

    private void Start()
    {
        Destroy(this.gameObject, disappearTime);
    }
}

public enum PowerUpType
{
    heal,
    boost
}

