# Solution Lab-05

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/00dab1cd01c9aebf514ed8c85ca62ec350f9c066)

La solution pr�sent�e ci dessous int�gre l'injection de d�pendances

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