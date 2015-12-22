using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FMA.Contracts
{
     [DebuggerDisplay("Title: {Title}")]
    public class Material
    {
        public Material(int id, string title, string description, IEnumerable<MaterialField> materialFields, byte[] flyerFrontSide, byte[] flyerBackside)
        {
            Id = id;
            Title = title;
            Description = description;

            MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;
        }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<MaterialField> MaterialFields { get; private set; }

        public byte[] FlyerFrontSide { get; private set; }

        public byte[] FlyerBackside    { get; private set; }
    }
}
