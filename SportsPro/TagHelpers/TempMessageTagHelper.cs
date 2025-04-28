using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("temp-message")]
    public class TempMessageTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tempData = ViewContext.TempData;
            string message = tempData["message"] as string ?? tempData["delete"] as string;

            output.TagName = "h4";
            output.Attributes.SetAttribute("class", "bg-info text-center text-white p-2");

            if (!string.IsNullOrEmpty(message))
                output.Content.SetContent(message);
            else
                output.SuppressOutput(); // Don't render anything if no message
        }
    }
}