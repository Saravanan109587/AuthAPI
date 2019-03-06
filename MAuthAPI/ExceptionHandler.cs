using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using log4net;
using System.Reflection;

namespace MAuthAPI
{
    public class CommonExceptionHandler : System.Web.Http.ExceptionHandling.ExceptionHandler
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Exception exception = context.Exception;
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            var path = AppDomain.CurrentDomain.BaseDirectory + @"ErrorLog";

            _log.Debug("Exeception path is " + path);
            _log.Error("Exeception   is " + exception.Message);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var Logdirectory = new DirectoryInfo(path);

            if (Logdirectory.GetFiles().Length > 30)
            {
                while (Logdirectory.GetFiles().Length <= 30)
                {
                    var DeletableFileName = (from f in Logdirectory.GetFiles()
                                             orderby f.LastWriteTime descending
                                             select f).Last().Name;
                    File.Delete(Path.Combine(path, DeletableFileName));
                }

            }
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(exception.Message);
            sb.AppendLine(exception.StackTrace);
            //For Some Spece
            sb.AppendLine("");

            File.AppendAllText(Path.Combine(path, "Error." + DateTime.Now.ToString("MM.dd.yyyy") + ".txt"), sb.ToString());

            //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });



        }
    }
    }