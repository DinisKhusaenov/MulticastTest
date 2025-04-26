using System;
using UnityEngine;

namespace UI.Menu
{
    public interface IGameMenuView
    {
        event Action StartClicked;
        event Action SettingsClicked;
        
        Transform transform { get; }
    }
}