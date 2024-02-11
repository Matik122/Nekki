using UnityEngine;

namespace Support
{
    public static class Extensions
    {
        public static bool LayerValidation(this Collider collider, string layerName) =>
            collider.gameObject.layer == LayerMask.NameToLayer(layerName);
    }
}