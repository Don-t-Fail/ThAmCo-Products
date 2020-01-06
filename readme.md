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
