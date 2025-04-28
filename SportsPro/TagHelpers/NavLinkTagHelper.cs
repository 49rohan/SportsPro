using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-controller, asp-action")]
    public class NavLinkTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string currentController = ViewContext.RouteData.Values["controller"]?.ToString();
            string currentAction = ViewContext.RouteData.Values["action"]?.ToString();

            string targetController = output.Attributes["asp-controller"]?.Value?.ToString();
            string targetAction = output.Attributes["asp-action"]?.Value?.ToString();

            bool isActive = string.Equals(currentController, targetController, StringComparison.OrdinalIgnoreCase)
                         && string.Equals(currentAction, targetAction, StringComparison.OrdinalIgnoreCase);

            if (isActive)
            {
                if (output.Attributes.ContainsName("class"))
                {
                    var existingClasses = output.Attributes["class"].Value.ToString();
                    output.Attributes.SetAttribute("class", $"{existingClasses} active");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
        }
    }
}