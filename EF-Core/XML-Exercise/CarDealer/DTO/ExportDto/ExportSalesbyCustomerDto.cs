using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDto
{
    [XmlType("customer")]
    public class ExportSalesbyCustomerDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }
        [XmlAttribute("bought-cars")]
        public string BoughtCars { get; set; }
        [XmlAttribute("spent-money")]
        public decimal MoneySpent { get; set; }
    }
}