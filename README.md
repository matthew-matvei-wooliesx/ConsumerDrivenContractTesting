# Consumer-driven Contract testing

This is just a sample / play-around to show the use of Pact-based consumer-driven contract tests. The `Wow.Pact` toolkit
is used to simplify creating the contracts and verifying them. Since this is not integrated with Azure DevOps pipelines
and therefore it's not integrated with the WooliesX's central Pact Broker, the upstream service verifies the contract
locally.

## Getting started

1. Build

    ```sh
    dotnet build
    ```

1. Run the consumer service's contracts to generate a pact locally

    ```sh
    cd ./test/ConsumerService.Contracts
    dotnet test
    ```

1. Run the upstream service's contracts to verify the consumer's pact

    ```sh
    cd ./test/UpstreamService.Contracts
    dotnet test
    ```
