# Solution Lab-04

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/fbfa1dff2c3a073921777d8757727b33a274ee0a)

## Cr�ation des vues

Dans le dossier `Views`, cr�er les dossiers suivants : 
- Classroom
- Student

Dans le dossier `Classroom`, cr�er les vues suivantes : 
- [Create](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Create.cshtml)
- [Delete](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Delete.cshtml)
- [Details](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Details.cshtml)
- [Edit](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Edit.cshtml)
- [Index](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/Index.cshtml)
- [View](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Classroom/View.cshtml)

Dans le dossier `Student`, cr�er les vues suivantes : 
- [Create](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Create.cshtml)
- [Delete](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Delete.cshtml)
- [Details](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Details.cshtml)
- [Edit](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Edit.cshtml)
- [Index](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/Index.cshtml)
- [IndexList](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Student/IndexList.cshtml)


Pour les vues du contr�leur `Teacher`, proc�der de la sorte : 
- Supprimer le contr�leur `Teacher` actuel
- Cr�er un nouveau contr�leur : `Contr�leur MVC5 avec vues, utilisant EntityFramework`
..- Renseigner le champs `Classe mod�le` par le mod�le `Teacher`
..- S�lectionner le bon contexte de donn�es
..- Cocher la case `G�n�rer les vues`
..- Nommer le contr�leur `TeacherController`

En ce qui concerne la [vue partielle](https://github.com/CodeTrainerFormation/ASPMVC/blob/04-Views/NetSchool/NetSchoolWeb/Views/Shared/_StudentsList.cshtml), faire un clic droit sur le dossier `Views/Shared` > Ajouter > Vue... 
- Nom de la vue : `_StudentsList`
- Template : `List`
- Classe mod�le : `Student`
- Cocher `Cr�er en tant que vue partielle`

## Modifier les contr�leurs

Pour ajouter de la validation sur un param�tre de l'url : 
```C#
[Route("{id:int:min(1)}")]
```

Pour renommer une action : 
```C#
[ActionName("NouveauNom")]
```