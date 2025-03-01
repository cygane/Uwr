using Microsoft.AspNetCore.Mvc.Routing;

public class CMSCustomRouteTransformer : DynamicRouteValueTransformer
{
    public const string DEFAULTPAGEEXTENSION = ".html";
    public const string CMS = "CMS";
    public const string SITENAME = "siteName";
    public const string PAGENAME = "pageName";

    public override async ValueTask<RouteValueDictionary> TransformAsync(
        HttpContext httpContext, RouteValueDictionary values)
    {
        if (!values.ContainsKey("sitepage")) return values;

        var virtualPath = values["sitepage"]?.ToString();
        if (string.IsNullOrEmpty(virtualPath)) return values;

        string[] segments = virtualPath.ToLower().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length >= 1)
        {
            if (segments.Last().IndexOf(DEFAULTPAGEEXTENSION, StringComparison.OrdinalIgnoreCase) > 0)
            {
                values["controller"] = "Page";
                values["action"] = "Render";
                values[SITENAME] = string.Join("/", segments.Take(segments.Length - 1));
                values[PAGENAME] = segments.Last().Substring(0, segments.Last().IndexOf('.'));
            }
            else if (!segments.Last().Contains("."))
            {
                values["controller"] = "Page";
                values["action"] = "Render";
                values[SITENAME] = string.Join("/", segments);
                values[PAGENAME] = "index.html";
            }
        }

        return values;
    }
}
