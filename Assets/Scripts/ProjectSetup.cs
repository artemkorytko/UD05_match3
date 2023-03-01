using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class ProjectSetup : IInitializable

    {
    private readonly ProjectSettings _settings;

    public ProjectSetup(ProjectSettings settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        Application.targetFrameRate = _settings.TargetFps;
        Input.multiTouchEnabled = _settings.IsMultitouch;
    } 
    
    }
}