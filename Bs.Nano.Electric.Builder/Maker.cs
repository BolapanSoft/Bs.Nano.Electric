// Ignore Spelling: Iek Tprod

using Microsoft.Extensions.Logging;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Data.Common;
#if NET48
using System.Data.Entity; // EF6
#else
using Microsoft.EntityFrameworkCore; // EF Core
#endif
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Bs.Nano.Electric.Builder {
    public partial class Maker {
        internal const System.Globalization.NumberStyles NumberStyle =
            NumberStyles.AllowLeadingWhite |
            NumberStyles.AllowTrailingWhite |
            NumberStyles.AllowLeadingSign |
            NumberStyles.AllowDecimalPoint;
        private readonly IElectricBuilderConfiguration configuration;
        private readonly ILogger logger;
        private readonly ResourceManager resourceManager;
        //private readonly string outDir;
        //private readonly string dbTemplate;
        //private readonly string imageDir;
        //private readonly string graphicPath;
        //private readonly ILogger logger = DI.Provider.GetService<ILogger<Make>>().NotNull();
        //private static readonly SortedDictionary<string, (string tableName, int id)> productsCache = new();
        //private readonly Lazy<Bs.Nano.Electric.Interop.Service> lzService;
        //private readonly Lazy<XmlLoader> lzXmlLoader;

        internal Maker(ILogger logger, ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            this.logger = logger;
            this.configuration = configuration;
            this.resourceManager = resourceManager;
            //lzXmlLoader = new Lazy<XmlLoader>(() => new XmlLoader(configuration));
            //imageDir = configuration.ImagesPath;
            //graphicPath = configuration.GraphicsPath;
            //lzService = new(GetSerice(configuration));
            //outDir = new DirectoryInfo(configuration.OutputPath);
            //if (!outDir.Exists) { outDir.Create(); }
            //string templateDbName = configuration.GetSection("DbTemplate").Value ?? "empty 8_5.sdf";
            //dbTemplate = Path.Combine(configuration.TemplatePath, templateDbName);
        }

        //internal string GetDbFileName() {
        //    var vers = typeof(Maker).Assembly.GetName().Version;
        //    //return Path.Combine(Configuration.OutputPath, $"{Configuration.OutputDbName} v{vers.Major}.{vers.Minor}.{vers.Build}-n8.5.sdf");
        //    var dbName = configuration.DbName ?? configuration.OutputDbNameBase;
        //    if (dbName.EndsWith(".sdf")) {
        //        // full name in config
        //        return Path.Combine(configuration.OutputPath, dbName);
        //    }
        //    else {
        //        return Path.Combine(configuration.OutputPath, $"{dbName} v{vers.Major}.{vers.Minor}.{vers.Build}-n8.5.sdf");
        //    }
        //}
        //internal static int GetMaxId<TEntity>(DbSet<TEntity> set) where TEntity : class, IHaveId {
        //    // Берем максимум среди локальных данных, если они есть
        //    int localMax = set.Local.Count > 0
        //        ? set.Local.Max(p => p.Id)
        //        : 0;

        //    // Берем максимум в базе, если там есть записи
        //    int dbMax = set.AsNoTracking().Select(p => p.Id).DefaultIfEmpty(0).Max();

        //    // Возвращаем наибольшее значение
        //    return Math.Max(localMax, dbMax);
        //}
        public static DbImage GetOrCreateDbImage(Context context, string fullSourceFileName, (string? ImageName, string? Description, string Category) target) {
            if (string.IsNullOrEmpty(target.ImageName)) {
                throw new InvalidDataException("Не задано имя файла изображения.");
            }
            string imageName = Path.ChangeExtension(target.ImageName, ".png"); // in nanoCad Electro image always in PNG format
            DbImage? image = context.DbImages.Local.FirstOrDefault(p => p.Text == imageName) ??
                context.DbImages.FirstOrDefault(p => p.Text == imageName);
            if (image is null) {
                ////string imgFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullSourceFileName);
                //if (!File.Exists(fullSourceFileName)) {
                //    throw new FileNotFoundException(fullSourceFileName);
                //}
                //FileInfo fi = new FileInfo(fullSourceFileName);

                //image = LoadImage(context, target.ImageName, target.Category, fi);
                ////var image = context.DbImages.Find(idImg);
                //image.Description = image.Description;
                ////image.Category = target.Category;
                string? foundFile = null;

                if (File.Exists(fullSourceFileName)) {
                    foundFile = fullSourceFileName;
                }
                else {
                    // список допустимых расширений
                    string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tif", ".tiff", ".webp" };

                    string dir = Path.GetDirectoryName(fullSourceFileName)!;
                    string baseName = Path.GetFileNameWithoutExtension(fullSourceFileName);

                    foreach (var ext in allowedExtensions) {
                        string candidate = Path.Combine(dir, baseName + ext);
                        if (File.Exists(candidate)) {
                            foundFile = candidate;
                            break;
                        }
                    }
                }

                if (foundFile is null) {
                    throw new FileNotFoundException($"Не удалось найти файл изображения: {fullSourceFileName}");
                }

                FileInfo fi = new FileInfo(foundFile);

                image = LoadImage(context, target.ImageName, target.Category, fi);
                image.Description = image.Description;
            }
            return image;
        }
        internal static DbImage LoadImage(Context context, string imgName, string category, FileInfo fi) {
            //imgName = string.IsNullOrEmpty(imgName) ? fi.Name : imgName;
            imgName = Path.ChangeExtension(imgName, ".png");
            //DbImage? img;
            //if (string.IsNullOrEmpty(category)) {
            //    img = context.DbImages.FirstOrDefault(img => img.Text == imgName);
            //}
            //else {
            //    img = context.DbImages.FirstOrDefault(img => img.Text == imgName & img.Category == category);
            //}
            var imgId = context.FindImageId(imgName, category);
            DbImage? img = null;
            if (imgId.HasValue) {
                img = context.DbImages.Find(imgId.Value);
            }
            var data = GetData(fi);
            /*using (var stream = new MemoryStream(1024 * 8))*/ {
                using (var image = Image.Load<Rgba32>(data)) {
                    // Определяем размеры (максимум 400px по каждой стороне)
                    int targetWidth = Math.Min(image.Width, 400);
                    int targetHeight = Math.Min(image.Height, 400);
                    // Масштабируем (с сохранением пропорций)
                    image.Mutate(x => x.Resize(new ResizeOptions {
                        Size = new Size(targetWidth, targetHeight),
                        Mode = ResizeMode.Max // сохраняет пропорции, вмещает в рамку
                    }));
                    var pngEncoder = new PngEncoder {
                        CompressionLevel = PngCompressionLevel.Level6, // примерно соответствует Quality=65
                        TransparentColorMode = PngTransparentColorMode.Preserve // (важно, если есть прозрачность)
                    };
                    // Сохраняем в PNG (важно для совместимости nanoCad BIM Электро)
                    using (var stream = new MemoryStream()) {
                        image.Save(stream, pngEncoder);

                        var bytes = stream.ToArray();
                        if (img is not null) {
                            img.Image = bytes;
                            return img;
                        }

                        return context.CreateImage(imgName, category, bytes);
                    }
                }
            }

        }
        internal static byte[] GetData(FileInfo fi) {
            byte[] data;
            using (FileStream fileStream = fi.OpenRead()) {
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
            }
            return data;
        }
    }
}
