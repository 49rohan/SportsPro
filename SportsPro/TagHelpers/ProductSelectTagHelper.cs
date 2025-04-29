using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsPro.Services;
using System.Linq;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("product-select", Attributes = ForAttributeName)]
    public class ProductSelectTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly IProductService _productService;

        public ProductSelectTagHelper(IProductService productService)
        {
            _productService = productService;
        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var products = _productService.GetAllProducts();

            output.TagName = "select";
            output.Attributes.SetAttribute("class", "form-control");
            output.Attributes.SetAttribute("id", For.Name);
            output.Attributes.SetAttribute("name", For.Name);

            foreach (var product in products)
            {
                var option = new TagBuilder("option");
                option.Attributes["value"] = product.ProductID.ToString();
                option.InnerHtml.Append(product.Name);
                output.Content.AppendHtml(option);
            }
        }
    }
}
