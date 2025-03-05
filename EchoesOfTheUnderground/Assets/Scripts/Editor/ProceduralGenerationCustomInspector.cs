using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ProceduralGeneration))]

public class ProceduralGenerationCustomInspector : Editor
{
    public VisualTreeAsset visualTreeAsset;
    private ProceduralGeneration proceduralGeneration;
    private Button SaveWorldButton;
    private Button DeleteWorldButton;
    private Button OpenFileButton;

    private void OnEnable()
    {
        proceduralGeneration= (ProceduralGeneration)target;
    }
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement visualElement = new VisualElement();

        visualTreeAsset.CloneTree(visualElement);
        //Buttons 
        SaveWorldButton = visualElement.Query<Button>("savefile");
        SaveWorldButton.RegisterCallback<ClickEvent>(SaveFile);
        DeleteWorldButton = visualElement.Query<Button>("deletefile");
        DeleteWorldButton.RegisterCallback<ClickEvent>(DeleteFile);
        OpenFileButton = visualElement.Query<Button>("openfile");
        OpenFileButton.RegisterCallback<ClickEvent>(OpenFile);
        return visualElement;
    }

    private void SaveFile(ClickEvent evt)
    {
        proceduralGeneration.SaveWorld();
    }
    private void DeleteFile(ClickEvent evt)
    { 
        proceduralGeneration.DeleteWorld();
    }
    private void OpenFile(ClickEvent evt)
    { 
        proceduralGeneration.OpenFile();
    }
}
