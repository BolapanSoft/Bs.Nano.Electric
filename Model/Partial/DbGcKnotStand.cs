using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Представляет элемент комплектации "Стойка"
    /// </summary>
    [XmlRoot("DbGcKnotStand")]
    public class DbGcKnotStand : KitElement, IXmlSerializable {
        public bool IsEnabled { get; set; }
        public bool IsUse { get; set; }
        public int Length { get; set; }

        public ScsGutterBolting Bolting { get; set; }
        public DbGcKnotStand() {
            IsEnabled = true;
            IsUse = true;
        }

        public IEnumerable<DbGcKnotLevel> Levels {
            get {
                return base.GetChildren<DbGcKnotLevel>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
        }
        public void AddChild(DbGcKnotLevel unit) {
            Children.Add(unit);
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!(Bolting is null)) {
                writer.WriteStartElement("Bolting");
                writer.WriteAttributeString("TableName", "ScsGutterBolting");
                writer.WriteAttributeString("Id", Bolting.Id.ToString());
                writer.WriteEndElement();
            }
            writer.WriteElementString("IsEnabled", IsEnabled.ToString());
            writer.WriteElementString("IsUse", IsUse.ToString());
            writer.WriteElementString("Length", Length.ToString());
        }
    }
    /// <summary>
    /// Секция прямая
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot("DbGcStrightSegment")]
    public class DbGcStrightSegment : KitElement, IXmlSerializable {
        public bool IsEnabled { get; set; }
        public bool IsUse { get; set; }
        public string? GutterCoverSeria { get; private set; }
        public string? GutterSeria { get; private set; }
        /// <summary>
        /// Тип комплектации
        /// </summary>
        public DbGcStrightSegmentComplectType ComplectType { get; set; }
        public DbGcStrightSegment() {
            ComplectType = DbGcStrightSegmentComplectType.METER;
            IsEnabled = false;
            IsUse = false;
        }
        public void SetGutterSeria(ScsGutterCanal element) {
            GutterSeria = element.Series;
            IsEnabled = true;
            IsUse = true;
            return;
        }
        public void SetGutterCoverSeria(DbScsGutterCover element) {
            GutterCoverSeria = element.Series;
            IsEnabled = true;
            return;
        }

        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            IsEnabled = true;
            Children.Add(unit);
        }


        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString("ComplectType", ComplectType.ToString());
            if (!string.IsNullOrEmpty(GutterCoverSeria)) {
                writer.WriteElementString("GutterCoverSeria", GutterCoverSeria);
            }
            if (!string.IsNullOrEmpty(GutterSeria)) {
                writer.WriteElementString("GutterSeria", GutterSeria);
            }
            writer.WriteElementString("IsEnabled", IsEnabled.ToString());
            writer.WriteElementString("IsUse", IsUse.ToString());
        }
    }
    /// <summary>
    /// Перегородка
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcStrightPartition))]
    public class DbGcStrightPartition : KitElement, IXmlSerializable {
        public bool IsEnabled { get; set; }
        public bool IsUse { get; set; }
        public string? GutterPartitionSeria { get; private set; }
        /// <summary>
        /// Тип комплектации
        /// </summary>
        public DbGcStrightSegmentComplectType ComplectType { get; set; }
        public DbGcStrightPartition() {
            ComplectType = DbGcStrightSegmentComplectType.METER;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            IsEnabled = true;
            Children.Add(unit);
        }

        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString(nameof(ComplectType), ComplectType.ToString());
            if (!string.IsNullOrEmpty(GutterPartitionSeria)) {
                writer.WriteElementString(nameof(GutterPartitionSeria), GutterPartitionSeria);
            }
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }

        public void SetGutterPartitionSeria(DbScsGutterPartition element) {
            GutterPartitionSeria = element.Series;
            IsEnabled = true;
            IsUse = true;
        }
    }

    /// <summary>
    /// Секция угловая горизонтальная
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcHBend))]
    public class DbGcHBend : KitElement, IXmlSerializable {
        private readonly Func<ScsGcFitting, bool> IsCompatible;
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? BendCoverSeria { get; private set; }
        public string? BendSeria { get; private set; }
        public void SetBendSeria(ScsGcFitting gutterFitting) {
            if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.BEND
                  && IsCompatible(gutterFitting)) {
                BendSeria = gutterFitting.Series;
                IsEnabled = true;
                IsUse = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendSeria,
                    $"Для присвоения серии элементу {ScsGutterFittingTypeEnum.BEND.GetDescription()} необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.BEND.GetDescription()} с углом поворота {BendType.GetDescription()}\"");
        }
        public void SetBendCoverSeria(DbScsGcCoverUnit gutterFitting) {
            if (gutterFitting.CoverType == ScsGcCoverType.BEND) {
                BendCoverSeria = gutterFitting.Series;
                IsEnabled = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendCoverSeria,
                    $"Для присвоения серии крышки элементу {ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER.GetDescription()} необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.VERTICAL_BEND_INNER.GetDescription()}\"");
        }

        /// <summary>
        /// Тип комплектации
        /// </summary>
        public ScsBendTypeEnum BendType { get; private set; }
        public DbGcHBend(ScsBendTypeEnum bendType) {
            BendType = bendType;
            switch (bendType) {
                case ScsBendTypeEnum.DIRECT:
                case ScsBendTypeEnum.OTHER:
                    IsCompatible = (gutterFitting) => gutterFitting.BendType == ScsBendTypeEnum.DIRECT;
                    break;
                case ScsBendTypeEnum.OPTIONAL:
                    IsCompatible = (gutterFitting) => gutterFitting.BendType == ScsBendTypeEnum.OPTIONAL;
                    break;
                case ScsBendTypeEnum.ANGLE_45:
                    IsCompatible = (gutterFitting) => gutterFitting.BendType == ScsBendTypeEnum.ANGLE_45;
                    break;
                case ScsBendTypeEnum.OPTIONAL_0_45:
                    IsCompatible = (gutterFitting) => gutterFitting.BendType == ScsBendTypeEnum.OPTIONAL_0_45;
                    break;
                default:
                    IsCompatible = (gutterFitting) => false;
                    break;
            }
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(BendCoverSeria)) {
                writer.WriteElementString(nameof(BendCoverSeria), BendCoverSeria);
            }
            if (!string.IsNullOrEmpty(BendSeria)) {
                writer.WriteElementString(nameof(BendSeria), BendSeria);
            }
            writer.WriteElementString(nameof(BendType), BendType.ToString());
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }

    /// <summary>
    /// Секция угловая вертикальная внутренняя
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcVBendInner))]
    public class DbGcVBendInner : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? BendInnerCoverSeria { get; private set; }
        public string? BendInnerSeria { get; private set; }
        public void SetBendInnerSeria(ScsGcFitting gutterFitting) {
            if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER
                  && gutterFitting.VerticalBendType == this.BendType) {
                BendInnerSeria = gutterFitting.Series;
                IsEnabled = true;
                IsUse = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendInnerSeria,
                    $"Для присвоения серии элементу {ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER.GetDescription()} необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER.GetDescription()} с углом поворота {BendType.GetDescription()}\"");
        }
        public void SetBendInnerCoverSeria(DbScsGcCoverUnit gutterFitting) {
            if (gutterFitting.CoverType == ScsGcCoverType.VERTICAL_BEND_INNER) {
                BendInnerCoverSeria = gutterFitting.Series;
                IsEnabled = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendInnerCoverSeria,
                    $"Для присвоения серии крышки элементу {ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER.GetDescription()} необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.VERTICAL_BEND_INNER.GetDescription()}\"");
        }

        /// <summary>
        /// Тип комплектации
        /// </summary>
        public ScsVerticalBendTypeEnum BendType { get; set; }
        public DbGcVBendInner(ScsVerticalBendTypeEnum bendType) {
            BendType = bendType;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(BendInnerCoverSeria)) {
                writer.WriteElementString(nameof(BendInnerCoverSeria), BendInnerCoverSeria);
            }
            if (!string.IsNullOrEmpty(BendInnerSeria)) {
                writer.WriteElementString(nameof(BendInnerSeria), BendInnerSeria);
            }
            writer.WriteElementString(nameof(BendType), BendType.ToString());
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
    /// <summary>
    /// Секция угловая вертикальная внешняя
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcVBendOuter))]
    public class DbGcVBendOuter : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? BendOuterCoverSeria { get; private set; }
        public string? BendOuterSeria { get; private set; }
        public void SetBendOuterSeria(ScsGcFitting gutterFitting) {
            if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER
                  && gutterFitting.VerticalBendType == this.BendType) {
                BendOuterSeria = gutterFitting.Series;
                IsEnabled = true;
                IsUse = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendOuterSeria,
                    $"Для присвоения серии элементу {ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER.GetDescription()} необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER.GetDescription()} с углом поворота {BendType.GetDescription()}\"");
        }
        public void SetBendOuterCoverSeria(DbScsGcCoverUnit gutterFitting) {
            if (gutterFitting.CoverType == ScsGcCoverType.VERTICAL_BEND_OUTER) {
                BendOuterCoverSeria = gutterFitting.Series;
                IsEnabled = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendOuterCoverSeria,
                    $"Для присвоения серии крышки элементу {ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER.GetDescription()} необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.VERTICAL_BEND_OUTER.GetDescription()}\"");
        }
        /// <summary>
        /// Тип комплектации
        /// </summary>
        public ScsVerticalBendTypeEnum BendType { get; private set; }
        public DbGcVBendOuter(ScsVerticalBendTypeEnum bendType) {
            BendType = bendType;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(BendOuterCoverSeria)) {
                writer.WriteElementString(nameof(BendOuterCoverSeria), BendOuterCoverSeria);

            }
            if (!string.IsNullOrEmpty(BendOuterSeria)) {
                writer.WriteElementString(nameof(BendOuterSeria), BendOuterSeria);
            }
            writer.WriteElementString(nameof(BendType), BendType.ToString());
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
    /// <summary>
    /// Секция угловая вертикальная универсальная
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcVBendUniversal))]
    public class DbGcVBendUniversal : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? BendSeria { get; private set; }
        public void SetBendOuterSeria(ScsGcFitting gutterFitting) {
            if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.VERTICAL_BENT_UNIVERSE
                  && gutterFitting.VerticalUniversalBendType == ScsVerticalUniversalBendTypeEnum.ANGLE_90) {
                BendSeria = gutterFitting.Series;
                IsEnabled = true;
                IsUse = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(BendSeria,
                    $"Для присвоения серии необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.VERTICAL_BENT_UNIVERSE.GetDescription()} с углом поворота {ScsVerticalUniversalBendTypeEnum.ANGLE_90.GetDescription()}\"");
        }
        public DbGcVBendUniversal() {
            //BendType = ScsVerticalBendTypeEnum.ANGLE_90;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(BendSeria)) {
                writer.WriteElementString(nameof(BendSeria), BendSeria);

            }
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
    /// <summary>
    /// Секция Т-образная горизонтальная
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcTriple))]
    public class DbGcTriple : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? TripleCoverSeria { get; private set; }
        public string? TripleSeria { get; private set; }
        public void SetTripleSeria(ScsGcFitting gutterFitting) {
            if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.TRIPLE) {
                TripleSeria = gutterFitting.Series;
                IsEnabled = true;
                IsUse = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(TripleSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.TRIPLE.GetDescription()}\"");
        }
        public void SetTripleCoverSeria(DbScsGcCoverUnit gutterFitting) {
            if (gutterFitting.CoverType == ScsGcCoverType.TRIPLE) {
                TripleCoverSeria = gutterFitting.Series;
                IsEnabled = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(TripleCoverSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.TRIPLE.GetDescription()}\"");
        }

        private int tripleType = 0;
        /// <summary>
        /// Тип секции
        /// </summary>
        /// <remarks>0=>Секция Т-образная горизонтальная со сменой ширины лотка,
        /// 1=>Секция Т-образная горизонтальная без смены ширины лотка.</remarks>
        public int TripleType {
            get { return tripleType; }
            private set {
                if (value == 0 | value == 1) {
                    tripleType = value;
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(TripleType), "Полю TripleType можно присваивать только значения \"0\" или \"1\".");
            }
        }
        public DbGcTriple(int tripleType) {
            TripleType = tripleType;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(TripleCoverSeria)) {
                writer.WriteElementString(nameof(TripleCoverSeria), TripleCoverSeria);
            }
            if (!string.IsNullOrEmpty(TripleSeria)) {
                writer.WriteElementString(nameof(TripleSeria), TripleSeria);
            }
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
            writer.WriteElementString(nameof(TripleType), TripleType.ToString());
        }
    }
    /// <summary>
    /// Секция Х-образная горизонтальная
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcCross))]
    public class DbGcCross : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? CrossCoverSeria { get; private set; }
        public string? CrossSeria { get; private set; }
        public void SetCrossSeria(ScsGcFitting gutterFitting) {
            if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.CROSS) {
                CrossSeria = gutterFitting.Series;
                IsEnabled = true;
                IsUse = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(CrossSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.CROSS.GetDescription()}\"");
        }
        public void SetCrossCoverSeria(DbScsGcCoverUnit gutterFitting) {
            if (gutterFitting.CoverType == ScsGcCoverType.CROSS) {
                CrossCoverSeria = gutterFitting.Series;
                IsEnabled = true;
                return;
            }
            else
                throw new ArgumentOutOfRangeException(CrossCoverSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.CROSS.GetDescription()}\"");
        }

        private int tripleType = 0; // 0 - без смены ширины лотка; 1- со сменой ширины лотка
        /// <summary>
        /// Тип секции
        /// </summary>
        public int CrossType {
            get { return tripleType; }
            private set {
                if (value == 0 | value == 1) {
                    tripleType = value;
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(CrossType), "Полю CrossType можно присваивать только значения \"0\" или \"1\".");
            }
        }
        public DbGcCross(int crossType) {
            CrossType = crossType;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(CrossCoverSeria)) {
                writer.WriteElementString(nameof(CrossCoverSeria), CrossCoverSeria);
            }
            if (!string.IsNullOrEmpty(CrossSeria)) {
                writer.WriteElementString(nameof(CrossSeria), CrossSeria);
            }
            writer.WriteElementString(nameof(CrossType), CrossType.ToString());
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
    /// <summary>
    /// Секция соединительная переходная
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcMsPassage))]
    public class DbGcMsPassage : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        public string? PassageCoverSeria { get; private set; }
        public string? PassageSeria { get; private set; }
        public void SetPassageSeria(ScsGcFitting gutterFitting) {
            if (IsVertical) {
                if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.VERTICAL_PASSAGE) {
                    PassageSeria = gutterFitting.Series;
                    IsEnabled = true;
                    IsUse = true;
                    return;
                }
                else
                    throw new ArgumentOutOfRangeException(PassageSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.VERTICAL_PASSAGE.GetDescription()}\"");

            }
            else {
                if (gutterFitting.FittingType == ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE) {
                    PassageSeria = gutterFitting.Series;
                    IsEnabled = true;
                    IsUse = true;
                    return;
                }
                else
                    throw new ArgumentOutOfRangeException(PassageSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Лотки\\{ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE.GetDescription()}\"");
            }
        }
        public void SetPassageCoverSeria(DbScsGcCoverUnit gutterFitting) {
            if (IsVertical) {
                if (gutterFitting.CoverType == ScsGcCoverType.VERTICAL_PASSAGE) {
                    PassageCoverSeria = gutterFitting.Series;
                    IsEnabled = true;
                    return;
                }
                else
                    throw new ArgumentOutOfRangeException(PassageCoverSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.VERTICAL_PASSAGE.GetDescription()}\"");

            }
            else {
                if (gutterFitting.CoverType == ScsGcCoverType.HORIZONTAL_PASSAGE) {
                    PassageCoverSeria = gutterFitting.Series;
                    IsEnabled = true;
                    return;
                }
                else
                    throw new ArgumentOutOfRangeException(PassageCoverSeria, $"Для присвоения серии необходимо передать экземпляр типа \"Крышки\\{ScsGcCoverType.HORIZONTAL_PASSAGE.GetDescription()}\"");
            }
        }
        private int tripleType = 0;
        /// <summary>
        /// Тип секции
        /// </summary>
        public int PassageType { get; private set; }
        public bool IsVertical { get => PassageType == 1; }
        public DbGcMsPassage(bool isVertical) {
            if (isVertical) {
                PassageType = 1;
            }
            else {
                PassageType = 0;
            }
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            if (!string.IsNullOrEmpty(PassageCoverSeria)) {
                writer.WriteElementString(nameof(PassageCoverSeria), PassageCoverSeria);
            }
            if (!string.IsNullOrEmpty(PassageSeria)) {
                writer.WriteElementString(nameof(PassageSeria), PassageSeria);
            }
            writer.WriteElementString(nameof(PassageType), PassageType.ToString());
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
    /// <summary>
    /// Соединитель на стык
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcMsJoint))]
    public class DbGcMsJoint : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        //public string? PassageCoverSeria { get; set; }
        public string? JointSeria { get; private set; }
        public void SetJointSeria(ScsGcFitting scsGcFitting) {
            if (scsGcFitting.FittingType == ScsGutterFittingTypeEnum.JOINT) {
                JointSeria = scsGcFitting.Series;
                IsEnabled = true;
                IsUse = true;
            }
            else
                throw new ArgumentOutOfRangeException(JointSeria, $"Для присвоения серии необходимо передать экземпляр типа \"{ScsGutterFittingTypeEnum.JOINT.GetDescription()}\"");
        }
        /// <summary>
        /// Коэффициент запаса
        /// </summary>
        public double Coef {
            get;
            set;
        }
        public DbGcMsJoint() {
            Coef = 1.0;
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            //writer.WriteElementString(nameof(PassageCoverSeria), PassageCoverSeria);
            if (!string.IsNullOrEmpty(JointSeria)) {
                writer.WriteElementString(nameof(JointSeria), JointSeria);
            }
            writer.WriteElementString(nameof(Coef), Coef.ToString("0.#"));
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
    /// <summary>
    /// Торцевая заглушка
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcMsCork))]
    public class DbGcMsCork : KitElement, IXmlSerializable {
        /// <summary>
        /// Наличие данного типа соединения
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        public bool IsUse { get; set; }
        //public string? PassageCoverSeria { get; set; }
        public string? CorkSeria { get; private set; }
        public void SetCorkSeria(ScsGcFitting scsGcFitting) {
            if (scsGcFitting.FittingType == ScsGutterFittingTypeEnum.CORK) {
                CorkSeria = scsGcFitting.Series;
                IsEnabled = true;
                IsUse = true;
            }
            else
                throw new ArgumentOutOfRangeException(CorkSeria, $"Для присвоения серии необходимо передать экземпляр типа \"{ScsGutterFittingTypeEnum.CORK.GetDescription()}\"");
        }

        public DbGcMsCork() {
            IsEnabled = false;
            IsUse = false;
        }

        public IEnumerable<DbGcMsAcceesory> Accessories {
            get {
                return base.GetChildren<DbGcMsAcceesory>();
            }
        }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
            IsEnabled = true;
        }
        public void AddChild(DbGcMsAcceesory unit) {
            Children.Add(unit);
            IsEnabled = true;
        }

        protected override void WriteProperties(XmlWriter writer) {
            //writer.WriteElementString(nameof(PassageCoverSeria), PassageCoverSeria);
            if (!string.IsNullOrEmpty(CorkSeria)) {
                writer.WriteElementString(nameof(CorkSeria), CorkSeria);
            }
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
}
