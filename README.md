# Solution Lab-02

[Voir le diff�rentiel avec le lab pr�c�dent](https://github.com/CodeTrainerFormation/ASPMVC/commit/8d12d11c0ef0d711c75623f5b381a5570006c5ac)

## Cr�ation des mod�les

Dans le dossier `Models`, cr�er 4 classes : 
- [Student](https://github.com/CodeTrainerFormation/ASPMVC/blob/02-Model/NetSchool/NetSchoolWeb/Models/Student.cs)
- [Classroom](https://github.com/CodeTrainerFormation/ASPMVC/blob/02-Model/NetSchool/NetSchoolWeb/Models/Classroom.cs)
- [Teacher](https://github.com/CodeTrainerFormation/ASPMVC/blob/02-Model/NetSchool/NetSchoolWeb/Models/Teacher.cs)
- [Person](https://github.com/CodeTrainerFormation/ASPMVC/blob/02-Model/NetSchool/NetSchoolWeb/Models/Person.cs)

## Cr�ation du contexte de donn�es

Ajouter un dossier `Data` dans le projet

> Attention ! Ajouter `EntityFramework` en d�pendance du projet via le NuGet Package Manager

Dans ce dossier, ajouter les classes : 
- [SchoolDB](https://github.com/CodeTrainerFormation/ASPMVC/blob/02-Model/NetSchool/NetSchoolWeb/Data/SchoolDb.cs) qui h�rite de `DbContext`
- [SchoolInitializer](https://github.com/CodeTrainerFormation/ASPMVC/blob/02-Model/NetSchool/NetSchoolWeb/Data/SchoolInitializer.cs)

Ensuite, pour initialiser la base de donn�es, 2 solutions : 
- Ajouter la ligne suivantes dans la m�thode `Application_Start()` du fichier `Global.asax.cs` : 
```C#
Database.SetInitializer<SchoolDb>(new SchoolInitializer());
```
- Editer le fichier Web.config, en ajoutant les lignes suivantes : 
```xml
<configuration>
  ...
  <entityFramework>
    <contexts>
      <context type="NetSchoolWeb.Data.SchoolDb, NetSchoolWeb">
        <databaseInitializer type="NetSchoolWeb.Data.SchoolInitializer, NetSchoolWeb" />
      </context>
    </contexts>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
      <parameters>
        <parameter value="mssqllocaldb" />
      </parameters>
    </defaultConnectionFactory>
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
    </providers>
  </entityFramework>
</configuration>
```