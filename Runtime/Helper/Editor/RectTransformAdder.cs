using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D.Animation;

namespace DivineSkies.Tools.Helper
{
    public static class RectTransformAdder
    {
        private static readonly Dictionary<SpriteSkin, GameObject[]> _boneRefs = new();
        [MenuItem("GameObject/FEA/Add RectTransforms recursive")]
        public static void AddRectTransforms(MenuCommand menuCommand)
        {
            var go = menuCommand.context as GameObject;
            AddRectTransformRecursive(go);

            foreach (var kvp in _boneRefs)
            {
                typeof(SpriteSkin).GetField("m_RootBone", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(kvp.Key, kvp.Value[0].transform);
                for (int i = 0; i < kvp.Value.Length; i++)
                {
                    kvp.Key.boneTransforms[i] = kvp.Value[i].transform;
                }
            }
            _boneRefs.Clear();
        }

        private static void AddRectTransformRecursive(GameObject parent)
        {
            if (parent.transform is not RectTransform)
            {
                if (parent.TryGetComponent<SpriteSkin>(out var skin))
                {
                    _boneRefs.Add(skin, skin.boneTransforms.Select(b => b.gameObject).ToArray());
                }
                parent.AddComponent<RectTransform>();
            }

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                AddRectTransformRecursive(parent.transform.GetChild(i).gameObject);
            }
        }
    }
}