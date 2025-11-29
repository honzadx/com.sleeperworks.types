using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class CurveSO : ValueSOT<AnimationCurve> 
    {
        public override void ResetDefault()
        {
            if (_defaultValue == null)
            {
                value = new AnimationCurve();
                return;
            }
            
            value = new AnimationCurve(_defaultValue.keys)
            {
                preWrapMode  = _defaultValue.preWrapMode,
                postWrapMode = _defaultValue.postWrapMode
            };
        }
        
        public static implicit operator AnimationCurve (CurveSO curveSO) => curveSO.value;
    }
}