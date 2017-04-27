# Solution Lab-07

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/67bb8f673ed3c95e41e199e978a1796bc5a00369?diff=split)

## Tests unitaires

### Cr�ation du projet

Dans la solution, cr�er un nouveau projet `NetSchoolTest` de type `Projet Test Unitaires (.NET Framework)`.

Supprimer la classe pr�sente par d�faut dans le projet.

Ajouter les r�f�rences suivantes au projet : 
- Microsoft.Aspnet.Mvc (rechercher MVC5 sur NuGet)
- DomainModel
- NetSchool
- EntityFramework

Cr�er un dossier `Controllers` dans le projet.

### Cr�ation des tests unitaires

Dans le dossier `Controllers`, cr�er une classe `StudentControllerTest`.

Annoter la classe avec `[TestClass]`
```C#
[TestClass]
public class StudentControllerTest
{
    //...
}
```

Cr�er un test unitaire qui v�rifie que la bonne vue est retourn�e par le contr�leur 
```C#
[TestMethod]
public void TestIndexView()
{
    var controller = new StudentController();

    var result = controller.Index() as ViewResult;

    Assert.AreEqual("IndexList", result.ViewName);
}
```

Cr�er un test unitaire qui v�rifie le type de donn�e envoy� � la vue
```C#
[TestMethod]
public void TestDetailType()
{
    var controller = new StudentController();

    var result = controller.Details(3) as ViewResult;

    Assert.IsInstanceOfType(result.ViewData.Model, typeof(Student));
}
```

Cr�er un test unitaire qui v�rifie le type de vue retourn�e en cas d'erreur dans le contr�leur

Au pr�alable, �diter la m�thode `Detail` du `StudentController` comme suit :
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

Surcharger la m�thode `OnException` dans le `StudentController`
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

D�clencher une exception dans une action
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

Dans le contr�leur `TeacherController`, annoter la m�thode `Detail` avec `[HandleError]`
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

Cr�er la vue `Error404.cshtml` dans le dossier `Views\Shared`
```html
<hgroup>
    <h1>Error 404</h1>
    <h2>Oops, impossible d'acc�der � cette page</h2>
</hgroup>
```
