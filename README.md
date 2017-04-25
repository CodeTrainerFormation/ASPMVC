# Solution Lab-05

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/00dab1cd01c9aebf514ed8c85ca62ec350f9c066)

La solution présentée ci dessous ne contient pas l'injection de dépendances

## Séparer la solution

### Création des projets

Dans la solution, ajouter 2 nouveaux projets de type `Librairie de classes` nommés: 
- DAL
- DomainModel

Ajouter les références suivantes : 
- Dans le projet DAL : 
  - EntityFramework
  - DomainModel
- Dans le projet DomainModel : 
  - System.ComponentModel.DataAnnotations
- Dans le projet NetSchool : 
  - DAL
  - DomainModel

### Mis à jour des projets

L'étape suivante consiste à déplacer les fichiers précédemment créés dans les nouveaux projets : 
- Les classes se trouvant dans le dossier `Models` sont déplacées dans le projet `DomainModel`.
- Les classes se trouvant dans le dossier `Data` sont déplacées dans le projet `DAL`.

**Attention ! Modifier les namespaces en fonction du projet dans lequel les classes se situent.**

- `NetSchool.Data` devient `DAL`
- `NetSchool.Models` devient `DomainModel` 

**Les `using` des contrôleurs sont également à mettre à jour.**

**Les vues qui utilise un `@model` sont également impactées par ce changement.**

L'initialisation de la base de données dans le fichier `Web.config` doit être édtiée : 
```xml
  ...
  <context type="DAL.SchoolDb, DAL">
    <databaseInitializer type="DAL.SchoolInitializer, DAL" />
  </context>
  ...
```

## Helper HTML

### Création

Dans le projet NetSchool, créer un dossier `SDK`, et dans ce dossier, créer un dossier `Helpers`.

> Le helper créé ci dessous ne sera pas une méthode d'extension d'un helper existant

Dans le dossier `Helpers`, créer une classe `FormHelper`. 

Le helper proposé a pour but de créer un bouton `submit` dans un formulaire.

Au sein de la classe, créer une méthode SubmitBtn comme suit : 
```C#
public class FormHelper
{

    public static IHtmlString SubmitBtn(string content = null, string classname = null)
    {
        content = content ?? "Envoyer";
        classname = classname ?? "default";

        return new HtmlString(string.Format("<input type=\"submit\" value=\"{0}\" class=\"btn btn-{1}\" />", content, classname));
    }

}
```

### Utilisation

Renseigner dans le fichier `Views\web.config` le namespace du helper.

```xml
<configuration>
  ...
  <system.web.webPages.razor>
    ...
    <pages pageBaseType="System.Web.Mvc.WebViewPage">
      <namespaces>
        ...
        <add namespace="NetSchoolWeb.SDK.Helpers" />
      </namespaces>
    </pages>
  </system.web.webPages.razor>
  ...
</configuration>
```

Le helper est maintenant utilisable dans une vue : 
```cshtml
@FormHelper.SubmitBtn("Créer", "success")
```

## Validateur

### Création

Créer un dossier `Validators` soit : 
- dans le projet `DomainModel` si la solution est séparée en plusieurs projets
- dans le dossier `SDK` du projet `NetSchool` si la séparation des projets n'a pas été faite

Dans ce dossier créer une classe `PhoneNumber` qui hérite de `ValidationAttribute`.

Surcharger la méthode `IsValid(object value, ValidationContext validationContext)` : 
```C#
public class PhoneNumber : ValidationAttribute
{

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        String phone = Convert.ToString(value);

        Regex regPhoneNumber = new Regex(@"^(\+33 |0)[1-9]( \d\d){4}$");
        if (!regPhoneNumber.IsMatch(phone))
        {
            return new ValidationResult("Format invalide : +33 X XX XX XX XX ou XX XX XX XX XX sont autorisés");
        }

        return ValidationResult.Success;
    }

}
```

> L'expression régulière permet de vérifier 2 formats de numéros : '+33 X YY ZZ AA BB' ou '0X YY ZZ AA BB'

### Utilisation

Il suffit d'utiliser la classe comme une annotation sur un modèle.

Par exemple, sur la classe `Teacher`, il est possible d'ajouter une propriété `Phone` : 

```C#
public class Teacher : Person
{
    [PhoneNumber]
    public string Phone { get; set; }
}
```