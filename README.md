# Solution Lab-04

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/fbfa1dff2c3a073921777d8757727b33a274ee0a)

## Création des vues

Dans le dossier `Views`, créer les dossiers suivants : 
- Classroom
- Student

Dans le dossier `Classroom`, créer les vues suivantes : 
- [Create](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Create.cshtml)
- [Delete](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Delete.cshtml)
- [Details](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Details.cshtml)
- [Edit](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Edit.cshtml)
- [Index](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Index.cshtml)
- [View](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/View.cshtml)

Dans le dossier `Student`, créer les vues suivantes : 
- [Create](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Create.cshtml)
- [Delete](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Delete.cshtml)
- [Details](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Details.cshtml)
- [Edit](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Edit.cshtml)
- [Index](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Index.cshtml)
- [IndexList](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/IndexList.cshtml)


Pour les vues du contrôleur `Teacher`, procéder de la sorte : 
- Supprimer le contrôleur `Teacher` actuel
- Créer un nouveau contrôleur : `Contrôleur MVC5 avec vues, utilisant EntityFramework`
..- Renseigner le champs `Classe modèle` par le modèle `Teacher`
..- Sélectionner le bon contexte de données
..- Cocher la case `Générer les vues`
..- Nommer le contrôleur `TeacherController`

En ce qui concerne la [vue partielle](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Shared/_StudentsList.cshtml), faire un clic droit sur le dossier `Views/Shared` > Ajouter > Vue... 
- Nom de la vue : `_StudentsList`
- Template : `List`
- Classe modèle : `Student`
- Cocher `Créer en tant que vue partielle`

## Modifier les contrôleurs

Pour ajouter de la validation sur un paramètre de l'url : 
```C#
[Route("{id:int:min(1)}")]
```

Pour renommer une action : 
```C#
[ActionName("NouveauNom")]
```