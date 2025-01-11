using FEA.Modules;
using FEA.Modules.UI;
using System;
using UnityEngine;

namespace DivineSkies.Tools.Extension
{
    public static class TransformExtensions
    {
        public static RectTransform GetRectTransform(this UIBehaviour self) => self.transform as RectTransform;

        public static void DestroyChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
}