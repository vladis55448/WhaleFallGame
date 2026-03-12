using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;


[Graph(AssetExtension)]
[Serializable]
class DialogGraph : Graph
{
    public const string AssetExtension = "simpleg";

    [MenuItem("Assets/Create/Dialog/New Graph", false)]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogGraph>();
    }
}

[Serializable]
class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("").Build();
    }
}

[Serializable]
class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("").Build();
    }
}


[Serializable]
class DialogNode : Node
{
    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption("Text", typeof(string)).Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("").Build();
        context.AddOutputPort("").Build();
        context.AddInputPort<string>("Character").Build();
    }
}

[Serializable]
class EventNode : Node
{
    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption("EventId", typeof(string)).Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("").Build();
        context.AddOutputPort("").Build();
    }
}
