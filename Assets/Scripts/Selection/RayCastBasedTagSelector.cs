using System;
using UnityEngine;

namespace Selection
{
    public class RayCastBasedTagSelector : MonoBehaviour, ISelector
    {
        [SerializeField] private string selectableTag = "Selectable";
        

        private Transform selection;

        public void Check(Ray ray)
        {
            this.selection = null;

            if (!Physics.Raycast(ray, out var hit)) return;
        
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag) &&
                Vector3.Distance(selection.transform.position, PlayerController.Instance.transform.position) < 4f)
            {
                this.selection = selection;
            }
        }

        public Transform GetSelection()
        {
            return selection;
        }
    }
}