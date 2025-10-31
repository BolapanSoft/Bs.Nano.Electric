// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbLtKiTable {
#if InitDbContextEnums
        public KiCurveType CurveType { get; set; }
        private static readonly CultureInfo XmlOutCulture = CultureInfo.GetCultureInfo("ru-RU");
        private static readonly CultureInfo ParseCulture = CultureInfo.GetCultureInfo("ru-RU");
        [XmlRoot("Table")]
        public record struct Table([XmlElement("TableData")] TableData TableData, [XmlElement("ListRoot")] ListRoot ListRoot) {
            public static Table FromXml(string xml) {
                if (xml == null)
                    throw new ArgumentNullException(nameof(xml));

                var serializer = new XmlSerializer(typeof(Table));
                using var sr = new StringReader(xml);
                var table = (Table?)serializer.Deserialize(sr);
                return table ??
                    throw new InvalidDataException(nameof(xml));
            }
            public string ToXml() {
                var serializer = new XmlSerializer(typeof(Table));
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty); // убрать namespace xsi/xsd

                var settings = new XmlWriterSettings {
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    OmitXmlDeclaration = false
                };

                using var sw = new StringWriter();
                using var xw = XmlWriter.Create(sw, settings);
                serializer.Serialize(xw, this, ns);
                return sw.ToString();
            }
        }
        public record struct TableData(
            [property: XmlElement("LtLightCurve")] LtLightCurve LtLightCurve
        ) {
        }

        public record struct LtLightCurve(
            [property: XmlElement("CurveName")] string CurveName
        ) {
        }

        public record struct ListRoot(
            [property: XmlElement("LtKccRecord")] List<LtKccRecord> Records
        ) {
        }

        public struct LtKccRecord {
            // --- числовые свойства (хранятся в памяти) ---
            [XmlIgnore]
            public double RoomIndex { get; set; }              // 0..50

            [XmlIgnore]
            public double ReflCoefCeiling { get; set; }        // 0..1

            [XmlIgnore]
            public double ReflCoefWall { get; set; }           // 0..1

            [XmlIgnore]
            public double ReflCoefWSurface { get; set; }       // 0..1

            [XmlIgnore]
            public int ActivityFactor { get; set; }            // >0, 0..100 check

            // --- XML-посредники (читаются/пишутся XmlSerializer'ом) ---
            // При десериализации XmlSerializer вызовет сеттеры этих свойств,
            // которые парсят строку и заполняют числовые поля.
            // При сериализации XmlSerializer вызовет getter - он вернёт форматированную строку.

            [XmlElement("RoomIndex")]
            public string RoomIndexXml {
                get => FormatRoomIndex(RoomIndex);
                set => RoomIndex = ParseDoubleAndValidate(value, nameof(RoomIndex), 0.0, 50.0);
            }

            [XmlElement("ReflCoefCeiling")]
            public string ReflCoefCeilingXml {
                get => FormatCoef(ReflCoefCeiling);
                set => ReflCoefCeiling = ParseDoubleAndValidate(value, nameof(ReflCoefCeiling), 0.0, 1.0);
            }

            [XmlElement("ReflCoefWall")]
            public string ReflCoefWallXml {
                get => FormatCoef(ReflCoefWall);
                set => ReflCoefWall = ParseDoubleAndValidate(value, nameof(ReflCoefWall), 0.0, 1.0);
            }

            [XmlElement("ReflCoefWSurface")]
            public string ReflCoefWSurfaceXml {
                get => FormatCoef(ReflCoefWSurface);
                set => ReflCoefWSurface = ParseDoubleAndValidate(value, nameof(ReflCoefWSurface), 0.0, 1.0);
            }

            [XmlElement("ActivityFactor")]
            public string ActivityFactorXml {
                get => ActivityFactor.ToString(CultureInfo.InvariantCulture);
                set => ActivityFactor = ParseIntAndValidate(value, nameof(ActivityFactor), 1, 500);
            }

            // --- Вспомогательные методы ---

            private static double ParseDoubleAndValidate(string? s, string fieldName, double min, double max) {
                if (string.IsNullOrWhiteSpace(s))
                    throw new InvalidOperationException($"Field {fieldName} is null or empty.");

                var trimmed = s!.Trim();

                // Попробуем разные культуры (поддержка запятой и точки)
                if (double.TryParse(trimmed, System.Globalization.NumberStyles.Number, ParseCulture, out var d)) {
                    ValidateRange(d, min, max, fieldName);
                    return d;
                }
                throw new FormatException($"Field {fieldName} has invalid numeric format: '{s}'.");
            }

            private static int ParseIntAndValidate(string? s, string fieldName, int minInclusive, int maxInclusive) {
                if (string.IsNullOrWhiteSpace(s))
                    throw new InvalidOperationException($"Field {fieldName} is null or empty.");

                var trimmed = s!.Trim();

                if (int.TryParse(trimmed, System.Globalization.NumberStyles.Integer, ParseCulture, out var i)) {
                    ValidateRange(i, minInclusive, maxInclusive, fieldName);
                    return i;
                }
                throw new FormatException($"Field {fieldName} has invalid integer format: '{s}'.");
            }

            private static void ValidateRange(double v, double min, double max, string fieldName) {
                if (v < min || v > max)
                    throw new ArgumentOutOfRangeException(fieldName, v, $"{fieldName} must be in [{min}, {max}].");
            }

            private static void ValidateRange(int v, int min, int max, string fieldName) {
                if (v < min || v > max)
                    throw new ArgumentOutOfRangeException(fieldName, v, $"{fieldName} must be in [{min}, {max}].");
            }

            private static string FormatCoef(double v) {
                // формат: всегда запятая и 2 знака
                return v.ToString("F2", XmlOutCulture);
            }

            private static string FormatRoomIndex(double v) {
                // RoomIndex — тоже с запятой и 2 знаками согласно требованию
                return v.ToString("F2", XmlOutCulture);
            }
        }
        private Table? _curveDb;
        //[Column(nameof(CurveDb), TypeName = "ntext")]
        [Column(nameof(CurveDb))]
        [MaxLength(-1)] // Недокументированное значение. Однако, отключает проверку на длину строки.
        public string? CurveDbString {
            get { return _curveDb?.ToXml()?? null; }
            set {
                if (value == null) {
                    _curveDb = null;
                }
                else {
                    var doc = Table.FromXml(value);
                    _curveDb = doc;
                }
            }
        }

        [NotMapped]
        public Table? CurveDb {
            get { return _curveDb; }
            set { _curveDb = value; }
        }

#endif
    }
}
