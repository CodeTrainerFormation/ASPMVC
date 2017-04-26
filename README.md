# Solution Lab-05

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/00fc641f663ed53fbeab40dc899a8485ff095085)

La solution présentée ci dessous **intègre** l'injection de dépendances

## Préparation de l'injection de dépendances

### Intaller les packages

Dans le projet `NetSchool`, avec le Nuget Package Manager, ajouter les packages suivants : 
- Ninject
- Ninject.Web.Common
- Ninject.MVC5

Après l'installation de ces packages, un fichier nommé `NinjectWebCommon.cs` doit apparaître dans le dossier `App_Start`

### Créer le Dependency Resolver

Dans le projet `NetSchool`, créer un dossier `Infrastructure`. Puis ajouter une classe nommée `NinjectDependencyResolver` à ce dossier.

La classe doit être implémentée de la sorte : 
```C#
public class NinjectDependencyResolver : IDependencyResolver
{
    private IKernel kernel;

    public NinjectDependencyResolver(IKernel kernelParam)
    {
        kernel = kernelParam;
        AddBindings();
    }

    public object GetService(Type serviceType)
    {
        return kernel.TryGet(serviceType);
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        return kernel.GetAll(serviceType);
    }

    public void AddBindings()
    {

    }
}
```

### Intégrer Ninject

Dans le fichier `NinjectWebCommon`, localiser la méthode `RegisterServices(IKernel kernel)`, et la modifier de la sorte : 
```C#
private static void RegisterServices(IKernel kernel)
{
    DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
}  
```

### Créer un nouveau projet

Toujours dans une optique de séparation des concepts, créer un nouveau projet `Repository` de type `Librairie de classes`.

Ce projet contiendra les interactions avec la base de données afin de soulager les contrôleurs et faciliter la réutilisation.

Ajouter les références suivantes au projet : 
- EntityFramework
- DAL
- DomainModel

## Utiliser l'injection de dépendances

> Dans cette solution, seul un exemple sur `Student` est présent, les étapes présentées sont à renouveller pour chaque "repository"

### Création des interfaces

Dans le projet `Repository`, créer un dossier `Repositories`, et dans celui ci, créer un dossier `Abstract`.

Dans le dossier `Abstract`, il convient de créer nos interfaces qui seront utilisées dans les contrôleurs.

Créer une interface `IStudentRepository` : 
```C#
public interface IStudentRepository
{
    IEnumerable<Student> AllStudents();
    Student GetStudent(int id);
    int AddStudent(Student student);
    int EditStudent(Student student);
    int DeleteStudent(int id);
	void Dispose();
}
```

### Création des classes

Dans le dossier `Repositories` du projet `Repository`, créer les classes qui implémentes les interfaces précédemment créées.

Créer un classe `StudentRepository` : 
```C#
public class StudentRepository : IStudentRepository
{
    private SchoolDb context;

    public StudentRepository() : this(new SchoolDb())
    { }

    public StudentRepository(SchoolDb ctx) 
    {
        this.context = ctx;
    }

    public int AddStudent(Student student)
    {
        context.Students.Add(student);
        return context.SaveChanges();
    }

    public IEnumerable<Student> AllStudents()
    {
        return context.Students.ToList();
    }

    public int DeleteStudent(int id)
    {
        Student student = this.GetStudent(id);
        if(student.Equals(null))
        {
            return 0;
        }

        context.Students.Remove(student);
        return context.SaveChanges();
    }

	public void Dispose()
    {
        context.Dispose();
    }

    public int EditStudent(Student student)
    {
        context.Entry(student).State = EntityState.Modified;
        return context.SaveChanges();
    }

    public Student GetStudent(int id)
    {
        return context.Students.Find(id);
    }
}
```

### Injecter les classes

> Dans le projet NetSchool, ajouter la référence du projet `Repository`

Dans un premier temps, il faut lier les interfaces à leur classes.

Dans la classe `NinjectDependencyResolver` qui se situe dans le dossier `Infrastructure` du projet `NetSchool`, localiser la méthode `AddBindings()`.

Dans cette méthode, lier les interfaces aux classes de la manière suivante : 
```C#
public void AddBindings()
{
    kernel.Bind<IStudentRepository>().To<StudentRepository>();
}
```

### Modifier les contrôleurs

La dernière étape consiste à mettre à jour les contrôleurs.

Créer un constructeur au contrôleur qui recoit le "repository" lui correspondant : 
```C#
public class StudentController : Controller
{
    private IStudentRepository repository;

    public StudentController(IStudentRepository studentRepository)
    {
        this.repository = studentRepository;
    }
	// ...
}
```

[Voir le reste des modifications du contrôleur](https://github.com/CodeTrainerFormation/ASPMVC/blob/05-SoC-DI/NetSchool/NetSchoolWeb/Controllers/StudentController.cs)