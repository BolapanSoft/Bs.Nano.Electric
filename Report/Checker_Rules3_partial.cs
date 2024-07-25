using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
        // Правила раздела 3 Коммутационные аппараты.
        [ReportRule(@"",
                    3, 0)]
        [RuleCategory(".", "")]
        public void Rule_03_00() {
            using (var context = connector.Connect()) {
                throw new NotImplementedException();
            }

        }
        #region 100 Автоматические выключчатели

        [ReportRule(@"Для автоматических выключателей должны быть заполнены технические данные:
NominalCurrent	DbPoleCountEnum	VoltageType	MaxCommutation	DynResistance
Номинальный ток, А	Количество полюсов	Номинальное напряжение сети, В	Предельная коммутационная способность при 380/220В Ics, кА	Электродинамическая стойкость при 380/220В Icm, кА",
                    3, 101)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_101() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                  .Where(p => !((p.NominalCurrent.HasValue & p.DbPoleCountEnum.HasValue & p.VoltageType.HasValue & p.MaxCommutation.HasValue & p.DynResistance.HasValue) &&
                                (p.NominalCurrent > 0 & p.DbPoleCountEnum > 0 & p.VoltageType > 0 & p.MaxCommutation > 0 & p.DynResistance > 0)))
                    .Select(p => new { p.Code, p.Series, p.NominalCurrent, p.DbPoleCountEnum, p.VoltageType, p.MaxCommutation, p.DynResistance })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.NominalCurrent, p.DbPoleCountEnum?.GetDescription(), p.VoltageType?.GetDescription(), p.MaxCommutation, p.DynResistance))));
                }
            }

        }

        [ReportRule(@"Для автоматических выключателей должны быть заполнены механические данные:
ContactType	MaxCordS
Конструктивное исполнение	Макс. сечение проводника, мм^2",
                    3, 102)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_102() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                  .Where(p => !((p.ContactType.HasValue & p.MaxCordS.HasValue) &&
                                (p.MaxCordS > 0)))
                    .Select(p => new { p.Code, p.Series, p.ContactType, p.MaxCordS })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.ContactType?.GetDescription(), p.MaxCordS))));
                }
            }

        }
        [ReportRule(@"Для автоматических выключателей должны быть заполнены габариты:
Height	Width	Depth   DbIsModule
Высота, мм	Ширина, мм	Глубина, мм Модульный",
                    3, 103)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_103() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                  .Where(p => !((p.Height.HasValue & p.Width.HasValue & p.Depth.HasValue & p.DbIsModule.HasValue) &&
                                (p.Height > 0 & p.Width > 0 & p.Depth > 0)))
                    .Select(p => new { p.Code, p.Series, p.Height, p.Width, p.Depth, p.DbIsModule })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Height, p.Width, p.Depth, Convert(p.DbIsModule.HasValue)))));
                }
            }

        }
        [ReportRule(@"Для автоматических выключателей должен быть заполнен тип монтажа: MountType Крепление",
                    3, 104)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_104() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                  .Where(p => !((p.MountType.HasValue) &&
                                (p.MountType > 0)))
                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType?.GetDescription()))));
                }
            }

        }
        [ReportRule(@"Для автоматических выключателей должны быть заполнено наличие типов расцепителей:
IsHeatR	IsElMagR	IsElectronicR	HasUzo
Наличие теплового расцепителя	Наличие электромагнитного расцепителя	Наличие электронного расцепителя	Наличие дифференциального расцепителя",
            3, 105)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_105() {

            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                  .Where(p => !(p.IsHeatR.HasValue & p.IsElMagR.HasValue & p.IsElectronicR.HasValue & p.HasUzo.HasValue))
                    .Select(p => new { p.Code, p.Series, p.IsHeatR, p.IsElMagR, p.IsElectronicR, p.HasUzo })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (Convert(p.IsHeatR), Convert(p.IsElMagR), Convert(p.IsElectronicR), Convert(p.HasUzo)))));
                }
            }

        }
        [ReportRule(@"Для автоматических выключателей с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
                    3, 110)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_110() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                   .Where(p => p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL | p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL_OR_BOARD)
                  .Where(p => !(p.RailMountTypeFlagged.HasValue &&
                                (p.RailMountTypeFlagged > 0)))
                    .Select(p => new { p.Code, p.Series, p.RailMountTypeFlagged })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.RailMountTypeFlagged))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с указанием ""Модульный"" должен быть заполнен параметр: DbModuleCount Количество модулей 18мм, шт",
                    3, 111)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_111() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.DbIsModule == true)
                    .Where(p => !((p.DbModuleCount.HasValue) &&
                                (p.DbModuleCount > 0)))
                    .Select(p => new { p.Code, p.Series, p.DbModuleCount })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbModuleCount))));
                }
            }
        }

        [ReportRule(@"Для автоматических выключателей с наличием теплового расцепителя должен быть заполнен параметр: CurrentScale Токи уставки расцепителя в зоне перегрузки Ir, А",
                    3, 112)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_112() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsHeatR == true)
                    .Select(p => new { p.Code, p.Series, p.CurrentScale })
                   .ToList()
                   .Where(p => !(
                                TryParseAsDouble(p.CurrentScale, out double scale).IsSuccess
                                && scale > 0))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (TryParseAsDouble(p.CurrentScale, out _).Value))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием электромагнитного расцепителя должен быть заполнен параметр: CurrentChoice Способ задания уставки расцепителя",
                    3, 113)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_113() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsElMagR == true)
                    .Where(p => p.CurrentChoice == null)
                    .Select(p => new { p.Code, p.Series, p.CurrentChoice })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.CurrentChoice?.GetDescription()))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием электромагнитного или электронного расцепителя и способом задания уставки расцепителя в зоне КЗ ""По кратности (Km)"" должны быть заполнены параметры:
MultiplScale	MinSensivity	MaxSensivity	UnlinkTimeScale
Кратности уставки расцепителя Кm, о.е	Коэффициент гарантированного несрабатывания, о.е.	Коэффициент гарантированного срабатывания, о.е.	Время срабатывания расцепителя в зоне КЗ tm, с",
                    3, 114)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_114() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => (p.IsElMagR == true | p.IsElectronicR == true) & p.CurrentChoice == ElCurrentChoiseEnum.BY_MULTIPLICITY)
                    .Select(p => new { p.Code, p.Series, p.MultiplScale, p.MinSensivity, p.MaxSensivity, p.UnlinkTimeScale })
                    .ToList()
                    .Where(p => !(!string.IsNullOrEmpty(p.MultiplScale) &
                                (p.MinSensivity.HasValue && p.MinSensivity > 0) &
                                (p.MaxSensivity.HasValue && p.MaxSensivity > 0) &
                                (TryParseAsDouble(p.UnlinkTimeScale, out double value).IsSuccess && value > 0)))

                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MultiplScale, p.MinSensivity, p.MaxSensivity, TryParseAsDouble(p.UnlinkTimeScale, out _).Value))
                        ));
                    ;
                }
            }
        }

        [ReportRule(@"Для автоматических выключателей с наличием электромагнитного или электронного расцепителя и способом задания уставки расцепителя ""По току (Im)"" должны быть заполнены параметры:
CurrReleaseScale	MinSensivity	MaxSensivity	UnlinkTimeScale
Токи уставки расцепителя Im, А	Коэффициент гарантированного несрабатывания, о.е.	Коэффициент гарантированного срабатывания, о.е.	Время срабатывания расцепителя в зоне КЗ tm, с",
                    3, 115)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_115() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => (p.IsElMagR == true | p.IsElectronicR == true) & p.CurrentChoice == ElCurrentChoiseEnum.BY_CURRENT)
                    .Select(p => new { p.Code, p.Series, p.CurrReleaseScale, p.MinSensivity, p.MaxSensivity, p.UnlinkTimeScale })
                    .ToList()
                    .Where(p => !(
                                (TryParseAsDouble(p.CurrReleaseScale, out double valueCurrent).IsSuccess && valueCurrent > 0) &
                                (p.MinSensivity.HasValue && p.MinSensivity > 0) &
                                (p.MaxSensivity.HasValue && p.MaxSensivity > 0) &
                                (TryParseAsDouble(p.UnlinkTimeScale, out double value).IsSuccess && value > 0)))

                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code,
                                                (TryParseAsDouble(p.CurrReleaseScale, out _).Value,
                                                p.MinSensivity,
                                                p.MaxSensivity,
                                                TryParseAsDouble(p.UnlinkTimeScale, out _).Value))
                        ));
                    ;
                }
            }
        }

        [ReportRule(@"Для автоматических выключателей с наличием электромагнитного или электронного расцепителя и способом задания уставки расцепителя ""По типу (A/B/C/D/...)"" должны быть заполнены параметры:
AutomatCharReleaseType	AutomatReleaseMinCoef	AutomatReleaseMaxCoef	UnlinkTimeScale
Тип характеристики срабатывания расцепителя Tm, А	Кратность нижней границы, о.е.	Кратность верхней границы, о.е.	Время срабатывания расцепителя в зоне КЗ tm, с",
                    3, 116)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_116() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => (p.IsElMagR == true | p.IsElectronicR == true) & p.CurrentChoice == ElCurrentChoiseEnum.BY_CURVE)
                    .Select(p => new { p.Code, p.Series, p.AutomatCharReleaseType, p.AutomatReleaseMinCoef, p.AutomatReleaseMaxCoef, p.UnlinkTimeScale })
                    .ToList()
                    .Where(p => !(
                                (!string.IsNullOrEmpty(p.AutomatCharReleaseType) &
                                (p.AutomatReleaseMinCoef.HasValue && p.AutomatReleaseMinCoef > 0) &
                                (p.AutomatReleaseMaxCoef.HasValue && p.AutomatReleaseMaxCoef > 0) &
                                (TryParseAsDouble(p.UnlinkTimeScale, out double value).IsSuccess && value > 0))
                                )
                           )

                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (
                                            p.AutomatCharReleaseType,
                                            p.AutomatReleaseMinCoef,
                                            p.AutomatReleaseMaxCoef,
                                            TryParseAsDouble(p.UnlinkTimeScale, out _).Value)
                                            )
                        ));
                    ;
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием электронного расцепителя должны быть заполнены параметры расцепителя в зоне перегрузки: 
CurrentScale	TimeReleaseIr	TimeOeReleaseIr
Токи уставки расцепителя в зоне перегрузки Ir, А	Время срабатывания в зоне перегрузки tr, c	Кратность тока для времени tr, о.е.",
                    3, 117)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_117() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsElectronicR == true)
                    .Select(p => new { p.Code, p.Series, p.CurrentScale, p.TimeReleaseIr, p.TimeOeReleaseIr })
                   .ToList()
                   .Where(p => !(
                                (TryParseAsDouble(p.CurrentScale, out var value).IsSuccess
                                    && value > 0) &
                                (TryParseAsDouble(p.TimeReleaseIr, out var valueTRI).IsSuccess
                                    && valueTRI > 0) &
                                (p.TimeOeReleaseIr.HasValue && p.TimeOeReleaseIr > 0)
                                ))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.CurrentScale, p.TimeReleaseIr, p.TimeOeReleaseIr))));
                }
            }
        }

        [ReportRule(@"Для автоматических выключателей с наличием электронного расцепителя должны быть заполнены параметры расцепителя в зоне КЗ: 
CurrentChoice	IsMultiplicityOfCurrentForTmTime    KzInstantCurrentChoice
Способ задания уставки расцепителя	Наличие кратности тока для времени tm (для функции I2t) Способ задания уставки мгновенного расцепителя",
                    3, 118)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_118() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsElectronicR == true)
                    .Select(p => new { p.Code, p.Series, p.CurrentChoice, p.IsMultiplicityOfCurrentForTmTime, p.KzInstantCurrentChoice })
                   .ToList()
                   .Where(p => p.CurrentChoice == null | p.IsMultiplicityOfCurrentForTmTime == null | p.KzInstantCurrentChoice == null)
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.CurrentChoice?.GetDescription(), Convert(p.IsMultiplicityOfCurrentForTmTime), p.KzInstantCurrentChoice?.GetDescription()))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием электронного расцепителя, при указании параметра ""Наличие кратности тока для времени tm""=""Да"", должен быть заполнен параметр: 
MultiplicityOfCurrentForTmTime	Кратность тока для времени tm (для функции I2t), о.е.",
                    3, 119)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_119() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsElectronicR == true & p.IsMultiplicityOfCurrentForTmTime == true)
                    .Select(p => new { p.Code, p.Series, p.MultiplicityOfCurrentForTmTime })
                   .ToList()
                   .Where(p => !(p.MultiplicityOfCurrentForTmTime.HasValue && p.MultiplicityOfCurrentForTmTime > 0))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MultiplicityOfCurrentForTmTime))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием электронного расцепителя, при указании параметра ""Способ задания уставки мгновенного расцепителя""=""По току (Ii)"", должны быть заполнены параметры: 
KzIiScale	UnlinkTimeElectronicScale
Токи уставки мгновенного расцепителя Ii, о.е.	Время срабатывания мгновенного расцепителя ti, с",
                    3, 120)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_120() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsElectronicR == true & p.KzInstantCurrentChoice == ElInstantKzCurrentChoiseEnum.BY_CURRENT)
                    .Select(p => new { p.Code, p.Series, p.KzIiScale, p.UnlinkTimeElectronicScale })
                   .ToList()
                   .Where(p => !(TryParseAsDouble(p.KzIiScale, out double value).IsSuccess && value > 0 &&
                                   TryParseAsDouble(p.UnlinkTimeElectronicScale, out value).IsSuccess && value > 0))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (TryParseAsDouble(p.KzIiScale, out _).Value, TryParseAsDouble(p.UnlinkTimeElectronicScale, out _).Value))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием электронного расцепителя, при указании параметра ""Способ задания уставки мгновенного расцепителя""=""По кратности (Ki)"", должны быть заполнены параметры: 
KzKiScale	UnlinkTimeElectronicScale
Кратности уставки мгновенного расцепителя Ki, о.е.	Время срабатывания мгновенного расцепителя ti, с",
                    3, 121)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_121() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.IsElectronicR == true & p.KzInstantCurrentChoice == ElInstantKzCurrentChoiseEnum.BY_MULTIPLICITY)
                    .Select(p => new { p.Code, p.Series, p.KzKiScale, p.UnlinkTimeElectronicScale })
                   .ToList()
                   .Where(p => !(TryParseAsDouble(p.KzKiScale, out double value).IsSuccess && value > 0 &&
                                   TryParseAsDouble(p.UnlinkTimeElectronicScale, out value).IsSuccess && value > 0))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (TryParseAsDouble(p.KzKiScale, out _).Value, TryParseAsDouble(p.UnlinkTimeElectronicScale, out _).Value))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей с наличием дифференциального расцепителя должен быть заполнен параметр: CurrentScaleUzo Токи уставок I∆, мА",
            3, 122)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_122() {
            string error = "Значение должно быть целым числом от 1 до 300";

            string convert(string currentScale) {
                (var isDouble, var errorMessage) = TryParseAsDouble(currentScale, out double value);
                if (isDouble) {
                    if (IsCorrectCurrentScaleUzo(value)) {
                        return currentScale;
                    }
                    else {
                        return error;
                    }
                }
                else {
                    return errorMessage;
                }
            }
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.HasUzo == true)
                    .Select(p => new { p.Code, p.Series, p.CurrentScaleUzo })
                   .ToList()
                   .Where(p => !(TryParseAsDouble(p.CurrentScaleUzo, out double value).IsSuccess &&
                                IsCorrectCurrentScaleUzo(value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (convert(p.CurrentScaleUzo)))));
                }
            }
        }
        #endregion

        #region 200 Предохранители
        [ReportRule(@"Для предохранителей должны быть заполнены технические данные:
Voltage	NominalCurrent	DbPoleCountEnum	CurrentScale	MaxCommutation
Номинальное напряжение, В	Номинальный ток, А	Количество полюсов	Уставка плавкой вставки, А	Предельная коммутационная способность, кА",
            3, 201)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElSafeDevice))]
        public void Rule_03_201() {
            using (var context = connector.Connect()) {
                var errors = context.ElSafeDevices
                    .Select(p => new { p.Code, p.Series, p.Voltage, p.DbPoleCountEnum, p.NominalCurrent, p.MaxCommutation, p.CurrentScale })
                   .ToList()
                    .Where(p => !((p.Voltage.HasValue & p.NominalCurrent.HasValue & p.DbPoleCountEnum.HasValue & p.MaxCommutation.HasValue) &&
                                (p.Voltage > 0 & p.NominalCurrent > 0 & p.DbPoleCountEnum > 0 & p.MaxCommutation > 0) &&
                                (TryParseAsInt(p.CurrentScale, out int cs).IsSuccess & cs > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Voltage, p.NominalCurrent, p.DbPoleCountEnum?.GetDescription(), TryParseAsInt(p.CurrentScale, out _).Value, p.MaxCommutation))));
                }
            }

        }
        [ReportRule(@"Для предохранителей должны быть заполнены габариты:
Height	Width	Depth
Высота, мм	Ширина, мм	Глубина, мм",
                    3, 203)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElSafeDevice))]
        public void Rule_03_203() {
            using (var context = connector.Connect()) {
                var errors = context.ElSafeDevices
                  .Where(p => !((p.Height.HasValue & p.Width.HasValue & p.Depth.HasValue) &&
                                (p.Height > 0 & p.Width > 0 & p.Depth > 0)))
                    .Select(p => new { p.Code, p.Series, p.Height, p.Width, p.Depth })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Height, p.Width, p.Depth))));
                }
            }

        }
        [ReportRule(@"Для предохранителей должен быть заполнен параметр: MountType  Крепление",
                    3, 204)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElSafeDevice))]
        public void Rule_03_204() {
            using (var context = connector.Connect()) {
                var errors = context.ElSafeDevices
                  .Where(p => !((p.MountType.HasValue) &&
                                (p.MountType > 0)))
                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType?.GetDescription()))));
                }
            }

        }
        [ReportRule(@"Для предохранителей с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
            3, 210)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElSafeDevice))]
        public void Rule_03_210() {
            using (var context = connector.Connect()) {
                var errors = context.ElSafeDevices
                   .Where(p => p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL | p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL_OR_BOARD)
                  .Where(p => !(p.RailMountTypeFlagged.HasValue &&
                                (p.RailMountTypeFlagged > 0)))
                    .Select(p => new { p.Code, p.Series, p.RailMountTypeFlagged })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.RailMountTypeFlagged))));
                }
            }
        }



        #endregion

        #region 300 Рубильники
        [ReportRule(@"Для рубильников должны быть заполнены технические данные:
Voltage	NominalCurrent	Poles	MaxDynCurrent
Номинальное напряжение, В	Номинальный ток, А	Количество полюсов	Номинальный условный ток короткого замыкания, Icm, кА",
            3, 301)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElKnifeSwitch))]
        public void Rule_03_301() {
            using (var context = connector.Connect()) {
                var errors = context.ElKnifeSwitches
                    .Select(p => new { p.Code, p.Series, p.Voltage, p.Poles, p.NominalCurrent, p.MaxDynCurrent })
                   .ToList()
                    .Where(p => !((p.Voltage.HasValue & p.NominalCurrent.HasValue & p.Poles.HasValue & p.MaxDynCurrent.HasValue) &&
                                (p.Voltage > 0 & p.NominalCurrent > 0 & p.Poles > 0 & p.MaxDynCurrent > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Voltage, p.NominalCurrent, p.Poles?.GetDescription(), p.MaxDynCurrent))));
                }
            }

        }
        [ReportRule(@"Для рубильников должны быть заполнены механические данные:
MaxCordS
Макс. сечение проводника, мм^2",
            3, 302)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElKnifeSwitch))]
        public void Rule_03_302() {
            using (var context = connector.Connect()) {
                var errors = context.ElKnifeSwitches
                  .Where(p => !(p.MaxCordS.HasValue) &&
                                (p.MaxCordS > 0))
                    .Select(p => new { p.Code, p.Series, p.MaxCordS })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MaxCordS))));
                }
            }

        }
        [ReportRule(@"Для рубильников должны быть заполнены габариты:
DbHeight	DbWidth	DbDepth    DbIsModule
Высота, мм	Ширина, мм	Глубина, мм Модульный",
                    3, 303)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElKnifeSwitch))]
        public void Rule_03_303() {
            using (var context = connector.Connect()) {
                var errors = context.ElKnifeSwitches
                  .Where(p => !((p.DbHeight.HasValue & p.DbWidth.HasValue & p.DbDepth.HasValue & p.DbIsModule.HasValue) &&
                                (p.DbHeight > 0 & p.DbWidth > 0 & p.DbDepth > 0)))
                    .Select(p => new { p.Code, p.Series, p.DbHeight, p.DbWidth, p.DbDepth, p.DbIsModule })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbHeight, p.DbWidth, p.DbDepth, Convert(p.DbIsModule)))));
                }
            }

        }
        [ReportRule(@"Для рубильников должен быть заполнен тип монтажа: MountType Крепление",
            3, 304)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElKnifeSwitch))]
        public void Rule_03_304() {
            using (var context = connector.Connect()) {
                var errors = context.ElKnifeSwitches
                  .Where(p => !((p.MountType.HasValue) &&
                                (p.MountType > 0)))
                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType?.GetDescription()))));
                }
            }

        }
        [ReportRule(@"Для рубильников с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
            3, 310)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElKnifeSwitch))]
        public void Rule_03_310() {
            using (var context = connector.Connect()) {
                var errors = context.ElKnifeSwitches
                   .Where(p => p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL | p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL_OR_BOARD)
                  .Where(p => !(p.RailMountTypeFlagged.HasValue &&
                                (p.RailMountTypeFlagged > 0)))
                    .Select(p => new { p.Code, p.Series, p.RailMountTypeFlagged })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.RailMountTypeFlagged))));
                }
            }
        }
        [ReportRule(@"Для рубильников с указанием ""Модульный"" должен быть заполнен параметр: DbModuleCount Количество модулей 18мм, шт",
                    3, 311)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElKnifeSwitch))]
        public void Rule_03_311() {
            using (var context = connector.Connect()) {
                var errors = context.ElKnifeSwitches
                    .Where(p => p.DbIsModule == true)
                    .Where(p => !((p.DbModuleCount.HasValue) &&
                                (p.DbModuleCount > 0)))
                    .Select(p => new { p.Code, p.Series, p.DbModuleCount })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbModuleCount))));
                }
            }
        }

        #endregion
        #region 400 УЗО
        [ReportRule(@"Для УЗО должны быть заполнены технические данные:
DbVoltage	DbNominalCur	CurrentScaleUzo	DbPoleCountEnum
Номинальное напряжение Un, В	Номинальный ток In, А	Ток уставки дифференциального расцепителя I∆, мA	Количество полюсов",
                    3, 401)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_401() {
            string error = "Значение должно быть целым числом от 1 до 300";

            string convert(string currentScale) {
                (var isDouble, var errorMessage) = TryParseAsDouble(currentScale, out double value);
                if (isDouble) {
                    if (IsCorrectCurrentScaleUzo(value)) {
                        return currentScale;
                    }
                    else {
                        return error;
                    }
                }
                else {
                    return errorMessage;
                }
            }
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                    .Select(p => new { p.Code, p.Series, p.DbVoltage, p.DbPoleCountEnum, p.DbNominalCur, p.CurrentScaleUzo})
                   .ToList()
                  .Where(p => !((p.DbVoltage.HasValue & p.DbPoleCountEnum.HasValue & p.DbNominalCur.HasValue) &&
                                (p.DbVoltage > 0 & p.DbPoleCountEnum > 0 & p.DbNominalCur > 0 ) &&
                                (TryParseAsDouble(p.CurrentScaleUzo, out double value).IsSuccess &&
                                IsCorrectCurrentScaleUzo(value))))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbVoltage, p.DbNominalCur, convert(p.CurrentScaleUzo), p.DbPoleCountEnum?.GetDescription()))));
                }
            }

        }
        [ReportRule(@"Для УЗО должны быть заполнен параметр: MaxCordS Макс. сечение проводника, мм^2",
             3, 402)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_402() {
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                  .Where(p => !(( p.MaxCordS.HasValue) &&
                                (p.MaxCordS > 0)))
                    .Select(p => new { p.Code, p.Series,  p.MaxCordS })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, ( p.MaxCordS))));
                }
            }

        }
        [ReportRule(@"Для УЗО должны быть заполнены габариты:
DbHeight	DbWidth	DbDepth   DbIsModule
Высота, мм	Ширина, мм	Глубина, мм Модульный",
                    3, 403)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_403() {
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                  .Where(p => !((p.DbHeight.HasValue & p.DbWidth.HasValue & p.DbDepth.HasValue & p.DbIsModule.HasValue) &&
                                (p.DbHeight > 0 & p.DbWidth > 0 & p.DbDepth > 0)))
                    .Select(p => new { p.Code, p.Series, p.DbHeight, p.DbWidth, p.DbDepth, p.DbIsModule })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbHeight, p.DbWidth, p.DbDepth, Convert(p.DbIsModule.HasValue)))));
                }
            }

        }
        [ReportRule(@"Для УЗО должен быть заполнен тип монтажа: MountType Крепление",
                    3, 404)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_404() {
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                  .Where(p => !((p.MountType.HasValue) &&
                                (p.MountType > 0)))
                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType?.GetDescription()))));
                }
            }

        }
        [ReportRule(@"Для УЗО с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
                    3, 410)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_410() {
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                   .Where(p => p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL | p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL_OR_BOARD)
                  .Where(p => !(p.RailMountTypeFlagged.HasValue &&
                                (p.RailMountTypeFlagged > 0)))
                    .Select(p => new { p.Code, p.Series, p.RailMountTypeFlagged })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.RailMountTypeFlagged))));
                }
            }
        }
        [ReportRule(@"Для УЗО с указанием ""Модульный"" должен быть заполнен параметр: DbModuleCount Количество модулей 18мм, шт",
                    3, 411)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_411() {
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                    .Where(p => p.DbIsModule == true)
                    .Where(p => !((p.DbModuleCount.HasValue) &&
                                (p.DbModuleCount > 0)))
                    .Select(p => new { p.Code, p.Series, p.DbModuleCount })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbModuleCount))));
                }
            }
        }
        #endregion



        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должны быть внесены параметры ""Номинальное напряжение"", ""Номинальный ток"", ""Количество полюсов"".",
                    3, 501)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_501() {
            using (var context = connector.Connect()) {
                var products = context.ElStarters
                    .Select(p => new { p.Code, p.Series, p.DbDeviceType, p.Voltage, p.NominalCurrent, p.DbPoleCountEnum });
                var errors = products
                    .Where(p => !(p.Voltage > 0.0) | !(p.NominalCurrent > 0.0) | !p.DbPoleCountEnum.HasValue)
                    .ToList()
                    .Select(p => (Key: (p.DbDeviceType?.GetDescription(), p.Series, p.Code), Value: (p.Voltage, p.NominalCurrent, p.DbPoleCountEnum?.GetDescription())))
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }

        }

        private static bool IsCorrectCurrentScaleUzo(double value) {
            if (((double)((int)value)) == value &&
                 value >= 1.0 && value <= 300.0
                 ) {
                return true;
            }
            return false;
        }
        private (bool IsSuccess, string Value) TryParseAsInt(string str, out int intValue) {
            if (string.IsNullOrEmpty(str)) {
                intValue = 0;
                return (false, string.Empty);
            }
            bool isSuccess = int.TryParse(str, NumberStyles.None, CultureInfo.GetCultureInfo("Ru-ru"), out intValue);
            string value = isSuccess ? str : $"Значение \"{str}\" не соответствует целому числу.";
            return (isSuccess, value);
        }

        private (bool IsSuccess, string Value) TryParseAsDouble(string str, out double dValue) {
            if (string.IsNullOrEmpty(str)) {
                dValue = double.NaN;
                return (false, string.Empty);
            }
            bool isSuccess = double.TryParse(str, NumberStyles.Number, CultureInfo.GetCultureInfo("Ru-ru"), out dValue);
            string value = isSuccess ? str : $"Значение \"{str}\" не соответствует шаблону \"0,##\"";
            return (isSuccess, value);
        }
        private string Convert(bool? v) {
            return v.HasValue ? (v == true ? "Да" : "Нет") : string.Empty;
        }

    }
}
