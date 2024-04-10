using Nano.Electric;
using System.Collections.Generic;

namespace Bs.Nano.Electric.Report {
    internal class Resources {
        public const string DbCaeMaterialUtilitySet_Description = "Материалы и комплектация\\Комплектации материалов";
        public const string DbGcMountSystemSet_Description = "Конфигурации КНС\\Конфигурации трасс лотков";
        public const string DbScsGutterUtilitySet_Description = "Крепления лотков\\Конфигурации узлов крепления";
        public const string DbScsGcSeriaConfiguration_Description = "Лотки\\Конфигурации соединительных элементов";
        /*
- КНС.Крепления лотков#Аксессуары крепления 20230601.xlsx
- КНС.Крепления лотков#Элементы крепления 20230601.xlsx
- КНС.Лотки#Аксессуары лотков 20230601.xlsx
- КНС.Лотки#Перегородки 20230601.xlsx
- КНС.Лотки.Секции прямые#Крышки 20230601.xlsx
- КНС.Лотки.Секции прямые#Лотки 20230601.xlsx
- КНС.Лотки.Секции соединительные#Крышки 20230601.xlsx
- КНС.Лотки.Секции соединительные#Лотки 20230601.xlsx
- КНС.Материалы и комплектация#Материалы 20230605.xlsx
 */

        //private static readonly Dictionary<string, string> tableDescriptions = new Dictionary<string, string> {
        //        { nameof(ScsGutterCanal), "КНС.Лотки.Секции прямые#Лотки 20230601.xlsx" },
        //        { nameof(DbScsGutterCover), "КНС.Лотки.Секции прямые#Крышки 20230601.xlsx" },
        //        { nameof(DbScsGutterPartition), "КНС.Лотки#Перегородки 20230601.xlsx" },
        //        { nameof(ScsGcFitting), "КНС.Лотки.Секции соединительные#Лотки 20230601.xlsx" },
        //        { nameof(DbScsGcCoverUnit), "КНС.Лотки.Секции соединительные#Крышки 20230601.xlsx" },
        //        { nameof(DbScsGcAccessoryUnit), "КНС.Лотки#Аксессуары лотков 20230601.xlsx" },
        //        { nameof(ScsGutterBolting), "КНС.Крепления лотков#Элементы крепления 20230601.xlsx" },
        //        { nameof(DbScsGcBoltingAccessoryUnit), "КНС.Крепления лотков#Аксессуары крепления 20230601.xlsx" },
        //        { nameof(CaeMaterialUtility), "КНС.Материалы и комплектация#Материалы 20230605.xlsx" }
        //    };
        private static readonly Dictionary<string, string> tableImageCategories = new Dictionary<string, string> {
                { nameof(ScsGutterCanal), "Секции прямые\\Лотки" },
                { nameof(DbScsGutterCover), "Секции прямые\\Крышки" },
                { nameof(DbScsGutterPartition), "Секции прямые\\Перегородки" },
                { nameof(ScsGcFitting), "Секции соединительные\\Лотки" },
                { nameof(DbScsGcCoverUnit), "Секции соединительные\\Крышки" },
                { nameof(DbScsGcAccessoryUnit), "Аксессуары лотков" },
                { nameof(ScsGutterBolting), "Крепления лотков\\Элементы крепления" },
                { nameof(DbScsGcBoltingAccessoryUnit), "Крепления лотков\\Аксессуары крепления" },
                { nameof(CaeMaterialUtility), "Материалы и комплектация\\Материалы" },
                { nameof(DbScsGutterUtilitySet), "Крепления лотков\\Конфигурации узлов крепления" },
                { nameof(DbGcMountSystem), "Конфигурации КНС\\Конфигурации трасс лотков" },

           };
        public static IReadOnlyDictionary<string, string> ImageCategory { get { return tableImageCategories; } }
    }
}
