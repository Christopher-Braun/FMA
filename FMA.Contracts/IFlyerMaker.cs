using System;
using System.Collections.Generic;

namespace FMA.Contracts
{
    public interface IFlyerMaker
    {
        event Action<CustomMaterial> FlyerCreated;
        void SetMaterials(IEnumerable<Material> materials);
        void SetSelectedMaterial(Material material);
    }
}