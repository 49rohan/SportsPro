using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-route-filter")]
    public class NavPillTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentFilter = ViewContext.ViewBag.Filter as string;
            var targetFilter = output.Attributes["asp-route-filter"]?.Value?.ToString();

            bool isActive = string.Equals(currentFilter, targetFilter, StringComparison.OrdinalIgnoreCase);

            if (isActive)
            {
                var classAttr = output.Attributes["class"];
                string existingClass = classAttr?.Value?.ToString() ?? "";
                output.Attributes.SetAttribute("class", $"{existingClass} active".Trim());
            }
        }
    }
}
