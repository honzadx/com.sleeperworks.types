using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ScriptableFlow.Runtime.Events;

namespace ScriptableFlow.Editor
{
    public class ResultEventBusSOTEditor<T, TResult> : UnityEditor.Editor where T : ResultEventBusSOT<TResult>
    {
        private const int MAX_EVENT_HISTORY_COUNT = 25;
        
        private T _target;
        private SerializedProperty _eventLogOptionsProperty;
        private TResult _lastEventValue;
        private TResult _triggerEventValue;
        private float _lastEventRealTimeSinceStartup;
        private bool _listenersListFoldout;
        private Vector2 _listenersListScrollPos;
        private bool _eventListFoldout;
        private Vector2 _eventListScrollPos;
        private List<string> _eventHistory;
        private int _eventCount;

        private Func<TResult, TResult> _drawLastEventValueFieldFunction;
        private Func<TResult, TResult> _drawTriggerEventValueFieldFunction;
        
        protected virtual EditorTypeHint typeHint => EditorTypeHint.None;
        
        protected virtual Func<TResult, TResult> GetDrawFieldOfTResultFunction(string label) => 
            v => (TResult)EditorGUIFunctions.DrawField(label, v, typeof(TResult), typeHint);

        private void OnEnable()
        {
            _eventHistory = new();
            _target = (T)target;
            _target.AddListener(OnEventRaised);
            _eventLogOptionsProperty = serializedObject.FindProperty("_eventLogOptions");
            _drawLastEventValueFieldFunction = GetDrawFieldOfTResultFunction("Last Event Value");
            _drawTriggerEventValueFieldFunction = GetDrawFieldOfTResultFunction("Raise Event Value");
        }

        private void OnDisable()
        {
            _target.RemoveListener(OnEventRaised);
        }

        public override bool RequiresConstantRepaint() => true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUIFunctions.DrawDecayMeter("Event Decay Meter", _lastEventRealTimeSinceStartup);
            using (new EditorGUI.DisabledScope(true))
                _drawLastEventValueFieldFunction.Invoke(_lastEventValue);
            EditorGUILayout.PropertyField(_eventLogOptionsProperty);
            
            var callbackArray = _target.@event.GetInvocationList()
                .Where(callback => !callback.Target.GetType().IsSubclassOf(typeof(ResultEventBusSOTEditor<T, TResult>)))
                .ToArray();
            _listenersListFoldout = EditorGUILayout.Foldout(_listenersListFoldout, $"Listeners ({callbackArray.Length})");
            if(_listenersListFoldout && callbackArray.Length > 0)
            {
                using var indentScope = new EditorGUI.IndentLevelScope();
                _listenersListScrollPos = EditorGUILayout.BeginScrollView(
                    _listenersListScrollPos,
                    GUILayout.Height(
                        Mathf.Min(
                            callbackArray.Length * (EditorGUIUtility.singleLineHeight + 2), 
                            EditorGUIStatics.MAX_LIST_VIEW_SIZE)));
                foreach (var callback in callbackArray)
                {
                    EditorGUILayout.SelectableLabel($"{callback.Method.Name}: {callback.Target}", GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
                EditorGUILayout.EndScrollView();
            }
            
            GUILayout.BeginHorizontal();
            _eventListFoldout = EditorGUILayout.Foldout(_eventListFoldout, $"Events Since Viewed ({_eventCount})");
            GUILayout.EndHorizontal();
            if(_eventListFoldout && _eventHistory.Count > 0)
            {
                using var indentScope = new EditorGUI.IndentLevelScope();
                _eventListScrollPos = EditorGUILayout.BeginScrollView(
                    _eventListScrollPos,
                    GUILayout.Height(
                        Mathf.Min(
                            _eventHistory.Count * (EditorGUIUtility.singleLineHeight + 2), 
                            EditorGUIStatics.MAX_LIST_VIEW_SIZE)));
                for(int i = _eventHistory.Count - 1; i >= 0; i--)
                {
                    EditorGUILayout.SelectableLabel(_eventHistory[i], GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
                EditorGUILayout.EndScrollView();
            }
            
            GUILayout.Space(10);
            
            var debugRect = EditorGUILayout.BeginVertical();
            {
                EditorGUI.DrawRect(debugRect, EditorGUIStatics.s_debugBackgroundColor);
                EditorGUIFunctions.DrawHeaderText("DEBUG");
                _triggerEventValue = _drawTriggerEventValueFieldFunction.Invoke(_triggerEventValue);
                if (GUILayout.Button("Raise"))
                    _target.Raise(_triggerEventValue);
                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEventRaised(TResult value)
        {
            if (_eventHistory.Count >= MAX_EVENT_HISTORY_COUNT)
                _eventHistory.RemoveAt(0);
            _eventHistory.Add($"{_eventCount++}.\t[Time: {Time.realtimeSinceStartup:0.00}] {value?.ToString() ?? "<null>"} ");
            _lastEventValue = value;
            _lastEventRealTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
        }
    }

    [CustomEditor(typeof(SignalEventBusSO), true)]
    public class SignalEventBusSOEditor : UnityEditor.Editor
    {
        private const int MAX_EVENT_HISTORY_COUNT = 25;
        
        private SignalEventBusSO _target;
        private SerializedProperty _eventLogOptionsProperty;
        private float _lastEventRealTimeSinceStartup;
        private bool _listenersListFoldout;
        private Vector2 _listenersListScrollPos;
        private bool _eventListFoldout;
        private Vector2 _eventListScrollPos;
        private List<string> _eventHistory;
        private int _eventCount;

        private void OnEnable()
        {
            _eventHistory = new();
            _target = (SignalEventBusSO)target;
            _target.AddListener(OnEventRaised);
            _eventLogOptionsProperty = serializedObject.FindProperty("_eventLogOptions");
        }

        private void OnDisable()
        {
            _target.RemoveListener(OnEventRaised);
        }

        public override bool RequiresConstantRepaint() => true;

        public override void OnInspectorGUI()
        {
            EditorGUIFunctions.DrawDecayMeter("Event Decay Meter", _lastEventRealTimeSinceStartup);
            EditorGUILayout.PropertyField(_eventLogOptionsProperty);
            
            var callbackArray = _target.@event.GetInvocationList()
                .Where(callback => callback.Target.GetType() != typeof(SignalEventBusSOEditor))
                .ToArray();
            _listenersListFoldout = EditorGUILayout.Foldout(_listenersListFoldout, $"Listeners ({callbackArray.Length})");
            if(_listenersListFoldout && callbackArray.Length > 0)
            {
                using var indentScope = new EditorGUI.IndentLevelScope();
                _listenersListScrollPos = EditorGUILayout.BeginScrollView(
                    _listenersListScrollPos,
                    GUILayout.Height(
                        Mathf.Min(
                            callbackArray.Length * (EditorGUIUtility.singleLineHeight + 2), 
                            EditorGUIStatics.MAX_LIST_VIEW_SIZE)));
                foreach (var callback in callbackArray)
                {
                    EditorGUILayout.SelectableLabel($"{callback.Method.Name}: {callback.Target}", GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
                EditorGUILayout.EndScrollView();
            }
            
            GUILayout.BeginHorizontal();
            _eventListFoldout = EditorGUILayout.Foldout(_eventListFoldout, $"Events Since Viewed ({_eventCount})");
            GUILayout.EndHorizontal();
            if(_eventListFoldout && _eventHistory.Count > 0)
            {
                using var indentScope = new EditorGUI.IndentLevelScope();
                _eventListScrollPos = EditorGUILayout.BeginScrollView(
                    _eventListScrollPos,
                    GUILayout.Height(
                        Mathf.Min(
                            _eventHistory.Count * (EditorGUIUtility.singleLineHeight + 2), 
                            EditorGUIStatics.MAX_LIST_VIEW_SIZE)));
                for(int i =  _eventHistory.Count - 1; i >= 0; i--)
                {
                    EditorGUILayout.SelectableLabel(_eventHistory[i], GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
                EditorGUILayout.EndScrollView();
            }

            GUILayout.Space(10);
            
            var debugRect = EditorGUILayout.BeginVertical();
            {
                EditorGUI.DrawRect(debugRect, EditorGUIStatics.s_debugBackgroundColor);
                EditorGUIFunctions.DrawHeaderText("DEBUG");
                if (GUILayout.Button("Raise"))
                    _target.Raise();
                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
        }
        
        private void OnEventRaised()
        {
            if (_eventHistory.Count >= MAX_EVENT_HISTORY_COUNT)
                _eventHistory.RemoveAt(0);
            _eventHistory.Add($"{_eventCount++}.\t[Time: {Time.realtimeSinceStartup:0.00}] ");
            _lastEventRealTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
        }
    }

    [CustomEditor(typeof(BoolEventBusSO), true)]
    public class BoolEventBusSOEditor : ResultEventBusSOTEditor<BoolEventBusSO, bool>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Bool;
    }

    [CustomEditor(typeof(FloatEventBusSO), true)]
    public class FloatEventBusSOEditor : ResultEventBusSOTEditor<FloatEventBusSO, float>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Float;
    }

    [CustomEditor(typeof(IntEventBusSO), true)]
    public class IntEventBusSOEditor : ResultEventBusSOTEditor<IntEventBusSO, int>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Int;
    }
}