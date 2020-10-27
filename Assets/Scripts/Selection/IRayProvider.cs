using UnityEngine;

namespace Selection
{
    public interface IRayProvider
    {
        Ray CreateRay();
    }
}