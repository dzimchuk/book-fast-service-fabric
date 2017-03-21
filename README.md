# Book Fast

A Service Fabric based multitenant facility management and accommodation booking application.

## Features

- Microservices architecture
- Stateless and stateful services
- ASP.NET Core Web API and web frontend
- Swagger and AutoRest
- Per environment configuration integrated with ASP.NET Core infrastructure
- Redis cache
- Multitenant Azure AD organizational accounts
- Azure AD B2C authentication for customers
- OpenID Connect and OAuth2
- Azure Search
- Application Insights

## Configuration

BookFast.sfproj references `..\..\..\config\BookFast\Local.xml` that is used in local deployment profiles. This file is not included in the repository and you will need to provide your configuration overrides.

Here's a short description of configuration parameters:

```
<?xml version="1.0" encoding="utf-8"?>
<Application xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="fabric:/BookFast" xmlns="http://schemas.microsoft.com/2011/01/fabric">
  <Parameters>
    <Parameter Name="HttpsCertThumbprint" Value="Thumbprint of an SSL certificate installed in the local store" />
    
    <Parameter Name="ASPNETCORE_ENVIRONMENT" Value="Standard APS.NET Core environment setting, e.g. Development" />

    <Parameter Name="Data:DefaultConnection:ConnectionString" Value="Connection string to a SQL database" />
    <Parameter Name="Data:Azure:Storage:ConnectionString" Value="Connection string to an Azure storage account" />
    <Parameter Name="Data:Azure:Storage:SearchIndexQueueName" Value="search-index" />
    <Parameter Name="Data:Azure:Storage:ImageContainer" Value="book-fast" />
    
    <Parameter Name="Search:QueryKey" Value="Azure Search query key" />
    <Parameter Name="Search:AdminKey" Value="Azure Search admin key" />
    <Parameter Name="Search:ServiceName" Value="Azure Search service name" />
    <Parameter Name="Search:IndexName" Value="book-fast" />
    
    <Parameter Name="ApplicationInsights:InstrumentationKey" Value="Application Insights resource instrumentation key" />

    <Parameter Name="Redis:Configuration" Value="Redis connection string" />

    <Parameter Name="Authentication:AzureAd:B2C:Audience" Value="Your Azure AD B2C app client ID" />
    <Parameter Name="Authentication:AzureAd:B2C:Instance" Value="Your Azure AD B2C instance, e.g. https://login.microsoftonline.com/" />
    <Parameter Name="Authentication:AzureAd:B2C:TenantId" Value="Your Azure AD B2C tenant, e.g. devunleashedb2c.onmicrosoft.com" />
    <Parameter Name="Authentication:AzureAd:B2C:ClientId" Value="Your Azure AD B2C app client ID" />
    <Parameter Name="Authentication:AzureAd:B2C:ClientSecret" Value="Your Azure AD B2C app client secret" />
    <Parameter Name="Authentication:AzureAd:B2C:PostLogoutRedirectUri" Value="e.g. https://localhost:8686/" />
    <Parameter Name="Authentication:AzureAd:B2C:Policies:SignInOrSignUpPolicy" Value="B2C_1_TestSignUpAndSignInPolicy" />
    <Parameter Name="Authentication:AzureAd:B2C:Policies:EditProfilePolicy" Value="B2C_1_TestProfileEditPolicy" />

    <Parameter Name="Authentication:AzureAd:ApiApp:Instance" Value="Your Azure AD instance, e.g. https://login.microsoftonline.com/" />
    <Parameter Name="Authentication:AzureAd:ApiApp:Audience" Value="BookFast API AppId in Azure AD, e.g. https://devunleashed.onmicrosoft.com/book-fast-api" />
    <Parameter Name="Authentication:AzureAd:ApiApp:ValidIssuers" Value="Comma separated list of tenant identifiers, e.g. https://sts.windows.net/490789ec-b183-4ba5-97cf-e69ec8870130/,https://sts.windows.net/f418e7eb-0dcd-40be-9b81-c58c87c57d9a/" />
   
    <Parameter Name="Authentication:AzureAd:WebApp:ApiResource" Value="BookFast API AppId in Azure AD, e.g. https://devunleashed.onmicrosoft.com/book-fast-api" />
    <Parameter Name="Authentication:AzureAd:WebApp:Instance" Value="Your Azure AD instance, e.g. https://login.microsoftonline.com/" />
    <Parameter Name="Authentication:AzureAd:WebApp:ValidIssuers" Value="Comma separated list of tenant identifiers, e.g. https://sts.windows.net/490789ec-b183-4ba5-97cf-e69ec8870130/,https://sts.windows.net/f418e7eb-0dcd-40be-9b81-c58c87c57d9a/" />
    <Parameter Name="Authentication:AzureAd:WebApp:ClientId" Value="Your BookFast app's client ID" />
    <Parameter Name="Authentication:AzureAd:WebApp:ClientSecret" Value="Your BookFast app's client secret" />
    <Parameter Name="Authentication:AzureAd:WebApp:PostLogoutRedirectUri" Value="e.g. https://localhost:8686/" />
    
  </Parameters>
</Application>
```

Please inspect service and application manifests to understand how these parameters are used to configure services.

### Azure AD

Azure AD is used for organizational accounts of facility providers. You will need two applications in Azure AD: one for the APIs (Book Fast API app) and one for the web (BookFast app). Both applications should have multitenant support enabled. BookFast should have a delegated permission to access BookFast API app. If you're new to Azure AD the following post are going to help you out:

- [Protecting your APIs with Azure Active Directory](https://dzimchuk.net/protecting-your-apis-with-azure-active-directory/)
- [Enabling multitenant support in you Azure AD protected applications](https://dzimchuk.net/enabling-multitenant-support-in-you-azure-ad-protected-applications/)

Both apps have a user role called 'Facility Provider' that should be assigned to users to enable them to edit facilities. Please have a look at this [post](https://dzimchuk.net/application-and-user-permissions-in-azure-ad/) to understand how application and user roles are configured in Azure AD.

### Azure AD B2C

Customer accounts are managed in Azure AD B2C. It supports self sign up, profile editing and 3rd part identity providers.

You will need to create a B2C tenant and an app. You will also need to policies:

1. Sign in or sign up policy
2. Profile edit policy

You may also find this [post](https://dzimchuk.net/setting-up-your-aspnet-core-apps-and-services-for-azure-ad-b2c/) useful when setting you your application.

### SQL Database

BookFast.Facility.Data contain EFCore migrations to set up you SQL database schema.

### Azure Search

BookFast.Search.Adapter can be run from the command line as `dotnet run provision` in order to create an index in your Azure Search service. It will require the following parameters to be defined in [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets):

- Search:ServiceName
- Search:AdminKey
- Search:IndexName