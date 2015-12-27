using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FMA.Contracts
{
    [DebuggerDisplay("Title: {Title}")]
    public class Material
    {
        public Material(int id, string title, string description, IEnumerable<MaterialField> materialFields, byte[] flyerFrontSide, byte[] flyerBackside, string defaultFont = "")
        {
            Id = id;
            Title = title;
            Description = description;

            MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;

            if (String.IsNullOrEmpty(defaultFont) && this.MaterialFields.Any())
            {
                DefaultFont = this.MaterialFields.First().FontName;
            }
            else
            {
                DefaultFont = defaultFont;
            }
        }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public List<MaterialField> MaterialFields { get; private set; }

        public byte[] FlyerFrontSide { get; private set; }

        public byte[] FlyerBackside { get; private set; }
        public string DefaultFont { get; set; }
    }
}
