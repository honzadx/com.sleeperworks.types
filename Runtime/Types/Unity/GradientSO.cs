using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class GradientSO : ValueSOT<Gradient>
    {
        public override void ResetDefault()
        {
            value = new Gradient();

            if (_defaultValue == null)
                return;

            value.alphaKeys = _defaultValue.alphaKeys;
            value.colorKeys = _defaultValue.colorKeys;
            value.colorSpace = _defaultValue.colorSpace;
            value.mode = _defaultValue.mode;
        }
    }
}