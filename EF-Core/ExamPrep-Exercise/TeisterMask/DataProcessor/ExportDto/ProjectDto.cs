using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectDto
    {
        [XmlAttribute]
        public int TasksCount { get; set; }
        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }
        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }
        [XmlArray("Tasks")] 
        public TaskDto[] Tasks { get; set; }

    }
}
