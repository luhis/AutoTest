using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Extensions
{
    public static class ActionExtensions
    {
        public static ActionResult<T> ToAr<T>(this T? o)
        {
            return o == null ? new NotFoundResult() : o;
        }
        public static IActionResult ToIar(this IActionResult o)
        {
            return o;
        }
    }
}
