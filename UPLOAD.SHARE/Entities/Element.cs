using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UPLOAD.SHARE.Entities
{
    public class Element
    {
        public string Group { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

        [JsonPropertyName("small")]
        public string Sign { get; set; }
        public double Molar { get; set; }
        public IList<int> Electrons { get; set; }

        public override string ToString()
        {
            return $"{Sign} - {Name}";
        }

    }
}
