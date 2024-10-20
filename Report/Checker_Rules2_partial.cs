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
// Правила раздела 6 Электроустановочные изделия.
// Правила раздела 7 Кабеленесущие системы.
// Правила раздела 8 Кабельно-проводниковая продукция.
// Правила раздела 9 Параметры исполнения.
// Правила раздела 10 Материалы и комплектации.

namespace Bs.Nano.Electric.Report {
    public partial class Checker {
        // Правила раздела 2 Распределительные устройства.
        [ReportRule(@"",
                    2, 0)]
        [RuleCategory("Полнота заполнения технических данных.", "")]
        public void Rule_02_00() {
            using (var context = connector.Connect()) {
                throw new NotImplementedException();
            }

        }

        #region 100 Шкафы
        // Шкафы
        [ReportRule("Для шкафа должен быть заполнен параметр StructureType \"Укомплектованность аппаратурой\".",
                    2, 101)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_101() {
            using (var context = connector.Connect()) {
                var errors = context.ElBoards
                   .Where(p => p.StructureType == null)
                   .Select(p => new { p.Code, p.Series })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнен параметр для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }

        }
        [ReportRule("Для некомплектного шкафа должен быть заполнен параметр BoardType \"Тип некомплектного шкафа\".",
                    2, 102)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_102() {
            using (var context = connector.Connect()) {
                var errors = context.ElBoards
                   .Where(p => p.StructureType == BoardStructureType.NOT_GANGED & p.BoardType == null)
                   .Select(p => new { p.Code, p.Series })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнен параметр для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        [ReportRule("Для некомплектного шкафа типа \"Модульный\" должен быть заполнены параметры RowCount \"Количество рядов\" и  ModuleCountInRow \"Количество модулей (18 мм) в ряду\".",
                    2, 103)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_103() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Where(p => p.StructureType == BoardStructureType.NOT_GANGED & p.BoardType == BoardType.MODULAR)
                   .Select(p => new { p.Code, p.Series, p.RowCount, p.ModuleCountInRow })
                   .ToList();
                var errors = products
                    .Where(p => !((p.RowCount.HasValue & p.ModuleCountInRow.HasValue) && p.RowCount > 0 && p.ModuleCountInRow > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.RowCount, p.ModuleCountInRow))));
                }
            }
        }
        [ReportRule(@"Для некомплектного шкафа типа ""Универсальный"" должны быть заполнены минимальные отступы:
LeftMinIndent Минимальный отступ от левого края, мм
RightMinIndent Минимальный отступ от правого края, мм
TopMinIndent Минимальный отступ от верха, мм
BottomMinIndent Минимальный отступ от низа, мм",
                    2, 104)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_104() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Where(p => p.StructureType == BoardStructureType.NOT_GANGED & p.BoardType == BoardType.UNIVERSAL)
                   .Select(p => new { p.Code, p.Series, p.LeftMinIndent, p.RightMinIndent, p.TopMinIndent, p.BottomMinIndent })
                   .ToList();
                var errors = products
                    .Where(p => !((p.LeftMinIndent.HasValue & p.RightMinIndent.HasValue & p.TopMinIndent.HasValue & p.BottomMinIndent.HasValue) &&
                                (p.LeftMinIndent > 0 & p.RightMinIndent > 0 & p.TopMinIndent > 0 & p.BottomMinIndent > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.LeftMinIndent, p.RightMinIndent, p.TopMinIndent, p.BottomMinIndent))));
                }
            }
        }
        [ReportRule(@"Для комплектного шкафа должны быть заполнены электрические параметры:
DbVoltage Номинальное напряжение, В
DbInom Номинальный ток, А
ShockIkz Максимальный ударный ток КЗ, кА",
                    2, 105)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_105() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Where(p => p.StructureType == BoardStructureType.GANGED)
                   .Select(p => new { p.Code, p.Series, p.DbVoltage, p.DbInom, p.ShockIkz })
                   .ToList();
                var errors = products
                    .Where(p => !((p.DbVoltage.HasValue & p.DbInom.HasValue & p.ShockIkz.HasValue) &&
                                (p.DbVoltage > 0 & p.DbInom > 0 & p.ShockIkz > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbVoltage, p.DbInom, p.ShockIkz))));
                }
            }
        }
        [ReportRule(@"Для утопленного шкафа должны быть заполнены габариты наружной и утопленной части:
DbFlushedHeight DbFlushedWidth  DbFlushedDepth  DbOutHeight DbOutWidth  DbOutDepth
Высота утопленной части, мм Ширина утопленной части, мм Глубина утопленной части, мм    Высота наружной части, мм	Ширина наружной части, мм	Глубина наружной части, мм",
                    2, 106)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_106() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Where(p => p.DbInstType == InstBoxEnum.INST_FLUSH)
                   .Select(p => new { p.Code, p.Series, p.DbFlushedHeight, p.DbFlushedWidth, p.DbFlushedDepth, p.DbOutHeight, p.DbOutWidth, p.DbOutDepth })
                   .ToList();
                var errors = products
                    .Where(p => !((p.DbFlushedHeight.HasValue & p.DbFlushedWidth.HasValue & p.DbFlushedDepth.HasValue & p.DbOutHeight.HasValue & p.DbOutWidth.HasValue & p.DbOutDepth.HasValue) &&
                                (p.DbFlushedHeight > 0 & p.DbFlushedWidth > 0 & p.DbFlushedDepth > 0 & p.DbOutHeight > 0 & p.DbOutWidth > 0 & p.DbOutDepth > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbFlushedHeight, p.DbFlushedWidth, p.DbFlushedDepth, p.DbOutHeight, p.DbOutWidth, p.DbOutDepth))));
                }
            }
        }


        [ReportRule(@"Для навесного или напольного шкафа должны быть заполнены габариты:
DbHeight Высота, мм
DbWidth Ширина, мм
DbDepth Глубина, мм",
                    2, 110)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_110() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Where(p => p.DbInstType == InstBoxEnum.INST_MOUNTED | p.DbInstType == InstBoxEnum.INST_FLOOR)
                   .Select(p => new { p.Code, p.Series, p.DbHeight, p.DbWidth, p.DbDepth })
                   .ToList();
                var errors = products
                    .Where(p => !((p.DbHeight.HasValue & p.DbWidth.HasValue & p.DbDepth.HasValue) &&
                                (p.DbHeight > 0 & p.DbWidth > 0 & p.DbDepth > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbHeight, p.DbWidth, p.DbDepth))));
                }
            }
        }
        [ReportRule(@"Для шкафа должны быть заполнены данные:
DbInstType Способ установки
CableLeadIn Подвод кабеля",
                    2, 111)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_111() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Where(p => p.StructureType == BoardStructureType.GANGED)
                   .Select(p => new { p.Code, p.Series, p.DbInstType, p.CableLeadIn })
                   .ToList();
                var errors = products
                    .Where(p => !((p.DbInstType.HasValue & p.CableLeadIn.HasValue)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbInstType?.GetDescription(), p.CableLeadIn?.GetDescription()))));
                }
            }
        }
        [ReportRule(@"Для шкафа должна быть назначена графика",
                    2, 112)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBoard))]
        public void Rule_02_112() {
            using (var context = connector.Connect()) {
                var products = context.ElBoards
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef>0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика для {errors.Count} шкафов.",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion
        #region 300 Ящики
        [ReportRule(@"Для элементов ""Ящики"" должна быть назначена графика",
                    2, 312)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElBox))]
        public void Rule_02_312() {
            using (var context = connector.Connect()) {
                var products = context.ElBoxes
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Ящики\".",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion

        #region 400 Ящики с трансформатором
        [ReportRule(@"Для элемента таблицы ""Ящик с трансформатором"" должны быть заполнены технические данные:
NominalPower	PhaseCount	VoltageUp	VoltageDownValue
Номинальная мощность, кВт	Количество фаз	Номинальное напряжение обмотки ВН, В	Номинальное напряжение обмотки НН, В",
          2, 401)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElShieldingUnit))]
        public void Rule_02_401() {
            using (var context = connector.Connect()) {
                var products = context.ElShieldingUnits
                   .Select(p => new { p.Code, p.Series, p.NominalPower, p.PhaseCount, p.VoltageUp, p.VoltageDownValue })
                   .Where(p => !((p.NominalPower.HasValue & p.PhaseCount.HasValue & p.VoltageUp.HasValue & p.VoltageDownValue.HasValue) &&
                                (p.NominalPower > 0 & p.PhaseCount > 0 & p.VoltageUp > 0 & p.VoltageDownValue > 0)))
                   //.ToList()
                   ;
                var errors = products
                    //.Where(p => !((p.NominalPower.HasValue & p.PhaseCount.HasValue & p.VoltageUp.HasValue & p.VoltageDownValue.HasValue) &&
                    //            (p.NominalPower > 0 & p.PhaseCount > 0 & p.VoltageUp > 0 & p.VoltageDownValue > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.NominalPower, p.PhaseCount, p.VoltageUp, p.VoltageDownValue))));
                }
            }
        }
        [ReportRule(@"Для элемента таблицы ""Ящик с трансформатором"" должны быть заполнены габариты:
DbHeight Высота, мм
DbWidth Ширина, мм
DbDepth Глубина, мм",
            2, 410)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElShieldingUnit))]
        public void Rule_02_410() {
            using (var context = connector.Connect()) {
                var products = context.ElShieldingUnits
                   .Select(p => new { p.Code, p.Series, p.DbHeight, p.DbWidth, p.DbDepth })
                   .ToList();
                var errors = products
                    .Where(p => !((p.DbHeight.HasValue & p.DbWidth.HasValue & p.DbDepth.HasValue) &&
                                (p.DbHeight > 0 & p.DbWidth > 0 & p.DbDepth > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbHeight, p.DbWidth, p.DbDepth))));
                }
            }
        }
        [ReportRule(@"Для элемента таблицы ""Ящик с трансформатором"" должны быть заполнены данные: CableLeadIn Подвод кабеля",
                    2, 411)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElShieldingUnit))]
        public void Rule_02_411() {
            using (var context = connector.Connect()) {
                var errors = context.ElShieldingUnits
                   .Where(p => !(p.CableLeadIn.HasValue))
                   //.Where(p => p.StructureType == BoardStructureType.GANGED)
                   .Select(p => new { p.Code, p.Series, p.CableLeadIn })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.CableLeadIn?.GetDescription()))));
                }
            }
        }
        [ReportRule(@"Для элементов ""Ящики с трансформатором"" должна быть назначена графика",
            2, 412)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElShieldingUnit))]
        public void Rule_02_412() {
            using (var context = connector.Connect()) {
                var products = context.ElShieldingUnits
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Ящики с трансформатором\".",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion
    }
}
