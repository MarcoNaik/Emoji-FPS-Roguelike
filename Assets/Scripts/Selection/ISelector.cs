using UnityEngine;

namespace Selection
{
    public interface ISelector
    {
        void Check(Ray ray);
        Transform GetSelection();
    }
}