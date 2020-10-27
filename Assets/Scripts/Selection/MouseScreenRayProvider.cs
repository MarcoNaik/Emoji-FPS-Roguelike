using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Selection
{
    public class MouseScreenRayProvider : MonoBehaviour, IRayProvider
    {
        private Camera camera1;
        private bool iscamera1NotNull;

        private void Start()
        {
            iscamera1NotNull = camera1 != null;
            camera1 = Camera.main;
        }

        public Ray CreateRay()
        {
            Debug.Assert(iscamera1NotNull, "Camera.main != null");
            return camera1.ScreenPointToRay(Input.mousePosition);
        }
    }
}