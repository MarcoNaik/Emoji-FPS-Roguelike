using UnityEngine;

namespace Selection
{
    internal class HighlightSelectionResponse : MonoBehaviour
    {

        public void OnDeselect(Transform selection, Material defaultMaterial)
        {
            var selectionRenderer = selection.GetComponentInChildren<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = defaultMaterial;
            }
        }

        public void OnSelect(Transform selection, Material highlightMaterial)
        {
            var selectionRenderer = selection.GetComponentInChildren<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = highlightMaterial;
            }
        }
    }
}