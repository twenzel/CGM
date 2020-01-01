#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"
#tool "nuget:?package=nuget.commandline&version=5.3.0"
#tool "nuget:?package=Codecov&version=1.9.0"

#addin "nuget:?package=Cake.Coverlet&version=2.3.4"
#addin "nuget:?package=Cake.Codecov&version=0.7.0"

var target = Argument("target", "Default");
var nugetApiKey = Argument("nugetApiKey", EnvironmentVariable("nugetApiKey"));
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
//    Build Variables
/////////////////////////////////////////////////////////////////////
var solution = "./codessentials.CGM.sln";
var project = File("./src/codessentials.CGM.csproj").Path.MakeAbsolute(Context.Environment);
var outputDir = Directory("./buildArtifacts/").Path.MakeAbsolute(Context.Environment);
var packageOutputDir = Directory("./buildArtifacts/Package").Path.MakeAbsolute(Context.Environment);
var outputDirNuget = outputDir+"NuGet/";
var testResultsPath = outputDir.CombineWithFilePath("TestResults.xml");
var codeCoverageResultFile = "CodeCoverageResults.xml";
var nugetPublishFeed = "https://api.nuget.org/v3/index.json";


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Setup(context =>
{
	Information($"Output directory: {outputDir.FullPath}");
	Information($"Package output directory: {packageOutputDir.FullPath}");
	Information($"Main project path: {project.FullPath}");	
});

Task("Clean")
	.Description("Removes the output directory")
	.Does(() => {
	  
	if (DirectoryExists(outputDir))
	{
		DeleteDirectory(outputDir, new DeleteDirectorySettings {
			Recursive = true,
			Force = true
		});
	}
	CreateDirectory(outputDir);
});

GitVersion versionInfo = null;
Task("Version")
	.Description("Retrieves the current version from the git repository")
	.Does(() => {
		
		versionInfo = GitVersion(new GitVersionSettings {
			UpdateAssemblyInfo = false
		});
		
		Information("Major:\t\t\t\t\t" + versionInfo.Major);
		Information("Minor:\t\t\t\t\t" + versionInfo.Minor);
		Information("Patch:\t\t\t\t\t" + versionInfo.Patch);
		Information("MajorMinorPatch:\t\t\t" + versionInfo.MajorMinorPatch);
		Information("SemVer:\t\t\t\t\t" + versionInfo.SemVer);
		Information("LegacySemVer:\t\t\t\t" + versionInfo.LegacySemVer);
		Information("LegacySemVerPadded:\t\t\t" + versionInfo.LegacySemVerPadded);
		Information("AssemblySemVer:\t\t\t\t" + versionInfo.AssemblySemVer);
		Information("FullSemVer:\t\t\t\t" + versionInfo.FullSemVer);
		Information("InformationalVersion:\t\t\t" + versionInfo.InformationalVersion);
		Information("BranchName:\t\t\t\t" + versionInfo.BranchName);
		Information("Sha:\t\t\t\t\t" + versionInfo.Sha);
		Information("NuGetVersionV2:\t\t\t\t" + versionInfo.NuGetVersionV2);
		Information("NuGetVersion:\t\t\t\t" + versionInfo.NuGetVersion);
		Information("CommitsSinceVersionSource:\t\t" + versionInfo.CommitsSinceVersionSource);
		Information("CommitsSinceVersionSourcePadded:\t" + versionInfo.CommitsSinceVersionSourcePadded);
		Information("CommitDate:\t\t\t\t" + versionInfo.CommitDate);
	});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Version")
	.Does(() => {				

		var settings = new DotNetCoreBuildSettings {
			Configuration = configuration,
			OutputDirectory = outputDir		 
		};
	 
		settings.MSBuildSettings = new DotNetCoreMSBuildSettings()
		 .WithProperty("PackageVersion", versionInfo.NuGetVersionV2)
		 .WithProperty("Version", versionInfo.AssemblySemVer)
		 .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
		 .WithProperty("PackageOutputPath", packageOutputDir.FullPath)
		 .WithProperty("SourceLinkCreate", "true");
	 
		// creates also the NuGet packages
		DotNetCoreBuild(project.FullPath, settings);	
	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		var settings = new DotNetCoreTestSettings {
			Configuration = configuration,
			Logger = "trx;logfilename=" + testResultsPath
		};

		var coveletSettings = new CoverletSettings
        {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.opencover,
            CoverletOutputDirectory = outputDir,
            CoverletOutputName = codeCoverageResultFile,
        };
	
		DotNetCoreTest("./tests/codessentials.CGM.Tests.csproj", settings, coveletSettings);	

		var codecovSettings = new CodecovSettings{
			Branch = versionInfo.BranchName,
			Build = versionInfo.FullSemVer,
			Commit = versionInfo.Sha,	
			Files = new List<string>(new[]{outputDir.CombineWithFilePath(codeCoverageResultFile).ToString()})	
		};
		
		Codecov(codecovSettings);
	});


Task("Pack")
	.IsDependentOn("Test")
	.IsDependentOn("Version")
	.Does(() => {
		
		var settings = new DotNetCorePackSettings
		{			
			Configuration = configuration,
			OutputDirectory = outputDirNuget,
			NoBuild = true	
		};

		settings.MSBuildSettings = new DotNetCoreMSBuildSettings()
			.WithProperty("PackageVersion", versionInfo.NuGetVersionV2)
			.WithProperty("SourceLinkCreate", "true");
	 
		 
		DotNetCorePack(project.FullPath, settings);			
	});
	
Task("Publish")	
	.IsDependentOn("Pack")	
	.Description("Pushes the created NuGet packages to nuget.org")  
	.Does(() => {
	
		// Get the paths to the packages.
		var packages = GetFiles(outputDirNuget + "*.nupkg");

		// Push the package.
		NuGetPush(packages, new NuGetPushSettings {
			Source = nugetPublishFeed,
			ApiKey = nugetApiKey
		});	
	});
	
Task("Default")
	.IsDependentOn("Test");

RunTarget(target);