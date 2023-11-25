using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace Web_153502_Tolstoi.Helpers
{
    [HtmlTargetElement("pager")]
    public class PageHelper : TagHelper
    {
        [HtmlAttributeName("current-page")]
        public int? CurrentPage { get; set; }

        [HtmlAttributeName("max-pages")]
        public int? MaxPages { get; set; }

        [HtmlAttributeName("category")]
        public string? Category { get; set; }

        [HtmlAttributeName("admin")]
        public bool? Admin { get; set; } = false;

        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _httpContext;

        public PageHelper(LinkGenerator linkGen, IHttpContextAccessor contextAccessor)
        {
            _linkGenerator = linkGen;
            _httpContext = contextAccessor.HttpContext;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string backButton = "", leftButton = "", midButton = "", rightButton = "", forwardButton = "";
            string? leftUrl = "";
            string? rightUrl = "";

            var attr = () => Admin.Value ? "href" : "ajax-href";

            if (!Admin.Value)
            {
                leftUrl = _linkGenerator.GetUriByAction(_httpContext, action: "Index", controller: "Game", values: new { category = Category, pageno = CurrentPage - 1 });
                rightUrl = _linkGenerator.GetUriByAction(_httpContext, action: "Index", controller: "Game", values: new { category = Category, pageno = CurrentPage + 1 });
            }
            else
            {
                leftUrl = _linkGenerator.GetUriByPage(_httpContext, page: "/Index", values: new { area = "Admin", pageNo = CurrentPage - 1 });
                rightUrl = _linkGenerator.GetUriByPage(_httpContext, page: "/Index", values: new { area = "Admin", pageNo = CurrentPage + 1 });
            }


            if (CurrentPage > 1)
            {
                backButton = "<li class=\"page-item\">" +
                                $"<a class=\"page-link\" {attr()}=\"{leftUrl}\">Назад</a>" +
                             "</li>";

                leftButton = "<li class=\"page-item\">" +
                                $"<a class=\"page-link\" {attr()}=\"{leftUrl}\">{CurrentPage - 1}</a>" +
                             "</li>";

            }
            else
            {
                backButton = "<li class=\"page-item disabled\">" +
                                "<span class=\"page-link\">Назад</span>" +
                            "</li>";
            }

            midButton = "<li class=\"page-item active\" aria-current=\"page\">" +
                            $"<span class=\"page-link\">{CurrentPage}</span>" +
                        "</li>";


            if (CurrentPage < MaxPages)
            {
                rightButton = "<li class=\"page-item\">" +
                                $"<a class=\"page-link\" {attr()}=\"{rightUrl}\">{CurrentPage + 1}</a>" +
                              "</li>";
                forwardButton = "<li class=\"page-item\">" +
                                $"<a class=\"page-link\" {attr()}=\"{rightUrl}\">Вперед</a>" +
                              "</li>";
            }
            else
            {
                forwardButton = "<li class=\"page-item disabled\">" +
                                    "<span class=\"page-link\">Вперед</span>" +
                                "</li>";
            }

            output.Content.SetHtmlContent("<nav aria-label=\"...\"><ul class=\"pagination\">" + backButton + leftButton + midButton + rightButton + forwardButton + "</ul></nav>");
        }
    }
}