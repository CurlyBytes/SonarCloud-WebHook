$ts = New-TimeSpan -Seconds 10
$finishTime = (Get-Date) + $ts

while (($finishTime.CompareTo((Get-Date))) -gt 0)
{
    $response = null
    try
    {
        $response = Invoke-RestMethod -Uri "http://localhost:8420/api/hook/1" -Method Get       
    }
    catch
    {
        $ErrorMessage = $_.Exception.Message
        Write-Error $ErrorMessage
    }

    if (!$response)
    {
        continue
    }

    if ("False".Equals($response))
    {
        Throw "Quality gate failed"
    }
}