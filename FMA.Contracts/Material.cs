using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FMA.Contracts
{
    [DebuggerDisplay("Title: {Title}")]
    public class Material
    {
        public Material(int id, string title, string description, IEnumerable<MaterialField> materialFields, byte[] flyerFrontSide, byte[] flyerBackside, string defaultFont)
        {
            Id = id;
            Title = title;
            Description = description;

            MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;

            if (string.IsNullOrEmpty(defaultFont) && MaterialFields.Any())
            {
                DefaultFont = MaterialFields.First().FontName;
            }
            else
            {
                DefaultFont = "Arial";
            }
        }

        public int Id { get; }

        public string Title { get; }

        public string Description { get;  }

        public List<MaterialField> MaterialFields { get; }

        public byte[] FlyerFrontSide { get;  }

        public byte[] FlyerBackside { get; }

        public string DefaultFont { get;  }
    }
}
