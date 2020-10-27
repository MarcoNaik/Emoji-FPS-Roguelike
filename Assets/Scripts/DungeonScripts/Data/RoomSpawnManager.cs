using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator.Scripts.Data
{
    [System.Serializable]
    public class RoomSpawnManager : ScriptableObject{
        public List<RoomSpawnData> enemySpawnLocations = new List<RoomSpawnData>();
    }
}
