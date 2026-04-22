#:sdk Cake.Sdk@6.1.1

#:package Cake.Sonar@5.0.0

InstallTools(
    "dotnet:?package=GitVersion.Tool&version=6.7.0",
    "dotnet:?package=dotnet-sonarscanner&version=11.2.1"
);

var target = Argument("target", "Default");
var nugetApiKey = Argument("nugetApiKey", EnvironmentVariable("nugetApiKey"));
var configuration = Argument("configuration", "Release");
var sonarLogin = Argument("sonarLogin", EnvironmentVariable("sonarLogin"));

//////////////////////////////////////////////////////////////////////
//    Build Variables
/////////////////////////////////////////////////////////////////////
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

GitVersion? versionInfo = null;
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

        if (versionInfo is null)
            throw new InvalidOperationException("GitVersion information is not available.");

        var settings = new DotNetBuildSettings {
			Configuration = configuration,
			OutputDirectory = outputDir		 
		};	 		

		settings.MSBuildSettings = new DotNetMSBuildSettings()
		{
			PackageVersion = versionInfo.SemVer,
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
			Token = sonarLogin,
			UseCoreClr = true,
			VsTestReportsPath = testResultsPath.ToString(),
			OpenCoverReportsPath = codeCoverageResultFilePath.ToString(),
            ArgumentCustomization = args => args.Append("/d:sonar.scanner.scanAll=false") // disable Multi-Language analysis
        });
	});

Task("SonarEnd")
	.WithCriteria(!isLocalBuild)
	.Does(() => {
		SonarEnd(new SonarEndSettings {
            Token = sonarLogin
		});
	});


Task("Pack")
	.IsDependentOn("Test")
	.IsDependentOn("Version")
	.Does(() => {

        if (versionInfo is null)
            throw new InvalidOperationException("GitVersion information is not available.");

        var settings = new DotNetPackSettings
		{			
			Configuration = configuration,
			OutputDirectory = outputDirNuget,
			NoBuild = true	
		};

		settings.MSBuildSettings = new DotNetMSBuildSettings() {
				PackageVersion = versionInfo.SemVer
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

        foreach (var package in packages)
        {
            // Push the package.
            DotNetNuGetPush(package, new DotNetNuGetPushSettings
            {
                Source = nugetPublishFeed,
                ApiKey = nugetApiKey
            });
        }
	});	

Task("Default")
	.IsDependentOn("SonarBegin")
	.IsDependentOn("Test")	
	.IsDependentOn("SonarEnd")
	.IsDependentOn("Pack")
	.IsDependentOn("Publish");

RunTarget(target);
