//AutoGenerate

using System;
using GDFFoundation;

namespace GDFRuntime
{
    /// <summary>
    /// Represents information about the assembly for the <see cref="GDFRuntime"/> namespace.
    /// Provides metadata such as Git information, version details, and build data.
    /// Inherits from <see cref="GDFAssemblyInformation{T}"/> and implements specific overrides for the assembly.
    /// </summary>
    public class GDFAssemblyInfo : GDFAssemblyInformation<GDFAssemblyInfo>
        {
            /// <summary>
            /// Retrieves the Configuration used to build this DLL.
            /// </summary>
            /// <returns>The configuration used as a <see cref="string"/>.</returns>
            public override string Configuration() => "Debug";
        
            /// <summary>
            /// Retrieves the full Git commit hash.
            /// </summary>
            /// <returns>The full commit hash as a <see cref="string"/>.</returns>
            public override string GitCommit() => "5a08a90ceb501c7eacbcfdab97892ec6f702a68c";
            
            /// <summary>
            /// Retrieves the short Git commit hash.
            /// </summary>
            /// <returns>The short commit hash as a <see cref="string"/>.</returns>
            public override string GitShortCommit() => "5a08a90ce";

            /// <summary>
            /// Retrieves the name of the Git branch for this assembly.
            /// </summary>
            /// <returns>The Git branch name as a <see cref="string"/>.</returns>
            public override string GitBranch() => "main";

            /// <summary>
            /// Retrieves the Git tag associated with the current project version.
            /// </summary>
            /// <returns>
            /// The Git tag as a <see cref="string"/>. If no tag is found, an error message is returned.
            /// </returns>
            public override string GitTag() => "fatal: No names found, cannot describe anything.";

            /// <summary>
            /// Retrieves the author of the last Git commit.
            /// </summary>
            /// <returns>The name of the last commit author as a <see cref="string"/>.</returns>
            public override string GitLastCommitAuthor() => "Boulogne Quentin";

            /// <summary>
            /// Retrieves the date of the last Git commit.
            /// </summary>
            /// <returns>The date of the last Git commit as a <see cref="string"/>.</returns>
            public override string GitLastCommitDate() => "2025-09-17T16:50:17+02:00";

            /// <summary>
            /// Indicates whether the current state of the Git repository has uncommitted changes.
            /// </summary>
            /// <returns>A <see cref="string"/> representation of whether the repository is dirty ("true" for dirty, "false" otherwise).</returns>
            public override string GitIsDirty() => "false";

            /// <summary>
            /// Retrieves the name of the current assembly information.
            /// </summary>
            /// <returns>The name of the assembly as a <see cref="string"/>.</returns>
            public override string Name() => "GDFRuntime";
            
            /// <summary>
            /// Provides a description of the project.
            /// </summary>
            /// <returns>A string containing the description of the project as defined in <see cref="GDFAssemblyInfo"/>.</returns>
            public override string Description() => "Runtime";

            /// <summary>
            /// Retrieves the notes associated with this assembly information.
            /// </summary>
            /// <returns>The notes as a <see cref="string"/>.</returns>
            public override string Notes() => "note";

            /// <summary>
            /// Retrieves the company name associated with the assembly.
            /// </summary>
            /// <returns>The company name as a <see cref="string"/>.</returns>
            public override string Company() => "idéMobi";

            /// <summary>
            /// Retrieves the copyright information for the assembly.
            /// </summary>
            /// <returns>The copyright information as a <see cref="string"/>.</returns>
            public override string Copyright() => "Copyright © idéMobi 2025";

            /// <summary>
            /// Retrieves the build date of the assembly.
            /// </summary>
            /// <returns>The build date as a <see cref="string"/>.</returns>
            public override string BuildDate() => "2025-09-17 16:50:17";

            /// <summary>
            /// Determines whether the NuGet SDK is used for the current assembly information setup.
            /// </summary>
            /// <returns>A <see cref="bool"/> indicating whether the NuGet SDK is in use.</returns>
            public override bool IsNuGet() => false;

            // /// <summary>
            // /// Retrieves the version information of the assembly.
            // /// </summary>
            // /// <returns>The version string of the assembly as a <see cref="string"/>.</returns>
            //public override string Version() => "2025.917.1650";

            // /// <summary>
            // /// Retrieves the file version information of the assembly.
            // /// </summary>
            // /// <returns>The file version of the assembly as a <see cref="string"/>.</returns>
            //public override string FileVersion() => "2025.917.1650";

            /// <summary>
            /// Gets the target .NET version of the project.
            /// </summary>
            /// <returns>The target .NET version as a <see cref="string"/>.</returns>
            public override string DotNet() => "net9.0";
       
        }
}
