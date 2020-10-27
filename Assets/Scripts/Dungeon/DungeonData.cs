using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator {
    public class DungeonData : ScriptableObject {

        public List<DungeonSet> sets = new List<DungeonSet>();
    }
}