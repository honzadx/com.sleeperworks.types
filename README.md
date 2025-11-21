Scriptable Flow
===

A package that is designed for a ScriptableObject-heavy workflow, inspired by the 2017 Unite talk [Game Architecture with Scriptable Objects](https://www.youtube.com/watch?v=raQ3iHhE_Kk).

Features:
* <b>ScriptableObject Creator EditorWindow</b> - Quickly create `ScriptableObject` assets without navigating nested context menus
* <b>Event System</b> - `SignalEventBusSO` and `ResultEventBusSOT<T>` with inspector visualizers
* <b>Value Data Types</b> - `ValueSOT<T>` with inspector visualizers
* <b>Dependency Injection</b> - Multiple `IServiceLocator` options: Basic, Scoped, and Locked