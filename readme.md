ThAmCo.Products
=====
| Master | Develop | CodeFactor
|--|--|--|
| [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Products/_apis/build/status/Don-t-Fail.ThAmCo-Products?branchName=master)](https://dev.azure.com/Stedoss/ThAmCo.Products/_build/latest?definitionId=2&branchName=master) | [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Products/_apis/build/status/Don-t-Fail.ThAmCo-Products?branchName=develop)](https://dev.azure.com/Stedoss/ThAmCo.Products/_build/latest?definitionId=2&branchName=develop) | [![CodeFactor](https://www.codefactor.io/repository/github/don-t-fail/thamco-products/badge)](https://www.codefactor.io/repository/github/don-t-fail/thamco-products)


## Overview
This web service handles products within the ThAmCo project. Stores product data and is the main place where products are shown to users.

## Branch Management
The main branch is `Master`, and should only be committed to/pushed to when the service has been confirmed working (this includes passing tests and human testing). All pushes to `Master` will be pushed into production.  
`Develop` is where features may be added (only by means of feature branches), tests should all pass when being pushed to this branch, however some things may not work fully.  
`Feature` branches are assumed unstable, and are actively being worked on. CI does not build these branches.

# Assignment Checklist
## System Architecture 
### Original Architecture Diagram with Feedback

### Updated Architecture Diagram with Comments

### Solo Development Plan

### Peer Point Distribution

## System Implementation
### Security Topics
Due to no central Login/Accounts for the project, please view the Accounts controller in `Controllers/AccountController.cs` as this is where authentication takes place within this service.

### Data Distribution
Data Distribution within this project mainly relies on making HTTP requests. Little information is duplicated across services (particularly saved to databases) to try to make data as up to date as possible. Due to this, pages may take a little longer to load (or fail to load), however these would generally occur for Staff and Admin pages, as customer pages are optimised to display as much data as they can; loading pages without some elements (like reviews).

### Network Resilience
For network resilience, `Polly` is used for creating a HTTPClient factory as a service in the project. There are two client builders defined within `Startup.cs`, these two being `StandardRequest` and `ReviewRequest`. These contain different properties, `StandardRequest` deals with most of the backend requests that aren't too reliant of customer pages. The main requests sent from the customer pages are retrieving `Reviews` and `Stock`, if these services are unable to be reached after the first try, product data will be displayed without this extra data. This allows for products to still be viewed, with only slight delays to page loading times. These HTTPFactories can be seen at `lines 43-54`, the two containing different http request rules.

### Tools and Frameworks
Multiple tools and frameworks are used within this project. These are listed below.
#### Language and Environment
* C# with ASPNET Core version 2.2.
* Visual Studio 2019 (with ReSharper) was used for most of the development, debugging builds were usually ran in ISS.
* IntelliJ Rider was also used as it has a very good testing interface, along with ReSharper built in.
* Visual Studio Code was used to write readme files, along with some tests to make sure the `dotnet test` functionality worked within the project.
* Windows 10 - Development Environment.
* Linux Ubuntu and Debian, including Docker.
* MSSQL Database. (Running in Docker for production).
* GitKraken was mainly used as the Git client for this project, however GitHub Desktop was also used for some commits.
* CodeFactor was used just to check code quality on pull request.

#### Testing
* Visual Studio Test Tools (Microsoft.VisualStudio.TestTools.UnitTesting). Testing can be run by going to the root directory of this project and using
>dotnet test  

* Moq is used for a HttpClient mock. This can be seen in `ThAmCo.Products.Tests/Controllers/ProductsControllerTests.cs lines 68-79`. These get given data depending on the test, as can be seen at the start of the test `GetProductIndex_AllValid_AllReturned() lines 84-104`.
* Test doubles (test data) are available within the test cs file (`ThAmCo.Products.Tests/Controllers/ProductsControllerTests.cs`), and are inside of their own `Data` class. These are located at the start of the file, in `lines 25-66`. These were then used within the `ThAmCo.Products/Data/ProductsContext/MockProductsContext.cs` to act as the context fake.
* ARC (Advanced REST Client) and Postman were used to test API endpoints within this project. They were useful to test endpoints within my own services, along with creating the DTOs required for retrieving data from other services.

#### Continuous Integration
* Azure Pipelines running on a Windows agent. The Azure Pipelines configuration can be found in the root directory, in `azure-pipelines.yml`. A build is scheduled when a commit is pushed to the Master or Develop branch. A build is also scheduled when a pull request is made to the Develop branch, and is checked by means of a Github Status Check. This means when a pull request is made, Github requests a build from Azure Pipelines, and will now allow the branch to be merged unless the build passes.
* AppVeyor was used for a short period as an addition to Azure Pipelines, however was removed due to compatibility issues with build images.

### Configuring Deployment
* A `Dockerfile` is included within this repo as a means of deploying this service. This `Dockerfile` is configured to be used with the produced artefact from Azure Pipelines, and has not been designed to be used for debugging, building, or run directly inside of the repo. To deploy the docker image, please follow these instructions:
>Take produced artefact from Azure Pipelines (ThAmCo-Products.zip)  
Extract to a folder called Products  
Place Dockerfile one up from the Product directory  
docker build thamco-products .  

This should produce a docker image that can then be launched.  
`Docker-compose` would also be used when all services are available with their dockerfile. This would be the ideal way of deploying all services at once, along with their database.

### Deployment Video

## System Demonstration
### Component Demonstration

### Peer Point Distribution
