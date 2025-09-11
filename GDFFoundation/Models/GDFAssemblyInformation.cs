#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFAssemblyInfo.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#endregion

namespace GDFFoundation
{
    public interface IGDFAssemblyInformation
    {
        public string Configuration()=> "no configuration";
        public string GitCommit() => "error";
        public string GitShortCommit() => "error";
        public string GitBranch() => "error";
        public string GitTag() => "error";
        public string GitLastCommitAuthor() => "error";
        public string GitLastCommitDate() => "error";
        public string GitIsDirty() => "error";
        public string Name() => "error";
        public string Description() => "error";
        public string Notes() => "error";
        public string Company() => "error";
        public string Copyright() => "error";
        public string BuildDate() => "error";
        public string DotNet() => "error";
        public bool IsNuGet() => false;
        public bool DebugStatus { set; get; }
        public bool DebugTrace { set; get; }
        public string Version() => "error";
        public string FileVersion() => "error";
    }

    public class GDFAssemblyInformation<T> : GDFAssemblyInformation where T : GDFAssemblyInformation<T>, new()
    {
        private static T _singleton = null;

        public static T Shared
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new T();
                    LibrariesWorkflow.AddAssemblyInfo(_singleton);
                }

                return _singleton;
            }
        }
    }

    public class GDFAssemblyInformation : IGDFAssemblyInformation
    {
        public virtual string Configuration()=> "no configuration";
        public virtual string GitCommit() => "unknown";
        public virtual string GitShortCommit() => "unknown";
        public virtual string GitBranch() => "unknown";
        public virtual string GitTag() => "unknown";
        public virtual string GitLastCommitAuthor() => "unknown";
        public virtual string GitLastCommitDate() => "unknown";
        public virtual string GitIsDirty() => "unknown";
        public virtual string Name()
        {
            Assembly assembly = this.GetType().Assembly;
            string result = assembly.GetName().Name ?? "unknown";
            return result;
        }
        public virtual string Description() => "unknown";
        public virtual string Notes() => "unknown";
        public virtual string Company() => "unknown";
        public virtual string Copyright() => "unknown";
        public virtual string BuildDate() => "unknown";
        public virtual string DotNet() => "unknown";
        
        public virtual bool IsNuGet() => true;

        public virtual string Version()
        {
            Assembly assembly = this.GetType().Assembly;
            string result = assembly.GetName().Version?.ToString() ?? "unknown";
            return result;
        }
        public virtual string FileVersion()
        {
            Assembly assembly = this.GetType().Assembly;
            var fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            string result = fileVersionAttribute?.Version ?? "unknown";
            return result;
        }
        public bool DebugStatus { set; get; }
        public bool DebugTrace { set; get; }

        public bool Printed { set; get; } = false;

        public void ConsolePrint()
        {
            if (Printed == false)
            {
                Printed = true;
                if (GDFFoundation.GDFConstants.PrintAscii.HasFlag(PrintAsciiKind.Logo))
                {
                    PrintLogo();
                }

                if (GDFFoundation.GDFConstants.PrintAscii.HasFlag(PrintAsciiKind.Version))
                {
                    PrintVersion();
                }

                if (GDFFoundation.GDFConstants.PrintAscii.HasFlag(PrintAsciiKind.Information))
                {
                    PrintInformation();
                }
            }
        }
        
        public void Information(Assembly assembly)
        {

        }

        public static GDFAssemblyInformation GetExecuting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return LibrariesWorkflow.GetForAssembly(assembly);
        }

        #region Static fields and properties

        static readonly Dictionary<char, string[]> RectangleFont = new()
        {
            {
                '°', new[]
                {
                    @"                        ",
                    @"      __--------__      ",
                    @"    (    _    _    )    ",
                    @"  /     |_|  |_|     \  ",
                    @"  `-________________-'  ",
                    @"      _____  _____      ",
                    @"      `-__|  |__-'      ",
                    @"                        ",
                    @"                        ",
                }
            },
            {
                ' ', new[]
                {
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                }
            },
            {
                '!', new[]
                {
                    "    ",
                    " __ ",
                    "|  |",
                    "|  |",
                    "|__|",
                    "|__|",
                    "    ",
                    "    ",
                    "    ",
                }
            },
            {
                '#', new[]
                {
                    "         ",
                    "   _ _   ",
                    " _| | |_ ",
                    "|_     _|",
                    "|_     _|",
                    "  |_|_|  ",
                    "         ",
                    "         ",
                    "         ",
                }
            },
            {
                '$', new[]
                {
                    "       ",
                    "   _   ",
                    " _| |_ ",
                    "|   __|",
                    "|__   |",
                    "|_   _|",
                    "  |_|  ",
                    "       ",
                    "       ",
                }
            },
            {
                '%', new[]
                {
                    "       ",
                    "       ",
                    " __ __ ",
                    "|__|  |",
                    "|   __|",
                    "|__|__|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                '&', new[]
                {
                    "       ",
                    "   _   ",
                    " _| |_ ",
                    "|   __|",
                    "|   __|",
                    "|_   _|",
                    "  |_|  ",
                    "       ",
                    "       ",
                }
            },
            {
                '\'', new[]
                {
                    "   ",
                    " _ ",
                    "| |",
                    "|_|",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                }
            },
            {
                '(', new[]
                {
                    "     ",
                    "   _ ",
                    " _|_|",
                    "| |  ",
                    "| |  ",
                    "|_|_ ",
                    "  |_|",
                    "     ",
                    "     ",
                }
            },
            {
                ')', new[]
                {
                    "     ",
                    " _   ",
                    "|_|_ ",
                    "  | |",
                    "  | |",
                    " _|_|",
                    "|_|  ",
                    "     ",
                    "     ",
                }
            },
            {
                '*', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "| | | |",
                    "|-   -|",
                    "|_|_|_|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                '+', new[]
                {
                    "       ",
                    "       ",
                    "   _   ",
                    " _| |_ ",
                    "|_   _|",
                    "  |_|  ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                ',', new[]
                {
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    " _ ",
                    "| |",
                    "|_|",
                    "   ",
                    "   ",
                }
            },
            {
                '-', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '.', new[]
                {
                    "   ",
                    "   ",
                    " _ ",
                    "|_|",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                    "   ",
                }
            },
            {
                '/', new[]
                {
                    "     ",
                    "     ",
                    "   _ ",
                    "  / |",
                    " / / ",
                    "|_/  ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '0', new[]
                {
                    " ___ ",
                    "|   |",
                    "| | |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '1', new[]
                {
                    " ___   ",
                    "|_  |  ",
                    " _| |_ ",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                '2', new[]
                {
                    " ___ ",
                    "|_  |",
                    "|  _|",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '3', new[]
                {
                    " ___ ",
                    "|_  |",
                    "|_  |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '4', new[]
                {
                    " ___ ",
                    "| | |",
                    "|_  |",
                    "  |_|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '5', new[]
                {
                    " ___ ",
                    "|  _|",
                    "|_  |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '6', new[]
                {
                    " ___ ",
                    "|  _|",
                    "| . |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '7', new[]
                {
                    " ___ ",
                    "|_  |",
                    "  | |",
                    "  |_|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '8', new[]
                {
                    " ___ ",
                    "| . |",
                    "| . |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '9', new[]
                {
                    " ___ ",
                    "| . |",
                    "|_  |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                ':', new[]
                {
                    "   ",
                    "   ",
                    " _ ",
                    "|_|",
                    " _ ",
                    "|_|",
                    "   ",
                    "   ",
                    "   ",
                }
            },
            {
                ';', new[]
                {
                    "   ",
                    "   ",
                    " _ ",
                    "|_|",
                    " _ ",
                    "| |",
                    "|_|",
                    "   ",
                    "   ",
                }
            },
            {
                '<', new[]
                {
                    "     ",
                    "   __",
                    "  / /",
                    " / / ",
                    "< <  ",
                    " \\ \\ ",
                    "  \\_\\",
                    "     ",
                    "     ",
                }
            },
            {
                '=', new[]
                {
                    "       ",
                    "       ",
                    "       ",
                    " _____ ",
                    "|_____|",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                '>', new[]
                {
                    "     ",
                    "__   ",
                    "\\ \\  ",
                    " \\ \\ ",
                    "  > >",
                    " / / ",
                    "/_/  ",
                    "     ",
                    "     ",
                }
            },
            {
                '?', new[]
                {
                    "       ",
                    " _____ ",
                    "|___  |",
                    "  |  _|",
                    "  |_|  ",
                    "  |_|  ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                '@', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  __ |",
                    "| |___|",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'A', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  ^  |",
                    "|     |",
                    "|__|__|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'B', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "| __  |",
                    "| __ -|",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'C', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|    _|",
                    "|   |_ ",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'D', new[]
                {
                    "       ",
                    "       ",
                    " ____  ",
                    "|    \\ ",
                    "|  |  |",
                    "|____/ ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'E', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|   __|",
                    "|   __|",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'F', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|   __|",
                    "|   __|",
                    "|__|   ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'G', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|   __|",
                    "|  |  |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'H', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  |  |",
                    "|     |",
                    "|__|__|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'I', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|     |",
                    " |   | ",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'J', new[]
                {
                    "       ",
                    "       ",
                    "    __ ",
                    " __|  |",
                    "|  |  |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'K', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  |  |",
                    "|    -|",
                    "|__|__|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'L', new[]
                {
                    "       ",
                    "       ",
                    " __    ",
                    "|  |   ",
                    "|  |__ ",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'M', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|     |",
                    "| | | |",
                    "|_|_|_|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'N', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|   | |",
                    "| | | |",
                    "|_|___|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'O', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|     |",
                    "|  |  |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'P', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  _  |",
                    "|   __|",
                    "|__|   ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'Q', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|     |",
                    "|  |  |",
                    "|__  _|",
                    "   |__|",
                    "       ",
                    "       ",
                }
            },
            {
                'R', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "| __  |",
                    "|    -|",
                    "|__|__|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'S', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|   __|",
                    "|__   |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'T', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|_   _|",
                    "  | |  ",
                    "  |_|  ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'U', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  |  |",
                    "|  |  |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'V', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|  |  |",
                    "|  |  |",
                    " \\___/ ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'W', new[]
                {
                    "       ",
                    "       ",
                    " _ _ _ ",
                    "| | | |",
                    "| | | |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'X', new[]
                {
                    "       ",
                    "       ",
                    " __ __ ",
                    "|  |  |",
                    "|-   -|",
                    "|__|__|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'Y', new[]
                {
                    "       ",
                    "       ",
                    " __ __ ",
                    "|  |  |",
                    "|_   _|",
                    "  |_|  ",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'Z', new[]
                {
                    "       ",
                    "       ",
                    " _____ ",
                    "|__   |",
                    "|   __|",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                '[', new[]
                {
                    "     ",
                    " ___ ",
                    "|  _|",
                    "| |  ",
                    "| |  ",
                    "| |_ ",
                    "|___|",
                    "     ",
                    "     ",
                }
            },
            {
                '\\', new[]
                {
                    "     ",
                    "     ",
                    " _   ",
                    "| \\  ",
                    " \\ \\ ",
                    "  \\_|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                ']', new[]
                {
                    "     ",
                    " ___ ",
                    "|_  |",
                    "  | |",
                    "  | |",
                    " _| |",
                    "|___|",
                    "     ",
                    "     ",
                }
            },
            {
                '_', new[]
                {
                    "       ",
                    "       ",
                    "       ",
                    "       ",
                    "       ",
                    " _____ ",
                    "|_____|",
                    "       ",
                    "       ",
                }
            },
            {
                'a', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "| .'|",
                    "|__,|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'b', new[]
                {
                    "     ",
                    "     ",
                    " _   ",
                    "| |_ ",
                    "| . |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'c', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "|  _|",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'd', new[]
                {
                    "     ",
                    "     ",
                    "   _ ",
                    " _| |",
                    "| . |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'e', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "| -_|",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'f', new[]
                {
                    "     ",
                    "     ",
                    " ___ ",
                    "|  _|",
                    "|  _|",
                    "|_|  ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'g', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "| . |",
                    "|_  |",
                    "|___|",
                    "     ",
                    "     ",
                }
            },
            {
                'h', new[]
                {
                    "     ",
                    "     ",
                    " _   ",
                    "| |_ ",
                    "|   |",
                    "|_|_|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'i', new[]
                {
                    "   ",
                    "   ",
                    " _ ",
                    "|_|",
                    "| |",
                    "|_|",
                    "   ",
                    "   ",
                    "   ",
                }
            },
            {
                'j', new[]
                {
                    "     ",
                    "     ",
                    "   _ ",
                    "  |_|",
                    "  | |",
                    " _| |",
                    "|___|",
                    "     ",
                    "     ",
                }
            },
            {
                'k', new[]
                {
                    "     ",
                    "     ",
                    " _   ",
                    "| |_ ",
                    "| '_|",
                    "|_,_|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'l', new[]
                {
                    "   ",
                    "   ",
                    " _ ",
                    "| |",
                    "| |",
                    "|_|",
                    "   ",
                    "   ",
                    "   ",
                }
            },
            {
                'm', new[]
                {
                    "       ",
                    "       ",
                    "       ",
                    " _____ ",
                    "|     |",
                    "|_|_|_|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'n', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "|   |",
                    "|_|_|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'o', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "| . |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'p', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "| . |",
                    "|  _|",
                    "|_|  ",
                    "     ",
                    "     ",
                }
            },
            {
                'q', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "| . |",
                    "|_  |",
                    "  |_|",
                    "     ",
                    "     ",
                }
            },
            {
                'r', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "|  _|",
                    "|_|  ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                's', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "|_ -|",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                't', new[]
                {
                    "     ",
                    "     ",
                    " _   ",
                    "| |_ ",
                    "|  _|",
                    "|_|  ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'u', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " _ _ ",
                    "| | |",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'v', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " _ _ ",
                    "| | |",
                    " \\_/ ",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'w', new[]
                {
                    "       ",
                    "       ",
                    "       ",
                    " _ _ _ ",
                    "| | | |",
                    "|_____|",
                    "       ",
                    "       ",
                    "       ",
                }
            },
            {
                'x', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " _ _ ",
                    "|_'_|",
                    "|_,_|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                'y', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " _ _ ",
                    "| | |",
                    "|_  |",
                    "|___|",
                    "     ",
                    "     ",
                }
            },
            {
                'z', new[]
                {
                    "     ",
                    "     ",
                    "     ",
                    " ___ ",
                    "|- _|",
                    "|___|",
                    "     ",
                    "     ",
                    "     ",
                }
            },
            {
                '{', new[]
                {
                    "       ",
                    "   ___ ",
                    "  |  _|",
                    " _| |  ",
                    "|_  |  ",
                    "  | |_ ",
                    "  |___|",
                    "       ",
                    "       ",
                }
            },
            {
                '|', new[]
                {
                    "   ",
                    " _ ",
                    "| |",
                    "| |",
                    "| |",
                    "| |",
                    "|_|",
                    "   ",
                    "   ",
                }
            },
            {
                '}', new[]
                {
                    "       ",
                    " ___   ",
                    "|_  |  ",
                    "  | |_ ",
                    "  |  _|",
                    " _| |  ",
                    "|___|  ",
                    "       ",
                    "       ",
                }
            },
        };

        #endregion

        #region Static methods

        static string GenerateAsciiArt(string text)
        {
            string[] output = new string[9];

            foreach (char c in text)
            {
                if (RectangleFont.ContainsKey(c))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        output[i] += RectangleFont[c][i] + "";
                    }
                }
            }

            List<string> result = new List<string>();
            result.AddRange(output);
            result.RemoveAt(result.Count - 1);
            result.RemoveAt(result.Count - 1);
            return string.Join("\n", result);
        }

        #endregion

        #region Instance fields and properties

        #endregion

        #region Instance methods

        public void PrintInformation([CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            Console.ResetColor();
            Console.WriteLine($"File {callerFile} method {callerMethod} line {callerLine}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(Name());
            Console.ResetColor();
            if (IsNuGet())
            {
                Console.Write(@$" v{Version()} NuGet ");
            }
            else
            {
                Console.Write(@$" v{Version()} CsProj integration");
            }

            Console.WriteLine();
            Console.WriteLine(@$"____________________________________________________________________________________________________________");
        }

        public void PrintLogo()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(GenerateAsciiArt(Name().Replace("GDF", "°GDF ") + "4"));
            Console.ResetColor();
        }

        public void PrintVersion()
        {
            Console.ResetColor();
            Console.WriteLine(@$"{Name()} version {Version()} 2024-{DateTime.UtcNow.Year} ©");
            Console.ResetColor();
        }

        #endregion
    }
}