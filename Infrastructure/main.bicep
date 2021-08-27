param webAppName string {
  minLength: 2
  default: 'jpapp-kr-dev'
}

param location string {
  default: resourceGroup().location
}

var appServicePlanName = 'appPlan-${webAppName}'
var identityName = 'junparkSPN2'
var roleDefinitionId = resourceId('Microsoft.Authorization/roleDefinitions', '01db9ff5-520e-46b3-9e95-42797cd9c07d')
var roleAssignmentName = guid(identityName, roleDefinitionId)

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'S1'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: 'webAppIdentity'
  location: location
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: roleAssignmentName
  scope: containerRegistry
  properties: {
    roleDefinitionId: roleDefinitionId
    principalId: managedIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

resource webApp 'Microsoft.Web/sites@2020-06-01' = {
  name: webAppName
  location: location
  kind: 'app'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${managedIdentity.id}': {}
    }
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|5.0'
      alwaysOn: true
      appSettings: [
        {
          name: 'WEBSITE_WEBDEPLOY_USE_SCM'
          value: 'true'
        }
        {
          name: 'WEBSITES_PORT'
          value: '80'
        }
        {
          name: 'DOCKER_ENABLE_CI'
          value: 'true'
        }
      ]
    }
  }
}
resource containerRegistry 'Microsoft.ContainerRegistry/registries@2020-11-01-preview' = {
  name: '${webAppName}acr'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    adminUserEnabled: true
  }
}

output resourceGroupOutput string = resourceGroup().name
output webAppNameOutput string = webApp.name
output registryNameOutput string = containerRegistry.name