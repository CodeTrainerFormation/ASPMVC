# Solution Lab-05

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/fbfa1dff2c3a073921777d8757727b33a274ee0a)

La solution présentée ci dessous ne contient pas l'injection de dépendances

## Création des projets

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

## Mis à jour des projets

L'étape suivante consiste à déplacer les fichiers précédemment créés dans les nouveaux projets.
Les classes se trouvant dans le dossier `Models` sont déplacées dans le projet `DomainModel`.
Les classes se trouvant dans le dossier `Data` sont déplacées dans le projet `DAL`.

> **Attention ! Modifier les namespaces en fonction du projet dans lequel les classes se situent.**

- `NetSchool.Data` devient `DAL`
- `NetSchool.Models` devient `DomainModel` 

> **Les `using` des contrôleurs sont également à mettre à jour.**

> **Les vues qui utilise un `@model` sont également impactées par ce changement.**

L'initialisation de la base de données dans le fichier `Web.config` doit être édtiée : 
```xml
	...
	<context type="**DAL**.SchoolDb, **DAL**">
	  <databaseInitializer type="**DAL**.SchoolInitializer, **DAL**" />
	</context>
	...
```