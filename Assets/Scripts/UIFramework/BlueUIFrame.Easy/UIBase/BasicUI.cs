﻿using System;
using System.Reflection;
using BlueUIFrame.Easy.Demo;
using UnityEngine;

namespace BlueUIFrame.Easy
{
    public class BasicUI : UIBase
    {
        public override UILayer GetLayer()
        {
            return UILayer.BasicUI;
        }
    }
}
