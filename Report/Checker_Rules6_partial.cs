using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
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
        // Правила раздела 6 Электроустановочные изделия.
        // 6.100 Выключатели
        // 6.200 Переключатели на два направления
        // 6.300 Розетки
        // 6.400 Разветвительные коробки
        // 6.600 Доп. оборудование ЭУИ
        #region 6.100 Выключатели
        [ReportRule(@"Для элементов ""Выключатели"" должна быть назначена графика",
                            6, 112)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbElSwitch))]
        public void Rule_06_112() {
            using (var context = connector.Connect()) {
                var products = context.DbElSwitches
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Выключатели\".",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion
        #region 6.200 Переключатели на два направления
        [ReportRule(@"Для элементов ""Переключатели на два направления"" должна быть назначена графика",
                            6, 212)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbElCSwitch))]
        public void Rule_06_212() {
            using (var context = connector.Connect()) {
                var products = context.DbElCSwitches
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Переключатели на два направления\".",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion
        #region 6.300 Розетки
        [ReportRule(@"Для элементов ""Розетки"" должна быть назначена графика",
                            6, 312)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbElSocket))]
        public void Rule_06_312() {
            using (var context = connector.Connect()) {
                var products = context.DbElSockets
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Розетки\".",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion
        #region 6.400 Разветвительные коробки
        [ReportRule(@"Для элементов ""Разветвительные коробки"" должна быть назначена графика",
                            6, 412)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElDbCase))]
        public void Rule_06_412() {
            using (var context = connector.Connect()) {
                var products = context.ElDbCases
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Разветвительные коробки\".",
                        errors.Select(p => (p.Series, p.Code)));
                }
            }
        }
        #endregion
    }
}
