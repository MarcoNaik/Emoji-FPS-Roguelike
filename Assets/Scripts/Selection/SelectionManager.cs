using Items;
using UnityEngine;

namespace Selection
{
    public class SelectionManager : MonoBehaviour
    {
        private IRayProvider rayProvider;
        private ISelector selector;
        private HighlightSelectionResponse selectionResponse;
    
        private Transform currentSelection;

        private void Awake()
        {
            rayProvider = GetComponent<IRayProvider>();
            selector = GetComponent<ISelector>();
            selectionResponse = GetComponent<HighlightSelectionResponse>();
        }

        private void Update()
        {
            if (currentSelection != null) 
                selectionResponse.OnDeselect(currentSelection, currentSelection.GetComponent<Interactable>().defaultMat);
        
            selector.Check(rayProvider.CreateRay());
            currentSelection = selector.GetSelection();
            
            if (Input.GetKeyDown(KeyCode.E) && currentSelection != null)
            {
                Interactable item = currentSelection.GetComponentInParent<Interactable>();
                
                if(item != null) item.OnInteract();
            }
        
            if (currentSelection != null)
                selectionResponse.OnSelect(currentSelection, currentSelection.GetComponent<Interactable>().highlightMat);
        }
    }
}