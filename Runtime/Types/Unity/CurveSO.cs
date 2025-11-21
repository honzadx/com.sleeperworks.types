using UnityEngine;

namespace ScriptableFlow.Runtime
{
    public class CurveSO : ValueSOT<AnimationCurve>
    {
        private void OnEnable()
        {
            value = new AnimationCurve();

            if (_defaultValue != null)
                value.CopyFrom(_defaultValue);
        }
    }
}