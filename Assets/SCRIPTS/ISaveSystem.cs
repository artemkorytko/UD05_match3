using System;
using Cysharp.Threading.Tasks;



namespace Match3
{
    
    // ##################### Save 2 #########################################
    // ИНТЕРФЕЙС - от него наследуется SaveToJson
    // но постоянно пишет что вы недореализовали его, поэтому отключила наследование нафиг
    
    public interface ISaveSystem
    { 
                // в интерфейсах все публик
                // нельзя объявить переменные, приватный метод
                // нельзя тела {}
            
                // типо контейнера где лежат названия методов - чистейшая инкапсуляция
            
                // можно - МЕТОДЫ, СВойсьтва, ИВЕНТЫ
                // только сигнатуры метода (названия)

                // эта хрень не дает наследоваться:
                // public DataToSave DataToSave { get;  }  // доступ на чтение

                public UniTask Initialize(int defaultValue);
                public void SaveData();

                public void ResetSaved();
                public UniTask<bool> LoadData();

    }
    
}