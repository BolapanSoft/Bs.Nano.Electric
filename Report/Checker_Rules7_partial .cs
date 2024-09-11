using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Полный перечень разделов:
// Правила раздела 1 Трансформаторы, реакторы и УКРМ.
// Правила раздела 2 Распределительные устройства.
// Правила раздела 3 Коммутационные аппараты.
// Правила раздела 4 Приборы контроля и учета.
// Правила раздела 5 Электроприемники.
// Правила раздела 6 Кабельно-проводниковая продукция.
// Правила раздела 7 Кабеленесущие системы.
// Правила раздела 8 Комплектации.
// Правила раздела 9 Параметры исполнения.
// Правила раздела 10 Материалы и комплектации.

namespace Bs.Nano.Electric.Report {
    public partial class Checker {
        // Правила раздела 7 Кабеленесущие системы.
        [ReportRule(@"",
                    7, 0)]
        [RuleCategory(".", "")]
        public void Rule_07_00() {
            using (var context = connector.Connect()) {
                throw new NotImplementedException();
            }

        }


        [ReportRule(@"Для прямых секций лотков должны быть внесены длина, ширина, высота лотка",
            7, 1), RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_07_001() {
            using (var context = connector.Connect()) {
                var scsGutterCanals = context.ScsGutterCanals
                    .Select(p => new { p.Code, p.Series, p.GutterDepth, p.GutterHeight, p.SegLength })
                    .ToList();
                var errors = scsGutterCanals
                    .Where(p => !(p.SegLength > 1 & p.GutterDepth > 1 & p.GutterHeight > 1))
                    .Select(p => $"({p.Series}\\{p.Code} {nameof(p.GutterDepth)}:{p.GutterDepth},{nameof(p.GutterHeight)}:{p.GutterDepth},{nameof(p.SegLength)}:{p.SegLength}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов прямых секций лотков не внесена длина, ширина или высота лотка.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для перегородок должны быть внесены длина, высота",
            7, 2)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterPartition))]
        public void Rule_07_002() {
            using (var context = connector.Connect()) {
                var products = context.DbScsGutterPartitions.ToList();
                var errors = products.Where(p => !(p.PartitionHeight > 1 & p.PartitionLength > 1))
                    .Select(p => $"({p.Series}\\{p.Code} {nameof(p.PartitionHeight)}:{p.PartitionHeight}, {nameof(p.PartitionLength)}:{p.PartitionLength}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} перегородок не внесена длина или высота лотка.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для крышек прямых секций должны быть внесены ширина, высота",
            7, 3)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterCover))]
        public void Rule_07_003() {
            using (var context = connector.Connect()) {
                var products = context.DbScsGutterCovers
                    .ToList();
                var errors = products.Where(p => !(p.CoverWidth > 1 & p.CoverLength > 1))
                    .Select(p => $"({p.Series}\\{p.Code} {nameof(p.CoverWidth)}:{p.CoverWidth}, {nameof(p.CoverLength)}:{p.CoverLength}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов крышек прямых секций лотков не внесена длина или ширина.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция соединительная переходная вертикальная"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.",
            7, 4)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_004() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.WidthMainBranch)}:{p.WidthMainBranch}, {nameof(p.HeightMainBranch)}:{p.HeightMainBranch}, {nameof(p.WidthOutBranch)}:{p.WidthOutBranch}, {nameof(p.HeightOutBranch)}:{p.HeightOutBranch}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов Секция соединительная переходная вертикальная не полностью внесены геометрические размеры.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция соединительная переходная вертикальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule(@"Внутри серии элементов \""Секция соединительная переходная вертикальная\"" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            7, 5)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_005() {

            var ft = ScsGutterFittingTypeEnum.VERTICAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов \""Секция соединительная переходная горизонтальная\"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.",
            7, 6)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_006() {
            var ft = ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1))
                   .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.WidthMainBranch)}:{p.WidthMainBranch}, {nameof(p.HeightMainBranch)}:{p.HeightMainBranch}, {nameof(p.WidthOutBranch)}:{p.WidthOutBranch}, {nameof(p.HeightOutBranch)}:{p.HeightOutBranch}")
                   .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция соединительная переходная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule(@"Внутри серии элементов \""Секция соединительная переходная горизонтальная\"" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            7, 7)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_007() {
            var ft = ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }
        [ReportRule(@"Для элементов ""Секция соединительная переходная горизонтальная"" должен быть установлен тип перехода GutterPassageType",
            7, 8)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_008() {

            var ft = ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .Select(p => new { p.Code, p.Series, p.GutterPassageType });
                LinkedList<string> errors = new();
                foreach (var p in products) {
                    if (p.GutterPassageType.HasValue) {
                        var knownEnumValues = typeof(ScsGutterPassageType).GetEnumValues() as ScsGutterPassageType[];
                        if (knownEnumValues.Contains(p.GutterPassageType.Value))
                            continue;
                    }
                    else
                        errors.AddLast($"({p.Series}\\{p.Code} {nameof(p.GutterPassageType)}:{p.GutterPassageType})");
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция Т-образная горизонтальная"" должны быть внесены [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            7, 9)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_009() {
            var ft = ScsGutterFittingTypeEnum.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1))
                   .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.WidthMainBranch)}:{p.WidthMainBranch}, {nameof(p.HeightMainBranch)}:{p.HeightMainBranch}, {nameof(p.WidthOutBranch)}:{p.WidthOutBranch}, {nameof(p.HeightOutBranch)}:{p.HeightOutBranch}")
                   .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция Т-образная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция Т-образная горизонтальная"" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            7, 10)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_010() {
            var ft = ScsGutterFittingTypeEnum.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция угловая вертикальная внешняя"" должны быть внесены высота борта, ширина элемента и угол поворота.",
            7, 11)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_011() {

            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var knownEnumValues = typeof(ScsVerticalBendTypeEnum).GetEnumValues() as ScsVerticalBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.VerticalBendType.HasValue &&
                        knownEnumValues.Contains(p.VerticalBendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.VerticalBendType)}:{p.VerticalBendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая вертикальная внешняя\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая вертикальная внешняя"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота VerticalBendType].",
         7, 12)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_012() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.VerticalBendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.Height, p.Depth, p.VerticalBendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция угловая вертикальная внутренняя"" должны быть внесены высота борта Height, ширина элемента Depth и угол поворота VerticalBendType.",
            7, 13)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_013() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var knownEnumValues = typeof(ScsVerticalBendTypeEnum).GetEnumValues() as ScsVerticalBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.VerticalBendType.HasValue &&
                        knownEnumValues.Contains(p.VerticalBendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.VerticalBendType)}:{p.VerticalBendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая вертикальная внутренняя\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая вертикальная внутренняя"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота VerticalBendType].",
         7, 14)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_014() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.VerticalBendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.Height, p.Depth, p.VerticalBendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция угловая вертикальная универсальная"" должны быть внесены высота борта Height, ширина элемента Depth и угол поворота VerticalUniversalBendType.",
         7, 15)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_015() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BENT_UNIVERSE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var knownEnumValues = typeof(ScsVerticalUniversalBendTypeEnum).GetEnumValues() as ScsVerticalUniversalBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.VerticalUniversalBendType.HasValue &&
                        knownEnumValues.Contains(p.VerticalUniversalBendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.VerticalUniversalBendType)}:{p.VerticalUniversalBendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая вертикальная универсальная\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая вертикальная универсальная"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота VerticalUniversalBendType].",
         7, 16)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_016() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BENT_UNIVERSE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.VerticalUniversalBendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.Height, p.Depth, p.VerticalUniversalBendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsVerticalUniversalBendTypeEnum? VerticalUniversalBendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }
        [ReportRule(@"Для элементов ""Секция угловая горизонтальная"" должны быть внесены высота борта Height, ширина элемента Depth и угол поворота BendType.",
            7, 17)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_017() {
            var ft = ScsGutterFittingTypeEnum.BEND;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var knownEnumValues = typeof(ScsBendTypeEnum).GetEnumValues() as ScsBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.BendType.HasValue &&
                        knownEnumValues.Contains(p.BendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.BendType)}:{p.BendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая горизонтальная\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая горизонтальная"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота BendType].",
         7, 18)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_018() {
            var ft = ScsGutterFittingTypeEnum.BEND;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.BendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.Height, p.Depth, p.BendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsBendTypeEnum? BendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule013 = @"Для элементов ""Секция Х-образная горизонтальная"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.";
        [ReportRule(@"Для элементов ""Секция Х-образная горизонтальная"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.",
         7, 19)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_019() {
            Console.Write($"{GetIndex(nameof(Rule_07_013))}\t{sRule013}");

            var ft = ScsGutterFittingTypeEnum.CROSS;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.Width1Branch > 1 & p.Height1Branch > 1 & p.Width2Branch > 1 & p.Height2Branch > 1))
                      .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Width1Branch)}:{p.Width1Branch}, {nameof(p.Height1Branch)}:{p.Height1Branch}, {nameof(p.Width2Branch)}:{p.Width2Branch}, {nameof(p.Height2Branch)}:{p.Height2Branch}")
                      .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция Х-образная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Секция Х-образная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (Height1Branch, Height2Branch), ширина основного и отходящего элемента (Width1Branch, Width2Branch)].",
         7, 20)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_020() {
            var ft = ScsGutterFittingTypeEnum.CROSS;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Width1Branch, p.Height1Branch, p.Width2Branch, p.Height2Branch })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.Width1Branch, p.Height1Branch, p.Width2Branch, p.Height2Branch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Width1Branch, double? Height1Branch, double? Width2Branch, double? Height2Branch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }

        [ReportRule("Внутри серии элементов \"Крышки\\Секция Х-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента (CoverWidth1, CoverWidth2)].",
         7, 21)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_021() {

            var ft = ScsGcCoverType.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .Select(p => new { p.Series, p.Name, p.Code, p.CoverWidth1, p.CoverWidth2 })
                    .ToArray()
                    .GroupBy(p => p.Series, p => (p.Name, size: (p.CoverWidth1, p.CoverWidth2)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка Т-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышки\\Секция Т-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента (CoverWidth1, CoverWidth2)].",
        7, 22)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_022() {
            var ft = ScsGcCoverType.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth1, p.CoverWidth2 })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.CoverWidth1, p.CoverWidth2)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        //public const string sRule015 = @"Для элементов ""Крышка угловая вертикальная внешняя"" должна быть внесена ширина элемента.";
        [ReportRule(@"Для элементов ""Крышка угловая вертикальная внешняя"" должна быть внесена ширина элемента.",
         7, 23)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_023() {
            var ft = ScsGcCoverType.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.CoverWidth > 1))
                   .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.CoverWidth)}:{p.CoverWidth}")
                   .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка угловая вертикальная внешняя\" должно быть не более одного сочетания параметров [ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка угловая вертикальная внешняя\" должно быть не более одного сочетания параметров [ширина элемента CoverWidth].",
         7, 24)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_024() {
            var ft = ScsGcCoverType.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, p.CoverWidth));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<double?> values = new();
                    var errorValues = p.Where(item => !values.Add(item.CoverWidth));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule016 = @"Для элементов ""Крышка угловая горизонтальная"" должна быть внесена ширина элемента";
        [ReportRule(@"Для элементов ""Крышка угловая горизонтальная"" должна быть внесена ширина элемента.",
         7, 25)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_025() {
            var ft = ScsGcCoverType.BEND;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .Select(p => new { p.Code, p.CoverWidth });
                var errors = new LinkedList<(string Code, double?)>();
                foreach (var p in products) {
                    if (p.CoverWidth > 1)
                        continue;
                    else
                        errors.AddLast((p.Code, p.CoverWidth));
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка угловая горизонтальная\" должно быть не более одного сочетания параметров [ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка угловая горизонтальная\" должно быть не более одного сочетания параметров [ширина элемента CoverWidth].",
         7, 26)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_026() {
            var ft = ScsGcCoverType.BEND;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, p.CoverWidth));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<double?> values = new();
                    var errorValues = p.Where(item => !values.Add(item.CoverWidth));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule017 = @"Для элементов ""Крышка Х-образная горизонтальная"" должны быть внесены ширина основного и отходящего элемента (CoverWidth1, CoverWidth2).";
        [ReportRule(@"Для элементов ""Крышка Х-образная горизонтальная"" должны быть внесены ширина основного и отходящего элемента.",
         7, 27)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_027() {
            var ft = ScsGcCoverType.CROSS;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.CoverWidth1 > 1 & p.CoverWidth2 > 1))
                     .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.CoverWidth1)}:{p.CoverWidth1}, {nameof(p.CoverWidth2)}:{p.CoverWidth2}")
                     .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка Х-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка Х - образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента (CoverWidth1, CoverWidth2)].",
         7, 28)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_028() {
            var ft = ScsGcCoverType.CROSS;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth1, p.CoverWidth2 })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.CoverWidth1, p.CoverWidth2)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule018 = @"Для элементов ""Крышка угловая вертикальная внутренняя"" должна быть внесена ширина элемента.";
        [ReportRule(@"Для элементов ""Крышка угловая вертикальная внутренняя"" должна быть внесена ширина элемента.",
         7, 29)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_029() {
            var ft = ScsGcCoverType.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.CoverWidth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.CoverWidth)}:{p.CoverWidth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка угловая вертикальная внутренняя\" должно быть не более одного сочетания параметров [ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка угловая вертикальная внутренняя\" должно быть не более одного сочетания параметров [ширина элемента CoverWidth].",
         7, 30)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_07_030() {
            var ft = ScsGcCoverType.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, p.CoverWidth));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<double?> values = new();
                    var errorValues = p.Where(item => !values.Add(item.CoverWidth));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для прямых секций лотков должны быть внесены полезная ширина GutterUsefullHeight, высота GutterUsefullDepth лотка.",
         7, 31)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_07_031() {
            using (var context = connector.Connect()) {
                var products = context.ScsGutterCanals
                    .ToList();
                var errors = products.Where(p => !(p.GutterUsefullHeight > 1 & p.GutterUsefullDepth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.GutterUsefullHeight)}:{p.GutterUsefullHeight}, {nameof(p.GutterUsefullDepth)}:{p.GutterUsefullDepth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        public const string sRule021 = @"Для прямых секций лотков должен быть внесен график допустимой нагрузки.";
        [ReportRule(@"Для прямых секций лотков должен быть внесен график допустимой нагрузки.",
         7, 32)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_07_032() {
            using (var context = connector.Connect()) {
                var scsGutterCanals = context.ScsGutterCanals
                    .Select(p => new { p.Series, p.Code, p.GraphLoadingPrp });
                var errors = new LinkedList<(string, string)>();
                foreach (var p in scsGutterCanals) {
                    if (string.IsNullOrEmpty(p.GraphLoadingPrp))
                        errors.AddLast((p.Series, p.Code));
                    else
                        continue;
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Для всех лотков должен быть указан тип лотка и серия
        /// </summary>
        /// <remarks>Учитываются свойства GutterType, Series</remarks>
        [ReportRule(@"Для всех лотков должен быть указан тип лотка GutterType и серия Series.",
        7, 33)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_07_033() {
            Dictionary<(int, string), (double, double, double)> knownSeries = new Dictionary<(int, string), (double, double, double)>();

            string? CheckRule(string code, string name, int? GutterType, string? Series) {

                bool isMatch = !(GutterType.HasValue & !string.IsNullOrWhiteSpace(Series));
                return isMatch ? code : null;
            };

            using (var context = connector.Connect()) {
                var errors = new LinkedList<string>();
                foreach (var p in context.ScsGutterCanals.Select(p => new { p.Code, p.Name, p.GutterType, p.Series })) {
                    var key = CheckRule(p.Code, p.Name, (int)p.GutterType, p.Series);
                    if (key != null) {
                        errors.AddLast(key);
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии лотка должно быть не более одного сочетания параметров [Высота борта]x[ширина]x[длина секции]
        /// </summary>
        /// <remarks>Учитываются свойства GutterType, Series, GutterDepth, GutterHeight, SegLength</remarks>
        [ReportRule(@"Внутри серии лотка должно быть не более одного сочетания параметров [Высота борта GutterHeight]x[ширина GutterDepth]x[длина секции SegLength].",
          7, 34)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_07_034() {
            using (var context = connector.Connect()) {
                var products = context.ScsGutterCanals
                     .Select(p => new { p.GutterType, p.Series, p.Name, p.GutterDepth, p.GutterHeight, p.SegLength })
                     .ToArray()
                     .GroupBy(p => (p.GutterType.GetDescription(), p.Series), p => (p.Name, size: (p.GutterDepth, p.GutterHeight, p.SegLength)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? GutterDepth, double? GutterHeight, double? SegLength)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }

        /// <summary>
        /// Внутри серии разделительных перегородок должно быть не более одного сочетания параметров [Высота]x[длина секции]
        /// </summary>
        /// <remarks>Учитываются свойства Series, PartitionHeight, PartitionLength</remarks>
        [ReportRule(@"Внутри серии разделительных перегородок должно быть не более одного сочетания параметров [Высота PartitionHeight]x[длина секции PartitionLength].",
         7, 35)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterPartition))]
        public void Rule_07_035() {
            using (var context = connector.Connect()) {
                var products = context.DbScsGutterPartitions
                    .Select(p => new { p.Series, p.Name, p.PartitionHeight, p.PartitionLength })
                    .ToArray()
                    .GroupBy(p => p.Series, p => (p.Name, size: (p.PartitionHeight, p.PartitionLength)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? PartitionHeight, double? PartitionLength)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }
        /// <summary>
        /// Внутри серии крышек прямых секций лотков должно быть не более одного сочетания параметров [Ширина]x[длина секции]
        /// </summary>
        /// <remarks>Учитываются свойства Series, CoverWidth, CoverLength</remarks>
        [ReportRule(@"Внутри серии крышек прямых секций лотков должно быть не более одного сочетания параметров [Ширина CoverWidth]x[длина секции CoverLength].",
          7, 36)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterCover))]
        public void Rule_07_036() {
            using (var context = connector.Connect()) {
                var products = context.DbScsGutterCovers
                    .Select(p => new { p.Series, p.Name, p.CoverWidth, p.CoverLength })
                    .ToArray()
                    .GroupBy(p => p.Series, p => (p.Name, size: (p.CoverWidth, p.CoverLength)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double?, double?)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Для элементов  \"Торцевая заглушка\" должны быть внесены параметры [высота борта, ширина элемента]
        /// </summary>
        [ReportRule("Для элементов  \"Торцевая заглушка\" должны быть внесены параметры [высота борта Height, ширина элемента Depth].",
         7, 37)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_037() {
            bool CheckRule(double? Height, double? Depth) {

                bool isAllRight = Height.HasValue && Height.Value >= 1.0
                                    && Depth.HasValue && Depth.Value >= 1.0;
                return isAllRight;

            };
            var ft = ScsGutterFittingTypeEnum.CORK;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft).ToArray()
                    .ToList();
                var errors = products.Where(p => !CheckRule(p.Height, p.Depth))
                  .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}")
                  .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов  \"Торцевая заглушка\" должно быть не более одного сочетания параметров [высота борта, ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов  \"Торцевая заглушка\" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth].",
         7, 38)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_07_038() {
            var ft = ScsGutterFittingTypeEnum.CORK;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth })
                        .ToArray()
                        .GroupBy(p => p.Series, p => (p.Name, size: (p.Height, p.Depth)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double?, double?)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }

        [ReportRule(@"Для элементов ""Консоль"" должна быть внесена полезная ширина элемента Length.",
         7, 39)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_039() {

            var ft = ScsGutterBoltingTypeEnum.CONSOLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.Length > 1))
                 .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Length)}:{p.Length}")
                 .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Профиль"" должна быть внесена длина элемента ProfileLength.",
         7, 40)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_040() {
            var ft = ScsGutterBoltingTypeEnum.PROFILE;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.ProfileLength > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.ProfileLength)}:{p.ProfileLength}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Скоба"" должна быть внесена полезная ширина элемента Length.",
         7, 41)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_041() {
            var ft = ScsGutterBoltingTypeEnum.CRAMP;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.Length > 1))
                  .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Length)}:{p.Length}")
                  .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Стойка"" должны быть внесены крепление MountType, вид стойки StandType, высота элемента Heigth.",
           7, 42)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_042() {
            var ft = ScsGutterBoltingTypeEnum.POST;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .Select(p => new { p.Code, p.Series, p.Name, p.MountType, p.StandType, p.Heigth });
                var errors = new LinkedList<string>();
                foreach (var p in products) {
                    if (p.Heigth > 1) {
                        if (p.MountType.HasValue) {
                            var knownEnumValues = typeof(ScsGcStandMountType).GetEnumValues() as ScsGcStandMountType[];
                            if (knownEnumValues.Contains(p.MountType.Value)) {
                                if (p.StandType.HasValue) {
                                    var knownStandTypeEnumValues = typeof(ScsGcStandType).GetEnumValues() as ScsGcStandType[];
                                    if (knownStandTypeEnumValues.Contains(p.StandType.Value)) {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    errors.AddLast($"({p.Series}\\{p.Name}  {nameof(p.MountType)}:{p.MountType?.GetDescription()}, {nameof(p.StandType)}:{p.StandType?.GetDescription()}, {nameof(p.Heigth)}:{p.Heigth}");
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Шпилька"" должна быть внесена высота элемента Heigth.",
         7, 43)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_043() {
            var ft = ScsGutterBoltingTypeEnum.STUD;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.Heigth > 1))
                  .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Heigth)}:{p.Heigth}")
                  .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""L- подвес или C- подвес"" должны быть внесена высота элемента.",
         7, 44)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_044() {
            var ft = ScsGutterBoltingTypeEnum.CONSOLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft & (p.ConsoleMountType == ScsGcConsoleMountType.CELL | p.ConsoleMountType == ScsGcConsoleMountType.L_WALL))
                    .ToList();
                var errors = products.Where(p => !(p.Heigth > 1))
                  .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Heigth)}:{p.Heigth}")
                  .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Перекладина"" должна быть внесена полезная ширина элемента.",
         7, 45)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_07_045() {
            var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                var errors = products.Where(p => !(p.Length > 1))
                  .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Length)}:{p.Length}")
                  .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        //[ReportRule(@"Для элементов ""Аксессуары лотков"" должно быть внесено значение AccessoryType (Тип элемента).",
        // 7, 47)]
        //[RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcAccessoryUnit))]
        //public void Rule_07_047() {
        //    using (var context = connector.Connect()) {
        //        var products = context.ScsTubeFittings
        //            .Where(p => p.FittingType == ScsTubeFittingTypeEnum.OTHER)
        //            .ToList();
        //        var errors = products.Where(p => !string.IsNullOrEmpty(p.DbOtherName))
        //        .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.DbOtherName)}:\"{p.DbOtherName}\"")
        //        .ToList();
        //        if (errors.Count > 0) {

        //            FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
        //               errors);
        //        }
        //    }
        //}
        [ReportRule(@"Для элементов ""Трубы"" должен быть внесен тип трубы.",
         7, 401)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsPipe))]
        public void Rule_07_401() {
            using (var context = connector.Connect()) {
                var products = context.ScsPipes
                     .Where(p => p.TubeType == null)
                     .ToList();
                var errors = products
                 .Select(p => (p.Series, p.Code))
                 .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы"" должны быть внесен габариты: Внутренний диаметр InternalDiameter и Внешний диаметр ExternalDiameter.",
         7, 402)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsPipe))]
        public void Rule_07_402() {
            using (var context = connector.Connect()) {
                var products = context.ScsPipes
                     .Select(p => new { p.Code, p.Series, p.InternalDiameter, p.ExternalDiameter })
                     .ToList();
                var errors = products
                 .Where(p => p.InternalDiameter == null | p.ExternalDiameter == null || (p.InternalDiameter < 1 | p.ExternalDiameter < 1))
                 .Select(p => (p.Series, p.Code, (p.InternalDiameter, p.ExternalDiameter)))
                 .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        //[ReportRule(@"Для элементов ""Трубы"" внутри серии должно быть одно сочетание габаритов: Внешний диаметр InternalDiameter и Внутренний диаметр ExternalDiameter.",
        // 7, 403)]
        //[RuleCategory("Полнота заполнения технических данных.", nameof(ScsPipe))]
        //public void Rule_07_403() {
        //    using (var context = connector.Connect()) {
        //        var products = context.ScsPipes
        //             .Select(p => new { p.Code, p.Series, p.InternalDiameter, p.ExternalDiameter })
        //             .ToList();
        //        var errors = products
        //            .GroupBy(p => p.Series)
        //            .SelectMany(s => s.GroupBy(gr => (gr.ExternalDiameter, gr.InternalDiameter))
        //                              .Where(gr => gr.Count() > 1))
        //            .ToList();
        //        if (errors.Count > 0) {
        //            FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
        //               errors.SelectMany(gr => (gr.Select(el => (el.Code, el.Series, gr.Key)))));
        //        }
        //    }
        //}
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" должен быть внесен Тип элемента FittingType.",
         7, 410)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_410() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == null)
                    .ToList();
                var errors = products
                   .Select(p => (p.Series, p.Code))
                   .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Другой"" должно быть внесено значение  Тип элемента DbOtherName.",
         7, 411)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_411() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.OTHER & p.DbOtherName == null)
                    .ToList();
                var errors = products
                  .Select(p => (p.Series, p.Code))
                  .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Угол"" должно быть внесено значение Диаметр Diameter.",
         7, 412)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_412() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.ANGLE & (p.Diameter == null | p.Diameter <= 0))
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, p.Diameter))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Т-переход"" должны быть внесены значения Диаметр основной ветки DiameterMainBranch и Диаметр отходящей ветки DiameterOutBranch.",
         7, 413)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_413() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.TRIPLE & (p.DiameterMainBranch == null | p.DiameterMainBranch <= 0 | p.DiameterOutBranch == null | p.DiameterOutBranch <= 0))
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, (p.DiameterMainBranch, p.DiameterOutBranch)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Х-переход"" должны быть внесены значения Диаметр первой ветки Diameter1Branch и Диаметр второй ветки Diameter2Branch.",
         7, 414)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_414() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.CROSS & (p.Diameter1Branch == null | p.Diameter1Branch <= 0 | p.Diameter2Branch == null | p.Diameter2Branch <= 0))
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, (p.Diameter1Branch, p.Diameter2Branch)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Угол"" внутри серии значения параметра Диаметр Diameter не должны повторяться.",
         7, 415)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_415() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.ANGLE)
                    .Select(p => new { p.Series, p.Code, p.Diameter })
                    .ToList();
                var errors = products
                    .GroupBy(p => p.Series)
                    .SelectMany(s => s.GroupBy(gr => gr.Diameter)
                                      .Where(gr => gr.Count() > 1))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors.SelectMany(gr => (gr.Select(el => (el.Code, el.Series, gr.Key)))));
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Т-переход"" внутри серии должно быть не более одного сочетания параметров Диаметр основной ветки DiameterMainBranch и Диаметр отходящей ветки DiameterOutBranch.",
         7, 416)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_416() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.TRIPLE)
                    .Select(p => new { p.Code, p.Series, p.DiameterMainBranch, p.DiameterOutBranch })
                    .ToList();
                var errors = products
                    .GroupBy(p => p.Series)
                    .SelectMany(s => s.GroupBy(gr => (gr.DiameterMainBranch, gr.DiameterOutBranch))
                                      .Where(gr => gr.Count() > 1))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors.SelectMany(gr => (gr.Select(el => (el.Code, el.Series, gr.Key)))));
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Х-переход"" внутри серии должно быть не более одного сочетания параметров Диаметр первой ветки Diameter1Branch и Диаметр второй ветки Diameter2Branch.",
         7, 417)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_07_417() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.CROSS)
                    .Select(p => new { p.Code, p.Series, p.Diameter1Branch, p.Diameter2Branch })
                    .ToList();
                var errors = products
                    .GroupBy(p => p.Series)
                    .SelectMany(s => s.GroupBy(gr => (gr.Diameter1Branch, gr.Diameter2Branch))
                                      .Where(gr => gr.Count() > 1))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors.SelectMany(gr => (gr.Select(el => (el.Code, el.Series, gr.Key)))));
                }
            }
        }
        [ReportRule(@"Для элементов ""Короб"" должны быть внесены значения Длина сегмента,м. SegLength, Высота короба, мм. CabelHeight, Глубина короба, мм. CabelDepth.",
         7, 430)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCabelCanal))]
        public void Rule_07_430() {
            using (var context = connector.Connect()) {
                var products = context.ScsCabelCanals
                    .Select(p => new { p.Code, p.Series, p.SegLength, p.CabelHeight, p.CabelDepth })
                    .ToList();
                var errors = products
                    .Where(p => p.SegLength == null || p.CabelHeight == null || p.CabelDepth == null || p.SegLength <= 0 || p.CabelHeight <= 0 || p.CabelDepth <= 0)
                    .Select(p => (p.Series, p.Code, (p.SegLength, p.CabelHeight, p.CabelDepth)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Угол"", ""Заглушка"", ""Соединение на стык""  должны быть внесены значения Высота [короба], мм. Height, Глубина [короба], мм. Depth.",
        7, 431)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_431() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.PLAIN_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.INSIDE_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.OUTSIDE_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.CORK |
                        p.FittingType == ScsCableFittingTypeEnum.JOINT)
                    .Select(p => new { p.Code, p.Series, p.Height, p.Depth })
                    .ToList();
                var errors = products
                    .Where(p => p.Height == null || p.Depth == null || p.Height <= 0 || p.Depth <= 0)
                    .Select(p => (p.Series, p.Code, (p.Height, p.Depth)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Т-переход"" должны быть внесены параметры Габариты основной ветки (Высота HeightMainBranch, Глубина WidthMainBranch) и Габариты отходящей ветки (Высота HeightOutBranch, Глубина WidthOutBranch).",
        7, 432)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_432() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.TRIPLE)
                    .Select(p => new { p.Code, p.Series, p.HeightMainBranch, p.WidthMainBranch, p.HeightOutBranch, p.WidthOutBranch })
                    .ToList();
                var errors = products
                    .Where(p => p.HeightMainBranch == null || p.WidthMainBranch == null || p.HeightOutBranch == null || p.WidthOutBranch == null || p.HeightMainBranch <= 0 || p.WidthMainBranch <= 0 || p.HeightOutBranch <= 0 || p.WidthOutBranch <= 0)
                    .Select(p => (p.Series, p.Code, (p.HeightMainBranch, p.WidthMainBranch, p.HeightOutBranch, p.WidthOutBranch)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба с типом элемента ""Другой"" должно быть внесено значение  Тип элемента DbOtherName.",
         7, 433)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_433() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.OTHER & p.DbOtherName == null)
                    .Select(p => new { p.Code, p.Series })
                    .ToList();
                var errors = products
                  .Select(p => (p.Series, p.Code))
                  .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба значение параметра ""Серия"" должно совпадать со значением параметра ""Серия"" соответствующих прямых секций короба.",
         7, 434)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_434() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Select(p => p.Series)
                    .ToList();
                var errors = products
                    .Distinct()
                    .Where(s => !(context.ScsCabelCanals.Any(cc => DbFunctions.Like(cc.Series,s))))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Угол"", ""Заглушка"", ""Соединение на стык"" должен быть внесен короб с совпадающими значениями Высота Height, Глубина Depth.",
        7, 435)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_435() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.PLAIN_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.INSIDE_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.OUTSIDE_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.CORK |
                        p.FittingType == ScsCableFittingTypeEnum.JOINT)
                    .Where(p => !(context.ScsCabelCanals.Any(cc => DbFunctions.Like(cc.Series, p.Series) & p.Height == cc.CabelHeight & p.Depth == cc.CabelDepth)))
                    .Select(p => new { p.Code, p.Series, p.FittingType, p.Height, p.Depth })
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, p.FittingType, (p.Height, p.Depth)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Т-переход"" должен быть внесен короб с совпадающими значениями Габариты основной ветки (Высота HeightMainBranch, Глубина WidthMainBranch).",
        7, 436)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_436() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.TRIPLE)
                    .Where(p => !(context.ScsCabelCanals
                        .Any(cc => DbFunctions.Like(cc.Series, p.Series) &
                                p.HeightMainBranch == cc.CabelHeight & p.WidthMainBranch == cc.CabelDepth)))
                    .Select(p => new { p.Code, p.Series, p.HeightMainBranch, p.WidthMainBranch })
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, (p.HeightMainBranch, p.WidthMainBranch)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Т-переход"" должен быть внесен короб с совпадающими значениями Габариты отходящей ветки (Высота HeightOutBranch, Глубина WidthOutBranch).",
        7, 437)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_437() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.TRIPLE)
                    .Where(p => !(context.ScsCabelCanals
                        .Any(cc => DbFunctions.Like(cc.Series, p.Series) & p.HeightOutBranch == cc.CabelHeight & p.WidthOutBranch == cc.CabelDepth)))
                    .Select(p => new { p.Code, p.Series, p.HeightOutBranch, p.WidthOutBranch })
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, (p.HeightOutBranch, p.WidthOutBranch)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Угол"", ""Заглушка"", ""Соединение на стык"" внутри серии должно быть не более одного сочетания параметров Высота Height, Глубина Depth.",
        7, 438)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_438() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.PLAIN_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.INSIDE_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.OUTSIDE_ANGLE |
                        p.FittingType == ScsCableFittingTypeEnum.CORK |
                        p.FittingType == ScsCableFittingTypeEnum.JOINT)
                    .Select(p => new { p.Code, p.Series, p.FittingType, p.Height, p.Depth })
                    .ToList();
                var errors = products
                    .GroupBy(p => (p.Series, p.FittingType))
                    .SelectMany(s => s.GroupBy(gr => (gr.Height, gr.Depth)).Where(gr => gr.Count() > 1))
                    .SelectMany(p => p.Select(grEl => (grEl.Code, grEl.Series, grEl.FittingType, (grEl.Height, grEl.Depth))))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для соединительных элементов короба типа ""Т-переход"" внутри серии должно быть не более одного сочетания параметров Габариты основной ветки (Высота HeightMainBranch, Глубина WidthMainBranch) и Габариты отходящей ветки (Высота HeightOutBranch, Глубина WidthOutBranch).",
        7, 439)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsCableFitting))]
        public void Rule_07_439() {
            using (var context = connector.Connect()) {
                var products = context.ScsCableFittings
                    .Where(p => p.FittingType == ScsCableFittingTypeEnum.TRIPLE)
                    .Select(p => new { p.Code, p.Series, p.HeightMainBranch, p.WidthMainBranch, p.HeightOutBranch, p.WidthOutBranch })
                    .ToList();
                var errors = products
                    .GroupBy(p => (p.Series))
                    .SelectMany(s => s.GroupBy(p => (p.HeightMainBranch, p.WidthMainBranch, p.HeightOutBranch, p.WidthOutBranch)).Where(gr => gr.Count() > 1))
                    .SelectMany(p => p.Select(grEl => (grEl.Code, grEl.Series, (grEl.HeightMainBranch, grEl.WidthMainBranch, grEl.HeightOutBranch, grEl.WidthOutBranch))))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для перегородок коробов должны быть внесены параметры Высота Height и Длина перегородки Length.",
        7, 440)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbCableCanalPartition))]
        public void Rule_07_440() {
            using (var context = connector.Connect()) {
                var products = context.DbCableCanalPartitions
                    .Select(p => new { p.Code, p.Series, p.Height, p.Length })
                    .Where(p => p.Height == null || p.Length == null || p.Height <= 0 || p.Length <= 0)
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, (p.Height, p.Length)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для перегородок коробов значение параметра ""Серия"" должно совпадать со значением параметра ""Серия"" соответствующих прямых секций короба.",
         7, 441)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbCableCanalPartition))]
        public void Rule_07_441() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.DbCableCanalPartitions
                    .Select(p => p.Series)
                    .ToList();
                var errors = products
                    .Distinct()
                    .Where(s => !(context.ScsCabelCanals.Any(cc => DbFunctions.Like(cc.Series, s))))
                  .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для перегородки короба высотой Height должен быть внесен короб с соответствующим значением Глубина Depth.",
        7, 442)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbCableCanalPartition))]
        public void Rule_07_442() {
            using (var context = connector.Connect()) {
                var products = context.DbCableCanalPartitions
                    .Select(p => new { p.Code, p.Series, p.Height })
                    .Where(p => !(context.ScsCabelCanals.Any(cc => DbFunctions.Like(cc.Series, p.Series) & p.Height == cc.CabelDepth)))
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, p.Height))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для крышек коробов должны быть внесены параметры Ширина крышки CoverWidth и Длина крышки CoverLength.",
                7, 443)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DcCableCanalCover))]
        public void Rule_07_443() {
            using (var context = connector.Connect()) {
                var products = context.DcCableCanalCovers
                    .Select(p => new { p.Code, p.Series, p.CoverWidth, p.CoverLength })
                    .Where(p => p.CoverWidth == null || p.CoverLength == null || p.CoverWidth <= 0 || p.CoverLength <= 0)
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, (p.CoverWidth, p.CoverLength)))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для крышек коробов значение параметра ""Серия"" должно совпадать со значением параметра ""Серия"" соответствующих прямых секций короба.",
         7, 444)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DcCableCanalCover))]
        public void Rule_07_444() {
            using (var context = connector.Connect()) {
                var products = context.DcCableCanalCovers
                    .Select(p => p.Series)
                    .ToList();
                var errors = products
                    .Distinct()
                    .Where(s => !(context.ScsCabelCanals.Any(cc => DbFunctions.Like(cc.Series, s))))
                  .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для крышки короба шириной CoverWidth должен быть внесен короб с соответствующим значением высота CabelHeight.",
        7, 445)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DcCableCanalCover))]
        public void Rule_07_445() {
            using (var context = connector.Connect()) {
                var products = context.DcCableCanalCovers
                    .Select(p => new { p.Code, p.Series, p.CoverWidth })
                    .Where(p => !(context.ScsCabelCanals.Any(cc => DbFunctions.Like(cc.Series, p.Series) & p.CoverWidth == cc.CabelHeight)))
                    .ToList();
                var errors = products
                    .Select(p => (p.Series, p.Code, p.CoverWidth))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }

    }
}
