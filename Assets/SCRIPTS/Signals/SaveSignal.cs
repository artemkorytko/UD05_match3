namespace Match3.Signals
{
    public struct SaveSignal
    {
        public readonly int Points;
        
        public SaveSignal(DataToSave dataToSave)
        {
            // сколько очков 
            // зачем столько переменных и какая главная - я туплю
            Points = dataToSave.Points;
            
        }
        // все сигналы биндятся в SignalsInstaller

        
    }
}


