using UnityEngine;
using Zenject;

namespace Skripts
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>
        {
            
        }
        
        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition)
        {
            _configItem = configItem;
            _elementPosition = elementPosition;
        }
    }
}