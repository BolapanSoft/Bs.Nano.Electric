using Nano.Electric;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bs.Nano.Electric.Report {
    public partial class Checker {
        // Правила раздела 3. Коммутационные аппараты.
[ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должны быть внесены параметры ""Номинальное напряжение"", ""Номинальный ток"", ""Количество полюсов"".",
            2, 4)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_001() {
            using (var context = connector.Connect()) {
                var products = context.ElStarters
                    .Select(p => new { p.Code, p.Series, p.DbDeviceType, p.Voltage, p.NominalCurrent, p.DbPoleCountEnum });
                var errors = products
                    .Where(p=> !(p.Voltage>0.0) | !(p.NominalCurrent>0.0) | !p.DbPoleCountEnum.HasValue)
                    .ToList()
                    .Select(p=>(Key:(p.DbDeviceType?.GetDescription(), p.Series), Value: (p.Voltage, p.NominalCurrent, p.DbPoleCountEnum?.GetDescription())))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }

        }
    }
}
