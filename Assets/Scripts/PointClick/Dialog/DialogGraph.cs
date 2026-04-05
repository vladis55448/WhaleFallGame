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
    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption("EndState", typeof(EndState)).Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("").Build();
    }
}

public enum EndState
{
    FirstPersonMove,
    PointAndClick
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
class OptionsNode : Node
{
    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption("Text", typeof(string)).Build();
        context.AddOption("Option1", typeof(string)).Build();
        context.AddOption("Option2", typeof(string)).Build();
        context.AddOption("Option3", typeof(string)).Build();
        context.AddOption("Option4", typeof(string)).Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();
        context.AddOutputPort("OptionOutput1").Build();
        context.AddOutputPort("OptionOutput2").Build();
        context.AddOutputPort("OptionOutput3").Build();
        context.AddOutputPort("OptionOutput4").Build();
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

[Serializable]
class InventoryCheckNode : Node
{
    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption("ItemId", typeof(string)).Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("").Build();
        context.AddOutputPort("HasItem").Build();
        context.AddOutputPort("NoItem").Build();
    }
}