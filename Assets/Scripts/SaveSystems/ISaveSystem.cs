using Cysharp.Threading.Tasks;

namespace DefaultNamespace.SaveSystems
{
    public interface ISaveSystem
    {
        public GameData GameData { get; }  
            
        public UniTask Initialize();
        public void SaveData();
        public UniTask<bool> LoadData();
    }
}