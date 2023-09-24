using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants 
{
    public static Dictionary<PlayerController.State, string> PlayerAnimatorStateMap = new Dictionary<PlayerController.State, string>()
    {
        {PlayerController.State.Idle, "idle" },
        {PlayerController.State.RunningAndShooting, "run" },
        {PlayerController.State.Happy, "happy" },
        {PlayerController.State.Sad, "sad" },
    };


}
