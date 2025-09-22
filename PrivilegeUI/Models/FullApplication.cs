using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace PrivilegeUI.Models
{
    public class FullApplication
    {
        // using System.Xml.Serialization;
        // XmlSerializer serializer = new XmlSerializer(typeof(Htmlx));
        // using (StringReader reader = new StringReader(xml))
        // {
        //    var test = (Htmlx)serializer.Deserialize(reader);
        // }

        [XmlRoot(ElementName = "nameru")]
        public class Nameru
        {

            [XmlElement(ElementName = "br")]
            public object Br { get; set; }
        }

        [XmlRoot(ElementName = "reg")]
        public class Reg
        {

            [XmlElement(ElementName = "datareg")]
            public object Datareg { get; set; }

            [XmlElement(ElementName = "regnumber")]
            public object Regnumber { get; set; }

            [XmlElement(ElementName = "uslugnumber")]
            public int Uslugnumber { get; set; }
        }

        [XmlRoot(ElementName = "header")]
        public class Header
        {
            [XmlText]
            public string Text { get; set; }

            [XmlAnyElement("br")]
            public XmlElement Br { get; set; }
        }

        [XmlRoot(ElementName = "legalentity")]
        public class Legalentity
        {

            [XmlElement(ElementName = "kodorgout")]
            public int Kodorgout { get; set; }

            [XmlElement(ElementName = "nameorg")]
            public string Nameorg { get; set; }

            [XmlElement(ElementName = "structuralsubdivision")]
            public object Structuralsubdivision { get; set; }

            [XmlElement(ElementName = "nameorgsub")]
            public object Nameorgsub { get; set; }

            [XmlElement(ElementName = "post")]
            public string Post { get; set; }
        }

        [XmlRoot(ElementName = "destination")]
        public class Destination
        {

            [XmlElement(ElementName = "legalentity")]
            public Legalentity Legalentity { get; set; }
        }

        [XmlRoot(ElementName = "destinations")]
        public class Destinations
        {

            [XmlElement(ElementName = "destination")]
            public Destination Destination { get; set; }
        }

        [XmlRoot(ElementName = "p")]
        public class P
        {

            [XmlElement(ElementName = "appeal")]
            public string Appeal { get; set; }

            [XmlAttribute(AttributeName = "align")]
            public string Align { get; set; }

            [XmlText]
            public string Text { get; set; }

            [XmlElement(ElementName = "card_info")]
            public CardInfo CardInfo { get; set; }

            [XmlElement(ElementName = "person")]
            public Person Person { get; set; }

            [XmlElement(ElementName = "uslov")]
            public object Uslov { get; set; }
        }

        [XmlRoot(ElementName = "card_data")]
        public class CardData
        {

            [XmlElement(ElementName = "card_id")]
            public int CardId { get; set; }

            [XmlElement(ElementName = "last_4_digits")]
            public int Last4Digits { get; set; }
        }

        [XmlRoot(ElementName = "card_info")]
        public class CardInfo
        {

            [XmlElement(ElementName = "card_data")]
            public CardData CardData { get; set; }

            [XmlElement(ElementName = "lgota_text")]
            public string LgotaText { get; set; }
        }

        [XmlRoot(ElementName = "fio")]
        public class Fio
        {

            [XmlElement(ElementName = "fam")]
            public string Fam { get; set; }

            [XmlElement(ElementName = "im")]
            public string Im { get; set; }

            [XmlElement(ElementName = "ot")]
            public string Ot { get; set; }
        }

        [XmlRoot(ElementName = "row")]
        public class Row
        {

            [XmlElement(ElementName = "raion")]
            public string Raion { get; set; }

            [XmlElement(ElementName = "gorod")]
            public object Gorod { get; set; }

            [XmlElement(ElementName = "ulica")]
            public string Ulica { get; set; }

            [XmlElement(ElementName = "dom")]
            public int Dom { get; set; }

            [XmlElement(ElementName = "kvartira")]
            public object Kvartira { get; set; }

            [XmlElement(ElementName = "kladr")]
            public string Kladr { get; set; }

            [XmlElement(ElementName = "telefon")]
            public double Telefon { get; set; }

            [XmlElement(ElementName = "email")]
            public string Email { get; set; }
        }

        [XmlRoot(ElementName = "adress_proj")]
        public class AdressProj
        {

            [XmlElement(ElementName = "row")]
            public Row Row { get; set; }
        }

        [XmlRoot(ElementName = "vidan")]
        public class Vidan
        {

            [XmlElement(ElementName = "kem")]
            public string Kem { get; set; }
        }

        [XmlRoot(ElementName = "passport")]
        public class Passport
        {

            [XmlElement(ElementName = "ser")]
            public string Ser { get; set; }

            [XmlElement(ElementName = "nom")]
            public int Nom { get; set; }

            [XmlElement(ElementName = "vidan")]
            public Vidan Vidan { get; set; }
        }

        [XmlRoot(ElementName = "passport_data")]
        public class PassportData
        {

            [XmlElement(ElementName = "passport")]
            public Passport Passport { get; set; }
        }

        [XmlRoot(ElementName = "person")]
        public class Person
        {

            [XmlElement(ElementName = "fio")]
            public Fio Fio { get; set; }

            [XmlElement(ElementName = "adress_proj")]
            public AdressProj AdressProj { get; set; }

            [XmlElement(ElementName = "passport_data")]
            public PassportData PassportData { get; set; }

            [XmlElement(ElementName = "row")]
            public List<Row> Row { get; set; }
        }

        [XmlRoot(ElementName = "content")]
        public class Content
        {

            [XmlElement(ElementName = "p")]
            public List<P> P { get; set; }

            [XmlElement(ElementName = "blok")]
            public object Blok { get; set; }
        }

        [XmlRoot(ElementName = "a")]
        public class A
        {

            [XmlAttribute(AttributeName = "download")]
            public string Download { get; set; }

            [XmlAttribute(AttributeName = "href")]
            public string Href { get; set; }

            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "application")]
        public class Application
        {

            [XmlElement(ElementName = "a")]
            public A A { get; set; }

            [XmlElement(ElementName = "data")]
            public object Data { get; set; }
        }

        [XmlRoot(ElementName = "applications")]
        public class Applications
        {

            [XmlElement(ElementName = "application")]
            public List<Application> Application { get; set; }
        }

        [XmlRoot(ElementName = "container")]
        public class Container
        {

            [XmlElement(ElementName = "e-doc")]
            public string Edoc { get; set; }

            [XmlElement(ElementName = "nameru")]
            public Nameru Nameru { get; set; }

            [XmlElement(ElementName = "reg")]
            public Reg Reg { get; set; }

            [XmlElement(ElementName = "grifrestriction")]
            public object Grifrestriction { get; set; }

            [XmlElement(ElementName = "header")]
            public Header Header { get; set; }

            [XmlElement(ElementName = "destinations")]
            public Destinations Destinations { get; set; }

            [XmlElement(ElementName = "content")]
            public Content Content { get; set; }

            [XmlElement(ElementName = "applications")]
            public Applications Applications { get; set; }

            [XmlElement(ElementName = "kodformdoc")]
            public object Kodformdoc { get; set; }

            [XmlElement(ElementName = "kodorg")]
            public int Kodorg { get; set; }

            [XmlElement(ElementName = "kodorgsub")]
            public object Kodorgsub { get; set; }

            [XmlElement(ElementName = "dateblank")]
            public string Dateblank { get; set; }

            [XmlElement(ElementName = "style")]
            public List<string> Style { get; set; }

            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }

            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "servinfo")]
        public class Servinfo
        {

            [XmlElement(ElementName = "signaturesxml")]
            public object Signaturesxml { get; set; }

            [XmlElement(ElementName = "idgosuslug")]
            public int Idgosuslug { get; set; }

            [XmlElement(ElementName = "resultgosuslug")]
            public object Resultgosuslug { get; set; }

            [XmlElement(ElementName = "idxml")]
            public object Idxml { get; set; }

            [XmlElement(ElementName = "timestampout")]
            public object Timestampout { get; set; }

            [XmlElement(ElementName = "timestampin")]
            public object Timestampin { get; set; }
        }

        [XmlRoot(ElementName = "body2")]
        public class Body2
        {

            [XmlElement(ElementName = "container")]
            public Container Container { get; set; }

            [XmlElement(ElementName = "servinfo")]
            public Servinfo Servinfo { get; set; }
        }

        [XmlRoot(ElementName = "htmlx", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Htmlx
        {

            [XmlElement(ElementName = "body2")]
            public Body2 Body2 { get; set; }

            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }

            [XmlElement(ElementName = "lang")]
            public List<string> Lang { get; set; }

            [XmlText]
            public string Text { get; set; }
        }
    }
}