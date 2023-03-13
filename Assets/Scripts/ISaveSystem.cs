namespace Match3
{
    public interface ISaveSystem
    {
        public void SaveData(GameData data);

        public GameData LoadData();
    }
}