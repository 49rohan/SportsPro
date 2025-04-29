using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsPro.Services;
using System.Linq;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("technician-select", Attributes = ForAttributeName)]
    public class TechnicianSelectTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly ITechnicianService _technicianService;

        public TechnicianSelectTagHelper(ITechnicianService technicianService)
        {
            _technicianService = technicianService;
        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var technicians = _technicianService.GetAllTechnicians();

            output.TagName = "select";
            output.Attributes.SetAttribute("class", "form-control");
            output.Attributes.SetAttribute("id", For.Name);
            output.Attributes.SetAttribute("name", For.Name);

            foreach (var technician in technicians)
            {
                var option = new TagBuilder("option");
                option.Attributes["value"] = technician.TechnicianID.ToString();
                option.InnerHtml.Append(technician.Name);
                output.Content.AppendHtml(option);
            }
        }
    }
}