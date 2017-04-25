# Solution Lab-05

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/00dab1cd01c9aebf514ed8c85ca62ec350f9c066)

La solution présentée ci dessous intègre l'injection de dépendances

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