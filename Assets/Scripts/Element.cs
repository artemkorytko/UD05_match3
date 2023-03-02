﻿using System;
using Match3.Signals;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>{}

        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        private ParticleSystem _fx;

        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;
        private SignalBus _signalBus;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;
        private int _id;

        public Vector2 GridPosition => _gridPosition;

        public ElementConfigItem ConfigItem => _configItem;
        
        public bool IsActive { get; private set; }

        public int ID => _id;

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition, SignalBus signalBus)
        {
            _configItem = configItem;
            _elementPosition = elementPosition;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _localPosition = _elementPosition.LocalPosition;
            _gridPosition = _elementPosition.GridPosition;
            _id = _configItem.ID;
            _fx = GetComponentInChildren<ParticleSystem>();
            SetConfig();
            SetLocalPosition();
            Enable();
        }
        
        private void SetConfig()
        {
            iconSpriteRenderer.sprite = _configItem.Sprite;
        }

        private void SetLocalPosition()
        {
            transform.localPosition = _localPosition;
        }
        
        public void SetLocalPosition(Vector2 localPos, Vector2 gridPos)
        {
            _localPosition = localPos;
            _gridPosition = gridPos;
            SetLocalPosition();
        }
        
        private void Enable()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
        }

        public void ResetElement(ElementConfigItem config)
        {
            iconSpriteRenderer.sprite = config.Sprite;
            _id = config.ID;
            Enable();
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRenderer.enabled = isOn;
        }

        private void OnMouseUpAsButton()
        {
            OnClick();
        }

        private void OnClick()
        {
            _signalBus.Fire(new OnElementClickSignal(this));
        }

        public void Disable()
        {
            IsActive = false;
            gameObject.SetActive(false);
            _fx.Play();
        }
    }
}