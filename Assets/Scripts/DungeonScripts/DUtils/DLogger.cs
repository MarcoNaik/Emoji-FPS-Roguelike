﻿using UnityEngine;

namespace DungeonGenerator.Scripts.DUtils
{
    public class DLogger  {
        public static void Log(string _msg, Object _class = null) {
            if (_class != null) {
                Debug.Log(_class.ToString() + " :: " + _msg);
            } else {
                Debug.Log(_msg);
            }
        }
    }
}
