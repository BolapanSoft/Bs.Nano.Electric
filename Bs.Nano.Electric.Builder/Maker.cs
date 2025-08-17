// Ignore Spelling: Iek Tprod

using Microsoft.Extensions.Logging;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using OpenXMLROW = DocumentFormat.OpenXml.Spreadsheet.Row;
using System.Globalization;
using System.Text.Json;
using static Bs.XML.SpreadSheet.SheetCommon;

namespace Bs.Nano.Electric.Builder {
    public partial class Maker {
        internal const System.Globalization.NumberStyles NumberStyle =
            NumberStyles.AllowLeadingWhite |
            NumberStyles.AllowTrailingWhite |
            NumberStyles.AllowLeadingSign |
            NumberStyles.AllowDecimalPoint;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly Lazy<ResourceManager> lzResourceManager;
        //private readonly string outDir;
        //private readonly string dbTemplate;
        //private readonly string imageDir;
        //private readonly string graphicPath;
        //private readonly ILogger logger = DI.Provider.GetService<ILogger<Make>>().NotNull();
        //private static readonly SortedDictionary<string, (string tableName, int id)> productsCache = new();
        //private readonly Lazy<Bs.Nano.Electric.Interop.Service> lzService;
        //private readonly Lazy<XmlLoader> lzXmlLoader;

        internal Maker(ILogger logger, Configuration configuration) {
            this.logger = logger;
            this.configuration = configuration;
            lzResourceManager = new Lazy<ResourceManager>(() => new ResourceManager(configuration));
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
    }
}
