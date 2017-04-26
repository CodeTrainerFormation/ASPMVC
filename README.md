# Solution Lab-06

[Voir le diff�rentiel avec le lab pr�c�dent]()

La solution pr�sent�e ci dessous repart de la solution du **Lab-05 sans injection de d�pendances** 

## Cr�er les routes

Cr�er la route `StudentPhoto` dans le contr�leur `SutdentController` : 
```C#
[ActionName("StudentPhoto")]
public ActionResult Photo(int studentid)
{
    return Redirect("https://www.youtube.com/watch?v=oavMtUWDBTM&t=30");
}
```

Cr�er un prefix de route, deux solutions.
- Premi�re solution : les annotations : 
  - Dans le fichier `App_Start\RouteConfig.cs`, ajouter la ligne suivante
```C#
public static void RegisterRoutes(RouteCollection routes)
{
    routes.MapMvcAttributeRoutes();
    // ...
}
```
  - Ajouter l'annoation `RoutePrefix("prefix")` au contr�leur, et l'annotation `Route` sur toutes ses actions
```C#
[RoutePrefix("class")]
public class ClassroomController : Controller
{
    private SchoolDb db = new SchoolDb();

    // GET: Classroom
    [Route]
    public ActionResult Index()
    {
        return View(db.Classrooms.ToList());
    }
	//...
}
```
- Seconde solution : nouvelle "map" de route
  - Dans le fichier `App_Start\RouteConfig.cs`, ajouter une nouvelle "map" 
```C#
public static void RegisterRoutes(RouteCollection routes)
{
    routes.MapRoute(
        name: "classroom",
        url: "class/{action}/{id}",
        defaults: new { controller = "Classroom", action = "Index", id = UrlParameter.Optional }
    );

    //...
}
```

Cr�er un prefix d'action : 
- Dans le fichier `App_Start\RouteConfig.cs`, ajouter la ligne suivante
```C#
public static void RegisterRoutes(RouteCollection routes)
{
    routes.MapMvcAttributeRoutes();
    // ...
}
```
- Dans le contr�leur `ClassroomController` : 
```C#
public class ClassroomController : Controller
{
    [Route("view/{id}")]
    public ActionResult Details(int? id)
    {
        // ...
    }
	//...
}
```

Cr�er une route directement sur une action : 
- Dans le contr�leur `TeacherController`
```C#
public class TeacherController : Controller
{

    // GET: Teacher
    [Route("Prof")]
    [Route("Prof/List")]
    public ActionResult Index()
    {
        return View(db.Teachers.ToList());
    }
	// ...
}
```

## Cr�ation d'une contrainte

Cr�er un dossier `Infrastructure` dans le projet `NetSchool`.

Dans le dossier `Infrastructure`, cr�er une classe `DateConstraint`.
```C#
public class DateConstraint : IRouteConstraint
{
    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    {
        DateTime datetime;

        bool result = DateTime.TryParseExact(values[parameterName].ToString(),
        "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datetime);

        return result;
    }
}
```

Dans le contr�leur `TeacherController`, cr�er une action `IndexDateFilter` 
```C#
public ActionResult IndexDateFilter(string datetime)
{
    DateTime hiringDate;
    DateTime.TryParseExact(datetime, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hiringDate);

    return View("Index", db.Teachers.Where(t => t.HiringDate > hiringDate).ToList());
}
```

Dans le fichier `App_Start\RouteConfig.cs`, ajouter une route
```C#
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        //...

        routes.MapRoute(
            name: "TeacherHiringDate",
            url: "Prof/{datetime}",
            defaults: new { controller = "Teacher", action = "IndexDateFilter" },
            constraints: new { datetime = new DateConstraint() }
        );

        //...
    }
}
```