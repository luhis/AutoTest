using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Extensions
{
    public static class ActionExtensions
    {
        public static ActionResult<T> ToIac<T>(this T? o)
        {
            return o == null ? new NotFoundResult() : o;
        }
    }
}
