#
# ServiceFabricUpdaterTests.ps1
#

Import-Module ..\ps_modules\VstsTaskSdk

$input_SonarCloudWebHookAPI = "http://localhost:8420"
$input_TimeoutInSeconds = "100"
$input_BuildNumber = "1"

Invoke-VstsTaskScript -ScriptBlock { . ..\SonarCloudQualityGate\SonarCloudQualityGateKeeper.ps1 }