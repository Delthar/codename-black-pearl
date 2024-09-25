using System;
using UnityEngine;

public class OnCannonFireEventArgs : EventArgs
{
    public Transform firePosition;

    public OnCannonFireEventArgs(Transform firePosition)
    {
        this.firePosition = firePosition;
    }
}