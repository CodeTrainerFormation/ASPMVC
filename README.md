# Solution Lab-08

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/a8fcd0a07206f09017206c506a2a763d7f7f5f10?diff=split)

## Création d'un nouveau layout

Dans le dossier `Views\Shared`, créer un vue `_AdminLayout.cshtml`
```cshtml
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>@ViewBag.Title - My ASP.NET Application</title>
        @Styles.Render("~/Content/css")
        @Scripts.Render("~/bundles/modernizr")
    </head>
    <body>
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">Project name</a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="#">Dashboard</a></li>
                        <li><a href="#">Settings</a></li>
                        <li><a href="#">Profile</a></li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-3 col-md-2 sidebar">
                    <ul class="nav nav-sidebar">
                        @Html.LiActionLink("Etudiants", "Index", "Student")
                        @Html.LiActionLink("Classes", "Index", "Classroom")
                        @Html.LiActionLink("Professeurs", "Index", "Teacher")
                    </ul>
                    
                </div>
                <div class="col-sm-9 col-sm-offset-3 col-md-10 col-md-offset-2 main">
                    <h1 class="page-header">Dashboard</h1>
                    @RenderBody()
                </div>
            </div>
        </div>
        

        @Scripts.Render("~/bundles/jquery")
        @Scripts.Render("~/bundles/bootstrap")
        @RenderSection("scripts", required: false)
    </body>
</html>
```

Modifier le layout par défaut dans le fichier `Views\_ViewStart.cshtml`
```cshtml
@{
    Layout = "~/Views/Shared/_AdminLayout.cshtml";
}
```

Modifier le CSS du template dans le fichier `Content\Site.css`
```css
/*
 * Base structure
 */

/* Move down content because we have a fixed navbar that is 50px tall */
body {
    padding-top: 50px;
}


/*
 * Global add-ons
 */

.sub-header {
    padding-bottom: 10px;
    border-bottom: 1px solid #eee;
}

/*
 * Top navigation
 * Hide default border to remove 1px line.
 */
.navbar-fixed-top {
    border: 0;
}

/*
 * Sidebar
 */

/* Hide for mobile, show later */
.sidebar {
    display: none;
}

@media (min-width: 768px) {
    .sidebar {
        position: fixed;
        top: 51px;
        bottom: 0;
        left: 0;
        z-index: 1000;
        display: block;
        padding: 20px;
        overflow-x: hidden;
        overflow-y: auto; /* Scrollable contents if viewport is shorter than content. */
        background-color: #f5f5f5;
        border-right: 1px solid #eee;
    }
}

/* Sidebar navigation */
.nav-sidebar {
    margin-right: -21px; /* 20px padding + 1px border */
    margin-bottom: 20px;
    margin-left: -20px;
}

.nav-sidebar > li > a {
    padding-right: 20px;
    padding-left: 20px;
}

.nav-sidebar > .active > a,
.nav-sidebar > .active > a:hover,
.nav-sidebar > .active > a:focus {
    color: #fff;
    background-color: #428bca;
}


/*
 * Main content
 */

.main {
    padding: 20px;
}

@media (min-width: 768px) {
    .main {
        padding-right: 40px;
        padding-left: 40px;
    }
}

.main .page-header {
    margin-top: 0;
}


/*
 * Placeholder dashboard ideas
 */

.placeholders {
    margin-bottom: 30px;
    text-align: center;
}

.placeholders h4 {
    margin-bottom: 0;
}

.placeholder {
    margin-bottom: 20px;
}

.placeholder img {
    display: inline-block;
    border-radius: 50%;
}
```

#### Bonus : Créer un helper pour la classe active sur les liens

Dans le dossier `SDK\Helpers`, créer une classe `HtmlHelperExtension`
```C#
public static class HtmlHelperExtension
{

    public static MvcHtmlString LiActionLink(this HtmlHelper html, string text, string action, string controller)
    {
        var context = html.ViewContext;
        if (context.Controller.ControllerContext.IsChildAction)
            context = html.ViewContext.ParentActionViewContext;
        var routeValues = context.RouteData.Values;
        var currentController = routeValues["controller"].ToString();

        var str = String.Format("<li role=\"presentation\"{0}>{1}</li>",
            currentController.Equals(controller, StringComparison.InvariantCulture) ? " class=\"active\"" : String.Empty,
            html.ActionLink(text, action, controller).ToHtmlString()
        );
        return new MvcHtmlString(str);
    }

}
```

## Modifier le formulaire d'ajout d'un étudiant

Modifier le fichier `Views\Student\Create.cshtmnl`
```cshtml
<h2>Create</h2>

@Html.ValidationSummary(false, "", new { @class = "text-danger" })
<form method="post" class="form-horizontal">    
    <div class="form-group">
        <label class="col-sm-2 control-label">Prénom</label>
        <div class="col-sm-10">
            <input type="text" class="form-control" name="firstName" placeholder="Prénom" value="@Model.FirstName" >
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label">Nom</label>
        <div class="col-sm-10">
            <input type="text" class="form-control" name="lastName" placeholder="Nom" value="@Model.LastName">
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label">Age</label>
        <div class="col-sm-10">
            <input type="number" class="form-control" name="age" placeholder="Age" value="@Model.Age">
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label">Email</label>
        <div class="col-sm-10">
            <input type="email" class="form-control" name="email" placeholder="Email" value="@Model.Email">
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label">Moyenne</label>
        <div class="col-sm-10">
            <input type="text" class="form-control" name="average" placeholder="Moyenne" value="@Model.Average">
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-2 col-sm-10">
            <div class="checkbox">
                <label>
                    <input type="checkbox" name="isclassdelegate" value="@Model.IsClassDelegate"> Est délégué
                </label>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-2 col-sm-10">
            @FormHelper.SubmitBtn("Créer", "success")
        </div>
    </div>
</form>
```