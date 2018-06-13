﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviorBase : MonoBehaviour {
    public string Message;
    protected bool isEnabled = true;

    public virtual void Play()
    {
        isEnabled = false;
        LeaderController.GetInstance().Hide();
        PlayerController.GetInstance().PlayOver(transform);
    }

    public virtual bool GetEnabled()
    {
        return isEnabled;
    }
}
