# Solution Lab-07

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/67bb8f673ed3c95e41e199e978a1796bc5a00369?diff=split)

## Tests unitaires

### Création du projet

Dans la solution, créer un nouveau projet `NetSchoolTest` de type `Projet Test Unitaires (.NET Framework)`.

Supprimer la classe présente par défaut dans le projet.

Ajouter les références suivantes au projet : 
- Microsoft.Aspnet.Mvc (rechercher MVC5 sur NuGet)
- DomainModel
- NetSchool
- EntityFramework

Créer un dossier `Controllers` dans le projet.

### Création des tests unitaires

Dans le dossier `Controllers`, créer une classe `StudentControllerTest`.

Annoter la classe avec `[TestClass]`
```C#
[TestClass]
public class StudentControllerTest
{
    //...
}
```

Créer un test unitaire qui vérifie que la bonne vue est retournée par le contrôleur 
```C#
[TestMethod]
public void TestIndexView()
{
    var controller = new StudentController();

    var result = controller.Index() as ViewResult;

    Assert.AreEqual("IndexList", result.ViewName);
}
```

Créer un test unitaire qui vérifie le type de donnée envoyé à la vue
```C#
[TestMethod]
public void TestDetailType()
{
    var controller = new StudentController();

    var result = controller.Details(3) as ViewResult;

    Assert.IsInstanceOfType(result.ViewData.Model, typeof(Student));
}
```

Créer un test unitaire qui vérifie le type de vue retournée en cas d'erreur dans le contrôleur

Au préalable, éditer la méthode `Detail` du `StudentController` comme suit :
```C#
public ActionResult Details(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Student student = null;
    try
    {
        student = context.Students.Find(id);
    }catch(InvalidOperationException ioe)
    {
        return HttpNotFound();
    }
    if (student == null)
    {
        return HttpNotFound();
    }
    return View(student);
}
```

Puis ajouter le test unitaire : 
```C#
[TestMethod]
public void TestDetailError()
{
    var controller = new StudentController();

    var result = controller.Details(1);

    Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
}
```

## Exceptions

### CustomErrors

Editer le fichier `Web.config` en activant les `customErrors`
```xml
<configuration>
  <!-- ... -->
  <system.web>
    <!-- ... -->
    <customErrors mode="On" defaultRedirect="~/Error.html" redirectMode="ResponseRewrite">
        <error statusCode="404" redirect="~/Error404.cshtml"  />
    </customErrors>
  </system.web>
</configuration>
```

### OnException

Surcharger la méthode `OnException` dans le `StudentController`
```C#
protected override void OnException(ExceptionContext filterContext)
{
    if (filterContext.ExceptionHandled)
    {
        return;
    }
    filterContext.Result = new ViewResult
    {
        ViewName = "~/Views/Shared/Error.cshtml"
    };
    filterContext.ExceptionHandled = true;
}
```

Déclencher une exception dans une action
```C#
public ActionResult Details(int? id)
{
    //...
	if (student == null)
	{
		throw new HttpException(404, "Error");
    }
    //...
}
```

### HandleError

Dans le contrôleur `TeacherController`, annoter la méthode `Detail` avec `[HandleError]`
```C#
[HandleError(ExceptionType = typeof(HttpException), View = "Error404")]
public ActionResult Details(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Teacher teacher = db.Teachers.Find(id);
    if (teacher == null)
    {
        throw new HttpException();
    }
    return View(teacher);
}
```

Créer la vue `Error404.cshtml` dans le dossier `Views\Shared`
```html
<hgroup>
    <h1>Error 404</h1>
    <h2>Oops, impossible d'accéder à cette page</h2>
</hgroup>
```
