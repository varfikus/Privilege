using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [XmlRoot(ElementName = "pers_data", Namespace = "http://www.w3.org/1999/xhtml")]
        public class PersData
        {
            [XmlElement(ElementName = "fam", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Fam;

            [XmlElement(ElementName = "im", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Im;

            [XmlElement(ElementName = "ot", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Ot;
        }

        [XmlRoot(ElementName = "row", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Row
        {
            [XmlElement(ElementName = "raion", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Raion;

            [XmlElement(ElementName = "gorod", Namespace = "http://www.w3.org/1999/xhtml")]
            public object Gorod;

            [XmlElement(ElementName = "ulica", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Ulica;

            [XmlElement(ElementName = "dom", Namespace = "http://www.w3.org/1999/xhtml")]
            public int Dom;

            [XmlElement(ElementName = "kvartira", Namespace = "http://www.w3.org/1999/xhtml")]
            public object Kvartira;

            [XmlElement(ElementName = "kladr", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Kladr;

            [XmlElement(ElementName = "telefon", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Telefon;

            [XmlElement(ElementName = "email", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Email;
        }

        [XmlRoot(ElementName = "adress_proj", Namespace = "http://www.w3.org/1999/xhtml")]
        public class AdressProj
        {
            [XmlElement(ElementName = "row", Namespace = "http://www.w3.org/1999/xhtml")]
            public Row Row;
        }

        [XmlRoot(ElementName = "vidan", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Vidan
        {
            [XmlElement(ElementName = "kem", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Kem;

            [XmlElement(ElementName = "vidan", Namespace = "http://www.w3.org/1999/xhtml")]
            public object Vidan1;
        }

        [XmlRoot(ElementName = "passport", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Passport
        {
            [XmlElement(ElementName = "ser", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Ser;

            [XmlElement(ElementName = "nom", Namespace = "http://www.w3.org/1999/xhtml")]
            public int Nom;

            [XmlElement(ElementName = "vidan", Namespace = "http://www.w3.org/1999/xhtml")]
            public Vidan Vidan;
        }

        [XmlRoot(ElementName = "passport_data", Namespace = "http://www.w3.org/1999/xhtml")]
        public class PassportData
        {
            [XmlElement(ElementName = "passport", Namespace = "http://www.w3.org/1999/xhtml")]
            public Passport Passport;
        }

        [XmlRoot(ElementName = "tophead", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Tophead
        {
            [XmlElement(ElementName = "mesto", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Mesto;

            [XmlElement(ElementName = "pers_data", Namespace = "http://www.w3.org/1999/xhtml")]
            public PersData PersData;

            [XmlElement(ElementName = "adress_proj", Namespace = "http://www.w3.org/1999/xhtml")]
            public AdressProj AdressProj;

            [XmlElement(ElementName = "passport_data", Namespace = "http://www.w3.org/1999/xhtml")]
            public PassportData PassportData;

            [XmlElement(ElementName = "row", Namespace = "http://www.w3.org/1999/xhtml")]
            public List<Row> Row;
        }

        [XmlRoot(ElementName = "topheader", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Topheader
        {
            [XmlElement(ElementName = "tophead", Namespace = "http://www.w3.org/1999/xhtml")]
            public Tophead Tophead;
        }

        [XmlRoot(ElementName = "card_data", Namespace = "http://www.w3.org/1999/xhtml")]
        public class CardData
        {
            [XmlElement(ElementName = "card_id", Namespace = "http://www.w3.org/1999/xhtml")]
            public int CardId;

            [XmlElement(ElementName = "last_4_digits", Namespace = "http://www.w3.org/1999/xhtml")]
            public int Last4Digits;
        }

        [XmlRoot(ElementName = "a", Namespace = "http://www.w3.org/1999/xhtml")]
        public class A
        {
            [XmlAttribute(AttributeName = "download", Namespace = "")]
            public string Download;

            [XmlAttribute(AttributeName = "href", Namespace = "")]
            public string Href;

            [XmlText]
            public string Text;
        }

        [XmlRoot(ElementName = "application", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Application
        {
            [XmlElement(ElementName = "a", Namespace = "http://www.w3.org/1999/xhtml")]
            public A A;

            [XmlElement(ElementName = "data", Namespace = "http://www.w3.org/1999/xhtml")]
            public object Data;
        }

        [XmlRoot(ElementName = "applications", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Applications
        {
            [XmlElement(ElementName = "application", Namespace = "http://www.w3.org/1999/xhtml")]
            public List<Application> Application;
        }

        [XmlRoot(ElementName = "uslov", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Uslov
        {
            [XmlElement(ElementName = "p", Namespace = "http://www.w3.org/1999/xhtml")]
            public List<string> P;
        }

        [XmlRoot(ElementName = "content", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Content
        {
            [XmlElement(ElementName = "card_data", Namespace = "http://www.w3.org/1999/xhtml")]
            public CardData CardData;

            [XmlElement(ElementName = "lgota_text", Namespace = "http://www.w3.org/1999/xhtml")]
            public string LgotaText;

            [XmlElement(ElementName = "applications", Namespace = "http://www.w3.org/1999/xhtml")]
            public Applications Applications;

            [XmlElement(ElementName = "uslov", Namespace = "http://www.w3.org/1999/xhtml")]
            public Uslov Uslov;
        }

        [XmlRoot(ElementName = "container", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Container
        {
            [XmlElement(ElementName = "topheader", Namespace = "http://www.w3.org/1999/xhtml")]
            public Topheader Topheader;

            [XmlElement(ElementName = "centertitle", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Centertitle;

            [XmlElement(ElementName = "content", Namespace = "http://www.w3.org/1999/xhtml")]
            public Content Content;

            [XmlElement(ElementName = "dateblank", Namespace = "http://www.w3.org/1999/xhtml")]
            public string RawDate;

            [XmlIgnore]
            public DateTime Dateblank =>
            DateTime.TryParseExact(RawDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)
                ? parsed
                : default;

            [XmlElement(ElementName = "style", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Style;

            [XmlAttribute(AttributeName = "id", Namespace = "")]
            public string Id;

            [XmlText]
            public string Text;
        }

        [XmlRoot(ElementName = "servinfo", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Servinfo
        {
            [XmlElement(ElementName = "signaturesxml", Namespace = "http://www.w3.org/1999/xhtml")]
            public List<object> Signaturesxml;

            [XmlElement(ElementName = "idgosuslug", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Idgosuslug;

            [XmlElement(ElementName = "region", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Region;

            [XmlElement(ElementName = "electronic", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Electronic;

            [XmlElement(ElementName = "idservice", Namespace = "http://www.w3.org/1999/xhtml")]
            public int Idservice;

            [XmlElement(ElementName = "nameservice", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Nameservice;

            [XmlElement(ElementName = "kladr_prop", Namespace = "http://www.w3.org/1999/xhtml")]
            public object KladrProp;

            [XmlElement(ElementName = "kladr_about", Namespace = "http://www.w3.org/1999/xhtml")]
            public object KladrAbout;

            [XmlElement(ElementName = "lgota", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Lgota;

            [XmlElement(ElementName = "guid", Namespace = "http://www.w3.org/1999/xhtml")]
            public string Guid;

            [XmlElement(ElementName = "isul", Namespace = "http://www.w3.org/1999/xhtml")]
            public bool Isul;
        }

        [XmlRoot(ElementName = "body2", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Body2
        {

            [XmlElement(ElementName = "container", Namespace = "http://www.w3.org/1999/xhtml")]
            public Container Container;

            [XmlElement(ElementName = "servinfo", Namespace = "http://www.w3.org/1999/xhtml")]
            public Servinfo Servinfo;
        }

        [XmlRoot(ElementName = "htmlx", Namespace = "http://www.w3.org/1999/xhtml")]
        public class Htmlx
        {
            [XmlElement(ElementName = "body2", Namespace = "http://www.w3.org/1999/xhtml")]
            public Body2 Body2;

            [XmlAttribute(AttributeName = "xmlns", Namespace = "")]
            public string Xmlns;

            [XmlElement(ElementName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
            public List<string> Lang;

            [XmlText]
            public string Text;
        }
    }
}
