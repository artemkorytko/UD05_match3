using System;
using DefaultNamespace.Panels;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public enum Panel
    {
        None,
        Menu,
        Game
    }

    public class UIManager : MonoBehaviour
    {
        private MenuPanel _menuPanel;
        private GamePanel _gamePanel;

        private void Awake()
        {
            _menuPanel = GetComponentInChildren<MenuPanel>();
            _gamePanel = GetComponentInChildren<GamePanel>();
        }
        

        public void SetPanel(Panel panel)
        {
            _menuPanel.gameObject.SetActive(panel == Panel.Menu);
            _gamePanel.gameObject.SetActive(panel == Panel.Game);
        }
    }
}