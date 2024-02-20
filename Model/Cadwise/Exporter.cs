using Cadwise.Data;
using Cadwise.Reflection;
using Cadwise.Studio.Forms;
using Cadwise.Studio.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nanoElectric.Cadwise {
    internal class Exporter {
        public static void ExportKit(DbKitItem kit, DbKitItem newKitItem, IDataSource dest) {
            DataBaseExporter exporter = new DataBaseExporter(new IExportable[0], dest);
            ExportKitInternal(exporter, kit, newKitItem, null);
        }
        public IDataObject ExportSelectedObject(IDataObject dataObject, IDataSource dest) {
            if (dataObject == null || dataObject.DataSource == this) {
                return dataObject;
            }
            DataBaseExporter dataBaseExporter = new DataBaseExporter(new IExportable[1] { dataObject }, dest);
            dataBaseExporter.Export(true);
            return dataBaseExporter.Exported.FirstOrDefault();
        }
        private static void ExportKitInternal(DataBaseExporter exporter, DbKitItem kit, DbKitItem newKitItem, IProgressState state) {
            ExportOwnStruct( exporter, kit, newKitItem, state);
            ExportChildren( exporter, kit, newKitItem, state);
        }
        private static void ExportOwnStruct(DataBaseExporter exporter, DbKitItem kit, DbKitItem newKitItem, IProgressState state) {
            PropertyInfo[] properties = kit.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties) {
                DatabaseSerializableAttribute[] databaseSerializableAttribute = propertyInfo.GetDatabaseSerializableAttribute();
                if (databaseSerializableAttribute.Length == 0) {
                    continue;
                }
                switch (databaseSerializableAttribute[0].PropertyType) {
                    case DbSerializablePropertyType.Primitive:
                        if (propertyInfo.CanWrite) {
                            object value = propertyInfo.GetValue(kit, null);
                            propertyInfo.SetValue(newKitItem, value, null);
                        }
                        break;
                    case DbSerializablePropertyType.PrimitiveGroup: {
                            object value2 = propertyInfo.GetValue(kit, null);
                            if (propertyInfo.CanWrite) {
                                propertyInfo.SetValue(newKitItem, value2, null);
                            }
                            if (ReflectionUtils.GetReflectionConvertor(propertyInfo) is IDbPrimitiveGroupFieldConverter dbPrimitiveGroupFieldConverter) {
                                dbPrimitiveGroupFieldConverter.GetDependentObjects(kit, value2).ToList().ForEach(delegate (IDataObject dataObject) {
                                    exporter.Export(dataObject);
                                });
                            }
                            break;
                        }
                    case DbSerializablePropertyType.DbObject: {
                            if (!(propertyInfo.GetValue(kit, null) is IDbIdProvider dbIdProvider)) {
                                break;
                            }
                            IDataObject dataObject2 = kit.DataSource.DataTypedSources[dbIdProvider.GetType()]?.GetDataObject(dbIdProvider.DbId);
                            if (dataObject2 != null) {
                                IDataObject dataObject3 = exporter.Export(dataObject2);
                                if (dataObject3 != null) {
                                    propertyInfo.SetValue(newKitItem, dataObject3.Object, null);
                                }
                            }
                            break;
                        }
                }
            }
        }

        private static void ExportChildren(DataBaseExporter exporter, DbKitItem kit, DbKitItem newKitItem, IProgressState state) {
            if (!newKitItem.NeedLoadChildren) {
                return;
            }
            newKitItem.Children.Clear();
            foreach (IItem child in kit.Children) {
                if (child is DbKitItem dbKitItem && Activator.CreateInstance(dbKitItem.GetType()) is DbKitItem dbKitItem2) {
                    newKitItem.Children.Add(dbKitItem2);
                    ExportKitInternal(exporter, dbKitItem, dbKitItem2, state);
                }
            }
        }

    }
}
