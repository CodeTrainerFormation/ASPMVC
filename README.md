# Solution Lab-05

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/00fc641f663ed53fbeab40dc899a8485ff095085)

La solution pr�sent�e ci dessous **int�gre** l'injection de d�pendances

## Pr�paration de l'injection de d�pendances

### Intaller les packages

Dans le projet `NetSchool`, avec le Nuget Package Manager, ajouter les packages suivants : 
- Ninject
- Ninject.Web.Common
- Ninject.MVC5

Apr�s l'installation de ces packages, un fichier nomm� `NinjectWebCommon.cs` doit appara�tre dans le dossier `App_Start`

### Cr�er le Dependency Resolver

Dans le projet `NetSchool`, cr�er un dossier `Infrastructure`. Puis ajouter une classe nomm�e `NinjectDependencyResolver` � ce dossier.

La classe doit �tre impl�ment�e de la sorte : 
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

### Int�grer Ninject

Dans le fichier `NinjectWebCommon`, localiser la m�thode `RegisterServices(IKernel kernel)`, et la modifier de la sorte : 
```C#
private static void RegisterServices(IKernel kernel)
{
    DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
}  
```

### Cr�er un nouveau projet

Toujours dans une optique de s�paration des concepts, cr�er un nouveau projet `Repository` de type `Librairie de classes`.

Ce projet contiendra les interactions avec la base de donn�es afin de soulager les contr�leurs et faciliter la r�utilisation.

Ajouter les r�f�rences suivantes au projet : 
- EntityFramework
- DAL
- DomainModel

## Utiliser l'injection de d�pendances

> Dans cette solution, seul un exemple sur `Student` est pr�sent, les �tapes pr�sent�es sont � renouveller pour chaque "repository"

### Cr�ation des interfaces

Dans le projet `Repository`, cr�er un dossier `Repositories`, et dans celui ci, cr�er un dossier `Abstract`.

Dans le dossier `Abstract`, il convient de cr�er nos interfaces qui seront utilis�es dans les contr�leurs.

Cr�er une interface `IStudentRepository` : 
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

### Cr�ation des classes

Dans le dossier `Repositories` du projet `Repository`, cr�er les classes qui impl�mentes les interfaces pr�c�demment cr��es.

Cr�er un classe `StudentRepository` : 
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

> Dans le projet NetSchool, ajouter la r�f�rence du projet `Repository`

Dans un premier temps, il faut lier les interfaces � leur classes.

Dans la classe `NinjectDependencyResolver` qui se situe dans le dossier `Infrastructure` du projet `NetSchool`, localiser la m�thode `AddBindings()`.

Dans cette m�thode, lier les interfaces aux classes de la mani�re suivante : 
```C#
public void AddBindings()
{
    kernel.Bind<IStudentRepository>().To<StudentRepository>();
}
```

### Modifier les contr�leurs

La derni�re �tape consiste � mettre � jour les contr�leurs.

Cr�er un constructeur au contr�leur qui recoit le "repository" lui correspondant : 
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

[Voir le reste des modifications du contr�leur](https://github.com/CodeTrainerFormation/ASPMVC/blob/05-SoC-DI/NetSchool/NetSchoolWeb/Controllers/StudentController.cs)