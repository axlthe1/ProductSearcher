# Product management

## Description

Aim of this POC is to test a way to let a rest api solution to communicate with external providers via queue system

1. one of the core goal is to simply let the rest api to communicate with other sources with simple configuration
2. should be simple to produce new external sources and integrate inside the rest api solution
3. the soulution should be conteinerized so that could be scaled especially for external providers.

## Solution composition

1. ProductSearcher.ProductSearcherApi
   contains the rest api part
2. ProductSearcher.Models
   containes models used in this POC
3. ProductSearcher.Interfaces
   Contains interfaces used in this POC
4. ProductSearcher.DTO
   netstandar 2.1 project so that the DTO could be used also in legacy platform
5. ProductSearcher.BasicWorker
   Basic worker used in each external worker
6. OtherWorker
   One of the worker
7. SomeOtherWorker
   One of the worker
