using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetSchoolWeb.SDK.Helpers
{
    public class FormHelper
    {

        public static IHtmlString SubmitBtn(string content = null, string classname = null)
        {
            content = content ?? "Envoyer";
            classname = classname ?? "default";

            return new HtmlString(string.Format("<input type=\"submit\" value=\"{0}\" class=\"btn btn-{1}\" />", content, classname));
        }

    }
}