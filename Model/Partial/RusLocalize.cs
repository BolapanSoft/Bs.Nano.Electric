﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nano.Electric {
    /// <summary>
    /// Лотки\\Секции прямые\\Лотки
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Секции прямые\\Лотки")]
    public partial class ScsGutterCanal { }
    /// <summary>
    /// Лотки\\Секции прямые\\Крышки
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Секции прямые\\Крышки")]
    public partial class DbScsGutterCover { }
    /// <summary>
    /// Лотки\\Секции прямые\\Перегородки
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Секции прямые\\Перегородки")]
    public partial class DbScsGutterPartition { }
    /// <summary>
    /// Лотки\\Секции соединительные\\Лотки
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Секции соединительные\\Лотки")]
    public partial class ScsGcFitting { }
    /// <summary>
    /// Лотки\\Секции соединительные\\Крышки
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Секции соединительные\\Крышки")]
    public partial class DbScsGcCoverUnit { }
    /// <summary>
    /// Лотки\\Аксессуары лотков
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Аксессуары лотков")]
    public partial class DbScsGcAccessoryUnit { }
    /// <summary>
    /// Крепления лотков\\Элементы крепления
    /// </summary>
    [DefaultLocalizeValue("Крепления лотков\\Элементы крепления")]
    public partial class ScsGutterBolting { }
    /// <summary>
    /// Крепления лотков\\Аксессуары крепления
    /// </summary>
    [DefaultLocalizeValue("Крепления лотков\\Аксессуары крепления")]
    public partial class DbScsGcBoltingAccessoryUnit { }
    /// <summary>
    /// Крепления лотков\\Конфигурации узлов крепления
    /// </summary>
    [DefaultLocalizeValue("Крепления лотков\\Конфигурации узлов крепления")]
    public partial class DbScsGutterUtilitySet { }
    /// <summary>
    /// Лотки\\Конфигурации соединительных элементов
    /// </summary>
    [DefaultLocalizeValue("Лотки\\Конфигурации соединительных элементов")]
    public partial class DbScsGcSeriaConfigiration { }
    /// <summary>
    /// Материалы и комплектация\\Материалы
    /// </summary>
    [DefaultLocalizeValue("Материалы и комплектация\\Материалы")]
    public partial class CaeMaterialUtility { }
    /// <summary>
    /// Конфигурации КНС\\Конфигурации трасс лотков
    /// </summary>
    [DefaultLocalizeValue("Конфигурации КНС\\Конфигурации трасс лотков")]
    public partial class DbGcMountSystem { }

    [DefaultLocalizeValue("Короба")]
    public partial class ScsCabelCanal { }

    [DefaultLocalizeValue("Короба\\Соединительные элементы")]
    public partial class ScsCableFitting { }

    [DefaultLocalizeValue("Короба\\Перегородки")]
    public partial class DbCableCanalPartition { }

    [DefaultLocalizeValue("Короба\\Крышки")]
    public partial class DcCableCanalCover { }
    [DefaultLocalizeValue("Трубы")]
    public partial class ScsPipe { }

    [DefaultLocalizeValue("Трубы\\Соединительные элементы")]
    public partial class ScsTubeFitting { }

    [DefaultLocalizeValue("Трубы\\Конфигурации соединительных элементов")]
    public partial class DbScsTubeSeriesConfiguration { }

    [DefaultLocalizeValue("Конфигурации КНС\\Конфигурации трасс труб")]
    public partial class DbTbMountSystem { }

    [DefaultLocalizeValue("Конфигурации КНС\\Конфигурации трасс настенных коробов")]
    public partial class DbCcMountSystem { }
    /// <summary>
    /// Материалы и комплектация\\Комплектации материалов
    /// </summary>
    [DefaultLocalizeValue("Материалы и комплектация\\Комплектации материалов")]
    public partial class DbCaeMaterialUtilitySet { }

    [DefaultLocalizeValue("Изображения")]
    public partial class DbImage { }

    [DefaultLocalizeValue("Графика")]
    public partial class DbGraphic { }

    [DefaultLocalizeValue("Dwg Файлы")]
    public partial class DbDwgFiles { }



    [DefaultLocalizeValue("Параметры исполнения\\Степень защиты IP")]
    public partial class SafeDegree { }

    [DefaultLocalizeValue("Маркировка по взрывозащите")]
    public partial class ExplodeSafeLevel { }
    [DefaultLocalizeValue("Рабочие места\\Типы портов")]
    public partial class ScsPortType { }

    [DefaultLocalizeValue("Кабельно-проводниковая продукция\\Материалы изоляции кабелей")]
    public partial class ElWireIsolationMaterial { }

    [DefaultLocalizeValue("Кабельно-проводниковая продукция\\Материалы жил")]
    public partial class ElWireConductMaterial { }

    [DefaultLocalizeValue("Кабельно-проводниковая продукция\\Марки кабелей и проводов")]
    public partial class ElWireMark { }
    [DefaultLocalizeValue("Кабельно-проводниковая продукция\\Кабели и провода")]
    public partial class ElWire { }
    [DefaultLocalizeValue("Кабельно-проводниковая продукция\\Типы кабельных систем")]
    public partial class ScsCableSystemType { }
    [DefaultLocalizeValue("Кабельно-проводниковая продукция\\Патч-корды")]
    public partial class ScsPatchCord { }

    [DefaultLocalizeValue("Распределительные устройства\\Шкафы")]
    public partial class ElBoard { }

    [DefaultLocalizeValue("Распределительные устройства\\Блоки")]
    public partial class ElBlock { }

    [DefaultLocalizeValue("Распределительные устройства\\Ящики")]
    public partial class ElBox { }

    [DefaultLocalizeValue("Распределительные устройства\\Ящики с трансформатором")]
    public partial class ElShieldingUnit { }

    [DefaultLocalizeValue("Распределительные устройства\\Доп. оборудование РУ")]
    public partial class ElBoardUtility { }

    [DefaultLocalizeValue("Электроприемники\\Светильники\\Таблица КИ")]
    public partial class DbLtKiTable { }

    [DefaultLocalizeValue("Параметры исполнения\\Климатическое исполнение")]
    public partial class ClimateTable { }


    //[DefaultLocalizeValue("Доп. оборудование ЭУИ")]
    //public partial class ElSocketUtility { }

    [DefaultLocalizeValue("Электроприемники\\Светильники\\Доп. оборудование светильников")]
    public partial class ElLightUtility { }

    [DefaultLocalizeValue("Электроприемники\\Светильники")]
    public partial class ElLighting { }

    [DefaultLocalizeValue("Электроприемники\\Светильники\\Лампы")]
    public partial class ElLamp { }

    [DefaultLocalizeValue("Электроприемники\\Нагреватели")]
    public partial class ElDbHeater { }

    [DefaultLocalizeValue("Электроприемники\\Асинхронные двигатели")]
    public partial class ElDbEngine { }

    [DefaultLocalizeValue("Электроприемники\\Комплексные ЭП")]
    public partial class ElDbComplex { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Автоматические выключатели")]
    public partial class ElAutomat { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Предохранители")]
    public partial class ElSafeDevice { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Рубильники")]
    public partial class ElKnifeSwitch { }

    [DefaultLocalizeValue("Коммутационные аппараты\\УЗО")]
    public partial class ElUzo { }

    [DefaultLocalizeValue("Коммутационные аппараты\\УЗДП")]
    public partial class DbElUzdp { }
    [DefaultLocalizeValue("Коммутационные аппараты\\Пускатели, контакторы и реле")]
    public partial class ElStarter { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Ограничители перенапряжения")]
    public partial class ElOvervoltageSuppressor { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Частотные преобразователи")]
    public partial class ElFrequenceTransformer { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Оболочки")]
    public partial class ElCasing { }

    [DefaultLocalizeValue("Коммутационные аппараты\\Доп. оборудование КА")]
    public partial class ElFiderUtility { }

    [DefaultLocalizeValue("Общая информация")]
    public partial class DSInformation { }

  
    [DefaultLocalizeValue("Электроустановочные изделия\\Выключатели")]
    public partial class DbElSwitch { }

    [DefaultLocalizeValue("Электроустановочные изделия\\Переключатели на два направления")]
    public partial class DbElCSwitch { }
    [DefaultLocalizeValue("Электроустановочные изделия\\Доп. оборудование ЭУИ")]
    public partial class ElSocketUtility { }

    [DefaultLocalizeValue("Электроустановочные изделия\\Разветвительные коробки")]
    public partial class ElDbCase { }
    [DefaultLocalizeValue("Комплектации ЭУИ")]
    public partial class DbElSocketUtilitySet { }

    [DefaultLocalizeValue("Электроустановочные изделия\\Розетки")]
    public partial class DbElSocket { }

    [DefaultLocalizeValue("Комплектации светильников")]
    public partial class DbElLightUtilitySet { }

    [DefaultLocalizeValue("Комплектации КА")]
    public partial class DbElFiderUtilitySet { }

 
    [DefaultLocalizeValue("Комплектации РУ")]
    public partial class DbElBoardUtilitySet { }

    [DefaultLocalizeValue("Приборы контроля и учета\\Кнопочные посты управления")]
    public partial class ElPushButtonStation { }
    [DefaultLocalizeValue("Приборы контроля и учета\\Амперметры")]
    public partial class ElAmperemeter { }
    [DefaultLocalizeValue("Приборы контроля и учета\\Приборы управления")]
    public partial class ElControlDevice { }
    [DefaultLocalizeValue("Приборы контроля и учета\\Счетчики")]
    public partial class ElCounter { }
    [DefaultLocalizeValue("Приборы контроля и учета\\Трансформаторы тока")]
    public partial class ElCurrentTransformer { }
    [DefaultLocalizeValue("Приборы контроля и учета\\Вольтметры")]
    public partial class ElVoltmeter { }

    [DefaultLocalizeValue("Монтажные шкафы и панели. Коммутационные панели")]
    public partial class ScsSwitchUtpPanel { }
    [DefaultLocalizeValue("Монтажные шкафы и панели. Кабельные организаторы")]
    public partial class ScsOrganaizerPanel { }
    [DefaultLocalizeValue("Монтажные шкафы и панели. Блоки розеток")]
    public partial class ScsSwitchSocketPanel { }
    [DefaultLocalizeValue("Монтажные шкафы и панели. Аксессуары шкафов")]
    public partial class ScsShellUtility { }
    [DefaultLocalizeValue("Монтажные шкафы и панели. Аксессуары панелей")]
    public partial class ScsPanelUtilityUnit { }
   [DefaultLocalizeValue("Монтажные шкафы и панели. Активное оборудование")]
    public partial class ScsCommutatorPanel { }
    [DefaultLocalizeValue("Монтажные шкафы и панели. Монтажные шкафы 19″")]
    public partial class ScsShellDistr { }
    [DefaultLocalizeValue("Приборы. Сервисные колонны")]
    public partial class ScsServiceColumn { }
    [DefaultLocalizeValue("Приборы. Аксессуары сервисных колонн")]
    public partial class ScsServiceColumnUtilityUnit { }
    [DefaultLocalizeValue("Приборы. Лючки")]
    public partial class ScsHatch { }
     [DefaultLocalizeValue("Приборы. Аксессуары лючков")]
    public partial class ScsHatchUtilityUnit { }
    [DefaultLocalizeValue("Приборы. Розетки телекоммуникационные")]
    public partial class ScsUtpSocket { }
    /* 

    [DefaultLocalizeValue("")]
    public partial class ___ { }
    [DefaultLocalizeValue("")]
    public partial class ___ { }
    [DefaultLocalizeValue("")]
    public partial class ___ { }
    [DefaultLocalizeValue("")]
    public partial class ___ { }

    [DefaultLocalizeValue("")]
    public partial class ___ { }

    [DefaultLocalizeValue("")]
    public partial class ___ { }

    */
}
