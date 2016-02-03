using System.Collections.Generic;
using System.Linq;

namespace FMA.Contracts
{
    public class CustomMaterial
    {
        public CustomMaterial(int id, string title, string description, IEnumerable<MaterialField> materialFields, byte[] flyerFrontSide, byte[] flyerBackside, CustomLogo customLogo)
        {
            Id = id;
            Title = title;
            Description = description;

            MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;

            CustomLogo = customLogo;
        }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<MaterialField> MaterialFields { get; private set; }

        public byte[] FlyerFrontSide { get; private set; }

        public byte[] FlyerBackside { get; private set; }

        public CustomLogo CustomLogo { get; private set; }
    }
}
