using System;
using System.Collections.Generic;

namespace FMA.Contracts
{
    public interface IFlyerMakerView
    {
        event Action<CustomMaterial> FlyerCreated;
        void SetMaterials(IEnumerable<Material> materials, Material selectedMaterial= null);
    }
}