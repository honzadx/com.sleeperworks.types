using UnityEngine;

namespace ScriptableFlow.Runtime
{
    public class GradientSO : ValueSOT<Gradient>
    {
        private void OnEnable()
        {
            value = new Gradient();

            if (value == null)
                return;

            value.mode = value.mode;
            value.alphaKeys = value.alphaKeys;
            value.colorKeys = value.colorKeys;
            value.colorSpace = value.colorSpace;
        }
    }
}