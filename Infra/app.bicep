param location string = resourceGroup().location

param webAppName string = 'rubbed'
var appServicePlanName = '${webAppName}-app-plan'
var webSiteName = '${webAppName}-app'


param serverName string = '${webAppName}-db-server'

param sqlDBName string = '${webAppName}-db'

param workspaceName string = 'appInsightsWorkspace'
param appInsightsName string = 'appInsights'

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: serverName
  location: location
  properties: {
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Disabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'Group'
      login: 'db-admin'
      sid: '339092ec-51a3-4b1d-b588-d16860b4efac'
      tenantId: '88d823a1-6334-422c-8b78-1665d9b4cbac'
      azureADOnlyAuthentication: true
    }
  }
}

resource sqlDB 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: sqlDBName
  location: location
  properties: {
    useFreeLimit: true
    freeLimitExhaustionBehavior: 'AutoPause'
    autoPauseDelay: 60
  }
  sku: {
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 2
  }
}

var DbConnectionString = 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=db${sqlDB.name};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    family: 'F'
    capacity: 1
  }
  kind: 'linux'
}

resource appService 'Microsoft.Web/sites@2023-12-01' = {
  name: webSiteName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      http20Enabled: true
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ConnectionStrings__Default'
          value: DbConnectionString
        }
      ]
    }
    
    httpsOnly: true
  }  
}

resource workspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: workspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    workspaceCapping: {}
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'other'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Bluefield'
    WorkspaceResourceId: workspace.id
    RetentionInDays: 90
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
