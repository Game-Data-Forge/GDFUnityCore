#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj LibrariesWorkflow.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace GDFFoundation
{
    public static class LibrariesWorkflow
    {
        #region Static fields and properties

        private static readonly List<GDFAssemblyInformation> AssemblyInfoList = new List<GDFAssemblyInformation>();
        private static readonly Dictionary<string, string> Installed = new Dictionary<string, string>();
        private static readonly Dictionary<Assembly, GDFAssemblyInformation> DicoToRename = new Dictionary<Assembly, GDFAssemblyInformation>();
        private static readonly List<Assembly> InstalledAssemblies = new List<Assembly>();

        #endregion

        #region Static methods

        public static GDFAssemblyInformation GetForFoundation()
        {
            return GetForAssembly(typeof(GDFFoundation.GDFConstants).Assembly);
        }

        public static GDFAssemblyInformation GetForExecuting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return GetForAssembly(assembly);
        }

        public static GDFAssemblyInformation GetForAssembly(Assembly assembly)
        {
            if (DicoToRename.ContainsKey(assembly))
            {
                return DicoToRename[assembly];
            }
            else
            {
                return AddAssemblyInfo(assembly);
            }
        }

        public static GDFAssemblyInformation AddAssemblyInfo(Assembly assembly)
        {
            if (!InstalledAssemblies.Contains(assembly))
            {
                Type versionType = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == nameof(GDFAssemblyInfo) && typeof(GDFAssemblyInformation).IsAssignableFrom(t));
                if (versionType == null)
                {
                    throw new InvalidOperationException($"{nameof(GDFAssemblyInfo)} not found in {assembly.FullName}");
                }
                GDFAssemblyInformation result = (GDFAssemblyInformation)Activator.CreateInstance(versionType)!;
                result.Information(assembly);
                InstalledAssemblies.Add(assembly);
                AssemblyInfoList.Add(result);
                DicoToRename.TryAdd(assembly, result);

                result.ConsolePrint();
            }

            return DicoToRename[assembly];
        }

        public static GDFAssemblyInformation AddAssemblyInfo(GDFAssemblyInformation info)
        {
            Assembly assembly = info.GetType().Assembly;
            info.Information(assembly);
            if (!InstalledAssemblies.Contains(info.GetType().Assembly))
            {
                InstalledAssemblies.Add(info.GetType().Assembly);
                AssemblyInfoList.Add(info);
                DicoToRename.TryAdd(assembly, info);

                info.ConsolePrint();
            }

            return info;
        }

        public static string ExtractMethodName(Expression<Action> expr)
        {
            if (expr.Body is MethodCallExpression call)
            {
                return call.Method.Name;
            }

            if (expr.Body is UnaryExpression unary && unary.Operand is MethodCallExpression innerCall)
            {
                return innerCall.Method.Name;
            }

            throw new ArgumentException("Expression must be a method call", nameof(expr));
        }

        public static List<GDFAssemblyInformation> GetVersionDllList()
        {
            return new List<GDFAssemblyInformation>(AssemblyInfoList);
        }

        public static bool SetModuleInstalledByExpression(Expression<Action> expr, string who, string optional = "")
        {
            if (expr.Body is MethodCallExpression call)
            {
                string methodName = ExtractMethodName(expr);
                return SetModuleInstalledByInternal(methodName, who, optional);
            }

            throw new ArgumentException("Expression must be a method call", nameof(expr));
        }

        public static bool SetModuleInstalledByExpressionT<T>(Expression<Action<T>> methodExpr, string who, string optional = "")
        {
            if (methodExpr.Body is MethodCallExpression methodCall)
            {
                // string methodName = methodCall.Method.Name;
                var method = methodCall.Method;
                string methodName = method.Name;

                if (method.IsGenericMethod)
                {
                    var genericArguments = method.GetGenericArguments();
                    var genericPart = "<" + string.Join(", ", genericArguments.Select(t => t.Name)) + ">";
                    methodName += genericPart;
                }

                return SetModuleInstalledByInternal(methodName, who, optional);
            }

            throw new ArgumentException("Expression must be a method call", nameof(methodExpr));
        }

        /// <summary>
        ///     Attempts to mark an item as installed by a specific user or entity.
        ///     Logs a warning if the item is already marked as installed.
        /// </summary>
        /// <param name="what">The name of the item to install.</param>
        /// <param name="who">The identifier of the user or entity performing the installation.</param>
        /// <param name="optional"></param>
        /// <returns>
        ///     Returns true if the item is successfully marked as installed;
        ///     otherwise, false if the item was already marked as installed.
        /// </returns>
        private static bool SetModuleInstalledByInternal(string what, string who, string optional)
        {
            bool result = Installed.TryAdd(what, who);
            if (result == false)
            {
                string whoBefore = Installed[what];
                GDFLogger.Error($"Warning: possible conflict! The '{what}' was already installed by '{whoBefore}' before '{who}'! ({optional})");
            }
            else
            {
                GDFLogger.Success($"Add '{what}' by '{who}' ({optional})");
            }

            return result;
        }

        public static bool SetModuleInstalledByName(string what, string who, string optional = "")
        {
            return SetModuleInstalledByInternal(what, who, optional);
        }

        #endregion
    }
}