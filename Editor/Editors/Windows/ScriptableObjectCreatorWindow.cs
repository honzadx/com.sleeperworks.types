using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ScriptableFlow.Editor
{
    public class ScriptableObjectCreatorWindow : EditorWindow
    {
        private Entry[] _entries;

        private string _fileName;
        private bool _ignoreCase;
        private Entry[] _filteredEntries;
        private string[] _filteredEntryNames;
        private int _selectedIndex;

        private ScriptableObject _template;
        private UnityEditor.Editor _templateEditor;
        private bool _foldoutTemplate;

        private struct Entry
        {
            public Type type;
            public string name;
            public string fullName;
        }

        private string _filter;

        [MenuItem("Window/ScriptableObject Creator", priority = 10)]
        public static void OpenWindow()
        {
            ScriptableObjectCreatorWindow window = GetWindow<ScriptableObjectCreatorWindow>();
            window.titleContent = new GUIContent("ScriptableObject Creator");
            window.Show();
        }

        private void OnEnable()
        {
            List<Entry> validEntries = new();
                
            var flowAssembly = typeof(ScriptableFlow.Runtime.IntSO).Assembly;
            validEntries.AddRange(flowAssembly
                .GetTypes()
                .Where(IsValidScriptableObject)
                .Select(validType => new Entry
                {
                    type = validType,
                    name = validType.Name,
                    fullName = validType.FullName
                }));
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly == flowAssembly)
                    continue;
                var types = assembly
                    .GetTypes()
                    .Where(IsValidScriptableObject);
                var newEntries = types.Select(validType => new Entry
                {
                    type = validType,
                    name = validType.Name,
                    fullName = validType.FullName
                });
                validEntries.AddRange(newEntries);
            }

            _entries = validEntries.ToArray();
        }

        private void OnGUI()
        {
            var filter = EditorGUILayout.TextField("Filter", _filter);
            var ignoreCase = EditorGUILayout.Toggle("Ignore Case", _ignoreCase);

            bool ignoreCaseUpdated = _ignoreCase != ignoreCase;
            bool filterUpdated = filter != _filter | ignoreCaseUpdated | _filteredEntries == null;

            if (ignoreCaseUpdated)
                _ignoreCase = ignoreCase;

            if (filterUpdated)
            {
                var stringComparison = _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                _filter = filter;
                _filteredEntries = string.IsNullOrEmpty(_filter)
                    ? _entries
                    : _entries.Where(entry => entry.fullName.Contains(_filter, stringComparison)).ToArray();
                _filteredEntryNames = _filteredEntries.Select(filteredEntry => filteredEntry.fullName).ToArray();
                _selectedIndex = 0;
                _template = null;
                _templateEditor = null;
                _foldoutTemplate = false;
            }

            bool hasEntries = _filteredEntries.Length > 0;
            var selectedIndex = EditorGUILayout.Popup(_selectedIndex, _filteredEntryNames);
            bool indexChanged = selectedIndex != _selectedIndex;

            if (indexChanged)
                _selectedIndex = selectedIndex;

            using (new EditorGUI.DisabledScope(!hasEntries))
            {
                EditorGUILayout.LabelField("Type", hasEntries ? _filteredEntries[selectedIndex].name : "");

                if (indexChanged | filterUpdated)
                    _fileName = hasEntries ? _filteredEntries[selectedIndex].name : string.Empty;

                _fileName = EditorGUILayout.TextField("File Name", _fileName);
                _fileName = Regex.Replace(_fileName, @"[^A-Za-z0-9.]", string.Empty);

                var template = (ScriptableObject)EditorGUILayout.ObjectField("Template", _template,
                    hasEntries ? _filteredEntries[_selectedIndex].type : typeof(ScriptableObject), false);

                bool templateUpdated = _template != template;
                if (templateUpdated)
                {
                    _template = template;
                    _templateEditor = _template != null ? UnityEditor.Editor.CreateEditor(_template) : null;
                }

                var templateExists = _template != null;
                _foldoutTemplate = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutTemplate, "Template Inline View");
                if (_foldoutTemplate)
                {
                    using var templateIndent = new EditorGUI.IndentLevelScope();
                    using var templateDisabledScope = new EditorGUI.DisabledScope(true);
                    if (templateExists)
                        _templateEditor!.OnInspectorGUI();
                    else
                        EditorGUILayout.LabelField("No template selected");
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                if (GUILayout.Button("Create"))
                    OnCreateButtonPressed();
            }
        }

        private void OnCreateButtonPressed()
        {
            if (!TryGetActiveFolderPath(out string folderPath))
            {
                Debug.LogError("Failed to create asset. Could not retrieve Unity's active folder path.");
                return;
            }

            var assetPath = Path.Combine(folderPath, $"{_fileName}.asset");
            if (File.Exists(assetPath))
            {
                Debug.LogError($"Failed to create asset. An asset already exists at '{assetPath}'.");
                return;
            }

            ScriptableObject instance;
            if (_template == null)
            {
                instance = CreateInstance(_filteredEntries[_selectedIndex].type);
                AssetDatabase.CreateAsset(instance, assetPath);
            }
            else
            {
                var templatePath = AssetDatabase.GetAssetPath(_template);
                AssetDatabase.CopyAsset(templatePath, assetPath);
                instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            }

            Selection.activeObject = instance;
        }

        private static bool IsValidScriptableObject(Type type)
        {
            if (!type.IsSubclassOf(typeof(ScriptableObject)))
                return false;

            if (type.IsAbstract | type.IsGenericType | type.IsNotPublic)
                return false;

            if (string.IsNullOrEmpty(type.FullName) ||
                Regex.IsMatch(type.FullName, @"^(UnityEditor\.|UnityEngine\.|Unity\.)"))
                return false;

            if (type.GetCustomAttributes(typeof(ExcludeFromObjectFactoryAttribute), true).Length > 0)
                return false;

            var currentType = type;
            while (currentType != null)
            {
                if (currentType.IsGenericType &&
                    currentType.GetGenericTypeDefinition() == typeof(UnityEditor.ScriptableSingleton<>))
                    return false;
                currentType = currentType.BaseType;
            }

            return true;
        }

        private static bool TryGetActiveFolderPath(out string path)
        {
            path = string.Empty;
            var methodInfo = typeof(ProjectWindowUtil).GetMethod("TryGetActiveFolderPath",
                BindingFlags.Static | BindingFlags.NonPublic);
            if (methodInfo == null)
            {
                return false;
            }

            object[] args = { null };
            bool found = (bool)methodInfo!.Invoke(null, args);
            path = (string)args[0];

            return found;
        }
    }
}