using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Extensions;

public static class ActionExtensions
{
    public static ActionResult<T> ToAr<T>(this T? o)
    {
        return o is null ? new NotFoundResult() : o;
    }
}
