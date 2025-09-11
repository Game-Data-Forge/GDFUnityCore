using System;
using GDFFoundation;
using GDFRuntime;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GDFUnity.Editor
{
    public class BuildProcess : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            try
            {
                GDFLogger.Debug("Started GDF Build process...");
                GDFEditor.Launch.Wait();
                IRuntimeConfiguration configuration = EditorConfigurationEngine.Instance.CreateRuntimeConfiguration();
                EditorConfigurationEngine.Instance.Save(configuration);
                GDFLogger.Debug("GDF Build process done !");
            }
            catch (Exception e)
            {
                FailBuild(e);
            }
        }

        private void FailBuild (Exception e)
        {
            Debug.LogError(e);
            throw new BuildFailedException("FORCED FAILED");
        }
    }
}