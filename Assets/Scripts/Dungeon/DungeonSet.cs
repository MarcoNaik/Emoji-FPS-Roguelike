using System.Collections.Generic;
using DungeonGenerator.Scripts;
using UnityEngine;

namespace DungeonGenerator {
    public class DungeonSet : ScriptableObject {

        public string dungeonName = "";

        public List<Room> spawns = new List<Room>();
        public List<Room> bosses = new List<Room>();
        public List<Door> doors = new List<Door>();
        public List<Room> roomTemplates = new List<Room>();
        
    }
}
