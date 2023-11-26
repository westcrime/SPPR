using Serilog.Events;

namespace WEB_153502_Tolstoi.Logs
{
    public static class LogHelper
    {
        private static bool IsOK(HttpContext ctx)
        {
            var code = ctx.Response.StatusCode;
            if (code < 300)
            {
                return true;
            }

            return false;
        }

        public static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception ex) =>
        ex != null
            ? LogEventLevel.Error
            : ctx.Response.StatusCode > 499
                ? LogEventLevel.Error
                : IsOK(ctx)
                    ? LogEventLevel.Verbose
                    : LogEventLevel.Information;

    }
}
