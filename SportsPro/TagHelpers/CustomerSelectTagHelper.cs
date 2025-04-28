using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsPro.Services;
using System.Linq;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("customer-select", Attributes = ForAttributeName)]
    public class CustomerSelectTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly ICustomerService _customerService;

        public CustomerSelectTagHelper(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var customers = _customerService.GetAllCustomers();

            output.TagName = "select";
            output.Attributes.SetAttribute("class", "form-control");
            output.Attributes.SetAttribute("id", For.Name);
            output.Attributes.SetAttribute("name", For.Name);

            var placeholderOption = new TagBuilder("option");
            placeholderOption.Attributes["value"] = "";
            placeholderOption.InnerHtml.Append("-- Select Customer --");
            output.Content.AppendHtml(placeholderOption);

            foreach (var customer in customers)
            {
                var option = new TagBuilder("option");
                option.Attributes["value"] = customer.CustomerID.ToString();
                option.InnerHtml.Append(customer.FullName);
                output.Content.AppendHtml(option);
            }
        }
    }
}
