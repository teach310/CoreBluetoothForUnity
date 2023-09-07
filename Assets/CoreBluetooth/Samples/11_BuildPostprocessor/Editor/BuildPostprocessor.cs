using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace CoreBluetoothEditor
{
    public class BuildPostProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 1; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            var buildTarget = report.summary.platform;
            if (buildTarget == BuildTarget.iOS)
            {
                ProcessForiOS(report);
            }

            Debug.Log("BuildPostprocessor.OnPostprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
        }

        void ProcessForiOS(BuildReport report)
        {
            var buildOutputPath = report.summary.outputPath;
            UpdateInfoPlist(buildOutputPath, InfoPlistData());
            UpdatePBXProject(buildOutputPath, project =>
            {
                DisableBitcode(project);
            });
        }

        void UpdateInfoPlist(string buildOutputPath, Dictionary<string, string> plistData)
        {
            var plistPath = Path.Combine(buildOutputPath, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            var rootDict = plist.root;
            foreach (var kvp in plistData)
            {
                rootDict.SetString(kvp.Key, kvp.Value);
            }
            plist.WriteToFile(plistPath);
        }

        Dictionary<string, string> InfoPlistData()
        {
            return new Dictionary<string, string>()
            {
                { "NSBluetoothAlwaysUsageDescription", "use Bluetooth" }
            };
        }

        void UpdatePBXProject(string buildOutputPath, Action<PBXProject> action)
        {
            var pbxProjectPath = PBXProject.GetPBXProjectPath(buildOutputPath);
            var project = new PBXProject();
            project.ReadFromFile(pbxProjectPath);
            action(project);
            project.WriteToFile(pbxProjectPath);
        }

        void DisableBitcode(PBXProject project)
        {
            var mainTargetGuid = project.GetUnityMainTargetGuid();
            project.SetBuildProperty(mainTargetGuid, "ENABLE_BITCODE", "NO");

            var unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();
            project.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
        }
    }
}
