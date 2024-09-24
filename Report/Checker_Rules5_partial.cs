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
// Правила раздела 6 Кабельно-проводниковая продукция.
// Правила раздела 7 Кабеленесущие системы.
// Правила раздела 8 Комплектации.
// Правила раздела 9 Параметры исполнения.
// Правила раздела 10 Материалы и комплектации.
namespace Bs.Nano.Electric.Report {
    public partial class Checker {
        // Правила раздела 5 Электроприемники.
        // 5.100 Асинхронные двигатели
        // 5.200 Нагреватели
        // 5.300 Комплексные электроприемники
        // 5.400 Светильники
        // 5.500 Лампы
        // 5.600 Доп. оборудование светильников
        [ReportRule(@"Для светильников со съемной лампой должен быть указан тип используемой лампы с тем же самым цоколем.",
                    5, 401)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElLighting))]
        public void Rule_05_401() {
            using (var context = connector.Connect()) {
                var products = context.ElLightings
                    .Where(el => el.LampExistance == LampExistance.EXIST & el.Lamp != null)
                    .Select(el => new { el.Series, el.Code, el.Socle, LampSocle = el.Lamp.Socle }); ;
                    //.ToList();
                var errors = products.Where(l => l.Socle != l.LampSocle)
                    .ToArray()
                    .Select(l=>(l.Series, l.Code))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} ламп указана лампа с не подходящим типом цоколя.",
                        errors);
                }
            }

        }
        [ReportRule(@"В таблице ""Лампы"" под индексом id=0 должен быть внесен пустой элемент.",
                    5, 500)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElLamp))]
        public void Rule_05_500() {
            using (var context = connector.Connect()) {
                var lamp = context.ElLamps
                    .FirstOrDefault(l=>l.Id==0);
                if (lamp is null) {
                    FailRuleTest(@"Запись с индексом id=""0"" не внесена.");
                }
                else if (!string.IsNullOrEmpty(lamp.Name)) {
                    FailRuleTest(@"Запись с индексом id=""0"" содержит не пустой элемент.");
                }
            }

        }    
    }
}
