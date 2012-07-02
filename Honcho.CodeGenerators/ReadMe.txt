UPDATE [EDMXPATH] variable


---------------------------------------------------------------------------------------------
BuddyClass Generator Readme.
Ben Griswold, 11/01/2011
---------------------------------------------------------------------------------------------


---------------------------------------------------------------------------------------------
Overview of T4 (Text Template Transformation Toolkit)
---------------------------------------------------------------------------------------------

In Visual Studio, a T4 text template is a mixture of text blocks and control logic that 
can generate a text file. The control logic is written as fragments of program code in 
Visual C# or Visual Basic. The generated file can be text of any kind, such as a Web page, 
or a resource file, or program source code in any language. I've written and seen T4 
templates used for varying purposes from generating generic repositories, generic services, 
viewmodels, menus and EF buddy classes. You can even override the out-of-the-box controller
and view T4 templates in ASP.NET MVC.

Though Visual Studio recognizes the T4 .tt files and allows one to produce the generated 
code through the IDE, intellisense and syntax highlighting is not supported without an 
add-ins like T4 Toolbox (http://t4toolbox.codeplex.com/), 
T4 Editor (http://www.olegsych.com/2009/04/t4-editor-by-tangible-engineering/) and 
Visual T4 (http://visualstudiogallery.msdn.microsoft.com/40a887aa-f3be-40ec-a85d-37044b239591). 
Really, add-ins are nice but they aren't required once one is comfortable with the syntax.

I won't dive into the syntax here since the "Additional Reading" links offer many samples.


---------------------------------------------------------------------------------------------
Overview of the BuddyClass T4 Tempate
---------------------------------------------------------------------------------------------

The Metadata generator is based on a some of the templated code found in the 
SubSonic DAL Project (http://subsonicproject.com/docs/T4_Templates). It's the SubSonic code
which interrogates your database and maps database types to C# data types. The custom 
Metadata code sits on top of the SubSonic code and manipulates the output into multiple
Metadataes with annotations.


---------------------------------------------------------------------------------------------
How To Use the BuddyClass Generator
---------------------------------------------------------------------------------------------

The BuddyClass Generator consists of four files:

	BuddyClass.tt
	BuddyClass.ttinclude
	Settings.ttinclude
	SqlService.ttinclue
	App.Config


To generate your metadata files, you will need to update the following configuration information:

1. Update the App.Config files with your desired database connectionstring

2. Update the Settings.ttinclude file with the following:
	
	A. Desired namespace for the BuddyClasses. For example, DataDomain.Entities.

	B. Assign the connectionstring name in App.Config to the ConnectionStringName variable.  For
		example, AccountsDB.

	C. Assign the name of the database to the DatabaseName variable. 

	D. Not required, but you may update the ExcludeTables array with any tables in the database 
		which you do not wish to have BuddyClasses generated.

3. Right-click on the BuddyClass.tt file in Visual Studio's Solution Explorer and then select 
	"Run Custom Tool". This action will generate a BuddyClass.cs which is a child of BuddyClass.tt. 
	Note: The act of modifying and saving the .tt will run the custom tool as well.
	
4. Open BuddyClass.cs and inspect the contents of the C# file. Without a project reference to 
	System.ComponentModel.DataAnnotations, there will be a number of "Can not resolve symbol" 
	errors. If you want to make changes to the output, you'll want to edit the 
	BuddyClass.tt file and then regenerating.

6. Once satisfied with the output, the contents of the file can be copy and pasted into your solution.
	Alternatively, the BuddyClass generator can live within the project where it will ultimately
	reside.


---------------------------------------------------------------------------------------------
Additional Reading
---------------------------------------------------------------------------------------------

Code Generation and T4 Text Templates, MSDN
http://msdn.microsoft.com/en-us/library/bb126445.aspx	

T4 (Text Template Transformation Toolkit) Code Generation - Best Kept Visual Studio Secret, Scott Hanselman
http://www.hanselman.com/blog/T4TextTemplateTransformationToolkitCodeGenerationBestKeptVisualStudioSecret.aspx

Make Visual Studio Generate Your Repository, Rob Conery
http://wekeroad.com/blog/make-visual-studio-generate-your-repository/