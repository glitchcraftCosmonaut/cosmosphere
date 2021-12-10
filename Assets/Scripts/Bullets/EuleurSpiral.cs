using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace DanmakU.Fireables{
[Serializable]
public class EuleurSpiral : Fireable 
{
    public Range Count = 1;
    public Range Size;

    public EuleurSpiral(Range count, Range size) 
    {
        Count = count;
        Size = size;
    }

    public override void Fire(DanmakuConfig state) 
    {
        float size = Size.GetValue();
        int count = Mathf.RoundToInt(Count.GetValue());
        float angle = 0;
        float dAngle = (2 * Mathf.PI) / count;
        //count must be large value so dAngle is small
        for (int i = 0; i < count; i++)
        {
            float dx = size*Mathf.Cos(Mathf.Pow(angle,2))*dAngle;
            float dy = size*Mathf.Sin(Mathf.Pow(angle,2))*dAngle;
            state.Position.x += dx;
            state.Position.y += dy;
            Subfire(state);
            angle += dAngle;
        }
    }
}
}


