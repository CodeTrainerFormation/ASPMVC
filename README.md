# Solution Lab-05

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/fbfa1dff2c3a073921777d8757727b33a274ee0a)

La solution pr�sent�e ci dessous ne contient pas l'injection de d�pendances

## Cr�ation des projets

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

## Mis � jour des projets

L'�tape suivante consiste � d�placer les fichiers pr�c�demment cr��s dans les nouveaux projets.
Les classes se trouvant dans le dossier `Models` sont d�plac�es dans le projet `DomainModel`.
Les classes se trouvant dans le dossier `Data` sont d�plac�es dans le projet `DAL`.

> **Attention ! Modifier les namespaces en fonction du projet dans lequel les classes se situent.**

- `NetSchool.Data` devient `DAL`
- `NetSchool.Models` devient `DomainModel` 

> **Les `using` des contr�leurs sont �galement � mettre � jour.**

> **Les vues qui utilise un `@model` sont �galement impact�es par ce changement.**

L'initialisation de la base de donn�es dans le fichier `Web.config` doit �tre �dti�e : 
```xml
	...
	<context type="**DAL**.SchoolDb, **DAL**">
	  <databaseInitializer type="**DAL**.SchoolInitializer, **DAL**" />
	</context>
	...
```