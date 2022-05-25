#tool "dotnet:?package=GitVersion.Tool&version=5.8.1"
#tool "nuget:?package=NuGet.CommandLine&version=6.1.0"
#tool "nuget:?package=dotnet-sonarscanner&version=5.5.3"

#addin "nuget:?package=Cake.Sonar&version=1.1.30"

var target = Argument("target", "Default");
var nugetApiKey = Argument("nugetApiKey", EnvironmentVariable("nugetApiKey"));
var configuration = Argument("configuration", "Release");
var sonarLogin = Argument("sonarLogin", EnvironmentVariable("sonarLogin"));

//////////////////////////////////////////////////////////////////////
//    Build Variables
/////////////////////////////////////////////////////////////////////
var solution = "./codessentials.CGM.sln";
var project = File("./src/codessentials.CGM.csproj").Path.MakeAbsolute(Context.Environment);
var outputDir = Directory("./buildArtifacts/").Path.MakeAbsolute(Context.Environment);
var packageOutputDir = Directory("./buildArtifacts/Package").Path.MakeAbsolute(Context.Environment);
var outputDirNuget = outputDir.Combine("NuGet");
var outputDirTests = outputDir.Combine("Tests");
var codeCoverageResultFilePath = MakeAbsolute(outputDirTests).Combine("**/").CombineWithFilePath("coverage.opencover.xml");
var testResultsPath = MakeAbsolute(outputDirTests).CombineWithFilePath("*.trx");
var nugetPublishFeed = "https://api.nuget.org/v3/index.json";
var sonarProjectKey = "twenzel_CGM";
var sonarUrl = "https://sonarcloud.io";
var sonarOrganization = "twenzel";
var isLocalBuild = string.IsNullOrEmpty(EnvironmentVariable("GITHUB_REPOSITORY"));
var isPullRequest = !string.IsNullOrEmpty(EnvironmentVariable("GITHUB_HEAD_REF"));
var gitHubEvent = EnvironmentVariable("GITHUB_EVENT_NAME");
var isReleaseCreation = string.Equals(gitHubEvent, "release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Setup(context =>
{
	Information($"Output directory: {outputDir.FullPath}");
	Information($"Package output directory: {packageOutputDir.FullPath}");
	Information($"Main project path: {project.FullPath}");	
	Information($"Local build: {isLocalBuild}");
	Information($"Is pull request: {isPullRequest}");	
	Information($"Is release creation: {isReleaseCreation}");	
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

		var settings = new DotNetBuildSettings {
			Configuration = configuration,
			OutputDirectory = outputDir		 
		};	 		

		settings.MSBuildSettings = new DotNetMSBuildSettings()
		{
			PackageVersion = versionInfo.NuGetVersionV2,
			AssemblyVersion = versionInfo.AssemblySemVer,
			Version = versionInfo.AssemblySemVer,
			InformationalVersion = versionInfo.InformationalVersion
		}
		 .WithProperty("PackageOutputPath", packageOutputDir.FullPath)
		 .WithProperty("SourceLinkCreate", "true");
	 
		// creates also the NuGet packages
		DotNetBuild(project.FullPath, settings);	
	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{		
		var settings = new DotNetTestSettings
		{
			Configuration = configuration,
			Loggers = new[] {"trx;"},
			ResultsDirectory = outputDirTests,
			Collectors = new[] {"XPlat Code Coverage"},
			ArgumentCustomization = a => a.Append("-- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover")
		};

		DotNetTest("./tests/codessentials.CGM.Tests.csproj", settings);	
	});

Task("SonarBegin")
	.WithCriteria(!isLocalBuild)
	.Does(() => {
		SonarBegin(new SonarBeginSettings {
			Key = sonarProjectKey,
			Url = sonarUrl,
			Organization = sonarOrganization,
			Login = sonarLogin,
			UseCoreClr = true,
			VsTestReportsPath = testResultsPath.ToString(),
			OpenCoverReportsPath = codeCoverageResultFilePath.ToString()
		});
	});

Task("SonarEnd")
	.WithCriteria(!isLocalBuild)
	.Does(() => {
		SonarEnd(new SonarEndSettings {
			Login = sonarLogin
		});
	});


Task("Pack")
	.IsDependentOn("Test")
	.IsDependentOn("Version")
	.Does(() => {
		
		var settings = new DotNetPackSettings
		{			
			Configuration = configuration,
			OutputDirectory = outputDirNuget,
			NoBuild = true	
		};

		settings.MSBuildSettings = new DotNetMSBuildSettings() {
				PackageVersion = versionInfo.NuGetVersionV2
		}.WithProperty("SourceLinkCreate", "true");
	 		 
		DotNetPack(project.FullPath, settings);			
	});
	
Task("Publish")	
	.WithCriteria(isReleaseCreation)
	.IsDependentOn("Pack")	
	.Description("Pushes the created NuGet packages to nuget.org")  
	.Does(() => {
	
		Information($"Upload packages from {outputDirNuget.FullPath}");

		// Get the paths to the packages.
		var packages = GetFiles(outputDirNuget.CombineWithFilePath("*.nupkg").ToString());

		if (packages.Count == 0)
		{
			Error("No packages found to upload");
			return;
		}

		// Push the package.
		NuGetPush(packages, new NuGetPushSettings {
			Source = nugetPublishFeed,
			ApiKey = nugetApiKey
		});	
	});	

Task("Default")
	.IsDependentOn("SonarBegin")
	.IsDependentOn("Test")	
	.IsDependentOn("SonarEnd")
	.IsDependentOn("Pack")
	.IsDependentOn("Publish");

RunTarget(target);