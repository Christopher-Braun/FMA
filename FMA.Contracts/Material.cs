using System;
using System.Collections.Generic;
using System.Linq;

namespace FMA.Contracts
{
    public class Material
    {
        public Material(int id, string title, string description, IEnumerable<MaterialField> materialFields, byte[] flyerFrontSide, byte[] flyerBackside)
        {
            Id = id;
            Title = title;
            Description = description;

            this.MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;
        }

        public Int32 Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<MaterialField> MaterialFields { get; private set; }


        public Byte[] FlyerFrontSide { get; private set; }

        public Byte[] FlyerBackside    { get; private set; }
    }
}
