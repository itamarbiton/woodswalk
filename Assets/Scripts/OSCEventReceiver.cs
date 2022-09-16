using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCEventReceiver : MonoBehaviour
{
    public float fader1;
    public float fader2;

    public void updateFader1(float value)
    {
        fader1 = value;
    }

    public void updateFader2(float value)
    {
        fader2 = value;
    }
}
