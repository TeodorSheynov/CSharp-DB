using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDto
{
    [XmlType("cars")]
    public class ExportCarsFromMaker
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("model")]
        public string Model { get; set; }
        [XmlAttribute("travelled-distance")]
        public string TravelledDistance { get; set; }
    }
}