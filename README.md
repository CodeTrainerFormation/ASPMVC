# Solution Lab-09

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/90d6e0a150c5795185dd76f9fb2e5e4358f97cf6?diff=split)

## Créer un cache de sortie

Annoter une action avec `[OutputCache()]`
```c#
[OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
public ActionResult Index()
{
    return View(db.Classrooms.ToList());
}
```

## Créer une requête avec ActionLink
Installer avec le Nuget Package Manager le package suivant : 
- Microsoft.jQuery.Unobtrusive.Ajax

Créer un bundle pour ces scripts 
```C#
 bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
  "~/Scripts/jquery.unobtrusive*"
));
```

Utiliser le bundle dans le layout `Views\Shared\_AdminLayout.cshtml`
```cshtml
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <!-- ... -->
    </head>
    <body>
		<!-- ... --> 
       
        @Scripts.Render("~/bundles/jquery")
        @Scripts.Render("~/bundles/bootstrap")
        @Scripts.Render("~/bundles/ajax")
        @RenderSection("scripts", required: false)
    </body>
</html>
```

Editer la vue `Views\Student\IndexList.cshtml`
```cshtml
@model IEnumerable<DomainModel.Student>

@{
    ViewBag.Title = "IndexList";
}
<h2>IndexList</h2>

@Ajax.ActionLink("Rafraichir la liste des étudiants",
                    "List",
                    "Student",
                    new AjaxOptions
                    {
                        UpdateTargetId = "StudentList",
                        InsertionMode = InsertionMode.Replace,
                        HttpMethod = "GET",
                    })

<div id="StudentList">
    <p>
        @Html.ActionLink("Create New", "Create")
    </p>

    <table class="table">
        <tr>
            <th>
                @Html.DisplayNameFor(model => model.FirstName)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.LastName)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.Age)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.Email)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.Average)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.IsClassDelegate)
            </th>
            <th></th>
        </tr>

        @foreach (var item in Model)
        {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => item.FirstName)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.LastName)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Age)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Email)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Average)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.IsClassDelegate)
                </td>
                <td>
                    @Html.ActionLink("Edit", "Edit", new { id = item.PersonID }) |
                    @Html.ActionLink("Details", "Details", new { id = item.PersonID }) |
                    @Html.ActionLink("Delete", "Delete", new { id = item.PersonID })
                </td>
            </tr>
        }

    </table>

</div>
```

> Si l'annotation `ChildActionOnly` est présente sur l'action `List`, la supprimer

## Faire une requête ajax avec jQuery

Sur la vue `Views\Student\IndexList.cshtml`, éditer le dernier `td` pour lui ajouter un bouton : 
```cshtml
<td>
    <button class="btn btn-warning preview" data-id="@item.PersonID">Preview</button> | 
    @Html.ActionLink("Edit", "Edit", new { id = item.PersonID }) |
    @Html.ActionLink("Details", "Details", new { id = item.PersonID }) |
    @Html.ActionLink("Delete", "Delete", new { id = item.PersonID })
</td>
```

Créer une modale à la fin de la vue
```html
<div class="modal fade detail-modal" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="detail-modal-title">Loading...</h4>
            </div>
            <div class="modal-body" id="detail-modal-body">
                Loading...
            </div>
        </div>
    </div>
</div>
```

Créer un fichier `Student.js` dans le dossier `Scripts`
```js
$(document).ready(function () {

    var loading = "Loading...";

    $("td").on("click", "button.preview", function (e) {
        updateModal(); 

        var id = $(this).attr('data-id');
        $.ajax({
            url: "/Student/AjaxDetail/" + id,
            method: 'GET'
        }).then(function (student) {
            updateModal(student.name, "Age : " + student.age + " | Classe : " + student.classroom);
        }, function (error) {

        })
    });

    var updateModal = function (title, content) {
        if (title === undefined || content === undefined) {
            title = loading;
            content = loading;
        } 

        $("#detail-modal-title").html(title);
        $("#detail-modal-body").html(content);

        $(".detail-modal").modal('show');
    }

});
```

Renseigner ce fichier à la fin de la vue `Views\Student\IndexList.cshtml`
```cshtml
@section scripts {
    <script src="~/Scripts/Student.js"></script>
}
```

Créer une action dans le contrôleur `StudentController`
```C#
public JsonResult AjaxDetail(int id)
{
    if(!this.Request.IsAjaxRequest())
    {
        return Json(null);
    }
    Student student = context.Students.Find(id);

    return Json(new {
        name = student.FirstName + " " + student.LastName,
        age = student.Age,
        classroom = student.Classroom.Name
    }, JsonRequestBehavior.AllowGet);
}
```