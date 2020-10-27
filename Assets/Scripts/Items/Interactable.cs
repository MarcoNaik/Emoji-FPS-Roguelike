using UnityEngine;

namespace Items
{
    public abstract class Interactable : MonoBehaviour
    {
        public Material highlightMat;
        public Material defaultMat;
    
    
        public abstract void OnInteract();

    }
}