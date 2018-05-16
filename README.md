
# SonarCloud-WebHook project

This project contains the backend components to receive and enquire about the quality gate success/failure as received from SonarCloud.io

## Components
- State actor (SonarCloudWebHookStateActorService)
- Web (Routing) API (SonarWebHookFrontAPIService)

### SonarCloudWebHookStateActorService

This is a stateful actor with **single** partition but multiple (3) replicas. Should the load increase, we may add partitioning/sharding as necessary.

### SonarWebHookFrontAPIService

This is stateless ASP.NET core stateless actor. Its purpose is to route incoming requests to and from the state service.

its endpoints are
 - /api/hook
    - GET {id} - id of the build identifier (see below)
    - POST - this receives the payload from sonarcloud web hook and stores the quality gate flag along with build identifier (https://docs.sonarqube.org/display/SONAR/Webhooks)
 - /Probe
    - GET - used for load balancer to determine service health
  
  ## Build Identifier
  
  In order to associate build with the web hook post payload, a dedicated property has been reserver and contains unique build identifier. This is passed to the sonar cloud scanner and is then included in the web hook payload as a property.
  
  The full name of the property is `sonar.analysis.buildIdentifier`
 
 An example below
  ```json
  {
  "serverUrl": "https://sonarcloud.io",
  "taskId": "AWNReyfkiprWWeJOi2Bp",
  "status": "SUCCESS",
  "analysedAt": "2018-05-12T01:13:26+0200",
  "changedAt": "2018-05-12T01:13:26+0200",
  "project": {
    "key": "esw.labelling-api",
    "name": "labelling-api",
    "url": "https://sonarcloud.io/dashboard?id=esw.labelling-api"
  },
  "branch": {
    "name": "8543",
    "type": "PULL_REQUEST",
    "isMain": false,
    "url": "https://sonarcloud.io/project/issues?pullRequest=8543&id=esw.labelling-api&resolved=false"
  },
  "qualityGate": {
    "name": "Hardcoded short living branch quality gate",
    "status": "OK",
    "conditions": [
      {
        "metric": "open_issues",
        "operator": "GREATER_THAN",
        "value": "2",
        "status": "ERROR",
        "onLeakPeriod": false,
        "errorThreshold": "0"
      },
      {
        "metric": "reopened_issues",
        "operator": "GREATER_THAN",
        "value": "0",
        "status": "OK",
        "onLeakPeriod": false,
        "errorThreshold": "0"
      }
    ]
  },
  "properties": {
    "sonar.analysis.buildIdentifier":"1"
  }
}
```

## The build sequence

The typical build sequence would therefore be

- ### prepare build identifier
this can be achived with script similar to 

```
$newGuid =[guid]::NewGuid()
Write-Host "##vso[task.setvariable variable=sonarBuildIdentifier]$newGuid"
```
- ### pass the build identifier to scanner
when running the 'Prepare analysis on SonarCloud' make sure the variable is included in the Advanced/Additional Properties configuration
```
sonar.analysis.buildIdentifier=$(sonarBuildIdentifier)
```

- ### add SonarCloud Quality Gate Keeper task

this must be done after the `Publish Analysis Result` task. See https://github.com/eShopWorld/SonarCloud-WebHookVSTS/blob/master/README.md for details.
