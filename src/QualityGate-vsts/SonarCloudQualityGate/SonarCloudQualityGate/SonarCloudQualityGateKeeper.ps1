#
# SonarCloudQualityGateKeeper.ps1
#
[CmdletBinding(DefaultParameterSetName = 'None')]
param()

Write-Host "Starting SonarCloudQualityGateKeeper"

#Run Save-Module -Name VstsTaskSdk -Path .\ for get the Powershell VSTS SDK
# see https://github.com/Microsoft/vsts-task-lib/tree/master/powershell/Docs
#Trace-VstsEnteringInvocation $MyInvocation

$gateSuccess = $FALSE
try 
{

	$input_SonarCloudWebHookAPI = Get-VstsInput -Name 'SonarCloudWebHookAPI' -Require
	$input_TimeoutInSeconds = Get-VstsInput -Name 'TimeoutInSeconds' -Require
	$input_BuildNumber = Get-VstsInput -Name 'BuildNumber' -Require
   
	Write-Output "Inputs"
	Write-Output "input_SonarCloudWebHookAPI: $input_SonarCloudWebHookAPI"
	Write-Output "input_TimeoutInSeconds: $input_TimeoutInSeconds"
	Write-Output "input_BuildNumber: $input_BuildNumber"
	
	$ts = New-TimeSpan -Seconds $input_TimeoutInSeconds
	$finishTime = (Get-Date) + $ts

	while (($finishTime.CompareTo((Get-Date))) -gt 0)
	{		
		try
		{
			$completeUrl=$input_SonarCloudWebHookAPI+"`/"+$input_BuildNumber
			Write-Output "Get endpoint : $completeUrl"
			$response = Invoke-RestMethod -Uri $completeUrl -Method Get       
		}
		catch
		{
			$ErrorMessage = $_.Exception.Message
			Write-Output "REST endpoint call error $ErrorMessage"
            continue
		}

		if (!$response)
		{
			continue
		}

		if ("True".Equals($response))
		{
			$gateSuccess=true
            break            
		}
	}


}
catch 
{
	$ErrorMessage = $_.Exception.Message
} 
finally
{
    if (!$gateSuccess)
    {
        throw "Quality Gate failed"
    }
}

Write-Host "Ending SonarCloudQualityGateKeeper"