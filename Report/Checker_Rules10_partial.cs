using Nano.Electric;
using System;
using System.Data.Entity;
using System.Linq;
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
        // Правила раздела 10 Материалы и комплектации.
        [ReportRule(@"",
                    10, 0)]
        [RuleCategory(".", "")]
        public void Rule_10_00() {
            using (var context = connector.Connect()) {
                throw new NotImplementedException();
            }
        }
        [ReportRule(@"В таблице Материалы и комплектации/Материалы должна быть внесена единица измерения.",
                       10, 0)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(CaeMaterialUtility))]
        public void Rule_10_01() {
            using (var context = connector.Connect()) {
                var errors = context.CaeMaterialUtilities
                    //.Where(p => DbFunctions.Like(p.MeashureUnits,""))
                    .Where(p => p.MeashureUnits == null || DbFunctions.Like(p.MeashureUnits,""))
                    .Select(p => new {p.MaterialGroup, p.Series, p.Code, p.MeashureUnits })
                    .ToList()
                    .Select(p => $"({p.MaterialGroup}\\{((p.Series is null)?"":$"{p.Series}\\")}{p.Code} {nameof(p.MeashureUnits)}:\"{p.MeashureUnits}\"")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов таблицы Материалы и комплектации/Материалы не внесена единица измерения.",
                        errors);
                };
            }
        }
    }
}
