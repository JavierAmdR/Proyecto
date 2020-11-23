using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : EntityEvents
{
    public static PlayerEvents current;

    private void Awake()
    {
        current = this;
    }

}
