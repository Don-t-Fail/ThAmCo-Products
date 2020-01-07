ThAmCo.Products
=====
| Master | Develop | CodeFactor
|--|--|--|
| [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Products/_apis/build/status/Don-t-Fail.ThAmCo-Products?branchName=master)](https://dev.azure.com/Stedoss/ThAmCo.Products/_build/latest?definitionId=2&branchName=master) | [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Products/_apis/build/status/Don-t-Fail.ThAmCo-Products?branchName=develop)](https://dev.azure.com/Stedoss/ThAmCo.Products/_build/latest?definitionId=2&branchName=develop) | [![CodeFactor](https://www.codefactor.io/repository/github/don-t-fail/thamco-products/badge)](https://www.codefactor.io/repository/github/don-t-fail/thamco-products)

## Overview
This web service handles products within the ThAmCo project. Stores product data and is the main place where products are shown to users.
## Tools Used
* Visual Studio 2019
* Azure Pipelines
* CodeFactor
* Docker
## Testing
Azure Pipelines was the CI chosen for this project, and is triggered on commit and pull request to the master and develop branch.
=======

## Overview
This web service handles products within the ThAmCo project. Stores product data and is the main place where products are shown to users.
## Tools Used
* Visual Studio 2019 (With ReSharper)
* Visual Studio Test Suite (Microsoft.VisualStudio.TestTools.UnitTesting)
* Visual Studio Code
* Jetbrains Rider
* Azure Pipelines (Self-Hosted Windows Agent)
* ARC (Advanced REST Client)
* GitKraken
* GitHub Desktop
* CodeFactor
* Docker and Docker-Compose
## Testing
### CI
Azure Pipelines was the CI chosen for this project, and is triggered on commit and pull request to the master and develop branch. Merging to master and develop is not allowed if the solution does not build and tests do not pass.
### Test Suite
Testing is handled through the default Visual Studio test suite.
To run all the tests in the project, use 
>dotnet test


Or use the test suite within Visual Studio or Rider.
### Mocks
Mock data contexts are included as an interface; and are generally created with simple lists (LINQ makes this simple, as data contexts are treated like lists). Mock HTTP clients are formed using the Moq package.

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
For network resilience, `Polly` is used for creating a HTTPClient factory as a service in the project. There are two client builders defined within `Startup.cs`, these two being `StandardRequest` and `ReviewRequest`. These contain different properties, `StandardRequest` deals with most of the backend requests that aren't too reliant of customer pages. The main requests sent from the customer pages are retrieving `Reviews` and `Stock`, if these services are unable to be reached after the first try, product data will be displayed without this extra data. This allows for products to still be viewed, with only slight delays to page loading times. These HTTPFactories can be seen at lines `43-54`, the two containing different http request rules.

### Tools and Frameworks

### Unit Testing

### Test Doubles

### Configuring Deployment

### Deployment Video

## System Demonstration
### Component Demonstration

### Peer Point Distribution
