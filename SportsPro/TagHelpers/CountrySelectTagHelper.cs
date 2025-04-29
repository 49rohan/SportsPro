using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsPro.Services;
using System.Linq;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("country-select", Attributes = ForAttributeName)]
    public class CountrySelectTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly ICountryService _countryService;

        public CountrySelectTagHelper(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var countries = _countryService.GetAllCountries();

            output.TagName = "select";
            output.Attributes.SetAttribute("class", "form-control");
            output.Attributes.SetAttribute("id", For.Name);
            output.Attributes.SetAttribute("name", For.Name);

            foreach (var country in countries)
            {
                var option = new TagBuilder("option");
                option.Attributes["value"] = country.CountryID.ToString();
                option.InnerHtml.Append(country.Name);
                output.Content.AppendHtml(option);
            }
        }
    }
}
