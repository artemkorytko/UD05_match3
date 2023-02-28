using UnityEngine;
using Zenject;

namespace Skripts
{
    public class BoardController : IInitializable
    {
        private readonly ElementsConfig _config;
        private readonly Element.Factory _factory;

        public BoardController(ElementsConfig config, Element.Factory factory)
        {
            _config = config;
            _factory = factory;
        }
        
        
        public void Initialize()
        {
            _factory.Create(_config.GetItemByKey("Blue"), new ElementPosition(Vector2.zero, Vector2.zero));
        }
    }
}