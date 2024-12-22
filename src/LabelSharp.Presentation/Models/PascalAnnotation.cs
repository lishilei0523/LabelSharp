using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LabelSharp.Presentation.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("annotation")]
    public class PascalAnnotation
    {
        [XmlElement("folder", Form = XmlSchemaForm.Unqualified)]
        public string Folder { get; set; }

        [XmlElement("filename", Form = XmlSchemaForm.Unqualified)]
        public string Filename { get; set; }

        [XmlElement("path", Form = XmlSchemaForm.Unqualified)]
        public string Path { get; set; }

        [XmlElement("source", Form = XmlSchemaForm.Unqualified)]
        public AnnotationSource Source { get; set; }

        [XmlElement("size", Form = XmlSchemaForm.Unqualified)]
        public ImageSize ImageSize { get; set; }

        [XmlElement("segmented", Form = XmlSchemaForm.Unqualified)]
        public int Segmented { get; set; }

        [XmlElement("object", Form = XmlSchemaForm.Unqualified)]
        public AnnotationInfo[] Annotations { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    public class AnnotationSource
    {
        [XmlElement("database", Form = XmlSchemaForm.Unqualified)]
        public string Database { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    public class ImageSize
    {
        [XmlElement("width", Form = XmlSchemaForm.Unqualified)]
        public int Width { get; set; }

        [XmlElement("height", Form = XmlSchemaForm.Unqualified)]
        public int Height { get; set; }

        [XmlElement("depth", Form = XmlSchemaForm.Unqualified)]
        public int Depth { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    public class AnnotationInfo
    {
        [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement("pose", Form = XmlSchemaForm.Unqualified)]
        public string Pose { get; set; }

        [XmlElement("truncated", Form = XmlSchemaForm.Unqualified)]
        public int Truncated { get; set; }

        [XmlElement("difficult", Form = XmlSchemaForm.Unqualified)]
        public int Difficult { get; set; }

        [XmlElement("bndbox", Form = XmlSchemaForm.Unqualified)]
        public Location Location { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Location
    {
        [XmlElement("xmin", Form = XmlSchemaForm.Unqualified)]
        public int XMin { get; set; }

        [XmlElement("ymin", Form = XmlSchemaForm.Unqualified)]
        public int YMin { get; set; }

        [XmlElement("xmax", Form = XmlSchemaForm.Unqualified)]
        public int XMax { get; set; }

        [XmlElement("ymax", Form = XmlSchemaForm.Unqualified)]
        public int YMax { get; set; }
    }
}
