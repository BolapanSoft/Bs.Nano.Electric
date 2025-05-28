using DocumentFormat.OpenXml.Vml;
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
// Правила раздела 6 Электроустановочные изделия.
// Правила раздела 7 Кабеленесущие системы.
// Правила раздела 8 Кабельно-проводниковая продукция.
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
        #region 100 Автоматические выключатели

        [ReportRule(@"Для автоматических выключателей должны быть заполнены технические данные:
NominalCurrent	DbPoleCountEnum	VoltageType	MaxCommutation
Номинальный ток, А	Количество полюсов	Номинальное напряжение сети, В	Предельная коммутационная способность при 380/220В Ics, кА",
                    3, 101)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_101() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Select(p => new { p.Code, p.Series, p.NominalCurrent, p.DbPoleCountEnum, p.VoltageType, p.MaxCommutation })
                    .ToList()
                    .Where(p => !((p.NominalCurrent.HasValue & p.DbPoleCountEnum.HasValue & p.VoltageType.HasValue & p.MaxCommutation.HasValue) &&
                                (p.NominalCurrent > 0 & p.DbPoleCountEnum > 0 & IsDefined(p.VoltageType!.Value) & p.MaxCommutation > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.NominalCurrent,
                                p.DbPoleCountEnum.HasValue ? GetDescription(p.DbPoleCountEnum.Value) : string.Empty,
                                p.VoltageType.HasValue ? GetDescription(p.VoltageType.Value) : string.Empty,
                                p.MaxCommutation))));
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
                   .Select(p => new { p.Code, p.Series, p.ContactType, p.MaxCordS })
                   .ToList()
                   .Where(p => !((p.MaxCordS.HasValue && p.MaxCordS > 0) &&
                                 (p.ContactType.HasValue && IsDefined(p.ContactType.Value))
                                 ))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code,
                            (p.ContactType.HasValue ? GetDescription(p.ContactType.Value) : string.Empty,
                            p.MaxCordS))));
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
                    .Select(p => new { p.Code, p.Series, p.MountType })
                    .ToList()
                    .Where(p => !(p.MountType.HasValue &&
                                IsDefined(p.MountType.Value))
                                )
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty))));
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
        [ReportRule(@"Для автоматических выключателей, если заполнен параметр MaxCommutation ""Предельная коммутационная способность при 380/220В Ics, кА"", то должен быть заполнен параметр DynResistance ""Электродинамическая стойкость при 380/220В Icm, кА"".",
            3, 106)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_106() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.MaxCommutation > 0)
                    .Select(p => new { p.Code, p.Series, p.DynResistance })
                    .ToList()
                    .Where(p => !((p.DynResistance.HasValue) &&
                                (p.DynResistance > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DynResistance))));
                }
            }

        }
        [ReportRule(@"Для автоматических выключателей значение электродинамической стойкости DynResistance должно быть больше, либо равно значению предельной коммутационной способности MaxCommutation",
            3, 107)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_107() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.MaxCommutation > 0)
                    .Select(p => new { p.Code, p.Series, p.DynResistance, p.MaxCommutation })
                    .ToList()
                    .Where(p => !((p.DynResistance.HasValue) &&
                                (p.DynResistance >= p.MaxCommutation)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MaxCommutation, p.DynResistance))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей, если напряжение указано как 660/380 или 380/220; 660/380, то обязательно должны быть заполнены параметры DynResistance660 ""Предельная коммутационная способность при 660/380В Ics, кА"" и MaxCommutation660 ""Электродинамическая стойкость при 660/380В Icm, кА"".",
            3, 108)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_108() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.VoltageType == RatedNetVoltageType.Voltage660380 | p.VoltageType == (RatedNetVoltageType.Voltage660380 & RatedNetVoltageType.Voltage380220))
                    .Select(p => new { p.Code, p.Series, p.DynResistance660, p.MaxCommutation660 })
                    .ToList()
                    .Where(p => !(p.DynResistance660.HasValue && p.DynResistance660 > 0 &&
                                  p.MaxCommutation660.HasValue && p.MaxCommutation660>0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DynResistance660, p.MaxCommutation660))));
                }
            }
        }
         [ReportRule(@"Для автоматических выключателей значение электродинамической стойкости DynResistance660 должно быть больше, либо равно значению предельной коммутационной способности MaxCommutation660",
            3, 109)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_109() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => p.MaxCommutation660 > 0)
                    .Select(p => new { p.Code, p.Series, p.DynResistance660, p.MaxCommutation660 })
                    .ToList()
                    .Where(p => !((p.DynResistance660.HasValue) &&
                                (p.DynResistance660 >= p.MaxCommutation660)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DynResistance660, p.MaxCommutation660 ))));
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
                   .Where(p => !IsDoubleValuesList(p.CurrentScale))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, p.CurrentScale)));
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
                    .Select(p => new { p.Code, p.Series, p.CurrentChoice })
                   .ToList()
                   .Where(p => !(p.CurrentChoice.HasValue && IsDefined(p.CurrentChoice.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.CurrentChoice.HasValue ? GetDescription(p.CurrentChoice.Value) : string.Empty))));
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
                                IsDoubleValuesList(p.UnlinkTimeScale)
                                ))

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
                                IsDoubleValuesList(p.CurrReleaseScale) &
                                (p.MinSensivity.HasValue && p.MinSensivity > 0) &
                                (p.MaxSensivity.HasValue && p.MaxSensivity > 0) &
                                IsDoubleValuesList(p.UnlinkTimeScale)
                                ))

                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code,
                                                p.CurrReleaseScale,
                                                p.MinSensivity,
                                                p.MaxSensivity,
                                                p.UnlinkTimeScale
                        )));
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
                                !string.IsNullOrEmpty(p.AutomatCharReleaseType) &
                                (p.AutomatReleaseMinCoef.HasValue && p.AutomatReleaseMinCoef > 0) &
                                (p.AutomatReleaseMaxCoef.HasValue && p.AutomatReleaseMaxCoef > 0) &
                                IsDoubleValuesList(p.UnlinkTimeScale)
                                ))

                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (
                                            p.AutomatCharReleaseType,
                                            p.AutomatReleaseMinCoef,
                                            p.AutomatReleaseMaxCoef,
                                            p.UnlinkTimeScale
                                            )
                        )));
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
                                IsDoubleValuesList(p.CurrentScale) &
                                IsDoubleValuesList(p.TimeReleaseIr) &
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
                   .Where(p => !(p.CurrentChoice.HasValue && IsDefined(p.CurrentChoice.Value) &&
                                    p.IsMultiplicityOfCurrentForTmTime.HasValue &&
                                    p.KzInstantCurrentChoice.HasValue && IsDefined(p.KzInstantCurrentChoice.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code,
                            (p.CurrentChoice.HasValue ? GetDescription(p.CurrentChoice.Value) : string.Empty,
                            Convert(p.IsMultiplicityOfCurrentForTmTime),
                            p.KzInstantCurrentChoice.HasValue ? GetDescription(p.KzInstantCurrentChoice.Value) : string.Empty))));
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
                   .Where(p => !(IsDoubleValuesList(p.KzIiScale) &&
                                   IsDoubleValuesList(p.UnlinkTimeElectronicScale)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code,
                                (p.KzIiScale,
                                p.UnlinkTimeElectronicScale))));
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
                   .Where(p => !(IsDoubleValuesList(p.KzKiScale) &&
                                   IsDoubleValuesList(p.UnlinkTimeElectronicScale)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.KzKiScale, p.UnlinkTimeElectronicScale))));
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
        [ReportRule(@"Для автоматических выключателей параметр MultiplScale	""Кратности уставки расцепителя Кm, о.е"" должен быть задан в виде числа формата ""0,##"" или списка чисел, разделенных символом ""/""",
                    3, 123)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_123() {
            //bool isCorrectList(string strValues) {
            //    if (string.IsNullOrWhiteSpace(strValues))
            //        return false;
            //    var values = strValues.Split('/');
            //    foreach (var item in values) {
            //        if (!double.TryParse(item, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("Ru-ru"), out _))
            //            return false;
            //    }
            //    return true;
            //}
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Where(p => (p.IsElMagR == true | p.IsElectronicR == true) & p.CurrentChoice == ElCurrentChoiseEnum.BY_MULTIPLICITY)
                    .Select(p => new { p.Code, p.Series, p.MultiplScale })
                    .ToList()
                    .Where(p => !IsDoubleValuesList(p.MultiplScale))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MultiplScale))
                        ));
                    ;
                }
            }
        }


        [ReportRule(@"Для автоматических выключателей должны быть заполнены расчетные параметры:
ActiveResistance	InductiveResistance
Активное сопротивление полюса R, мОм	Реактивное сопротивление полюса X, мОм",
                    3, 150)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_150() {
            using (var context = connector.Connect()) {
                var errors = context.ElAutomats
                    .Select(p => new { p.Code, p.Series, p.ActiveResistance, p.InductiveResistance })
                    .ToList()
                    .Where(p => !((p.ActiveResistance.HasValue & p.InductiveResistance.HasValue) &&
                                (p.ActiveResistance > 0 & p.InductiveResistance > 0)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.ActiveResistance, p.InductiveResistance))));
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей разброс значения параметра ""Активное сопротивление полюса R, мОм"" не должен превышать 120% от среднего значения.",
                    3, 151)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_151() {
            using (var context = connector.Connect()) {
                var products = context.ElAutomats
                    .Select(p => new { p.Code, p.Purpose, p.Series, p.ActiveResistance })
                    .Where(p => ((p.ActiveResistance.HasValue) &&
                                (p.ActiveResistance > 0)))
                    .ToList()
                    .GroupBy(p => new { p.Purpose, p.Series })
                    .Select(gr => new {
                        gr.Key.Purpose,
                        gr.Key.Series,
                        ActiveResistanceMean = gr.Select(val => val.ActiveResistance).Where(val => val > 0 & val < 1500).Average(),
                        Products = gr
                    })
                    .ToList();
                var errors = products
                    .SelectMany(gr => gr.Products
                        .Where(p => p.ActiveResistance > gr.ActiveResistanceMean * 2.2)
                        .Select(pr => (gr.Purpose, gr.Series, pr.Code, (pr.ActiveResistance))))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не выполняется для {errors.Count} элементов.", errors);
                }
            }
        }
        [ReportRule(@"Для автоматических выключателей разброс значения параметра ""Реактивное сопротивление полюса X, мОм"" не должен превышать 120% от среднего значения.",
                    3, 152)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_152() {
            using (var context = connector.Connect()) {
                var products = context.ElAutomats
                    .Select(p => new { p.Code, p.Purpose, p.Series, p.InductiveResistance })
                    .Where(p => ((p.InductiveResistance.HasValue) &&
                                (p.InductiveResistance > 0)))
                    .ToList()
                    .GroupBy(p => new { p.Purpose, p.Series })
                    .Select(gr => new {
                        gr.Key.Purpose,
                        gr.Key.Series,
                        InductiveResistanceMean = gr.Select(val => val.InductiveResistance).Where(val => val > 0 & val < 1500).Average(),
                        Products = gr
                    })
                    .ToList();
                var errors = products
                    .SelectMany(gr => gr.Products
                        .Where(p => p.InductiveResistance > gr.InductiveResistanceMean * 2.2)
                        .Select(pr => (gr.Purpose, gr.Series, pr.Code, (pr.InductiveResistance))))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не выполняется для {errors.Count} элементов.", errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Коммутационные аппараты\Автоматические выключатели"" должна быть назначена графика",
                            3, 112)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElAutomat))]
        public void Rule_03_162() {
            using (var context = connector.Connect()) {
                var products = context.ElAutomats
                   .Select(p => new { p.Code, p.Series, p.DbGraphicRef })
                   .ToList();
                var errors = products
                    .Where(p => !(p.DbGraphicRef.HasValue & p.DbGraphicRef > 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не назначена графика {errors.Count} элементов \"Коммутационные аппараты\\Автоматические выключатели\".",
                        errors.Select(p => (p.Series, p.Code)));
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
                    .Where(p => !((p.Voltage.HasValue & p.NominalCurrent.HasValue & p.MaxCommutation.HasValue) &&
                                (p.Voltage > 0 & p.NominalCurrent > 0 & p.DbPoleCountEnum > 0 & p.MaxCommutation > 0) &&
                                (TryParseAsInt(p.CurrentScale, out int cs).IsSuccess & cs > 0) &&
                                p.DbPoleCountEnum.HasValue && IsDefined(p.DbPoleCountEnum.Value)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Voltage, p.NominalCurrent,
                                p.DbPoleCountEnum.HasValue ? GetDescription(p.DbPoleCountEnum.Value) : string.Empty,
                                TryParseAsInt(p.CurrentScale, out _).Value, p.MaxCommutation))));
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
                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList()
                   .Where(p => !((p.MountType.HasValue) &&
                                IsDefined(p.MountType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty))));
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
                    .Where(p => !((p.Voltage.HasValue & p.NominalCurrent.HasValue & p.MaxDynCurrent.HasValue) &&
                                (p.Voltage > 0 & p.NominalCurrent > 0 & p.MaxDynCurrent > 0) &&
                                p.Poles.HasValue && IsDefined(p.Poles.Value)))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Voltage, p.NominalCurrent,
                                    p.Poles.HasValue ? GetDescription(p.Poles.Value) : string.Empty,
                                    p.MaxDynCurrent))));
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

                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList()
                   .Where(p => !((p.MountType.HasValue) &&
                                IsDefined(p.MountType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty))));
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
                    .Select(p => new { p.Code, p.Series, p.DbVoltage, p.DbPoleCountEnum, p.DbNominalCur, p.CurrentScaleUzo })
                   .ToList()
                  .Where(p => !((p.DbVoltage.HasValue & p.DbNominalCur.HasValue) &&
                                (p.DbVoltage > 0 & p.DbNominalCur > 0) &&
                                p.DbPoleCountEnum.HasValue && IsDefined(p.DbPoleCountEnum.Value) &&
                                (TryParseAsDouble(p.CurrentScaleUzo, out double value).IsSuccess &&
                                IsCorrectCurrentScaleUzo(value))))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbVoltage, p.DbNominalCur, convert(p.CurrentScaleUzo),
                        p.DbPoleCountEnum.HasValue ? GetDescription(p.DbPoleCountEnum.Value) : string.Empty))));
                }
            }

        }
        [ReportRule(@"Для УЗО должны быть заполнен параметр: MaxCordS Макс. сечение проводника, мм^2",
             3, 402)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElUzo))]
        public void Rule_03_402() {
            using (var context = connector.Connect()) {
                var errors = context.ElUzoes
                  .Where(p => !((p.MaxCordS.HasValue) &&
                                (p.MaxCordS > 0)))
                    .Select(p => new { p.Code, p.Series, p.MaxCordS })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MaxCordS))));
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
                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList()
                   .Where(p => !(p.MountType.HasValue &&
                                IsDefined(p.MountType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty))));
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



        #region 500 Пускатели, контакторы и реле

        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должны быть внесены параметры:
Voltage	NominalCurrent	DbPoleCountEnum	CoilControlVoltage
Номинальное напряжение Un, В	Номинальный ток In, А	Количество полюсов	Напряжение катушки управления, В",
            3, 501)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_501() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters

                    .Select(p => new { p.Code, p.Series, p.Voltage, p.NominalCurrent, p.DbPoleCountEnum, p.CoilControlVoltage })
                   .ToList()
                   .Where(p => !((p.NominalCurrent.HasValue & p.Voltage.HasValue & p.CoilControlVoltage.HasValue) &&
                                (p.NominalCurrent > 0 & p.CoilControlVoltage > 0) &&
                                p.DbPoleCountEnum.HasValue && IsDefined(p.DbPoleCountEnum.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Voltage, p.NominalCurrent,
                        p.DbPoleCountEnum.HasValue ? GetDescription(p.DbPoleCountEnum.Value) : string.Empty,
                        p.CoilControlVoltage))));
                }
            }

        }
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должны быть заполнен параметр: MaxCordS Макс. сечение проводника, мм^2",
             3, 502)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_502() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
                  .Where(p => !((p.MaxCordS.HasValue) &&
                                (p.MaxCordS > 0)))
                    .Select(p => new { p.Code, p.Series, p.MaxCordS })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MaxCordS))));
                }
            }

        }
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должны быть заполнены габариты:
DbHeight	DbWidth	DbDepth   DbIsModule
Высота, мм	Ширина, мм	Глубина, мм Модульный",
                    3, 503)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_503() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
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
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должен быть заполнен тип монтажа: MountType Крепление",
                    3, 504)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_504() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters

                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList()
                   .Where(p => !(p.MountType.HasValue &&
                                IsDefined(p.MountType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty))));
                }
            }

        }
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должен быть заполнен параметр: DbDeviceType Тип аппарата",
            3, 505)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_505() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters

                    .Select(p => new { p.Code, p.Series, p.DbDeviceType })
                   .ToList()
                   .Where(p => !(p.DbDeviceType.HasValue && IsDefined(p.DbDeviceType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbDeviceType.HasValue ? GetDescription(p.DbDeviceType.Value) : string.Empty))));
                }
            }
        }
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
                        3, 510)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_510() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
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
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" с указанием ""Модульный"" должен быть заполнен параметр: DbModuleCount Количество модулей 18мм, шт",
                    3, 511)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_511() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
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
        [ReportRule(@"Для элементов таблицы ""Пускатели, контакторы и реле"" должен быть заполнен параметр: DbDeviceType Тип аппарата",
                    3, 512)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_512() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
                   .Select(p => new { p.Code, p.Series, p.DbDeviceType })
                   .ToList()
                   .Where(p => !(p.DbDeviceType.HasValue &&
                                IsDefined(p.DbDeviceType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbDeviceType.HasValue ? GetDescription(p.DbDeviceType.Value) : string.Empty))));
                }
            }
        }
        [ReportRule(@"Для пускателей должны быть заполнены параметры:
DbStarterType	LowBoundFaultCurrent	HightBoundFaultCurrent	ContactNOCount	ContactNZCount
Тип пускателя или контактора	Нижняя граница диапазона несрабатывания, А	Верхняя граница диапазона несрабатывания, А	Количество НО контактов	Количество НЗ контактов",
                    3, 513)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_513() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
                    .Where(p => p.DbDeviceType == ElDeviceTypeEnum.STARTER)
                    .Select(p => new { p.Code, p.Series, p.DbStarterType, p.LowBoundFaultCurrent, p.HightBoundFaultCurrent, p.ContactNOCount, p.ContactNZCount })
                    .ToList()
                    .Where(p => !(p.DbStarterType.HasValue && IsDefined(p.DbStarterType.Value) &&
                                p.LowBoundFaultCurrent.HasValue && p.LowBoundFaultCurrent > 0 &&
                                p.HightBoundFaultCurrent.HasValue && p.HightBoundFaultCurrent > 0 &&
                                TryParseAsInt(p.ContactNOCount, out var intValue).IsSuccess && intValue >= 0 &&
                                TryParseAsInt(p.ContactNZCount, out var intValue1).IsSuccess && intValue1 >= 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbStarterType.HasValue ? GetDescription(p.DbStarterType.Value) : string.Empty,
                                p.LowBoundFaultCurrent,
                                p.HightBoundFaultCurrent,
                                TryParseAsInt(p.ContactNOCount, out _).Value,
                                TryParseAsInt(p.ContactNZCount, out _).Value))));
                }
            }
        }
        [ReportRule(@"Для контакторов должны быть заполнены параметры:
DbStarterType	ContactNOCount	ContactNZCount
Тип пускателя или контактора	Количество НО контактов	Количество НЗ контактов",
                    3, 514)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_514() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
                    .Where(p => p.DbDeviceType == ElDeviceTypeEnum.CONTACTOR)
                    .Select(p => new { p.Code, p.Series, p.DbStarterType, p.ContactNOCount, p.ContactNZCount })
                    .ToList()
                    .Where(p => !(p.DbStarterType.HasValue && IsDefined(p.DbStarterType.Value) &&
                                TryParseAsInt(p.ContactNOCount, out var intValue).IsSuccess && intValue >= 0 &&
                                TryParseAsInt(p.ContactNZCount, out var intValue1).IsSuccess && intValue1 >= 0))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbStarterType.HasValue ? GetDescription(p.DbStarterType.Value) : string.Empty,
                                TryParseAsInt(p.ContactNOCount, out _).Value,
                                TryParseAsInt(p.ContactNZCount, out _).Value))));
                }
            }
        }
        [ReportRule(@"Для реле должен быть заполнен параметр:CurrentRelayType Тип реле",
                    3, 515)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElStarter))]
        public void Rule_03_515() {
            using (var context = connector.Connect()) {
                var errors = context.ElStarters
                    .Where(p => p.DbDeviceType == ElDeviceTypeEnum.RELAY)
                    .Select(p => new { p.Code, p.Series, p.CurrentRelayType })
                    .ToList()
                    .Where(p => string.IsNullOrEmpty(p.CurrentRelayType))
                    .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (string.Empty))));
                }
            }
        }
        #endregion
        #region 600 Ограничители перенапряжения
        [ReportRule(@"Для ограничителей перенапряжения должны быть внесены параметры:
Voltage	MaxVoltage	DischargeCurrent	MaxDischargeCurrent	PoleCount
Номинальное напряжение Un, В	Максимальное рабочее напряжение Uc, В	Номинальный разрядный ток In, кА	Максимальный разрядный ток Imax, кА	Количество полюсов",
            3, 601)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElOvervoltageSuppressor))]
        public void Rule_03_601() {
            using (var context = connector.Connect()) {
                var errors = context.ElOvervoltageSuppressors

                    .Select(p => new { p.Code, p.Series, p.Voltage, p.MaxVoltage, p.DischargeCurrent, p.MaxDischargeCurrent, p.PoleCount })
                   .ToList()
                   .Where(p => !((p.Voltage.HasValue & p.MaxVoltage.HasValue & p.DischargeCurrent.HasValue & p.MaxDischargeCurrent.HasValue) &&
                                (p.Voltage > 0 & p.MaxVoltage > 0 & p.DischargeCurrent > 0 & p.MaxDischargeCurrent > 0) &&
                                p.PoleCount.HasValue && IsDefined(p.PoleCount.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Voltage, p.MaxVoltage, p.DischargeCurrent, p.MaxDischargeCurrent,
                        p.PoleCount.HasValue ? GetDescription(p.PoleCount.Value) : string.Empty))));
                }
            }

        }
        [ReportRule(@"Для ограничителей перенапряжения должны быть заполнен параметр: MaxCordS Макс. сечение проводника, мм^2",
     3, 602)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElOvervoltageSuppressor))]
        public void Rule_03_602() {
            using (var context = connector.Connect()) {
                var errors = context.ElOvervoltageSuppressors
                  .Where(p => !((p.MaxCordS.HasValue) &&
                                (p.MaxCordS > 0)))
                    .Select(p => new { p.Code, p.Series, p.MaxCordS })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MaxCordS))));
                }
            }

        }
        [ReportRule(@"Для ограничителей перенапряжения должны быть заполнены габариты:
DbHeight	DbWidth	DbDepth   IsModular
Высота, мм	Ширина, мм	Глубина, мм Модульный",
                    3, 603)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElOvervoltageSuppressor))]
        public void Rule_03_603() {
            using (var context = connector.Connect()) {
                var errors = context.ElOvervoltageSuppressors
                  .Where(p => !((p.DbHeight.HasValue & p.DbWidth.HasValue & p.DbDepth.HasValue & p.IsModular.HasValue) &&
                                (p.DbHeight > 0 & p.DbWidth > 0 & p.DbDepth > 0)))
                    .Select(p => new { p.Code, p.Series, p.DbHeight, p.DbWidth, p.DbDepth, p.IsModular })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.DbHeight, p.DbWidth, p.DbDepth, Convert(p.IsModular.HasValue)))));
                }
            }

        }
        [ReportRule(@"Для ограничителей перенапряжения должен быть заполнен тип монтажа: MountType Крепление",
                    3, 604)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElOvervoltageSuppressor))]
        public void Rule_03_604() {
            using (var context = connector.Connect()) {
                var errors = context.ElOvervoltageSuppressors

                    .Select(p => new { p.Code, p.Series, p.MountType })
                   .ToList()
                   .Where(p => !(p.MountType.HasValue &&
                                IsDefined(p.MountType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty))));
                }
            }

        }
        [ReportRule(@"Для ограничителей перенапряжения с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
                        3, 610)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElOvervoltageSuppressor))]
        public void Rule_03_610() {
            using (var context = connector.Connect()) {
                var errors = context.ElOvervoltageSuppressors
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
        [ReportRule(@"Для ограничителей перенапряжения с указанием ""Модульный"" должен быть заполнен параметр: ModuleCount Количество модулей 18мм, шт",
                    3, 611)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElOvervoltageSuppressor))]
        public void Rule_03_611() {
            using (var context = connector.Connect()) {
                var errors = context.ElOvervoltageSuppressors
                    .Where(p => p.IsModular == true)
                    .Where(p => !((p.ModuleCount.HasValue) &&
                                (p.ModuleCount > 0)))
                    .Select(p => new { p.Code, p.Series, p.ModuleCount })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.ModuleCount))));
                }
            }
        }
        #endregion
        #region 700 Допю оборудование КА
        [ReportRule(@"Для элементов таблицы ""Доп. оборудование КА"" должны быть внесены параметры:
FiderUtilityType	InstallationType
Тип устройства	Монтаж",
    3, 701)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElFiderUtility))]
        public void Rule_03_701() {
            using (var context = connector.Connect()) {
                var errors = context.ElFiderUtilities

                    .Select(p => new { p.Code, p.Series, p.FiderUtilityType, p.InstallationType })
                   .ToList()
                   .Where(p => !(p.FiderUtilityType.HasValue && IsDefined(p.FiderUtilityType.Value) &&
                                p.InstallationType.HasValue && IsDefined(p.InstallationType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (
                        p.FiderUtilityType.HasValue ? GetDescription(p.FiderUtilityType.Value) : string.Empty,
                        p.InstallationType.HasValue ? GetDescription(p.InstallationType.Value) : string.Empty
                        ))));
                }
            }

        }
        [ReportRule(@"Для элементов таблицы ""Доп. оборудование КА"" с типом монтажа:""Внутрь шкафа"" должны быть внесены параметры:
MountType	DbIsModule	DimensionType
Крепление	Модульный	Габаритный тип",
    3, 702)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElFiderUtility))]
        public void Rule_03_702() {
            using (var context = connector.Connect()) {
                var errors = context.ElFiderUtilities
                    .Where(p => p.InstallationType == ElInstallationType.InsideShell)
                    .Select(p => new { p.Code, p.Series, p.MountType, p.DbIsModule, p.DimensionType })
                   .ToList()
                   .Where(p => !(p.MountType.HasValue && IsDefined(p.MountType.Value) &&
                                p.DbIsModule.HasValue &&
                                p.DimensionType.HasValue && IsDefined(p.DimensionType.Value)))
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (
                        p.MountType.HasValue ? GetDescription(p.MountType.Value) : string.Empty,
                        Convert(p.DbIsModule),
                        p.DimensionType.HasValue ? GetDescription(p.DimensionType.Value) : string.Empty
                        ))));
                }
            }

        }
        [ReportRule(@"Для элементов таблицы ""Доп. оборудование КА"" с типом монтажа:""Внутрь шкафа"" и с креплением на монтажную рейку должен быть заполнен параметр: RailMountTypeFlagged Тип монтажной рейки",
                        3, 703)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElFiderUtility))]
        public void Rule_03_703() {
            using (var context = connector.Connect()) {
                var errors = context.ElFiderUtilities
                   .Where(p => p.InstallationType == ElInstallationType.InsideShell & (p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL | p.MountType == ElControlRegisterDeviceMountType.MOUNT_RAIL_OR_BOARD))
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
        [ReportRule(@"Для элементов таблицы ""Доп. оборудование КА"" с типом монтажа:""Внутрь шкафа"" и с указанием ""Модульный"" должен быть заполнен параметр: DbModuleCount Количество модулей 18мм, шт",
            3, 704)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElFiderUtility))]
        public void Rule_03_704() {
            using (var context = connector.Connect()) {
                var errors = context.ElFiderUtilities
                    .Where(p => p.InstallationType == ElInstallationType.InsideShell & p.DbIsModule == true)
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
        [ReportRule(@"Для элементов таблицы ""Доп. оборудование КА"" с типом монтажа:""Внутрь шкафа"" и с указанием Габаритный тип:""Параллелепипед"" должны быть заполнены параметры:
Height	Width	Depth
Высота, мм	Ширина, мм	Глубина, мм",
            3, 705)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElFiderUtility))]
        public void Rule_03_705() {
            using (var context = connector.Connect()) {
                var errors = context.ElFiderUtilities
                    .Where(p => p.InstallationType == ElInstallationType.InsideShell & p.DimensionType == ElDimensionType.Parallelepiped)
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
        [ReportRule(@"Для элементов таблицы ""Доп. оборудование КА"" с типом монтажа:""Внутрь шкафа"" и с указанием Габаритный тип:""Цилиндр"" должны быть заполнены параметры:
Diameter	Depth
Диаметр, мм	Глубина, мм",
            3, 706)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ElFiderUtility))]
        public void Rule_03_706() {
            using (var context = connector.Connect()) {
                var errors = context.ElFiderUtilities
                    .Where(p => p.InstallationType == ElInstallationType.InsideShell & p.DimensionType == ElDimensionType.Cylinder)
                    .Where(p => !((p.Diameter.HasValue & p.Depth.HasValue) &&
                                (p.Diameter > 0 & p.Depth > 0)))
                    .Select(p => new { p.Code, p.Series, p.Diameter, p.Depth })
                   .ToList();
                if (errors.Any()) {
                    FailRuleTest($"Не заполнены параметры для {errors.Count} элементов.",
                        errors.Select(p => (p.Series, p.Code, (p.Diameter, p.Depth))));
                }
            }
        }
        #endregion
        

    }
}
