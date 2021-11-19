using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDto
{
    [XmlType("car")]
    public class ExportCarPartsDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }
        [XmlAttribute("model")]
        public string Model { get; set; }
        [XmlAttribute("travelled-distance")]
        public string TravelledDistance { get; set; }
        [XmlArray("parts")]
        public ExportPart[] Parts { get; set; }
    }

    [XmlType("part")]
    public class ExportPart
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("price")]
        public string Price { get; set; }
    }
}