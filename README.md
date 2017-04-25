# Solution Lab-05

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/00dab1cd01c9aebf514ed8c85ca62ec350f9c066)

La solution pr�sent�e ci dessous ne contient pas l'injection de d�pendances

## S�parer la solution

### Cr�ation des projets

Dans la solution, ajouter 2 nouveaux projets de type `Librairie de classes` nomm�s: 
- DAL
- DomainModel

Ajouter les r�f�rences suivantes : 
- Dans le projet DAL : 
  - EntityFramework
  - DomainModel
- Dans le projet DomainModel : 
  - System.ComponentModel.DataAnnotations
- Dans le projet NetSchool : 
  - DAL
  - DomainModel

### Mis � jour des projets

L'�tape suivante consiste � d�placer les fichiers pr�c�demment cr��s dans les nouveaux projets : 
- Les classes se trouvant dans le dossier `Models` sont d�plac�es dans le projet `DomainModel`.
- Les classes se trouvant dans le dossier `Data` sont d�plac�es dans le projet `DAL`.

**Attention ! Modifier les namespaces en fonction du projet dans lequel les classes se situent.**

- `NetSchool.Data` devient `DAL`
- `NetSchool.Models` devient `DomainModel` 

**Les `using` des contr�leurs sont �galement � mettre � jour.**

**Les vues qui utilise un `@model` sont �galement impact�es par ce changement.**

L'initialisation de la base de donn�es dans le fichier `Web.config` doit �tre �dti�e : 
```xml
  ...
  <context type="DAL.SchoolDb, DAL">
    <databaseInitializer type="DAL.SchoolInitializer, DAL" />
  </context>
  ...
```

## Helper HTML

### Cr�ation

Dans le projet NetSchool, cr�er un dossier `SDK`, et dans ce dossier, cr�er un dossier `Helpers`.

> Le helper cr�� ci dessous ne sera pas une m�thode d'extension d'un helper existant

Dans le dossier `Helpers`, cr�er une classe `FormHelper`. 

Le helper propos� a pour but de cr�er un bouton `submit` dans un formulaire.

Au sein de la classe, cr�er une m�thode SubmitBtn comme suit : 
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
@FormHelper.SubmitBtn("Cr�er", "success")
```

## Validateur

### Cr�ation

Cr�er un dossier `Validators` soit : 
- dans le projet `DomainModel` si la solution est s�par�e en plusieurs projets
- dans le dossier `SDK` du projet `NetSchool` si la s�paration des projets n'a pas �t� faite

Dans ce dossier cr�er une classe `PhoneNumber` qui h�rite de `ValidationAttribute`.

Surcharger la m�thode `IsValid(object value, ValidationContext validationContext)` : 
```C#
public class PhoneNumber : ValidationAttribute
{

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        String phone = Convert.ToString(value);

        Regex regPhoneNumber = new Regex(@"^(\+33 |0)[1-9]( \d\d){4}$");
        if (!regPhoneNumber.IsMatch(phone))
        {
            return new ValidationResult("Format invalide : +33 X XX XX XX XX ou XX XX XX XX XX sont autoris�s");
        }

        return ValidationResult.Success;
    }

}
```

> L'expression r�guli�re permet de v�rifier 2 formats de num�ros : '+33 X YY ZZ AA BB' ou '0X YY ZZ AA BB'

### Utilisation

Il suffit d'utiliser la classe comme une annotation sur un mod�le.

Par exemple, sur la classe `Teacher`, il est possible d'ajouter une propri�t� `Phone` : 

```C#
public class Teacher : Person
{
    [PhoneNumber]
    public string Phone { get; set; }
}
```