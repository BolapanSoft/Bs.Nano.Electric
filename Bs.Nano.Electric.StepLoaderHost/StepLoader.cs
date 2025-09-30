using Bs.Nano.Electric.Interop;
using Microsoft.EntityFrameworkCore;
using Nano.Electric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Bs.Nano.Electric.StepLoaderHost {
    [Serializable]
    public class StepLoader {
        #region Import/Save Graphics
        public int? LoadGraphic(string code, string category, string graphicName, string strConnection, string stepFileName, string graphicPath, string cadWisePath) {
            ContextConnector connector = new ContextConnector(strConnection);
            DbGraphic? graphic;
            using (var context = connector.Connect()) {
                DbSet<DbGraphic> set = context.DbGraphics;
                int id = context.GetNextId<DbGraphic>();// Make.GetMaxId(set) + 1;
                graphic = new();
                graphic.Id = id++;
                graphic.Name = graphicName;

                graphic.AutoSelectSize = false;
                graphic.Category = category;
                using (var transaction = context.Database.BeginTransaction()) {
                    try {
                        LoadDbGraphic(graphic, stepFileName, graphicPath, cadWisePath);
                        set.Add(graphic);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex) {
                        transaction.Rollback();
                        throw new InvalidOperationException($"Для артикула {code} произошла ошибка при загрузке файла 3Д модели \"{stepFileName}\" .", ex);
                    } 
                }
                return graphic.Id;
            }
        }

        private void LoadDbGraphic(DbGraphic graphic, string stepFileName, string graphicPath, string cadWisePath /*string?[] properties, IReadOnlyDictionary<string, string> row*/) {
            if (File.Exists(Path.Combine(graphicPath, stepFileName))) {

            }
            else if (File.Exists(Path.Combine(graphicPath, stepFileName + ".step"))) {
                stepFileName = stepFileName + ".step";
            }
            else if (File.Exists(Path.Combine(graphicPath, stepFileName + ".stp"))) {
                stepFileName = Path.ChangeExtension(stepFileName, stepFileName + ".stp");
            }
            else if (File.Exists(Path.Combine(graphicPath, Path.ChangeExtension(stepFileName, "step")))) {
                stepFileName = Path.ChangeExtension(stepFileName, "step");
            }
            else if (File.Exists(Path.Combine(graphicPath, Path.ChangeExtension(stepFileName, "stp")))) {
                stepFileName = Path.ChangeExtension(stepFileName, "stp");
            }

            var fi = new FileInfo(Path.Combine(graphicPath, stepFileName));
            //fi.Refresh();
            if (fi.Exists) { // Load from file
                Bs.Nano.Electric.Interop.Service cwInterop = GetService(cadWisePath);
                cwInterop.LoadStep(graphic, fi.FullName);
            }
            else {
                throw new FileNotFoundException($"Файл импорта графики \"{fi.FullName}\" не найден.");
            }
        }
        private static Service GetService(string cadWisePath) {

            var resolver = new Resolver(cadWisePath);
            var assembly = Assembly.LoadFile(Path.Combine(cadWisePath, "Cadwise.Graphic.Db.dll")); // "C Cadwise.Graphic.Db.dll");
            var service = new Bs.Nano.Electric.Interop.Service(cadWisePath);
            return service;
        }

        #endregion

    }
}
