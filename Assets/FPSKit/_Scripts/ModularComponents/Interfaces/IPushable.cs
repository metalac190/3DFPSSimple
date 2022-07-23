using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    void Push(Vector3 direction, float strength, 
        float duration, bool canMoveDuring = true);
}
