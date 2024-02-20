using Nano.Electric.Enums;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {

    [XmlRoot(nameof(DbScsGcSeriaConfigiration))]
    public class SeriaConfigirationKit : KitElement, IXmlSerializable {

        public SeriaConfigirationKit() {
            base.Children.Add(new DbGcStrightSegment());
            base.Children.Add(new DbGcStrightPartition());
            base.Children.Add(new DbGcHBend(bendType: ScsBendTypeEnum.DIRECT));
            base.Children.Add(new DbGcHBend(bendType: ScsBendTypeEnum.ANGLE_45));
            base.Children.Add(new DbGcHBend(bendType: ScsBendTypeEnum.OPTIONAL));
            base.Children.Add(new DbGcHBend(bendType: ScsBendTypeEnum.OPTIONAL_0_45));
            base.Children.Add(new DbGcVBendInner(bendType: ScsVerticalBendTypeEnum.ANGLE_90));
            base.Children.Add(new DbGcVBendInner(bendType: ScsVerticalBendTypeEnum.ANGLE_45));
            base.Children.Add(new DbGcVBendInner(bendType: ScsVerticalBendTypeEnum.ANGLE_0_90));
            base.Children.Add(new DbGcVBendOuter(bendType: ScsVerticalBendTypeEnum.ANGLE_90));
            base.Children.Add(new DbGcVBendOuter(bendType: ScsVerticalBendTypeEnum.ANGLE_45));
            base.Children.Add(new DbGcVBendOuter(bendType: ScsVerticalBendTypeEnum.ANGLE_0_90));
            base.Children.Add(new DbGcVBendUniversal());
            base.Children.Add(new DbGcTriple(tripleType: 0));
            base.Children.Add(new DbGcTriple(tripleType: 1));
            base.Children.Add(new DbGcCross(crossType: 0));
            base.Children.Add(new DbGcCross(crossType: 1));
            base.Children.Add(new DbGcMsPassage(isVertical: false));
            base.Children.Add(new DbGcMsPassage(isVertical: true));
            base.Children.Add(new DbGcMsJoint { });
            base.Children.Add(new DbGcMsCork { });
        }

        public DbGcStrightSegment DbGcStrightSegment { get => (DbGcStrightSegment)base.Children[0]; set => base.Children[0] = value; }
        public DbGcStrightPartition DbGcStrightPartition { get => (DbGcStrightPartition)base.Children[1]; set => base.Children[1] = value; }
        public DbGcHBend DbGcHBend_Direct { get => (DbGcHBend)base.Children[2]; set => base.Children[2] = value; }
        public DbGcHBend DbGcHBend_ANGLE_45 { get => (DbGcHBend)base.Children[3]; set => base.Children[3] = value; }
        public DbGcHBend DbGcHBend_OPTIONAL { get => (DbGcHBend)base.Children[4]; set => base.Children[4] = value; }
        public DbGcHBend DbGcHBend_OPTIONAL_0_45 { get => (DbGcHBend)base.Children[5]; set => base.Children[5] = value; }
        public DbGcVBendInner DbGcVBendInner_ANGLE_90 { get => (DbGcVBendInner)base.Children[6]; set => base.Children[6] = value; }
        public DbGcVBendInner DbGcVBendInner_ANGLE_45 { get => (DbGcVBendInner)base.Children[7]; set => base.Children[7] = value; }
        public DbGcVBendInner DbGcVBendInner_ANGLE_0_90 { get => (DbGcVBendInner)base.Children[8]; set => base.Children[8] = value; }
        public DbGcVBendOuter DbGcVBendOuter_ANGLE_90 { get => (DbGcVBendOuter)base.Children[9]; set => base.Children[9] = value; }
        public DbGcVBendOuter DbGcVBendOuter_ANGLE_45 { get => (DbGcVBendOuter)base.Children[10]; set => base.Children[10] = value; }
        public DbGcVBendOuter DbGcVBendOuter_ANGLE_0_90 { get => (DbGcVBendOuter)base.Children[11]; set => base.Children[11] = value; }
        public DbGcVBendUniversal DbGcVBendUniversal { get => (DbGcVBendUniversal)base.Children[12]; set => base.Children[12] = value; }
        /// <summary>
        /// Секция Т-образная горизонтальная со сменой ширины лотка
        /// </summary>
        public DbGcTriple DbGcTriple_0 { get => (DbGcTriple)base.Children[13]; set => base.Children[13] = value; }
        /// <summary>
        /// Секция Т-образная горизонтальная без смены ширины лотка
        /// </summary>
        public DbGcTriple DbGcTriple_1 { get => (DbGcTriple)base.Children[14]; set => base.Children[14] = value; }
        /// <summary>
        /// Секция Х-образная горизонтальная со сменой ширины лотка
        /// </summary>
        public DbGcCross DbGcCross_0 { get => (DbGcCross)base.Children[15]; set => base.Children[15] = value; }
        /// <summary>
        /// Секция Х-образная горизонтальная без смены ширины лотка
        /// </summary>
        public DbGcCross DbGcCross_1 { get => (DbGcCross)base.Children[16]; set => base.Children[16] = value; }
        /// <summary>
        /// Секция соединительная переходная горизонтальная
        /// </summary>
        public DbGcMsPassage DbGcMsPassage_0 { get => (DbGcMsPassage)base.Children[17]; set => base.Children[17] = value; }
        /// <summary>
        /// Секция соединительная переходная вертикальная
        /// </summary>
        public DbGcMsPassage DbGcMsPassage_1 { get => (DbGcMsPassage)base.Children[18]; set => base.Children[18] = value; }
        /// <summary>
        /// Соединитель на стык
        /// </summary>
        public DbGcMsJoint DbGcMsJoint { get => (DbGcMsJoint)base.Children[19]; set => base.Children[19] = value; }
        /// <summary>
        /// Торцевая заглушка
        /// </summary>
        public DbGcMsCork DbGcMsCork { get => (DbGcMsCork)base.Children[20]; set => base.Children[20] = value; }

        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
        private static string[]? partNames = null;
        public static IEnumerable<string> PartNames {
            get {
                partNames ??= new string[] {
                    nameof(DbGcStrightSegment),
                    nameof(DbGcStrightPartition),
                    nameof(DbGcHBend_Direct),
                    nameof(DbGcHBend_ANGLE_45),
                    nameof(DbGcHBend_OPTIONAL),
                    nameof(DbGcHBend_OPTIONAL_0_45),
                    nameof(DbGcVBendInner_ANGLE_90),
                    nameof(DbGcVBendInner_ANGLE_45),
                    nameof(DbGcVBendInner_ANGLE_0_90),
                    nameof(DbGcVBendOuter_ANGLE_90),
                    nameof(DbGcVBendOuter_ANGLE_45),
                    nameof(DbGcVBendOuter_ANGLE_0_90),
                    nameof(DbGcVBendUniversal),
                    nameof(DbGcTriple_0),
                    nameof(DbGcTriple_1),
                    nameof(DbGcCross_0),
                    nameof(DbGcCross_1),
                    nameof(DbGcMsPassage_0),
                    nameof(DbGcMsPassage_1),
                    nameof(DbGcMsJoint),
                    nameof(DbGcMsCork),
           };
                return partNames;
            }
        }
    }

}
