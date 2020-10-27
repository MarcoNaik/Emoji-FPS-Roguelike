using UnityEngine;

namespace DungeonGenerator.Scripts {
    public class GeneratorDoor : MonoBehaviour {
        public bool open = true;
        public GameObject voxelOwner;
        public GeneratorDoor sharedDoor;

        public Door door;
    }
}
